namespace FitTrackGymApp.Models;

public class MembershipPlan
{
    public int Id { get; set; }
    public string PlanName { get; set; } = string.Empty;
    public int DurationInMonths { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
}
