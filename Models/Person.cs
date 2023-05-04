using Interfaces;

namespace Models;

public class Person : IHasId
{
    public Person(string name)
    {
        Name = name;
    }

    public Guid Id { get; init; }

    public string Name { get; init; }
}