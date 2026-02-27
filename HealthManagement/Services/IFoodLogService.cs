using HealthManagement.Models;

namespace HealthManagement.Services
{
    public interface IFoodLogService
    {
        Task<FoodLog?> GetFoodLogByIdAsync(int id);
        Task<IEnumerable<FoodLog>> GetUserFoodLogsAsync(string userId);
        Task<IEnumerable<FoodLog>> GetUserFoodLogsByDateAsync(string userId, DateTime date);
        Task<decimal> GetDailyCalorieTotalAsync(string userId, DateTime date);
        Task AddFoodLogAsync(FoodLog foodLog);
        Task UpdateFoodLogAsync(FoodLog foodLog);
        Task DeleteFoodLogAsync(int id);
    }
}
