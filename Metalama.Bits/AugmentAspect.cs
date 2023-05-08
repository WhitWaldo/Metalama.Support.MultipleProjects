using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Code.SyntaxBuilders;

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

                //The following doesn't work either
                //var sb = new InterpolatedStringBuilder();
                //sb.AddText("_doodad.DoSomething<");
                //sb.AddExpression(baseStateType.ToType());
                //sb.AddText(@">(""MyObj"")");
                //fieldBuilder.InitializerExpression = sb.ToExpression();

                fieldBuilder.InitializerExpression =
                    ExpressionFactory.Parse($@"_doodad.DoSomething<{dictionaryType.ToDisplayString()}>(""state"")");
            });
    }
}