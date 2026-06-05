using FitTrackGymApp.Models;

namespace FitTrackGymApp.Services;

public class AiWorkoutService
{
    // IMPORTANT: Add your GenAI API Key here (e.g., OpenAI or Gemini) if you decide to use a real API.
    private const string ApiKey = "YOUR_AI_API_KEY_HERE";

    public async Task<AiWorkoutResponse> GenerateWorkoutPlanAsync(AiWorkoutRequest request)
    {
        try
        {
            // Simulate network delay for a real API call (e.g., 2 seconds)
            await Task.Delay(2000);

            // Here is where you would normally write code to call OpenAI/Gemini using HttpClient.
            // For example:
            // var response = await _httpClient.PostAsJsonAsync("https://api.openai.com/v1/completions", requestPayload);
            // var result = await response.Content.ReadFromJsonAsync<...>();

            // --- SIMULATED AI RESPONSE FOR TESTING ---
            // If the user hasn't provided a real API key, we generate a smart fallback string based on their inputs.
            string plan = BuildSimulatedPlan(request);

            return new AiWorkoutResponse
            {
                IsSuccess = true,
                SuggestionText = plan
            };
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
        string basePlan = $"AI Suggested Plan for {req.Age}yo {req.ExperienceLevel} aiming for {req.Goal}:\n\n";

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
