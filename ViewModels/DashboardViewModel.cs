using FitTrackGymApp.Services;

namespace FitTrackGymApp.ViewModels;

public partial class DashboardViewModel : BaseViewModel
{
    private readonly DatabaseService _databaseService;

    private int _totalMembers;
    public int TotalMembers { get => _totalMembers; set { _totalMembers = value; OnPropertyChanged(); } }

    private int _activeMembers;
    public int ActiveMembers { get => _activeMembers; set { _activeMembers = value; OnPropertyChanged(); } }

    private int _pendingPayments;
    public int PendingPayments { get => _pendingPayments; set { _pendingPayments = value; OnPropertyChanged(); } }

    public DashboardViewModel(DatabaseService databaseService)
    {
        Title = "Dashboard";
        _databaseService = databaseService;
    }

    public async Task LoadDashboardStatsAsync()
    {
        try
        {
            TotalMembers = await _databaseService.GetTotalMembersCountAsync();
            ActiveMembers = await _databaseService.GetActiveMembersCountAsync();
            PendingPayments = await _databaseService.GetPendingPaymentsCountAsync();
        }
        catch (Exception)
        {
            // Ignore errors on dashboard load for simplicity
        }
    }
}
