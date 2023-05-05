using Metalama.Framework.Aspects;
using Metalama.Framework.Code;

namespace Metalama.Bits;

public class AugmentAspect : TypeAspect
{
    /// <inheritdoc />
    public override void BuildAspect(IAspectBuilder<INamedType> builder)
    {
        var genericType = builder.Target.TypeParameters[0];

        var dictionaryType =
            ((INamedType) TypeFactory.GetType(typeof(IDictionary<,>))).WithTypeArguments(typeof(Guid), genericType.ToType());
        var baseStateType =
            ((INamedType) TypeFactory.GetType(typeof(Task<>))).WithTypeArguments(dictionaryType);

        var myField = builder.Advice.IntroduceField(builder.Target,
            "doodad",
            IntroductionScope.Instance,
            OverrideStrategy.Ignore,
            fieldBuilder =>
            {
                fieldBuilder.Accessibility = Accessibility.Private;
                //fieldBuilder.InitializerExpression =  ??
            });
    }
}