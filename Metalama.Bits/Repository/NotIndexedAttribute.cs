namespace Metalama.Bits.Repository;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public class NotIndexedAttribute : Attribute
{
}