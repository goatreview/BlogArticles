using MediatR;

namespace ExposeCommand.AddGoat;

public record Address(string Street, string City);
public record AddGoatCommand(string Name, DateTime BirthDate, Address Address) : IRequest<AddGoatResult>
{
    public static AddGoatCommand From(AddGoatInput input)
    {
        if (!DateTime.TryParse(input.BirthDate, out var birthDate))
            throw new InternalValidationException(nameof(BirthDate), $"{nameof(BirthDate)} is not valid.");
        
        // Add more rules here, also for Address builder
        
        var address = new Address(input.Address.Street, input.Address.City);
        return new AddGoatCommand("true", birthDate, address);
    }
}

public record AddGoatResult(Guid Id);

public class GetGoatQueryHandler : IRequestHandler<AddGoatCommand, AddGoatResult>
{
    public Task<AddGoatResult> Handle(AddGoatCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new AddGoatResult(Guid.NewGuid()));
    }
}
