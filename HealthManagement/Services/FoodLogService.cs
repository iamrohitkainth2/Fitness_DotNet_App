using HealthManagement.Models;
using HealthManagement.Repositories;

namespace HealthManagement.Services
{
    public class FoodLogService : IFoodLogService
    {
        private readonly IFoodLogRepository _foodLogRepository;

        public FoodLogService(IFoodLogRepository foodLogRepository)
        {
            _foodLogRepository = foodLogRepository;
        }

        public async Task<FoodLog?> GetFoodLogByIdAsync(int id)
        {
            return await _foodLogRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<FoodLog>> GetUserFoodLogsAsync(string userId)
        {
            return await _foodLogRepository.GetByUserIdAsync(userId);
        }

        public async Task<IEnumerable<FoodLog>> GetUserFoodLogsByDateAsync(string userId, DateTime date)
        {
            return await _foodLogRepository.GetByUserIdAndDateAsync(userId, date);
        }

        public async Task<decimal> GetDailyCalorieTotalAsync(string userId, DateTime date)
        {
            return await _foodLogRepository.GetTotalCaloriesByUserAndDateAsync(userId, date);
        }

        public async Task AddFoodLogAsync(FoodLog foodLog)
        {
            foodLog.DateLogged = DateTime.UtcNow;
            foodLog.CreatedAt = DateTime.UtcNow;
            await _foodLogRepository.AddAsync(foodLog);
            await _foodLogRepository.SaveChangesAsync();
        }

        public async Task UpdateFoodLogAsync(FoodLog foodLog)
        {
            await _foodLogRepository.UpdateAsync(foodLog);
        }

        public async Task DeleteFoodLogAsync(int id)
        {
            await _foodLogRepository.DeleteAsync(id);
        }
    }
}
