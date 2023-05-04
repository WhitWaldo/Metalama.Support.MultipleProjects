using System.Diagnostics;
using Metalama.Extensions.DependencyInjection;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Code.SyntaxBuilders;
using Microsoft.Extensions.Logging;

namespace Metalama.Bits;

public class InjectedLoggerAttribute : OverrideMethodAspect
{
    [IntroduceDependency]
    private readonly ILogger _logger;

    /// <summary>Default template of the new method implementation.</summary>
    /// <returns></returns>
    public override dynamic? OverrideMethod()
    {
        var entryMessage = BuildInterpolatedString(false);
        entryMessage.AddText(" started.");
        _logger.LogInformation((string)entryMessage.ToValue());
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
                _logger.LogTrace((string) successMessage.ToValue());
            }

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
                _logger.LogError(ex, (string) failureMessage.ToValue());
            }

            throw;
        }
        finally
        {
            stopwatch.Stop();
            using var guard = LoggingRecursionGuard.Begin();
            if (guard.CanLog)
            {
                var elapsedMessage = BuildInterpolatedString(false);
                elapsedMessage.AddText(" elapsed: ");
                elapsedMessage.AddExpression(stopwatch.ElapsedMilliseconds);
                elapsedMessage.AddText("ms");
                _logger.LogTrace((string)elapsedMessage.ToValue());
            }
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