using HealthManagement.Models;

namespace HealthManagement.Services
{
    public interface INutrientExtractionService
    {
        Task<NutritionEstimate?> ExtractFromTextAsync(string description, CancellationToken cancellationToken = default);
        Task<NutritionEstimate?> ExtractFromImageAsync(Stream imageStream, string contentType, CancellationToken cancellationToken = default);
    }
}
