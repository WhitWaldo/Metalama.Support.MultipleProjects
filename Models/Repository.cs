using Interfaces;
using Metalama.Bits;

namespace Models;

//[AugmentAspect]
public class Repository<T> where T : IHasId
{
    private readonly IDoodad _doodad;

    private Task<IDictionary<Guid, T>> _state => _doodad.DoSomething<IDictionary<Guid, T>, T>("myName");

    public Repository(IDoodad doodad)
    {
        _doodad = doodad;
    }
}