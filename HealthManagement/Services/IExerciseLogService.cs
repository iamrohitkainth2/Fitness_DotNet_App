using HealthManagement.Models;

namespace HealthManagement.Services
{
    public interface IExerciseLogService
    {
        Task<ExerciseLog?> GetExerciseLogByIdAsync(int id);
        Task<IEnumerable<ExerciseLog>> GetUserExerciseLogsAsync(string userId);
        Task<IEnumerable<ExerciseLog>> GetUserExerciseLogsByDateAsync(string userId, DateTime date);
        Task<decimal> GetDailyCaloriesBurnedAsync(string userId, DateTime date);
        Task AddExerciseLogAsync(ExerciseLog exerciseLog);
        Task UpdateExerciseLogAsync(ExerciseLog exerciseLog);
        Task DeleteExerciseLogAsync(int id);
        decimal CalculateCaloriesBurned(decimal weight, int durationMinutes, decimal metValue);
    }
}
