using System.ComponentModel.DataAnnotations;

namespace HealthManagement.Models
{
    public class ExerciseLog
    {
        public int Id { get; set; }
        
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
        
        [Required]
        public string ExerciseName { get; set; } = string.Empty;
        [Range(0, int.MaxValue)]
        public int DurationMinutes { get; set; }
        public string Intensity { get; set; } = string.Empty; // e.g., "light", "moderate", "vigorous"
        [Range(0, double.MaxValue)]
        public decimal CaloriesBurned { get; set; }
        
        // MET value (Metabolic Equivalent of Task) for the exercise
        public decimal? MetValue { get; set; }
        
        public DateTime DateLogged { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
