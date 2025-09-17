using CafeApp.Persistence;
using CafeApp.Domain.Entities;
using CafeApp.Application.Common.Validators;
using MediatR;

public class CreateCafeHandler : IRequestHandler<CreateCafeCommand, Guid>
{
    private readonly CafeDbContext _context;

    public CreateCafeHandler(CafeDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateCafeCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Cafe;

        //Validation
        CafeValidator.ValidateRequired(dto.Name, "Name");
        CafeValidator.ValidateRequired(dto.Description, "Description");
        CafeValidator.ValidateRequired(dto.Location, "Location");
        //
        var cafe = new Cafe
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            Logo = dto.Logo,
            Location = dto.Location
        };

        _context.Cafe.Add(cafe);
        await _context.SaveChangesAsync(cancellationToken);

        return cafe.Id;
    }
}
