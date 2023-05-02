namespace Models;

//public class Person : IIdentifiable
//{
//    public Person(string name)
//    {
//        Id = Guid.NewGuid();
//        Name = name;
//    }

//    public string Name { get; set; }

//    public Guid Id { get; init; }
//}

public record Person(string Name) : IIdentifiable
{
    public Guid Id { get; init; }
}