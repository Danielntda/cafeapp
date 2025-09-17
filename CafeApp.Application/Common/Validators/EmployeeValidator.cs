using System.Text.RegularExpressions;

namespace CafeApp.Application.Common.Validators;
public static class EmployeeValidator
{
    public static void ValidateRequired(string? value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{fieldName} is required.");
    }

    public static void ValidateEmail(string email)
    {
        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ArgumentException("EmailAddress format is invalid.");
    }

    public static void ValidatePhoneNumber(string phone)
    {
        if (!Regex.IsMatch(phone, @"^[89]\d{7}$"))
            throw new ArgumentException("PhoneNumber must start with 8 or 9 and be 8 digits.");
    }
}
