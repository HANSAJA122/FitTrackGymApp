namespace FitTrackGymApp.Models;

public class Member
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Gender { get; set; } = string.Empty;
    public DateTime JoinDate { get; set; }
    public int MembershipPlanId { get; set; }
    public string Status { get; set; } = "Active";
}
