using MediatR;
using CafeApp.Application.Cafes.Dtos;

public class CreateCafeCommand : IRequest<Guid>
{
    public CreateCafeDto Cafe { get; set; } = null!;
}
