using Interfaces;
using Metalama.Bits;

namespace Models;

[RepositoryAspect]
public class Repository<T> where T : IHasId
{
}