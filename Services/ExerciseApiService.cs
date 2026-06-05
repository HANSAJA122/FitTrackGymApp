using System.Net.Http.Json;
using FitTrackGymApp.Models;

namespace FitTrackGymApp.Services;

public class ExerciseApiService
{
    private readonly HttpClient _httpClient;
    
    // IMPORTANT: Add your free API key from https://api-ninjas.com/profile
    // Get a free key and paste it between the quotes below to test the API.
    private const string ApiKey = ""; 
    private const string BaseUrl = "https://api.api-ninjas.com/v1/exercises";

    public ExerciseApiService()
    {
        _httpClient = new HttpClient();
        
        // Add the API key to the request headers securely
        if (!string.IsNullOrWhiteSpace(ApiKey))
        {
            _httpClient.DefaultRequestHeaders.Add("X-Api-Key", ApiKey);
        }
    }

    public async Task<List<Exercise>> SearchExercisesAsync(string query)
    {
        try
        {
            // 1. Check for internet connection first using MAUI Essentials
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                throw new Exception("No internet connection. Please check your network and try again.");
            }

            if (string.IsNullOrWhiteSpace(ApiKey))
            {
                throw new Exception("API Key is missing! Please add your free API-Ninjas key in ExerciseApiService.cs");
            }

            // 2. Construct the URL. We search by name or muscle depending on what user types.
            string url = $"{BaseUrl}?name={Uri.EscapeDataString(query)}";
            
            // 3. Send the asynchronous GET request
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                // 4. Read the JSON and deserialize it directly into our C# List of Exercise objects
                var exercises = await response.Content.ReadFromJsonAsync<List<Exercise>>();
                return exercises ?? new List<Exercise>();
            }
            else
            {
                // If API Ninjas rejects the request (e.g. wrong key), read the error
                throw new Exception($"API failed with status code: {response.StatusCode}. Make sure your API key is correct.");
            }
        }
        catch (Exception ex)
        {
            // Rethrow so the ViewModel can show it to the user in a pop-up
            throw new Exception(ex.Message);
        }
    }
}
