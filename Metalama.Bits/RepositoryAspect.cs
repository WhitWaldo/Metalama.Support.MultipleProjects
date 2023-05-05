using Interfaces;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code.SyntaxBuilders;
using Metalama.Framework.Code;
using Microsoft.Extensions.Logging;

namespace Metalama.Bits;

/// <summary>
/// Turns the type decorated with this aspect into a simple repository.
/// </summary>
public class RepositoryAspect : TypeAspect
{
    /// <inheritdoc />
    public override void BuildAspect(IAspectBuilder<INamedType> builder)
    {
        var genericType = builder.Target.TypeParameters[0];

        var listType = ((INamedType)TypeFactory.GetType(typeof(List<>))).WithTypeArguments(genericType);
        var entities = builder.Advice.IntroduceField(builder.Target, "_entities", listType,
            IntroductionScope.Instance, OverrideStrategy.Ignore,
            fieldBuilder =>
            {
                fieldBuilder.InitializerExpression = ExpressionFactory.Parse("new()");
                fieldBuilder.Accessibility = Accessibility.Private;
            });

        //Add the ILogger field
        //var logger = builder.Advice.IntroduceField(builder.Target,
        //    "_logger", typeof(ILogger), buildField: fieldBuilder =>
        //    {
        //        //fieldBuilder.Writeability = Writeability.ConstructorOnly;
        //    });
        //var loggerType =
        //    ((INamedType)TypeFactory.GetType(typeof(ILogger<>))).WithTypeArguments(
        //        builder.Target.TypeDefinition.ToType());
        //builder.Advice.IntroduceParameter(builder.Target.Constructors.First(), "logger", typeof(ILogger),
        //    TypedConstant.Default(loggerType));

        //var eb = new ExpressionBuilder();
        //eb.AppendVerbatim("_logger = logger");
        //builder.Advice.AddInitializer(builder.Target.Constructors.First(), StatementFactory.FromExpression(eb.ToExpression()));

        builder.Advice.IntroduceMethod(
            builder.Target,
            nameof(Add),
            IntroductionScope.Instance,
            OverrideStrategy.Ignore,
            methodBuilder =>
            {
                methodBuilder.Accessibility = Accessibility.Public;
            },
            new { T = genericType, entitiesField = entities.Declaration });

        builder.Advice.IntroduceMethod(
            builder.Target,
            nameof(List),
            IntroductionScope.Instance,
            OverrideStrategy.Ignore,
            methodBuilder =>
            {
                methodBuilder.Accessibility = Accessibility.Public;
            },
            args: new { T = genericType, entitiesField = entities.Declaration });

        builder.Advice.IntroduceMethod(
            builder.Target,
            nameof(Update),
            IntroductionScope.Instance,
            OverrideStrategy.Ignore,
            methodBuilder =>
            {
                methodBuilder.Accessibility = Accessibility.Public;
            },
            args: new { T = genericType, entitiesField = entities.Declaration });

        builder.Advice.IntroduceMethod(
            builder.Target,
            nameof(Get),
            IntroductionScope.Instance,
            OverrideStrategy.Ignore,
            methodBuilder =>
            {
                methodBuilder.Accessibility = Accessibility.Public;
            },
            args: new { T = genericType, entitiesField = entities.Declaration });

        builder.Advice.IntroduceMethod(
            builder.Target,
            nameof(Remove),
            IntroductionScope.Instance,
            OverrideStrategy.Ignore,
            methodBuilder =>
            {
                methodBuilder.Accessibility = Accessibility.Public;
            },
            args: new { T = genericType, entitiesField = entities.Declaration });

        builder.Outbound.SelectMany(type => type.Methods)
            .AddAspectIfEligible<InjectedLoggerAttribute>();
    }

    /// <summary>
    /// Adds a thing.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="entitiesField"></param>
    [Template]
    private void Add<[CompileTime] T>(T data, IField entitiesField) where T : IHasId
    {
        var entities = (List<T>)entitiesField.Value;
        entities.Add(data);
    }

    [Template]
    private List<T> List<[CompileTime] T>(IField entitiesField) where T : IHasId
    {
        var entities = (List<T>)entitiesField.Value;
        return entities;
    }

    [Template]
    private void Update<[CompileTime] T>(IField entitiesField, T updatedData) where T : IHasId
    {
        var entities = (List<T>)entitiesField.Value;
        for (var a = 0; a < entities.Count; a++)
        {
            if (entities[a].Id == updatedData.Id)
            {
                entities[a] = updatedData;
            }
        }
    }

    [Template]
    private T? Get<[CompileTime] T>(Guid id, IField entitiesField) where T : IHasId
    {
        var entities = (List<T>)entitiesField.Value;
        var entity = entities.FirstOrDefault(a => a.Id == id);
        return entity;
    }

    [Template]
    private void Remove<[CompileTime] T>(Guid id, IField entitiesField) where T : IHasId
    {
        var entities = (List<T>)entitiesField.Value;
        var remaining = entities.Where(a => a.Id != id).ToList();
        entities.Clear();
        entities.AddRange(remaining);
    }
}