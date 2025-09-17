using MediatR;
using CafeApp.Application.Employees.Dtos;

public class UpdateEmployeeCommand : IRequest<bool>
{
    public UpdateEmployeeDto Employee { get; set; } = null!;
}
