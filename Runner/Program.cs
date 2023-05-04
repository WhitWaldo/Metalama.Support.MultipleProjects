using Autofac;
using Autofac.Extensions.DependencyInjection;
using Metalama.Bits;
using Metalama.Framework.Aspects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Models;

[assembly:AspectOrder(typeof(InjectedLoggerAttribute), typeof(RepositoryAspect))]
namespace Runner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(opt => opt.AddConsole().SetMinimumLevel(LogLevel.Trace));

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(serviceCollection);

            containerBuilder.RegisterType<Repository<Person>>();
            containerBuilder.RegisterType<Worker>();
            var container = containerBuilder.Build();
            var serviceProvider = new AutofacServiceProvider(container);
            
            var worker = serviceProvider.GetRequiredService<Worker>();
            worker.DoStuff("test value");
        }
    }
}