﻿using System.Diagnostics;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Code.SyntaxBuilders;
using Metalama.Framework.Diagnostics;
using Metalama.Framework.Eligibility;
using Microsoft.Extensions.Logging;

namespace Metalama.Bits;

public class LogAttribute : MethodAspect
{
    private static readonly DiagnosticDefinition<INamedType> _missingLoggerFieldError = new("LOG011", Severity.Error,
        "The type '{0}' must have a field 'ILogger _logger or a property 'ILogger Logger'.");

    private static readonly DiagnosticDefinition<(DeclarationKind, IFieldOrProperty)> _loggerFieldOrIncorrectTypeError =
        new("LOG02", Severity.Error, "The {0} '{1}' must be of type ILogger.");

    public override void BuildAspect(IAspectBuilder<IMethod> builder)
    {
        var declaringType = builder.Target.DeclaringType;

        //Finds a field named '_logger' or a property named 'Logger'
        var loggerFieldOrProperty = (IFieldOrProperty?) declaringType.AllFields.OfName("_logger").SingleOrDefault() ??
                                    declaringType.AllProperties.OfName("Logger").SingleOrDefault();

        //Report an error if the field or property does not exist
        if (loggerFieldOrProperty == null)
        {
            builder.Diagnostics.Report(_missingLoggerFieldError.WithArguments(declaringType));
            return;
        }

        //Verify the type of the logger field or property
        if (!loggerFieldOrProperty.Type.Is(typeof(ILogger)))
        {
            builder.Diagnostics.Report(
                _loggerFieldOrIncorrectTypeError.WithArguments((declaringType.DeclarationKind, loggerFieldOrProperty)));

            return;
        }

        //Override the target method with our template. Pass the logger field or property to the template

        builder.Advice.Override(builder.Target, nameof(this.OverrideMethod),
            new {loggerFieldOrProperty = loggerFieldOrProperty});
    }

    public override void BuildEligibility(IEligibilityBuilder<IMethod> builder)
    {
        base.BuildEligibility(builder);

        //Now that we reference an instance field, we cannot log static methods
        builder.MustNotBeStatic();
    }

    [Template]
    private dynamic? OverrideMethod(IFieldOrProperty loggerFieldOrProperty)
    {
        //Define a `logger` run-time variable and assign to the ILogger field or property
        var logger = (ILogger) loggerFieldOrProperty.Value!;

        var entryMessage = BuildInterpolatedString(false);
        entryMessage.AddText($"{meta.Target.Type.ToDisplayString(CodeDisplayFormat.MinimallyQualified)}.{meta.Target.Method.Name} started.");
        logger.LogInformation((string)entryMessage.ToValue());
        //_telemetryClient.TrackTrace(entryMessage.ToValue(), SeverityLevel.Information);//, BuildDictionaryOfProperties());
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        try
        {
            var result = meta.Proceed();

            //Display the success message = the message is different when the return is void
            var successMessage = BuildInterpolatedString(true);

            if (meta.Target.Method.ReturnType.Is(typeof(void)))
            {
                //When the method is void, display a constant text
                successMessage.AddText(" succeeded.");
            }
            else
            {
                //When the method has a return value, add to the message
                successMessage.AddText(" returned '");
                successMessage.AddExpression(result);
                successMessage.AddText("'.");
            }

            using var guard = LoggingRecursionGuard.Begin();
            if (guard.CanLog)
            {
                logger.LogTrace((string)successMessage.ToValue());
            }

            //_telemetryClient.TrackTrace((string)successMessage.ToValue());
            return result;
        }
        catch (Exception ex)
        {

            var failureMessage = BuildInterpolatedString(false);
            failureMessage.AddText(" failed: '");
            failureMessage.AddExpression(ex.Message);
            failureMessage.AddText("'");
            //_telemetryClient.TrackException(ex);
            using var guard = LoggingRecursionGuard.Begin();
            if (guard.CanLog)
            {
                logger.LogError(ex, (string)failureMessage.ToValue());
            }

            //_telemetryClient.TrackTrace(failureMessage.ToValue());
            throw;
        }
        finally
        {
            stopwatch.Stop();
            using var guard = LoggingRecursionGuard.Begin();
            if (guard.CanLog)
            {
                logger.LogTrace(
                    (string)
                    $"{meta.Target.Type.ToDisplayString(CodeDisplayFormat.MinimallyQualified)}.{meta.Target.Method.Name} elapsed (ms): {stopwatch.ElapsedMilliseconds}");
            }
            //_telemetryClient.TrackMetric($"", stopwatch.ElapsedMilliseconds);
        }
    }
    
    private static InterpolatedStringBuilder BuildInterpolatedString(bool includeOutParameters)
    {
        var stringBuilder = new InterpolatedStringBuilder();

        //Include the type and method name
        stringBuilder.AddText(meta.Target.Type.ToDisplayString(CodeDisplayFormat.MinimallyQualified));
        stringBuilder.AddText(".");
        stringBuilder.AddText(meta.Target.Method.Name);
        stringBuilder.AddText("(");
        var i = meta.CompileTime(0);

        //Include a placeholder for each parameter
        foreach (var p in meta.Target.Parameters)
        {
            var comma = i > 0 ? ", " : "";
            if (p.RefKind == RefKind.Out && !includeOutParameters)
            {
                //When the parameter is 'out', we cannot read that value
                stringBuilder.AddText($"{comma}{p.Name} = <out> ");
            }
            else if (IsSensitive(p))
            {
                stringBuilder.AddText($"{comma}{p.Name} = '******' ");
            }
            else
            {
                //Otherwise, add the parameter value
                stringBuilder.AddText($"{comma}{p.Name} = {{");
                stringBuilder.AddExpression(p.Value);
                stringBuilder.AddText("}");
            }

            i++;
        }

        stringBuilder.AddText(")");
        return stringBuilder;
    }

    private static bool IsSensitive(IParameter parameter)
    {
        return parameter.Attributes.OfAttributeType(typeof(SensitiveDataAttribute)).Any();
    }
}