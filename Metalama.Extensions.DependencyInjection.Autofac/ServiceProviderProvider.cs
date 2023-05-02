using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Metalama.Extensions.DependencyInjection.Autofac;

public static class ServiceProviderProvider
{
    public static Func<IServiceProvider> ServiceProvider { get; set; } =
        //() => new AutofacServiceProviderFactory()
        () => new AutofacServiceProviderFactory().CreateBuilder();
}