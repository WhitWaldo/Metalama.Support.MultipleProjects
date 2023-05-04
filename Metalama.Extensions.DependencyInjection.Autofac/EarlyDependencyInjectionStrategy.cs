using Metalama.Extensions.DependencyInjection.Implementation;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code;

namespace Metalama.Extensions.DependencyInjection.Autofac;

internal class EarlyDependencyInjectionStrategy : DefaultDependencyInjectionStrategy, ITemplateProvider
{
    public EarlyDependencyInjectionStrategy(DependencyContext context) : base(context) {}

    /// <summary>Pulls the dependency from a given constructor.</summary>
    protected override void PullDependency(IAspectBuilder<INamedType> aspectBuilder, IPullStrategy pullStrategy, IConstructor constructor)
    {
        aspectBuilder.Advice.WithTemplateProvider(this)
            .AddInitializer(
                constructor,
                nameof(this.InitializerTemplate),
                args: new {T = this.Context.FieldOrProperty.Type, fieldOrProperty = this.Context.FieldOrProperty});
    }


    [Template]
    public void InitializerTemplate<[CompileTime] T>(IFieldOrProperty fieldOrProperty)
    {
        var isRequired = this.Context.DependencyAttribute.GetIsRequired()
            .GetValueOrDefault(this.Context.Project.DependencyInjectionOptions().IsRequiredByDefault);

        if (isRequired)
        {
            fieldOrProperty.Value = ServiceProviderProvider.ServiceProvider().GetService(typeof(T))
                                    ?? throw new InvalidOperationException(
                                        $"The service '{fieldOrProperty.Type}' could not be obtained from the service locator.");
        }
        else
        {
            fieldOrProperty.Value = (T) ServiceProviderProvider.ServiceProvider().GetService(typeof(T));
        }
    }
}