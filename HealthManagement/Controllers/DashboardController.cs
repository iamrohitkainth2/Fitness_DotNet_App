using HealthManagement.Models;
using HealthManagement.Services;
using HealthManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HealthManagement.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IFoodLogService _foodLogService;
        private readonly IExerciseLogService _exerciseLogService;
        private readonly IHealthInsightsService _healthInsightsService;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(
            IFoodLogService foodLogService,
            IExerciseLogService exerciseLogService,
            IHealthInsightsService healthInsightsService,
            UserManager<ApplicationUser> userManager)
        {
            _foodLogService = foodLogService;
            _exerciseLogService = exerciseLogService;
            _healthInsightsService = healthInsightsService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(DateTime? date, bool generateInsight = false)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var selectedDate = date?.Date ?? DateTime.UtcNow.Date;

            var viewModel = new DashboardViewModel
            {
                SelectedDate = selectedDate,
                DailyCaloriesIntake = await _foodLogService.GetDailyCalorieTotalAsync(user.Id, selectedDate),
                DailyCaloriesBurned = await _exerciseLogService.GetDailyCaloriesBurnedAsync(user.Id, selectedDate)
            };

            for (var i = 6; i >= 0; i--)
            {
                var day = selectedDate.AddDays(-i);
                viewModel.WeeklyLabels.Add(day.ToString("ddd"));
                viewModel.WeeklyIntake.Add(await _foodLogService.GetDailyCalorieTotalAsync(user.Id, day));
                viewModel.WeeklyBurned.Add(await _exerciseLogService.GetDailyCaloriesBurnedAsync(user.Id, day));
            }

            if (generateInsight)
            {
                viewModel.IsInsightGenerated = true;
                viewModel.PersonalizedInsight = await _healthInsightsService.GenerateDailyInsightAsync(
                    viewModel.DailyCaloriesIntake,
                    viewModel.DailyCaloriesBurned,
                    viewModel.WeeklyIntake,
                    viewModel.WeeklyBurned,
                    user.DietaryGoal);
            }

            return View(viewModel);
        }
    }
}
