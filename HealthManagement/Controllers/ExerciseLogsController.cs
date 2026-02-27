using HealthManagement.Models;
using HealthManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HealthManagement.Controllers
{
    [Authorize]
    public class ExerciseLogsController : Controller
    {
        private readonly IExerciseLogService _exerciseLogService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ExerciseLogsController(IExerciseLogService exerciseLogService, UserManager<ApplicationUser> userManager)
        {
            _exerciseLogService = exerciseLogService;
            _userManager = userManager;
        }

        // GET: ExerciseLogs
        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, string? exerciseType)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Challenge();

            var logs = await _exerciseLogService.GetUserExerciseLogsAsync(user.Id);
            var filteredLogs = logs.AsQueryable();

            if (startDate.HasValue)
            {
                var start = startDate.Value.Date;
                filteredLogs = filteredLogs.Where(log => log.DateLogged.Date >= start);
            }

            if (endDate.HasValue)
            {
                var end = endDate.Value.Date.AddDays(1).AddTicks(-1);
                filteredLogs = filteredLogs.Where(log => log.DateLogged <= end);
            }

            if (!string.IsNullOrWhiteSpace(exerciseType))
            {
                filteredLogs = filteredLogs.Where(log => log.ExerciseName.Contains(exerciseType, StringComparison.OrdinalIgnoreCase));
            }

            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
            ViewBag.ExerciseType = exerciseType;

            return View(filteredLogs.ToList());
        }

        // GET: ExerciseLogs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ExerciseLogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExerciseLog model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Challenge();

            model.UserId = user.Id;
            model.DateLogged = DateTime.UtcNow;
            if (model.CaloriesBurned <= 0)
            {
                model.CaloriesBurned = _exerciseLogService.CalculateCaloriesBurned(user.Weight, model.DurationMinutes, model.MetValue ?? 1);
            }

            await _exerciseLogService.AddExerciseLogAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: ExerciseLogs/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var log = await _exerciseLogService.GetExerciseLogByIdAsync(id);
            if (log == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null || log.UserId != user.Id)
                return Forbid();

            return View(log);
        }

        // GET: ExerciseLogs/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var log = await _exerciseLogService.GetExerciseLogByIdAsync(id);
            if (log == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null || log.UserId != user.Id)
                return Forbid();

            return View(log);
        }

        // POST: ExerciseLogs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ExerciseLog model)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null || model.UserId != user.Id)
                return Forbid();

            if (model.CaloriesBurned <= 0)
            {
                model.CaloriesBurned = _exerciseLogService.CalculateCaloriesBurned(user.Weight, model.DurationMinutes, model.MetValue ?? 1);
            }

            await _exerciseLogService.UpdateExerciseLogAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: ExerciseLogs/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var log = await _exerciseLogService.GetExerciseLogByIdAsync(id);
            if (log == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null || log.UserId != user.Id)
                return Forbid();

            return View(log);
        }

        // POST: ExerciseLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _exerciseLogService.DeleteExerciseLogAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
