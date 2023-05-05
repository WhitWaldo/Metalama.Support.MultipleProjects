namespace Interfaces;

public interface IDoodad
{
    Task<T> DoSomething<T, R>(string name)
        where T : IDictionary<Guid, R>
        where R : IHasId;
}