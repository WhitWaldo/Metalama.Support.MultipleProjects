//using Metalama.Bits;
using Microsoft.Extensions.Logging;

namespace Models;

public sealed class Repository<T> where T : IIdentifiable
{
    public List<T> _entities = new();
    private readonly ILogger _logger;

    public Repository(ILogger<Repository<T>> logger)
    {
        _logger = logger;
    }

    public void Add(T data)
    {
        _entities.Add(data);
    }

    public void Remove(Guid id)
    {
        var remaining = _entities.Where(a => a.Id != id).ToList();
        _entities = remaining;
    }

    //Uncomment line 28 and comment out line 29, add the reference to `Metalama.Bits`, and uncomment the using statement and the logging will work for this class. Leave as-is and only `Runner` will show logging.
    //public void Update(Guid id, [SensitiveData]T data)
    public void Update(Guid id, T data)
    {
        for (var a = 0; a < _entities.Count; a++)
        {
            if (_entities[a].Id == id)
            {
                _entities[a] = data;
            }
        }
    }

    public List<T> List()
    {
        return _entities;
    }

    public T? Get(Guid id)
    {
        var entity = _entities.FirstOrDefault(a => a.Id == id);
        return entity;
    }
}