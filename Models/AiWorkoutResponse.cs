namespace FitTrackGymApp.Models;

public class AiWorkoutResponse
{
    public string SuggestionText { get; set; } = string.Empty;
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
}
