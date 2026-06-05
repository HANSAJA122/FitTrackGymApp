using System.Collections.ObjectModel;
using System.Windows.Input;
using FitTrackGymApp.Models;
using FitTrackGymApp.Services;

namespace FitTrackGymApp.ViewModels;

public partial class PaymentsViewModel : BaseViewModel
{
    private readonly DatabaseService _databaseService;

    public ObservableCollection<Payment> Payments { get; set; } = new();
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

    private int _id;
    private Member? _selectedMember;
    private string _amount = string.Empty;
    private DateTime _paymentDate = DateTime.Today;
    private DateTime _nextDueDate = DateTime.Today.AddMonths(1);
    private string _status = "Pending";
    private string _formTitle = "Add New Payment";

    public int Id { get => _id; set { _id = value; OnPropertyChanged(); } }
    public Member? SelectedMember { get => _selectedMember; set { _selectedMember = value; OnPropertyChanged(); } }
    public string Amount { get => _amount; set { _amount = value; OnPropertyChanged(); } }
    public DateTime PaymentDate { get => _paymentDate; set { _paymentDate = value; OnPropertyChanged(); } }
    public DateTime NextDueDate { get => _nextDueDate; set { _nextDueDate = value; OnPropertyChanged(); } }
    public string Status { get => _status; set { _status = value; OnPropertyChanged(); } }
    public string FormTitle { get => _formTitle; set { _formTitle = value; OnPropertyChanged(); } }

    public ICommand SavePaymentCommand { get; }
    public ICommand EditPaymentCommand { get; }
    public ICommand DeletePaymentCommand { get; }
    public ICommand ClearFormCommand { get; }

    public PaymentsViewModel(DatabaseService databaseService)
    {
        Title = "Payments";
        _databaseService = databaseService;

        SavePaymentCommand = new Command(async () => await SavePaymentAsync());
        EditPaymentCommand = new Command<Payment>(EditPayment);
        DeletePaymentCommand = new Command<Payment>(async (p) => await DeletePaymentAsync(p));
        ClearFormCommand = new Command(ClearForm);
    }

    public async Task LoadDataAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            // Load members for the picker
            var members = await _databaseService.GetMembersAsync();
            Members.Clear();
            foreach (var m in members) Members.Add(m);

            // Load payments
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
        var results = await _databaseService.SearchPaymentsAsync(SearchText);
        Payments.Clear();
        foreach (var p in results) Payments.Add(p);
    }

    private async Task SavePaymentAsync()
    {
        if (SelectedMember == null)
        {
            await Shell.Current.DisplayAlert("Validation Error", "Please select a member.", "OK");
            return;
        }
        if (!decimal.TryParse(Amount, out decimal amount) || amount <= 0)
        {
            await Shell.Current.DisplayAlert("Validation Error", "Amount must be greater than 0.", "OK");
            return;
        }
        if (NextDueDate < PaymentDate)
        {
            await Shell.Current.DisplayAlert("Validation Error", "Next due date cannot be before the payment date.", "OK");
            return;
        }

        var payment = new Payment
        {
            Id = Id,
            MemberId = SelectedMember.Id,
            Amount = amount,
            PaymentDate = PaymentDate,
            NextDueDate = NextDueDate,
            Status = Status
        };

        if (Id == 0)
        {
            await _databaseService.AddPaymentAsync(payment);
            await Shell.Current.DisplayAlert("Success", "Payment added successfully.", "OK");
        }
        else
        {
            await _databaseService.UpdatePaymentAsync(payment);
            await Shell.Current.DisplayAlert("Success", "Payment updated successfully.", "OK");
        }

        ClearForm();
        await PerformSearchAsync();
    }

    private void EditPayment(Payment payment)
    {
        if (payment == null) return;
        Id = payment.Id;
        
        // Find matching member in the loaded Members collection
        SelectedMember = Members.FirstOrDefault(m => m.Id == payment.MemberId);
        
        Amount = payment.Amount.ToString();
        PaymentDate = payment.PaymentDate;
        NextDueDate = payment.NextDueDate;
        Status = payment.Status;
        FormTitle = "Edit Payment";
    }

    private async Task DeletePaymentAsync(Payment payment)
    {
        if (payment == null) return;
        
        bool confirm = await Shell.Current.DisplayAlert("Confirm", $"Delete payment of ${payment.Amount} for {payment.Member?.FullName}?", "Yes", "No");
        if (confirm)
        {
            await _databaseService.DeletePaymentAsync(payment);
            await Shell.Current.DisplayAlert("Success", "Payment deleted.", "OK");
            await PerformSearchAsync();
        }
    }

    private void ClearForm()
    {
        Id = 0;
        SelectedMember = null;
        Amount = string.Empty;
        PaymentDate = DateTime.Today;
        NextDueDate = DateTime.Today.AddMonths(1);
        Status = "Pending";
        FormTitle = "Add New Payment";
    }
}
