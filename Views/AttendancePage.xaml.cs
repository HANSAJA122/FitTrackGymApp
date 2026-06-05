using FitTrackGymApp.ViewModels;

namespace FitTrackGymApp.Views;

public partial class AttendancePage : ContentPage
{
    public AttendancePage()
    {
        InitializeComponent();
        BindingContext = new AttendanceViewModel();
    }
}
