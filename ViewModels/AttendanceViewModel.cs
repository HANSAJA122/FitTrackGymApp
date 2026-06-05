using System.Collections.ObjectModel;
using System.Windows.Input;
using FitTrackGymApp.Models;
using FitTrackGymApp.Services;

namespace FitTrackGymApp.ViewModels;

public partial class AttendanceViewModel : BaseViewModel
{
    private readonly DatabaseService _databaseService;

    public ObservableCollection<Attendance> Attendances { get; set; } = new();
    public ObservableCollection<Member> Members { get; set; } = new();

    private string _searchText = string.Empty;
    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged();
            _ = PerformSearchAsync();
        }
    }

    private Member? _selectedMember;
    public Member? SelectedMember
    {
        get => _selectedMember;
        set { _selectedMember = value; OnPropertyChanged(); }
    }

    public ICommand CheckInCommand { get; }
    public ICommand DeleteAttendanceCommand { get; }

    public AttendanceViewModel(DatabaseService databaseService)
    {
        Title = "Attendance";
        _databaseService = databaseService;

        CheckInCommand = new Command(async () => await CheckInAsync());
        DeleteAttendanceCommand = new Command<Attendance>(async (a) => await DeleteAttendanceAsync(a));
    }

    public async Task LoadDataAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            var members = await _databaseService.GetMembersAsync();
            Members.Clear();
            foreach (var m in members) Members.Add(m);

            await PerformSearchAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to load data: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task PerformSearchAsync()
    {
        var results = await _databaseService.SearchAttendancesAsync(SearchText);
        Attendances.Clear();
        foreach (var a in results) Attendances.Add(a);
    }

    private async Task CheckInAsync()
    {
        if (SelectedMember == null)
        {
            await Shell.Current.DisplayAlert("Validation Error", "Please select a member to check in.", "OK");
            return;
        }

        bool hasCheckedIn = await _databaseService.HasCheckedInTodayAsync(SelectedMember.Id);
        if (hasCheckedIn)
        {
            await Shell.Current.DisplayAlert("Check-in Failed", $"{SelectedMember.FullName} has already checked in today.", "OK");
            return;
        }

        var attendance = new Attendance
        {
            MemberId = SelectedMember.Id,
            CheckInDateTime = DateTime.Now
        };

        await _databaseService.AddAttendanceAsync(attendance);
        await Shell.Current.DisplayAlert("Success", $"{SelectedMember.FullName} checked in successfully.", "OK");

        SelectedMember = null;
        await PerformSearchAsync();
    }

    private async Task DeleteAttendanceAsync(Attendance attendance)
    {
        if (attendance == null) return;

        bool confirm = await Shell.Current.DisplayAlert("Confirm", $"Delete attendance record for {attendance.Member?.FullName}?", "Yes", "No");
        if (confirm)
        {
            await _databaseService.DeleteAttendanceAsync(attendance);
            await Shell.Current.DisplayAlert("Success", "Record deleted.", "OK");
            await PerformSearchAsync();
        }
    }
}
