using Microsoft.AspNetCore.Identity;

namespace HealthManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int Age { get; set; }
        public decimal Weight { get; set; } // in kg
        public decimal Height { get; set; } // in cm
        public string DietaryGoal { get; set; } = string.Empty; // e.g., "weight loss", "muscle gain", "maintenance"
        
        public ICollection<FoodLog> FoodLogs { get; set; } = new List<FoodLog>();
        public ICollection<ExerciseLog> ExerciseLogs { get; set; } = new List<ExerciseLog>();
    }
}
