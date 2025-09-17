using CafeApp.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class DeleteCafeHandler : IRequestHandler<DeleteCafeCommand, bool>
{
    private readonly CafeDbContext _context;

    public DeleteCafeHandler(CafeDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteCafeCommand request, CancellationToken cancellationToken)
    {
        var cafe = await _context.Cafe
            .Include(c => c.Employees)
            .FirstOrDefaultAsync(c => c.Id == request.CafeId, cancellationToken);

        if (cafe == null)
            throw new ArgumentException($"Cafe with ID {request.CafeId} does not exist.");

        // Remove all employees under this café
        if (cafe.Employees != null && cafe.Employees.Any())
        {
            _context.Employee.RemoveRange(cafe.Employees);
        }

        _context.Cafe.Remove(cafe);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
