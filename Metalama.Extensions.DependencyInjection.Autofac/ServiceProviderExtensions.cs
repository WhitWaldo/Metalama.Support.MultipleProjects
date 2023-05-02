using System.Globalization;
using Autofac;

namespace Metalama.Extensions.DependencyInjection.Autofac
{
    public static class ServiceProviderExtensions
    {
        public static ILifetimeScope GetAutofacRoot(this IServiceProvider serviceProvider)
        {
            if (serviceProvider is not AutofacServiceProvider autofacServiceProvider)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                    "Unable to retrieve Autofac root lifetime scope from service provider of type {0}.", serviceProvider?.GetType()));
            }

            return autofacServiceProvider.LifetimeScope;
        }
    }
}
