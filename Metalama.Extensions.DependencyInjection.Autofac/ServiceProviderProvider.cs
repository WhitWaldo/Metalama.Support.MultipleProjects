using Autofac;

namespace Metalama.Extensions.DependencyInjection.Autofac;

public static class ServiceProviderProvider
{
    public static Func<IServiceProvider> ServiceProvider { get; set; } = () =>
    {
        var builder = new ContainerBuilder();
        var container = builder.Build();

        using var scope = container.BeginLifetimeScope();
        return new AutofacServiceProvider(scope);
    };
}