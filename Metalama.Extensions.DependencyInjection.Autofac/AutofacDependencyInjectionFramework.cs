using Metalama.Extensions.DependencyInjection.Implementation;
using Metalama.Framework.Aspects;

namespace Metalama.Extensions.DependencyInjection.Autofac
{
    [CompileTime]
    public class AutofacDependencyInjectionFramework : DefaultDependencyInjectionFramework
    {
        protected override DefaultDependencyInjectionStrategy GetStrategy(DependencyContext context) =>
            new EarlyDependencyInjectionStrategy(context);
    }
}