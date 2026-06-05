using FitTrackGymApp.ViewModels;

namespace FitTrackGymApp.Views;

public partial class AddEditMemberPage : ContentPage
{
    public AddEditMemberPage(AddEditMemberViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
