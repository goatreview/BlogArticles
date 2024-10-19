namespace ExposeCommand;

public record AddressInput(string? Street, string? City);
public record AddGoatInput(bool? Name, string? BirthDate, AddressInput? Address);

public class InternalValidationException(string field, string message) : Exception(message)
{
    public string Field { get; } = field;
}
