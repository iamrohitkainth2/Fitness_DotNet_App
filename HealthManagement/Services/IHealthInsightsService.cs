namespace HealthManagement.Services
{
    public interface IHealthInsightsService
    {
        Task<string> GenerateDailyInsightAsync(
            decimal dailyIntake,
            decimal dailyBurned,
            IEnumerable<decimal> weeklyIntake,
            IEnumerable<decimal> weeklyBurned,
            string dietaryGoal,
            CancellationToken cancellationToken = default);
    }
}
