namespace CafeApp.Application.Common.Validators;

public static class CafeValidator
{
    public static void ValidateRequired(string? value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{fieldName} is required.");
    }
}
