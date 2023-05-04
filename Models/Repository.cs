using Interfaces;
using Metalama.Bits.Repository;

namespace Models;

[RepositoryAspect]
public class Repository<T> where T : IHasId
{
}