namespace FitTrackGymApp.Models;

public class AiWorkoutRequest
{
    public string Goal { get; set; } = string.Empty;
    public int Age { get; set; }
    public string ExperienceLevel { get; set; } = string.Empty;
    public int DaysPerWeek { get; set; }
}
