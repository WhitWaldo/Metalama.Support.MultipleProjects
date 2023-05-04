using Metalama.Extensions.DependencyInjection;
using Metalama.Extensions.DependencyInjection.DotNet.Tests.Advice.EarlyOptional;
using Metalama.Extensions.DependencyInjection.DotNet.Tests.Advice.EarlyOptional.Metalama.Extensions.DependencyInjection.Autofac.Tests.Aspect;
using Metalama.Framework.Aspects;
using System.Runtime.CompilerServices;

[assembly: AspectOrder(typeof(DependencyAttribute), typeof(MyAspect))]

namespace Metalama.Extensions.DependencyInjection.DotNet.Tests.Advice.EarlyOptional;

namespace Metalama.Extensions.DependencyInjection.Autofac.Tests.Aspect;

public class MyAspect : TypeAspect
{

}