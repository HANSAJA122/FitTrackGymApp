using FitTrackGymApp.ViewModels;

namespace FitTrackGymApp.Views;

public partial class AiWorkoutPage : ContentPage
{
    public AiWorkoutPage(AiWorkoutViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
