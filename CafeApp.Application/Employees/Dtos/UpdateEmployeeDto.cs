namespace CafeApp.Application.Employees.Dtos
{
    public class UpdateEmployeeDto
    {
        public string Id { get; set; } = null!;       // UIXXXXXXX
        public string Name { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string CafeName { get; set; } = null!;   // optional cafe assignment
        public DateOnly? StartDate { get; set; }       // optional start date
    }
}
