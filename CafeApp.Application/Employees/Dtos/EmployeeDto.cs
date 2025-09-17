namespace CafeApp.Application.Employees.Dtos
{
    public class EmployeeDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string CafeName { get; set; } = null!;
        public DateOnly? StartDate { get; set; }
    }
}
