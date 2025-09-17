using CafeApp.Application.Cafes.Dtos;
using MediatR;

public class GetCafesQuery : IRequest<List<CafeDto>>
{
    public string? Location { get; set; }
}
