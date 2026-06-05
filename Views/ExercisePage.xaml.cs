using FitTrackGymApp.ViewModels;

namespace FitTrackGymApp.Views;

public partial class ExercisePage : ContentPage
{
    public ExercisePage()
    {
        InitializeComponent();
        BindingContext = new ExerciseViewModel();
    }
}
