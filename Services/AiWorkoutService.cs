using System.Net.Http.Json;
using FitTrackGymApp.Models;

namespace FitTrackGymApp.Services;

public class AiWorkoutService
{
    // IMPORTANT: Get a FREE Gemini API Key from https://aistudio.google.com/
    // Paste it between the quotes below to enable REAL AI. 
    // If left blank, the app will automatically use the simulated fallback plan.
    private const string ApiKey = ""; 
    private const string GeminiApiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent";

    private readonly HttpClient _httpClient;

    public AiWorkoutService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<AiWorkoutResponse> GenerateWorkoutPlanAsync(AiWorkoutRequest request)
    {
        try
        {
            // 1. If no API key is provided, use the simulated fallback for viva testing
            if (string.IsNullOrWhiteSpace(ApiKey))
            {
                await Task.Delay(1500); // Simulate network delay
                return new AiWorkoutResponse
                {
                    IsSuccess = true,
                    SuggestionText = BuildSimulatedPlan(request)
                };
            }

            // 2. If an API key IS provided, make a REAL call to Google's Gemini AI
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                throw new Exception("No internet connection.");
            }

            // Create the prompt for the AI
            string prompt = $"Act as an expert personal trainer. Create a short, highly effective 1-week workout plan for a {request.Age} year old {request.ExperienceLevel} aiming for {request.Goal}. They can work out {request.DaysPerWeek} days a week. Keep it concise, motivational, and easy to read. Do not use markdown headers, just simple bullet points.";

            // Construct the exact JSON payload expected by Gemini
            var payload = new
            {
                contents = new[]
                {
                    new { parts = new[] { new { text = prompt } } }
                }
            };

            // Send the request
            string urlWithKey = $"{GeminiApiUrl}?key={ApiKey}";
            var response = await _httpClient.PostAsJsonAsync(urlWithKey, payload);

            if (response.IsSuccessStatusCode)
            {
                // Parse the Gemini JSON response
                var jsonResult = await response.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();
                
                // Extract the generated text from the nested JSON structure
                string generatedText = jsonResult
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text").GetString() ?? "Could not extract text.";

                return new AiWorkoutResponse
                {
                    IsSuccess = true,
                    SuggestionText = generatedText.Trim()
                };
            }
            else
            {
                string errorBody = await response.Content.ReadAsStringAsync();
                throw new Exception($"Gemini API failed: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            return new AiWorkoutResponse
            {
                IsSuccess = false,
                ErrorMessage = $"AI Service Error: {ex.Message}"
            };
        }
    }

    private string BuildSimulatedPlan(AiWorkoutRequest req)
    {
        string basePlan = $"[SIMULATED MODE - ADD API KEY FOR REAL AI]\n\nAI Suggested Plan for {req.Age}yo {req.ExperienceLevel} aiming for {req.Goal}:\n\n";

        if (req.Goal == "Weight Loss")
            basePlan += "• Focus on high intensity interval training (HIIT) and cardio.\n";
        else if (req.Goal == "Muscle Gain")
            basePlan += "• Focus on progressive overload, heavy compound lifts.\n";
        else
            basePlan += "• Mix of moderate cardio and full-body strength training.\n";

        basePlan += $"\nSchedule ({req.DaysPerWeek} days/week):\n";
        for (int i = 1; i <= req.DaysPerWeek; i++)
        {
            basePlan += $"Day {i}: {(i % 2 != 0 ? "Active Workout" : "Active Recovery / Lighter Day")}\n";
        }

        return basePlan + "\nRemember to stay hydrated and prioritize protein intake!";
    }
}
