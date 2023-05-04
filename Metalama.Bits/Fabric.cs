//using Metalama.Framework.Code;
//using Metalama.Framework.Fabrics;

//namespace Metalama.Bits;

//internal class Fabric : TransitiveProjectFabric
//{
//    public override void AmendProject(IProjectAmender amender) =>
//        amender.Outbound
//            .SelectMany(compilation => compilation.AllTypes)
//            .Where(type => (type.Accessibility is Accessibility.Public or Accessibility.Internal) && type.Name != nameof(LoggingRecursionGuard.DisposeCookie) && type.Name != nameof(LoggingRecursionGuard))
//            .SelectMany(type => type.Methods)
//            .Where(method => method.Accessibility == Accessibility.Public && method.Name != "ToString")
//            .AddAspectIfEligible<LogAttribute>();
//}