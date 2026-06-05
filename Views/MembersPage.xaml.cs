using FitTrackGymApp.ViewModels;

namespace FitTrackGymApp.Views;

public partial class MembersPage : ContentPage
{
    public MembersPage()
    {
        InitializeComponent();
        BindingContext = new MembersViewModel();
    }
}
