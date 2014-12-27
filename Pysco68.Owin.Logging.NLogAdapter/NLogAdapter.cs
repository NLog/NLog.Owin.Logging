namespace Pysco68.Owin.Logging.NLogAdapter
{
    using Microsoft.Owin.Logging;
    using NLog;
    using System;
    using System.Diagnostics;

    public class NLogFactory : ILoggerFactory
    {
        /// <summary>
        /// Creates a new ILogger instance of the given name.
        /// </summary>
        /// <param name="name">The logger context name.</param>
        /// <returns>A logger instance.</returns>
        public Microsoft.Owin.Logging.ILogger Create(string name)
        {
            return new Logger(name);
        }        

        class Logger : Microsoft.Owin.Logging.ILogger
        {
            readonly NLog.Logger _logger;
           
            internal Logger(string name)
            {
                _logger = LogManager.GetLogger(name);
            }

            public bool WriteCore(TraceEventType eventType, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
            {
                var level = GetLogLevel(eventType);
                
                // According to docs http://katanaproject.codeplex.com/SourceControl/latest#src/Microsoft.Owin/Logging/ILogger.cs
                // "To check IsEnabled call WriteCore with only TraceEventType and check the return value, no event will be written."
                if (state == null)
                {
                    return _logger.IsEnabled(level);
                }
                if (!_logger.IsEnabled(level))
                {
                    return false;
                }

                _logger.Log(level, formatter(state, exception), exception);
                return true;
            }

            static LogLevel GetLogLevel(TraceEventType traceEventType)
            {
                switch (traceEventType)
                {
                    case TraceEventType.Critical:
                        return LogLevel.Fatal;
                    case TraceEventType.Error:
                        return LogLevel.Error;
                    case TraceEventType.Warning:
                        return LogLevel.Warn;
                    case TraceEventType.Information:
                        return LogLevel.Info;
                    case TraceEventType.Verbose:
                        return LogLevel.Trace;
                    case TraceEventType.Start:
                        return LogLevel.Debug;
                    case TraceEventType.Stop:
                        return LogLevel.Debug;
                    case TraceEventType.Suspend:
                        return LogLevel.Debug;
                    case TraceEventType.Resume:
                        return LogLevel.Debug;
                    case TraceEventType.Transfer:
                        return LogLevel.Debug;
                    default:
                        throw new ArgumentOutOfRangeException("traceEventType");
                }
            }
        }
    }
}