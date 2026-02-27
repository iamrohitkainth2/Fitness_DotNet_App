using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using HealthManagement.Settings;
using Microsoft.Extensions.Options;

namespace HealthManagement.Services
{
    public class HealthInsightsService : IHealthInsightsService
    {
        private const string ApiVersion = "2024-08-01-preview";
        private readonly HttpClient _httpClient;
        private readonly AzureOpenAISettings _settings;

        public HealthInsightsService(HttpClient httpClient, IOptions<AzureOpenAISettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        public async Task<string> GenerateDailyInsightAsync(
            decimal dailyIntake,
            decimal dailyBurned,
            IEnumerable<decimal> weeklyIntake,
            IEnumerable<decimal> weeklyBurned,
            string dietaryGoal,
            CancellationToken cancellationToken = default)
        {
            if (!HasValidConfiguration())
            {
                return "Set Azure OpenAI settings to enable personalized insights.";
            }

            var weeklyIntakeAvg = weeklyIntake.Any() ? weeklyIntake.Average() : 0;
            var weeklyBurnedAvg = weeklyBurned.Any() ? weeklyBurned.Average() : 0;

            var userPrompt = $"""
            Generate concise, practical health guidance based on:
            - Daily calories intake: {dailyIntake}
            - Daily calories burned: {dailyBurned}
            - Weekly average intake: {weeklyIntakeAvg:F0}
            - Weekly average burned: {weeklyBurnedAvg:F0}
            - Dietary goal: {dietaryGoal}

            Return 3 short bullet points in plain text.
            """;

            var endpoint = _settings.Endpoint.TrimEnd('/');
            var url = $"{endpoint}/openai/deployments/{_settings.DeploymentName}/chat/completions?api-version={ApiVersion}";

            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("api-key", _settings.ApiKey);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var payload = new
            {
                messages = new object[]
                {
                    new { role = "system", content = "You are a fitness coach. Keep recommendations safe, concise, and actionable." },
                    new { role = "user", content = userPrompt }
                },
                temperature = 0.4
            };

            request.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            using var response = await _httpClient.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return "Unable to generate AI insight right now. Please try again later.";
            }

            var raw = await response.Content.ReadAsStringAsync(cancellationToken);
            var insight = ExtractAssistantContent(raw);

            return string.IsNullOrWhiteSpace(insight)
                ? "Unable to generate AI insight right now. Please try again later."
                : insight.Trim();
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
