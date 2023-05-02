using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Models;

namespace Runner.DependencyInjection;

internal static class CompositionRoot
{
    public static IContainer Build()
    {
        var builder = new ContainerBuilder();

        builder.RegisterType<Repository<Person>>();
        builder.RegisterType<Worker>();

        var services = new ServiceCollection();

        services.AddLogging(opt => opt.SetMinimumLevel(LogLevel.Trace).AddConsole());

        builder.Populate(services);

        return builder.Build();
    }
}