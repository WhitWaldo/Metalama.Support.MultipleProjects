using Autofac;
using Runner.DependencyInjection;

namespace Runner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var compositionRoot = CompositionRoot.Build();
            var worker = compositionRoot.Resolve<Worker>();

            worker.DoStuff();
        }
    }
}