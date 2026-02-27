namespace HealthManagement.ViewModels
{
    public class DashboardViewModel
    {
        public DateTime SelectedDate { get; set; }
        public decimal DailyCaloriesIntake { get; set; }
        public decimal DailyCaloriesBurned { get; set; }
        public decimal DailyCalorieBalance => DailyCaloriesIntake - DailyCaloriesBurned;

        public List<string> WeeklyLabels { get; set; } = new();
        public List<decimal> WeeklyIntake { get; set; } = new();
        public List<decimal> WeeklyBurned { get; set; } = new();
        public bool IsInsightGenerated { get; set; }
        public string PersonalizedInsight { get; set; } = string.Empty;
    }
}
