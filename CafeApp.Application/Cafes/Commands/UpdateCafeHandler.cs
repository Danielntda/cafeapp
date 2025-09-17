using CafeApp.Persistence;
using CafeApp.Application.Cafes.Dtos;
using CafeApp.Application.Common.Validators;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class UpdateCafeHandler : IRequestHandler<UpdateCafeCommand, bool>
{
    private readonly CafeDbContext _context;

    public UpdateCafeHandler(CafeDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateCafeCommand request, CancellationToken cancellationToken)
    {
        var cafe = await _context.Cafe.FirstOrDefaultAsync(c => c.Id == request.Cafe.Id, cancellationToken);

        if (cafe == null)
            throw new ArgumentException($"Cafe with ID {request.Cafe.Id} does not exist.");

        // --- Validation ---
        CafeValidator.ValidateRequired(request.Cafe.Name, "Name");
        CafeValidator.ValidateRequired(request.Cafe.Description, "Description");
        CafeValidator.ValidateRequired(request.Cafe.Location, "Location");

        // --- Update fields ---
        cafe.Name = request.Cafe.Name;
        cafe.Description = request.Cafe.Description;
        cafe.Logo = request.Cafe.Logo;
        cafe.Location = request.Cafe.Location;

        _context.Cafe.Update(cafe);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
