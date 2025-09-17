using CafeApp.Persistence;
using CafeApp.Application.Cafes.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetCafesHandler : IRequestHandler<GetCafesQuery, List<CafeDto>>
{
    private readonly CafeDbContext _context;

    public GetCafesHandler(CafeDbContext context)
    {
        _context = context;
    }

    public async Task<List<CafeDto>> Handle(GetCafesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Cafe
            .Include(c => c.Employees)
            .AsQueryable();

        // Filter if location is provided
        if (!string.IsNullOrWhiteSpace(request.Location))
        {
            query = query.Where(c => c.Location == request.Location);
        }

        // Project into DTO
        var cafes = await query
            .Select(c => new CafeDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Logo = c.Logo,
                Location = c.Location,
                Employees = c.Employees.Count()
            })
            .OrderByDescending(c => c.Employees)
            .ToListAsync(cancellationToken);

        return cafes;
    }
}
