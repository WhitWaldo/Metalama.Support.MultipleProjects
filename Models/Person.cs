using Interfaces;

namespace Models;

//public record Person(string Name) : IIdentifiable
//{
//    public Guid Id { get; init; } = Guid.NewGuid();
//}

public class Person : IHasId
{
    public Person(string name)
    {
        Name = name;
    }

    public Guid Id { get; init; }

    public string Name { get; init; }
}