using HealthManagement.Data;
using HealthManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthManagement.Repositories
{
    public class FoodLogRepository : GenericRepository<FoodLog>, IFoodLogRepository
    {
        private readonly ApplicationDbContext _context;

        public FoodLogRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FoodLog>> GetByUserIdAsync(string userId)
        {
            return await _context.FoodLogs
                .Where(fl => fl.UserId == userId)
                .OrderByDescending(fl => fl.DateLogged)
                .ToListAsync();
        }

        public async Task<IEnumerable<FoodLog>> GetByUserIdAndDateAsync(string userId, DateTime date)
        {
            var startDate = date.Date;
            var endDate = startDate.AddDays(1);

            return await _context.FoodLogs
                .Where(fl => fl.UserId == userId && fl.DateLogged >= startDate && fl.DateLogged < endDate)
                .OrderByDescending(fl => fl.DateLogged)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalCaloriesByUserAndDateAsync(string userId, DateTime date)
        {
            var foodLogs = await GetByUserIdAndDateAsync(userId, date);
            return foodLogs.Sum(fl => fl.Calories);
        }
    }
}
