using HealthManagement.Models;

namespace HealthManagement.Repositories
{
    public interface IExerciseLogRepository : IRepository<ExerciseLog>
    {
        Task<IEnumerable<ExerciseLog>> GetByUserIdAsync(string userId);
        Task<IEnumerable<ExerciseLog>> GetByUserIdAndDateAsync(string userId, DateTime date);
        Task<decimal> GetTotalCaloriesBurnedByUserAndDateAsync(string userId, DateTime date);
    }
}
