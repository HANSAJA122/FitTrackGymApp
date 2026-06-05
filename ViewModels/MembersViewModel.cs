using System.Collections.ObjectModel;
using System.Windows.Input;
using FitTrackGymApp.Models;
using FitTrackGymApp.Services;
using FitTrackGymApp.Views;

namespace FitTrackGymApp.ViewModels;

public partial class MembersViewModel : BaseViewModel
{
    private readonly DatabaseService _databaseService;
    private ObservableCollection<Member> _members = new();
    private string _searchText = string.Empty;

    public ObservableCollection<Member> Members
    {
        get => _members;
        set { _members = value; OnPropertyChanged(); }
    }

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

    public ICommand AddMemberCommand { get; }
    public ICommand EditMemberCommand { get; }
    public ICommand DeleteMemberCommand { get; }

    public MembersViewModel(DatabaseService databaseService)
    {
        Title = "Members";
        _databaseService = databaseService;
        Members = new ObservableCollection<Member>();

        AddMemberCommand = new Command(async () => await GoToAddMemberAsync());
        EditMemberCommand = new Command<Member>(async (m) => await GoToEditMemberAsync(m));
        DeleteMemberCommand = new Command<Member>(async (m) => await DeleteMemberAsync(m));
    }

    public async Task LoadMembersAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            var members = await _databaseService.GetMembersAsync();
            Members.Clear();
            foreach (var m in members)
                Members.Add(m);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to load members: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task PerformSearchAsync()
    {
        var results = await _databaseService.SearchMembersAsync(SearchText);
        Members.Clear();
        foreach (var m in results)
            Members.Add(m);
    }

    private async Task GoToAddMemberAsync()
    {
        await Shell.Current.GoToAsync(nameof(AddEditMemberPage));
    }

    private async Task GoToEditMemberAsync(Member member)
    {
        if (member == null) return;
        var navigationParameter = new Dictionary<string, object>
        {
            { "MemberToEdit", member }
        };
        await Shell.Current.GoToAsync(nameof(AddEditMemberPage), navigationParameter);
    }

    private async Task DeleteMemberAsync(Member member)
    {
        if (member == null) return;
        
        bool confirm = await Shell.Current.DisplayAlert("Confirm", $"Are you sure you want to delete {member.FullName}?", "Yes", "No");
        if (confirm)
        {
            await _databaseService.DeleteMemberAsync(member);
            Members.Remove(member);
            await Shell.Current.DisplayAlert("Success", "Member deleted.", "OK");
        }
    }
}
