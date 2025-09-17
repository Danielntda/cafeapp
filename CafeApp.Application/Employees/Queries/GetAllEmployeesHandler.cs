using CafeApp.Persistence;
using CafeApp.Application.Employees.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetAllEmployeesHandler : IRequestHandler<GetAllEmployeesQuery, List<EmployeeDto>>
{
    private readonly CafeDbContext _context;

    public GetAllEmployeesHandler(CafeDbContext context)
    {
        _context = context;
    }

    public async Task<List<EmployeeDto>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Employee
            .Include(e => e.Cafe)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Cafe))
        {
            query = query.Where(e => e.Cafe != null && e.Cafe.Name == request.Cafe);
        }

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var employees = await query
            .Select(e => new EmployeeDto
            {
                Id = e.Id,
                Name = e.Name,
                EmailAddress = e.EmailAddress,
                PhoneNumber = e.PhoneNumber,
                Gender = e.Gender,
                CafeName = e.Cafe != null ? e.Cafe.Name : string.Empty,
                StartDate = e.StartDate.HasValue ? e.StartDate.Value : null
            })
            .OrderByDescending(e => e.StartDate.HasValue ? (today.DayNumber - e.StartDate.Value.DayNumber) : 0)
            .ToListAsync(cancellationToken);

        return employees;
    }
}
