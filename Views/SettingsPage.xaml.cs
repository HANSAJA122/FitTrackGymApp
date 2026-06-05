using FitTrackGymApp.ViewModels;

namespace FitTrackGymApp.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
        BindingContext = new SettingsViewModel();
    }
}
