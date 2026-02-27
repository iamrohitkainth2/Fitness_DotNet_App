using HealthManagement.Models;

namespace HealthManagement.Repositories
{
    public interface IFoodLogRepository : IRepository<FoodLog>
    {
        Task<IEnumerable<FoodLog>> GetByUserIdAsync(string userId);
        Task<IEnumerable<FoodLog>> GetByUserIdAndDateAsync(string userId, DateTime date);
        Task<decimal> GetTotalCaloriesByUserAndDateAsync(string userId, DateTime date);
    }
}
