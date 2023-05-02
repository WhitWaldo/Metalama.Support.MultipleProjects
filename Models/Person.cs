namespace Models;

public record Person(string Name) : IIdentifiable
{
    public Guid Id { get; init; } = Guid.NewGuid();
}