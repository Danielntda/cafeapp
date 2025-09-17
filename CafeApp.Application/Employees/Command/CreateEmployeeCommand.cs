using MediatR;
using CafeApp.Application.Employees.Dtos;

public class CreateEmployeeCommand : IRequest<string> // returns employee ID
{
    public CreateEmployeeDto Employee { get; set; } = null!;
}
