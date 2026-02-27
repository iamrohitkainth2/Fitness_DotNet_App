namespace HealthManagement.Models
{
    public class NutritionEstimate
    {
        public string FoodName { get; set; } = string.Empty;
        public decimal Calories { get; set; }
        public decimal Carbs { get; set; }
        public decimal Protein { get; set; }
        public decimal Fat { get; set; }
        public Dictionary<string, decimal> Micronutrients { get; set; } = new();
    }
}
