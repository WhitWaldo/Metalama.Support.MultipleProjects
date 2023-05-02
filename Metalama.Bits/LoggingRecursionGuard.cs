namespace Metalama.Bits;

public static class LoggingRecursionGuard
{
    [ThreadStatic]
    public static bool _isLogging;

    public static DisposeCookie Begin()
    {
        if (_isLogging)
        {
            return new DisposeCookie(false);
        }
        else
        {
            _isLogging = true;
            return new DisposeCookie(true);
        }
    }

    public class DisposeCookie : IDisposable
    {
        public bool CanLog { get; }

        public DisposeCookie(bool canLog)
        {
            CanLog = canLog;
        }

        public void Dispose()
        {
            if (CanLog)
            {
                _isLogging = false;
            }
        }
    }
}