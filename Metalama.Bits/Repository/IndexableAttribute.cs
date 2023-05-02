using Models;

namespace Metalama.Bits.Repository;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class IndexableAttribute<T> : Attribute
    where T : IIdentifiable 
{
}