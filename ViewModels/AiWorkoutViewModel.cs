using System.Collections.ObjectModel;
using System.Windows.Input;
using FitTrackGymApp.Models;
using FitTrackGymApp.Services;

namespace FitTrackGymApp.ViewModels;

public partial class AiWorkoutViewModel : BaseViewModel
{
    private readonly AiWorkoutService _aiService;

    // Pickers data
    public ObservableCollection<string> Goals { get; } = new() { "Weight Loss", "Muscle Gain", "General Fitness" };
    public ObservableCollection<string> ExperienceLevels { get; } = new() { "Beginner", "Intermediate", "Advanced" };
    public ObservableCollection<int> DaysOptions { get; } = new() { 1, 2, 3, 4, 5, 6, 7 };

    // Form properties
    private string _selectedGoal = string.Empty;
    public string SelectedGoal { get => _selectedGoal; set { _selectedGoal = value; OnPropertyChanged(); } }

    private string _age = string.Empty;
    public string Age { get => _age; set { _age = value; OnPropertyChanged(); } }

    private string _selectedExperience = string.Empty;
    public string SelectedExperience { get => _selectedExperience; set { _selectedExperience = value; OnPropertyChanged(); } }

    private int _selectedDays = 3; // Default
    public int SelectedDays { get => _selectedDays; set { _selectedDays = value; OnPropertyChanged(); } }

    // Result properties
    private string _generatedPlan = string.Empty;
    public string GeneratedPlan { get => _generatedPlan; set { _generatedPlan = value; OnPropertyChanged(); } }

    private bool _hasResult;
    public bool HasResult { get => _hasResult; set { _hasResult = value; OnPropertyChanged(); } }

    public ICommand GenerateCommand { get; }

    public AiWorkoutViewModel(AiWorkoutService aiService)
    {
        Title = "AI Coach";
        _aiService = aiService;
        GenerateCommand = new Command(async () => await GenerateWorkoutAsync());
    }

    private async Task GenerateWorkoutAsync()
    {
        // 1. Validation
        if (string.IsNullOrWhiteSpace(SelectedGoal))
        {
            await Shell.Current.DisplayAlert("Validation", "Please select a fitness goal.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(SelectedExperience))
        {
            await Shell.Current.DisplayAlert("Validation", "Please select your experience level.", "OK");
            return;
        }

        if (!int.TryParse(Age, out int ageInt) || ageInt < 12 || ageInt > 80)
        {
            await Shell.Current.DisplayAlert("Validation", "Please enter a valid age between 12 and 80.", "OK");
            return;
        }

        if (SelectedDays < 1 || SelectedDays > 7)
        {
            await Shell.Current.DisplayAlert("Validation", "Please select a valid number of days (1-7).", "OK");
            return;
        }

        // 2. Start Loading State
        if (IsBusy) return;
        IsBusy = true;
        HasResult = false;
        GeneratedPlan = string.Empty;

        try
        {
            // 3. Create Request Object
            var request = new AiWorkoutRequest
            {
                Goal = SelectedGoal,
                Age = ageInt,
                ExperienceLevel = SelectedExperience,
                DaysPerWeek = SelectedDays
            };

            // 4. Call Service
            var response = await _aiService.GenerateWorkoutPlanAsync(request);

            if (response.IsSuccess)
            {
                GeneratedPlan = response.SuggestionText;
                HasResult = true;
            }
            else
            {
                await Shell.Current.DisplayAlert("AI Error", response.ErrorMessage, "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"An unexpected error occurred: {ex.Message}", "OK");
        }
        finally
        {
            // 5. End Loading State
            IsBusy = false;
        }
    }
}
