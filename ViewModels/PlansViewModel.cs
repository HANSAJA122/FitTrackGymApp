using System.Collections.ObjectModel;
using System.Windows.Input;
using FitTrackGymApp.Models;
using FitTrackGymApp.Services;

namespace FitTrackGymApp.ViewModels;

public partial class PlansViewModel : BaseViewModel
{
    private readonly DatabaseService _databaseService;

    public ObservableCollection<MembershipPlan> Plans { get; set; } = new();

    private int _id;
    private string _planName = string.Empty;
    private string _durationInMonths = string.Empty;
    private string _price = string.Empty;
    private string _description = string.Empty;
    private string _formTitle = "Add New Plan";

    public int Id { get => _id; set { _id = value; OnPropertyChanged(); } }
    public string PlanName { get => _planName; set { _planName = value; OnPropertyChanged(); } }
    public string DurationInMonths { get => _durationInMonths; set { _durationInMonths = value; OnPropertyChanged(); } }
    public string Price { get => _price; set { _price = value; OnPropertyChanged(); } }
    public string Description { get => _description; set { _description = value; OnPropertyChanged(); } }
    public string FormTitle { get => _formTitle; set { _formTitle = value; OnPropertyChanged(); } }

    public ICommand SavePlanCommand { get; }
    public ICommand EditPlanCommand { get; }
    public ICommand DeletePlanCommand { get; }
    public ICommand ClearFormCommand { get; }

    public PlansViewModel(DatabaseService databaseService)
    {
        Title = "Membership Plans";
        _databaseService = databaseService;

        SavePlanCommand = new Command(async () => await SavePlanAsync());
        EditPlanCommand = new Command<MembershipPlan>(EditPlan);
        DeletePlanCommand = new Command<MembershipPlan>(async (p) => await DeletePlanAsync(p));
        ClearFormCommand = new Command(ClearForm);
    }

    public async Task LoadPlansAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            var plans = await _databaseService.GetPlansAsync();
            Plans.Clear();
            foreach (var p in plans)
                Plans.Add(p);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to load plans: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task SavePlanAsync()
    {
        if (string.IsNullOrWhiteSpace(PlanName))
        {
            await Shell.Current.DisplayAlert("Validation Error", "Plan Name cannot be empty.", "OK");
            return;
        }
        if (!int.TryParse(DurationInMonths, out int duration) || duration <= 0)
        {
            await Shell.Current.DisplayAlert("Validation Error", "Duration must be greater than 0.", "OK");
            return;
        }
        if (!decimal.TryParse(Price, out decimal price) || price <= 0)
        {
            await Shell.Current.DisplayAlert("Validation Error", "Price must be greater than 0.", "OK");
            return;
        }

        var plan = new MembershipPlan
        {
            Id = Id,
            PlanName = PlanName,
            DurationInMonths = duration,
            Price = price,
            Description = Description ?? string.Empty
        };

        if (Id == 0)
        {
            await _databaseService.AddMembershipPlanAsync(plan);
            await Shell.Current.DisplayAlert("Success", "Plan added successfully.", "OK");
        }
        else
        {
            await _databaseService.UpdateMembershipPlanAsync(plan);
            await Shell.Current.DisplayAlert("Success", "Plan updated successfully.", "OK");
        }

        ClearForm();
        await LoadPlansAsync();
    }

    private void EditPlan(MembershipPlan plan)
    {
        if (plan == null) return;
        Id = plan.Id;
        PlanName = plan.PlanName;
        DurationInMonths = plan.DurationInMonths.ToString();
        Price = plan.Price.ToString();
        Description = plan.Description;
        FormTitle = "Edit Plan";
    }

    private async Task DeletePlanAsync(MembershipPlan plan)
    {
        if (plan == null) return;
        
        bool confirm = await Shell.Current.DisplayAlert("Confirm", $"Are you sure you want to delete the {plan.PlanName} plan?", "Yes", "No");
        if (confirm)
        {
            await _databaseService.DeleteMembershipPlanAsync(plan);
            await Shell.Current.DisplayAlert("Success", "Plan deleted.", "OK");
            await LoadPlansAsync();
        }
    }

    private void ClearForm()
    {
        Id = 0;
        PlanName = string.Empty;
        DurationInMonths = string.Empty;
        Price = string.Empty;
        Description = string.Empty;
        FormTitle = "Add New Plan";
    }
}
