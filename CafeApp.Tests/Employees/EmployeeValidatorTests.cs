using CafeApp.Application.Common.Validators;

namespace CafeApp.Tests.Employees
{
    public class EmployeeValidatorTests
    {
        #region ValidateRequired Tests

        [Theory]
        [InlineData("John Doe")]
        [InlineData("abc")]
        public void ValidateRequired_ValidValue_DoesNotThrow(string value)
        {
            // Should not throw
            EmployeeValidator.ValidateRequired(value, "Name");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ValidateRequired_InvalidValue_ThrowsArgumentException(string value)
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                EmployeeValidator.ValidateRequired(value, "Name"));
            Assert.Equal("Name is required.", ex.Message);
        }

        #endregion

        #region ValidateEmail Tests

        [Theory]
        [InlineData("test@example.com")]
        [InlineData("user.name@domain.co")]
        public void ValidateEmail_ValidEmail_DoesNotThrow(string email)
        {
            EmployeeValidator.ValidateEmail(email);
        }

        [Theory]
        [InlineData("user@domain")]
        [InlineData("user@")]
        [InlineData("user.com")]
        [InlineData("")]
        public void ValidateEmail_InvalidEmail_ThrowsArgumentException(string email)
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                EmployeeValidator.ValidateEmail(email));
            Assert.Equal("EmailAddress format is invalid.", ex.Message);
        }

        #endregion

        #region ValidatePhoneNumber Tests

        [Theory]
        [InlineData("91234567")]
        [InlineData("81234567")]
        public void ValidatePhoneNumber_ValidPhone_DoesNotThrow(string phone)
        {
            EmployeeValidator.ValidatePhoneNumber(phone);
        }

        [Theory]
        [InlineData("71234567")]
        [InlineData("12345678")]
        [InlineData("8123456")]
        [InlineData("")]
        public void ValidatePhoneNumber_InvalidPhone_ThrowsArgumentException(string phone)
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                EmployeeValidator.ValidatePhoneNumber(phone));
            Assert.Equal("PhoneNumber must start with 8 or 9 and be 8 digits.", ex.Message);
        }

        #endregion
    }
}
