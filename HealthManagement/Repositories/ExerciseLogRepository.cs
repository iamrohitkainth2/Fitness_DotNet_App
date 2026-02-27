using HealthManagement.Data;
using HealthManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthManagement.Repositories
{
    public class ExerciseLogRepository : GenericRepository<ExerciseLog>, IExerciseLogRepository
    {
        private readonly ApplicationDbContext _context;

        public ExerciseLogRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ExerciseLog>> GetByUserIdAsync(string userId)
        {
            return await _context.ExerciseLogs
                .Where(el => el.UserId == userId)
                .OrderByDescending(el => el.DateLogged)
                .ToListAsync();
        }

        public async Task<IEnumerable<ExerciseLog>> GetByUserIdAndDateAsync(string userId, DateTime date)
        {
            var startDate = date.Date;
            var endDate = startDate.AddDays(1);

            return await _context.ExerciseLogs
                .Where(el => el.UserId == userId && el.DateLogged >= startDate && el.DateLogged < endDate)
                .OrderByDescending(el => el.DateLogged)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalCaloriesBurnedByUserAndDateAsync(string userId, DateTime date)
        {
            var exerciseLogs = await GetByUserIdAndDateAsync(userId, date);
            return exerciseLogs.Sum(el => el.CaloriesBurned);
        }
    }
}
