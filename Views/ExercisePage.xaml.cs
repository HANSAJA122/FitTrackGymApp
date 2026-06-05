using FitTrackGymApp.ViewModels;

namespace FitTrackGymApp.Views;

public partial class ExercisePage : ContentPage
{
    public ExercisePage(ExerciseViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
