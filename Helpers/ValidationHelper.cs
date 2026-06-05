using System.Text.RegularExpressions;

namespace FitTrackGymApp.Helpers;

public static class ValidationHelper
{
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        try
        {
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }

    public static bool IsValidAge(int age)
    {
        return age >= 12 && age <= 80;
    }

    public static bool IsNotEmpty(string value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }
}
