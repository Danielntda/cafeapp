using MediatR;
using System;

public class DeleteCafeCommand : IRequest<bool>
{
    public Guid CafeId { get; set; }
}
