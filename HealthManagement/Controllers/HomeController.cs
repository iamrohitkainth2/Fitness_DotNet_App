using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HealthManagement.Models;

namespace HealthManagement.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(int? statusCode = null)
    {
        var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        var path = HttpContext.Request.Path.Value;

        if (statusCode.HasValue)
        {
            _logger.LogWarning("HTTP {StatusCode} for path {Path}. RequestId: {RequestId}", statusCode.Value, path, requestId);
        }
        else
        {
            _logger.LogError("Unhandled exception for path {Path}. RequestId: {RequestId}", path, requestId);
        }

        return View(new ErrorViewModel
        {
            RequestId = requestId,
            StatusCode = statusCode,
            Path = path
        });
    }
}
