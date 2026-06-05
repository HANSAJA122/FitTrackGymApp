using FitTrackGymApp.ViewModels;

namespace FitTrackGymApp.Views;

public partial class AddEditMemberPage : ContentPage
{
    public AddEditMemberPage()
    {
        InitializeComponent();
        BindingContext = new AddEditMemberViewModel();
    }
}
