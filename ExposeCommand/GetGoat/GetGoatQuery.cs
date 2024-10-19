using MediatR;

namespace ExposeCommand.GetGoat;

public record GetGoatQuery(Guid Id) : IRequest<GetGoatResult>;
public record GetGoatResult(Guid Id, string Name, string? Description);

public class GetGoatQueryHandler : IRequestHandler<GetGoatQuery, GetGoatResult>
{
    public Task<GetGoatResult> Handle(GetGoatQuery request, CancellationToken cancellationToken)
    {
        // The goal here is not to implement a real 
        return Task.FromResult(new GetGoatResult(request.Id, "Goat", "Goat's description"));
    }
}