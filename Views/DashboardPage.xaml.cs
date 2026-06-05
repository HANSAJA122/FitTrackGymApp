using FitTrackGymApp.ViewModels;

namespace FitTrackGymApp.Views;

public partial class DashboardPage : ContentPage
{
    public DashboardPage()
    {
        InitializeComponent();
        BindingContext = new DashboardViewModel();
    }
}
