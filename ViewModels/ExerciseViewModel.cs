using System.Collections.ObjectModel;
using System.Windows.Input;
using FitTrackGymApp.Models;
using FitTrackGymApp.Services;

namespace FitTrackGymApp.ViewModels;

public partial class ExerciseViewModel : BaseViewModel
{
    private readonly ExerciseApiService _apiService;
    
    public ObservableCollection<Exercise> Exercises { get; set; } = new();

    private string _searchText = string.Empty;
    public string SearchText 
    { 
        get => _searchText; 
        set { _searchText = value; OnPropertyChanged(); } 
    }

    public ICommand SearchCommand { get; }

    public ExerciseViewModel(ExerciseApiService apiService)
    {
        Title = "API Exercises";
        _apiService = apiService;
        SearchCommand = new Command(async () => await SearchExercisesAsync());
    }

    private async Task SearchExercisesAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            await Shell.Current.DisplayAlert("Validation", "Please enter an exercise name or muscle to search.", "OK");
            return;
        }

        if (IsBusy) return;
        
        IsBusy = true; // Shows the loading indicator
        Exercises.Clear();

        try
        {
            // Call the API service
            var results = await _apiService.SearchExercisesAsync(SearchText);
            
            if (results.Count == 0)
            {
                await Shell.Current.DisplayAlert("No Results", "No exercises found for your search. Try another keyword.", "OK");
            }
            else
            {
                // Add the deserialized items to the UI collection
                foreach (var exercise in results)
                {
                    Exercises.Add(exercise);
                }
            }
        }
        catch (Exception ex)
        {
            // Display any API errors, internet errors, or missing key errors directly to the user
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false; // Hides the loading indicator
        }
    }
}
