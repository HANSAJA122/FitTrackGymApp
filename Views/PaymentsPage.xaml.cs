using FitTrackGymApp.ViewModels;

namespace FitTrackGymApp.Views;

public partial class PaymentsPage : ContentPage
{
    public PaymentsPage(PaymentsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is PaymentsViewModel vm)
        {
            await vm.LoadDataAsync();
        }
    }
}
