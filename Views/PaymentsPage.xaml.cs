using FitTrackGymApp.ViewModels;

namespace FitTrackGymApp.Views;

public partial class PaymentsPage : ContentPage
{
    public PaymentsPage()
    {
        InitializeComponent();
        BindingContext = new PaymentsViewModel();
    }
}
