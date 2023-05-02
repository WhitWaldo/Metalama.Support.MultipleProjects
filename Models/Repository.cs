using Metalama.Bits;
using Microsoft.Extensions.Logging;
using Models.Attributes;

namespace Models;

public sealed class Repository<T> where T : IIdentifiable
{
    private readonly ILogger _logger;

    public List<T> _entities = new();

    public Repository(ILogger<Repository<T>> logger)
    {
        _logger = logger;
    }


    [Log]
    public void Add(T data)
    {
        _entities.Add(data);
    }


    [Log]
    public void Remove(Guid id)
    {
        var remaining = _entities.Where(a => a.Id != id).ToList();
        _entities = remaining;
    }


    [Log]
    public void Update(Guid id, [SensitiveData]T data)
    {
        for (var a = 0; a < _entities.Count; a++)
        {
            if (_entities[a].Id == id)
            {
                _entities[a] = data;
            }
        }
    }


    [Log]
    public List<T> List()
    {
        return _entities;
    }


    [Log]
    public T? Get(Guid id)
    {
        var entity = _entities.FirstOrDefault(a => a.Id == id);
        return entity;
    }
}