using FitTrackGymApp.ViewModels;

namespace FitTrackGymApp.Views;

public partial class AiWorkoutPage : ContentPage
{
    public AiWorkoutPage()
    {
        InitializeComponent();
        BindingContext = new AiWorkoutViewModel();
    }
}
