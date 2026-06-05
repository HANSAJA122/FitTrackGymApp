using FitTrackGymApp.ViewModels;

namespace FitTrackGymApp.Views;

public partial class PlansPage : ContentPage
{
    public PlansPage()
    {
        InitializeComponent();
        BindingContext = new PlansViewModel();
    }
}
