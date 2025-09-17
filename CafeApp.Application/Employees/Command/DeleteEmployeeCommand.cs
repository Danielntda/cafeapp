using MediatR;

public class DeleteEmployeeCommand : IRequest<bool>
{
    public string EmployeeId { get; set; } = null!; // UIXXXXXXX
}
