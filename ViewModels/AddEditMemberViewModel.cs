using System.Windows.Input;
using FitTrackGymApp.Models;
using FitTrackGymApp.Services;
using FitTrackGymApp.Helpers;

namespace FitTrackGymApp.ViewModels;

[QueryProperty(nameof(MemberToEdit), "MemberToEdit")]
public partial class AddEditMemberViewModel : BaseViewModel
{
    private readonly DatabaseService _databaseService;

    private Member? _memberToEdit;
    public Member? MemberToEdit
    {
        get => _memberToEdit;
        set
        {
            _memberToEdit = value;
            if (value != null)
            {
                Id = value.Id;
                FullName = value.FullName;
                PhoneNumber = value.PhoneNumber;
                Email = value.Email;
                Age = value.Age.ToString();
                Gender = value.Gender;
                JoinDate = value.JoinDate;
                Status = value.Status;
                Title = "Edit Member";
            }
        }
    }

    private int _id;
    private string _fullName = string.Empty;
    private string _phoneNumber = string.Empty;
    private string _email = string.Empty;
    private string _age = string.Empty;
    private string _gender = string.Empty;
    private DateTime _joinDate = DateTime.Today;
    private string _status = "Active";

    public int Id { get => _id; set { _id = value; OnPropertyChanged(); } }
    public string FullName { get => _fullName; set { _fullName = value; OnPropertyChanged(); } }
    public string PhoneNumber { get => _phoneNumber; set { _phoneNumber = value; OnPropertyChanged(); } }
    public string Email { get => _email; set { _email = value; OnPropertyChanged(); } }
    public string Age { get => _age; set { _age = value; OnPropertyChanged(); } }
    public string Gender { get => _gender; set { _gender = value; OnPropertyChanged(); } }
    public DateTime JoinDate { get => _joinDate; set { _joinDate = value; OnPropertyChanged(); } }
    public string Status { get => _status; set { _status = value; OnPropertyChanged(); } }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public AddEditMemberViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
        Title = "Add Member";

        SaveCommand = new Command(async () => await SaveAsync());
        CancelCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
    }

    private async Task SaveAsync()
    {
        if (IsBusy) return;
        IsBusy = true;

        try
        {
            // Validation
            if (!ValidationHelper.IsNotEmpty(FullName))
            {
                await Shell.Current.DisplayAlert("Validation Error", "Full Name cannot be empty.", "OK");
                return;
            }
            if (!ValidationHelper.IsNotEmpty(PhoneNumber))
            {
                await Shell.Current.DisplayAlert("Validation Error", "Phone Number cannot be empty.", "OK");
                return;
            }
            if (!ValidationHelper.IsValidEmail(Email))
            {
                await Shell.Current.DisplayAlert("Validation Error", "Please enter a valid email address.", "OK");
                return;
            }
            if (!int.TryParse(Age, out int parsedAge) || !ValidationHelper.IsValidAge(parsedAge))
            {
                await Shell.Current.DisplayAlert("Validation Error", "Age must be between 12 and 80.", "OK");
                return;
            }
            if (!ValidationHelper.IsNotEmpty(Gender))
            {
                await Shell.Current.DisplayAlert("Validation Error", "Gender cannot be empty.", "OK");
                return;
            }
            if (Status != "Active" && Status != "Expired")
            {
                await Shell.Current.DisplayAlert("Validation Error", "Status must be Active or Expired.", "OK");
                return;
            }

            // Save logic
            var member = new Member
            {
                Id = Id,
                FullName = FullName,
                PhoneNumber = PhoneNumber,
                Email = Email,
                Age = parsedAge,
                Gender = Gender,
                JoinDate = JoinDate,
                Status = Status
            };

            if (Id == 0)
            {
                await _databaseService.AddMemberAsync(member);
                await Shell.Current.DisplayAlert("Success", "Member added successfully.", "OK");
            }
            else
            {
                await _databaseService.UpdateMemberAsync(member);
                await Shell.Current.DisplayAlert("Success", "Member updated successfully.", "OK");
            }

            // Go back
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to save member: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
