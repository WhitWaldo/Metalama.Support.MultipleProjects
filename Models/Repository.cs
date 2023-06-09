﻿using Models.Attributes;

namespace Models;

public sealed class Repository<T> where T : IIdentifiable
{
    public List<T> _entities = new();

    public void Add(T data)
    {
        _entities.Add(data);
    }

    public void Remove(Guid id)
    {
        var remaining = _entities.Where(a => a.Id != id).ToList();
        _entities = remaining;
    }

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