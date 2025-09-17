using CafeApp.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class DeleteEmployeeHandler : IRequestHandler<DeleteEmployeeCommand, bool>
{
    private readonly CafeDbContext _context;

    public DeleteEmployeeHandler(CafeDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _context.Employee
            .FirstOrDefaultAsync(e => e.Id == request.EmployeeId, cancellationToken);

        if (employee == null)
            throw new ArgumentException($"Employee with ID {request.EmployeeId} does not exist.");

        _context.Employee.Remove(employee);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
