//using Metalama.Extensions.DependencyInjection.Implementation;
//using Metalama.Framework.Aspects;
//using Metalama.Framework.Code;

//namespace Metalama.Extensions.DependencyInjection.Autofac;

//public class AutofacDependencyInjectionFramework : IDependencyInjectionFramework
//{
//    /// <summary>
//    /// Determines whether the current instance can handle a <see cref="T:Metalama.Extensions.DependencyInjection.DependencyAttribute" /> aspect or <see cref="T:Metalama.Extensions.DependencyInjection.IntroduceDependencyAttribute" /> advice.
//    /// The implementation can report diagnostics to <see cref="P:Metalama.Extensions.DependencyInjection.Implementation.DependencyContext.Diagnostics" />.
//    /// </summary>
//    /// <param name="context">A <see cref="T:Metalama.Extensions.DependencyInjection.Implementation.IntroduceDependencyContext" /> or <see cref="T:Metalama.Extensions.DependencyInjection.Implementation.ImplementDependencyContext" />.</param>
//    public bool CanHandleDependency(DependencyContext context)
//    {
//        throw new NotImplementedException();
//    }

//    /// <summary>
//    /// Processes the <see cref="T:Metalama.Extensions.DependencyInjection.IntroduceDependencyAttribute" /> advice, i.e. introduce a dependency defined by a custom aspect into the target
//    /// type of the aspect.
//    /// </summary>
//    /// <param name="context">Information regarding the dependency to inject.</param>
//    /// <param name="aspectBuilder">An <see cref="T:Metalama.Framework.Aspects.IAspectBuilder`1" /> for the target type.</param>
//    public void IntroduceDependency(IntroduceDependencyContext context, IAspectBuilder<INamedType> aspectBuilder)
//    {
//        throw new NotImplementedException();
//    }

//    /// <summary>
//    /// Processes the <see cref="T:Metalama.Extensions.DependencyInjection.DependencyAttribute" /> aspect, i.e. changes the target field or property of the aspect into a dependency.
//    /// </summary>
//    /// <param name="context">Information regarding the dependency to inject.</param>
//    /// <param name="aspectBuilder">The <see cref="T:Metalama.Framework.Aspects.IAspectBuilder`1" /> for the field or property to pull.</param>
//    public void ImplementDependency(ImplementDependencyContext context, IAspectBuilder<IFieldOrProperty> aspectBuilder)
//    {
//        throw new NotImplementedException();
//    }
//}