using MediatR;
using CafeApp.Application.Cafes.Dtos;

public class UpdateCafeCommand : IRequest<bool> // returns true if updated
{
    public UpdateCafeDto Cafe { get; set; } = null!;
}
