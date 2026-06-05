namespace FitTrackGymApp.Models;

public class Attendance
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public DateTime CheckInDateTime { get; set; }
}
