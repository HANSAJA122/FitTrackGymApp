using FitTrackGymApp.ViewModels;

namespace FitTrackGymApp.Views;

public partial class PlansPage : ContentPage
{
    public PlansPage(PlansViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is PlansViewModel vm)
        {
            await vm.LoadPlansAsync();
        }
    }
}
