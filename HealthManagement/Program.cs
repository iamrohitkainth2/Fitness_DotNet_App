using Azure;
using Azure.AI.OpenAI;
using Azure.Identity;
using HealthManagement.Data;
using HealthManagement.Models;
using HealthManagement.Repositories;
using HealthManagement.Services;
using HealthManagement.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// Configure ASP.NET Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI() // includes login/registration pages
    .AddDefaultTokenProviders();

// Configure Azure OpenAI Settings
var azureOpenAISettings = builder.Configuration.GetSection("AzureOpenAI").Get<AzureOpenAISettings>();
if (azureOpenAISettings != null)
{
    builder.Services.Configure<AzureOpenAISettings>(builder.Configuration.GetSection("AzureOpenAI"));
    
    // Register OpenAI Client
    builder.Services.AddSingleton(sp =>
    {
        var settings = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<AzureOpenAISettings>>().Value;
        if (!string.IsNullOrEmpty(settings.ApiKey))
        {
            return new Azure.AI.OpenAI.AzureOpenAIClient(new Uri(settings.Endpoint), new System.ClientModel.ApiKeyCredential(settings.ApiKey));
        }
        else
        {
            // Use DefaultAzureCredential for managed identity (deployment)
            return new Azure.AI.OpenAI.AzureOpenAIClient(new Uri(settings.Endpoint), new DefaultAzureCredential());
        }
    });
}

// Register Repositories
builder.Services.AddScoped<IFoodLogRepository, FoodLogRepository>();
builder.Services.AddScoped<IExerciseLogRepository, ExerciseLogRepository>();

// Register Services
builder.Services.AddScoped<IFoodLogService, FoodLogService>();
builder.Services.AddScoped<IExerciseLogService, ExerciseLogService>();
builder.Services.AddHttpClient<INutrientExtractionService, NutrientExtractionService>();
builder.Services.AddHttpClient<IHealthInsightsService, HealthInsightsService>();

builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
});

builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(365);
});

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "RequestVerificationToken";
});

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.RequestMethod |
                            HttpLoggingFields.RequestPath |
                            HttpLoggingFields.ResponseStatusCode |
                            HttpLoggingFields.Duration;
});

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());

    if (!builder.Environment.IsDevelopment())
    {
        options.Filters.Add(new RequireHttpsAttribute());
    }
});

// Razor Pages required for Identity UI
builder.Services.AddRazorPages();

var app = builder.Build();

app.UseHttpLogging();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
