using MediatR;
using CafeApp.Application.Employees.Dtos;

public class GetAllEmployeesQuery : IRequest<List<EmployeeDto>>
{
    public string? Cafe { get; set; } // optional filter by café name
}
