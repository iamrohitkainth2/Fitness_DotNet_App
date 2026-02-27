using HealthManagement.Models;
using HealthManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HealthManagement.Controllers
{
    [Authorize]
    public class FoodLogsController : Controller
    {
        private readonly IFoodLogService _foodLogService;
        private readonly INutrientExtractionService _nutrientExtractionService;
        private readonly UserManager<ApplicationUser> _userManager;

        public FoodLogsController(
            IFoodLogService foodLogService,
            INutrientExtractionService nutrientExtractionService,
            UserManager<ApplicationUser> userManager)
        {
            _foodLogService = foodLogService;
            _nutrientExtractionService = nutrientExtractionService;
            _userManager = userManager;
        }

        // GET: FoodLogs
        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, string? foodCategory)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Challenge();

            var logs = await _foodLogService.GetUserFoodLogsAsync(user.Id);
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

            if (!string.IsNullOrWhiteSpace(foodCategory))
            {
                filteredLogs = filteredLogs.Where(log => log.FoodName.Contains(foodCategory, StringComparison.OrdinalIgnoreCase));
            }

            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
            ViewBag.FoodCategory = foodCategory;

            return View(filteredLogs.ToList());
        }

        // GET: FoodLogs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FoodLogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FoodLog model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Challenge();

            model.UserId = user.Id;
            model.DateLogged = DateTime.UtcNow;
            await _foodLogService.AddFoodLogAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AnalyzeText(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                return BadRequest(new { message = "Description is required." });
            }

            var estimate = await _nutrientExtractionService.ExtractFromTextAsync(description);
            if (estimate == null)
            {
                return StatusCode(502, new { message = "Failed to analyze text with Azure OpenAI. Check AzureOpenAI settings." });
            }

            return Json(estimate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AnalyzeImage(IFormFile mealImage)
        {
            if (mealImage == null || mealImage.Length == 0)
            {
                return BadRequest(new { message = "Image is required." });
            }

            if (!mealImage.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new { message = "Only image files are allowed." });
            }

            await using var stream = mealImage.OpenReadStream();
            var estimate = await _nutrientExtractionService.ExtractFromImageAsync(stream, mealImage.ContentType);
            if (estimate == null)
            {
                return StatusCode(502, new { message = "Failed to analyze image with Azure OpenAI. Check AzureOpenAI settings." });
            }

            return Json(estimate);
        }

        // GET: FoodLogs/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var log = await _foodLogService.GetFoodLogByIdAsync(id);
            if (log == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null || log.UserId != user.Id)
                return Forbid();

            return View(log);
        }

        // GET: FoodLogs/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var log = await _foodLogService.GetFoodLogByIdAsync(id);
            if (log == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null || log.UserId != user.Id)
                return Forbid();

            return View(log);
        }

        // POST: FoodLogs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FoodLog model)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null || model.UserId != user.Id)
                return Forbid();

            await _foodLogService.UpdateFoodLogAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: FoodLogs/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var log = await _foodLogService.GetFoodLogByIdAsync(id);
            if (log == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null || log.UserId != user.Id)
                return Forbid();

            return View(log);
        }

        // POST: FoodLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _foodLogService.DeleteFoodLogAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
