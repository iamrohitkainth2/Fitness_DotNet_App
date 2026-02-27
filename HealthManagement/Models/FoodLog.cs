using System.ComponentModel.DataAnnotations;

namespace HealthManagement.Models
{
    public class FoodLog
    {
        public int Id { get; set; }
        
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
        
        [Required]
        public string FoodName { get; set; } = string.Empty;
        [Range(0, double.MaxValue)]
        public decimal Calories { get; set; }
        [Range(0, double.MaxValue)]
        public decimal Carbs { get; set; } // grams
        [Range(0, double.MaxValue)]
        public decimal Protein { get; set; } // grams
        [Range(0, double.MaxValue)]
        public decimal Fat { get; set; } // grams
        
        // Micronutrients stored as JSON string
        public string Micronutrients { get; set; } = "{}";
        
        // Image reference if food was identified from photo
        public string? ImagePath { get; set; }
        
        public DateTime DateLogged { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
