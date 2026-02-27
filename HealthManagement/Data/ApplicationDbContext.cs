using HealthManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HealthManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<FoodLog> FoodLogs { get; set; }
        public DbSet<ExerciseLog> ExerciseLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure FoodLog relationships
            builder.Entity<FoodLog>()
                .HasOne(fl => fl.User)
                .WithMany(u => u.FoodLogs)
                .HasForeignKey(fl => fl.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure ExerciseLog relationships
            builder.Entity<ExerciseLog>()
                .HasOne(el => el.User)
                .WithMany(u => u.ExerciseLogs)
                .HasForeignKey(el => el.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
