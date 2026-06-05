namespace FitTrackGymApp.Models;

public class Payment
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public Member? Member { get; set; } // Navigation property
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.Today;
    public DateTime NextDueDate { get; set; } = DateTime.Today.AddMonths(1);
    public string Status { get; set; } = "Pending";
}
