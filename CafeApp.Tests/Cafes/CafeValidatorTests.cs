using CafeApp.Application.Common.Validators;

public class CafeValidatorTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ValidateRequired_ShouldThrow_WhenValueIsNullOrEmpty(string? invalidValue)
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            CafeValidator.ValidateRequired(invalidValue, "TestField"));
        Assert.Contains("TestField is required", ex.Message);
    }

    [Fact]
    public void ValidateRequired_ShouldNotThrow_WhenValueIsValid()
    {
        var exception = Record.Exception(() =>
            CafeValidator.ValidateRequired("ValidValue", "TestField"));
        Assert.Null(exception);
    }
}
