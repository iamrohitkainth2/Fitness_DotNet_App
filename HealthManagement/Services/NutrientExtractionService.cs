using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using HealthManagement.Models;
using HealthManagement.Settings;
using Microsoft.Extensions.Options;

namespace HealthManagement.Services
{
    public class NutrientExtractionService : INutrientExtractionService
    {
        private const string ApiVersion = "2024-08-01-preview";
        private readonly HttpClient _httpClient;
        private readonly AzureOpenAISettings _settings;

        public NutrientExtractionService(HttpClient httpClient, IOptions<AzureOpenAISettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        public async Task<NutritionEstimate?> ExtractFromTextAsync(string description, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(description) || !HasValidConfiguration())
            {
                return null;
            }

            var userContent = $"Analyze this food description and estimate nutrition: {description}";
            return await RequestNutritionAsync(userContent, null, cancellationToken);
        }

        public async Task<NutritionEstimate?> ExtractFromImageAsync(Stream imageStream, string contentType, CancellationToken cancellationToken = default)
        {
            if (imageStream == null || imageStream.Length == 0 || !HasValidConfiguration())
            {
                return null;
            }

            using var memoryStream = new MemoryStream();
            await imageStream.CopyToAsync(memoryStream, cancellationToken);
            var base64 = Convert.ToBase64String(memoryStream.ToArray());
            var mediaType = string.IsNullOrWhiteSpace(contentType) ? "image/jpeg" : contentType;
            var dataUrl = $"data:{mediaType};base64,{base64}";

            return await RequestNutritionAsync(
                "Identify food items from this image and estimate nutrition.",
                dataUrl,
                cancellationToken);
        }

        private async Task<NutritionEstimate?> RequestNutritionAsync(string promptText, string? imageDataUrl, CancellationToken cancellationToken)
        {
            var endpoint = _settings.Endpoint.TrimEnd('/');
            var url = $"{endpoint}/openai/deployments/{_settings.DeploymentName}/chat/completions?api-version={ApiVersion}";

            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("api-key", _settings.ApiKey);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var systemPrompt = "You are a nutrition assistant. Return ONLY valid JSON with this schema: {\"foodName\":string,\"calories\":number,\"carbs\":number,\"protein\":number,\"fat\":number,\"micronutrients\":object}. Use micronutrient keys like iron, calcium, magnesium, potassium, zinc, vitaminA, vitaminB12, vitaminC, vitaminD.";

            object userMessage = imageDataUrl == null
                ? new { role = "user", content = promptText }
                : new
                {
                    role = "user",
                    content = new object[]
                    {
                        new { type = "text", text = promptText },
                        new { type = "image_url", image_url = new { url = imageDataUrl } }
                    }
                };

            var payload = new
            {
                messages = new object[]
                {
                    new { role = "system", content = systemPrompt },
                    userMessage
                },
                temperature = 0.2,
                response_format = new { type = "json_object" }
            };

            request.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            using var response = await _httpClient.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var raw = await response.Content.ReadAsStringAsync(cancellationToken);
            var content = ExtractAssistantContent(raw);
            if (string.IsNullOrWhiteSpace(content))
            {
                return null;
            }

            var estimate = JsonSerializer.Deserialize<NutritionEstimate>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return estimate;
        }

        private static string? ExtractAssistantContent(string completionJson)
        {
            using var document = JsonDocument.Parse(completionJson);
            var root = document.RootElement;

            if (!root.TryGetProperty("choices", out var choices) || choices.GetArrayLength() == 0)
            {
                return null;
            }

            var message = choices[0].GetProperty("message");
            if (!message.TryGetProperty("content", out var contentElement))
            {
                return null;
            }

            if (contentElement.ValueKind == JsonValueKind.String)
            {
                return contentElement.GetString();
            }

            if (contentElement.ValueKind == JsonValueKind.Array && contentElement.GetArrayLength() > 0)
            {
                var first = contentElement[0];
                if (first.TryGetProperty("text", out var textElement))
                {
                    return textElement.GetString();
                }
            }

            return null;
        }

        private bool HasValidConfiguration()
        {
            return !string.IsNullOrWhiteSpace(_settings.Endpoint)
                && !string.IsNullOrWhiteSpace(_settings.ApiKey)
                && !string.IsNullOrWhiteSpace(_settings.DeploymentName);
        }
    }
}
