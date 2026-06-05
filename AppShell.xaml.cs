using FitTrackGymApp.Views;

namespace FitTrackGymApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        Routing.RegisterRoute(nameof(AddEditMemberPage), typeof(AddEditMemberPage));
    }
}
