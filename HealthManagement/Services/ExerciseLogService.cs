using HealthManagement.Models;
using HealthManagement.Repositories;

namespace HealthManagement.Services
{
    public class ExerciseLogService : IExerciseLogService
    {
        private readonly IExerciseLogRepository _exerciseLogRepository;

        public ExerciseLogService(IExerciseLogRepository exerciseLogRepository)
        {
            _exerciseLogRepository = exerciseLogRepository;
        }

        public async Task<ExerciseLog?> GetExerciseLogByIdAsync(int id)
        {
            return await _exerciseLogRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<ExerciseLog>> GetUserExerciseLogsAsync(string userId)
        {
            return await _exerciseLogRepository.GetByUserIdAsync(userId);
        }

        public async Task<IEnumerable<ExerciseLog>> GetUserExerciseLogsByDateAsync(string userId, DateTime date)
        {
            return await _exerciseLogRepository.GetByUserIdAndDateAsync(userId, date);
        }

        public async Task<decimal> GetDailyCaloriesBurnedAsync(string userId, DateTime date)
        {
            return await _exerciseLogRepository.GetTotalCaloriesBurnedByUserAndDateAsync(userId, date);
        }

        public async Task AddExerciseLogAsync(ExerciseLog exerciseLog)
        {
            exerciseLog.DateLogged = DateTime.UtcNow;
            exerciseLog.CreatedAt = DateTime.UtcNow;
            await _exerciseLogRepository.AddAsync(exerciseLog);
            await _exerciseLogRepository.SaveChangesAsync();
        }

        public async Task UpdateExerciseLogAsync(ExerciseLog exerciseLog)
        {
            await _exerciseLogRepository.UpdateAsync(exerciseLog);
        }

        public async Task DeleteExerciseLogAsync(int id)
        {
            await _exerciseLogRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Calculate calories burned based on weight, duration, and MET value
        /// Formula: Calories = Weight (kg) * MET * Duration (hours)
        /// </summary>
        public decimal CalculateCaloriesBurned(decimal weight, int durationMinutes, decimal metValue)
        {
            var durationHours = durationMinutes / 60m;
            return weight * metValue * durationHours;
        }
    }
}
