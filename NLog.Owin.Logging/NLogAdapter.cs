namespace NLog.Owin.Logging
{
    using Microsoft.Owin.Logging;
    using NLog;
    using System;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// The OWIN ILoggerFactory implementation for NLog
    /// </summary>
    public class NLogFactory : ILoggerFactory
    {
        private static readonly object[] EventIdMapper = Enumerable.Range(0, 1000).Select(id => (object)id).ToArray();

        /// <summary>
        /// The log level translation function to get a NLog loglevel
        /// </summary>
        private readonly Func<TraceEventType, LogLevel> _getLogLevel;

        /// <summary>
        /// Create a logger factory with the default translation
        /// </summary>
        public NLogFactory()
        {
            this._getLogLevel = DefaultGetLogLevel;
        }

        /// <summary>
        /// Create a logger factory with a custom translation routine
        /// </summary>
        /// <param name="getLogLevel"></param>
        public NLogFactory(Func<TraceEventType, LogLevel> getLogLevel)
        {
            this._getLogLevel = getLogLevel;
        }

        /// <summary>
        /// This is the standard translation
        /// </summary>
        /// <param name="traceEventType"></param>
        /// <returns></returns>
        static LogLevel DefaultGetLogLevel(TraceEventType traceEventType)
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

        /// <summary>
        /// Creates a new ILogger instance of the given name.
        /// </summary>
        /// <param name="name">The logger context name.</param>
        /// <returns>A logger instance.</returns>
        public Microsoft.Owin.Logging.ILogger Create(string name)
        {
            return new Logger(name, this._getLogLevel);
        }        

        /// <summary>
        /// The wrapper arround NLog. Translates the logging levels
        /// </summary>
        private sealed class Logger : Microsoft.Owin.Logging.ILogger
        {
            private readonly Func<TraceEventType, LogLevel> _getLogLevel;

            private readonly NLog.Logger _logger;
           
            internal Logger(string name, Func<TraceEventType, LogLevel> getLogLevel)
            {
                this._getLogLevel = getLogLevel;
                this._logger = LogManager.GetLogger(name);
            }

            public bool WriteCore(TraceEventType eventType, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
            {
                var level = this._getLogLevel(eventType);

                // According to docs http://katanaproject.codeplex.com/SourceControl/latest#src/Microsoft.Owin/Logging/ILogger.cs
                // "To check IsEnabled call WriteCore with only TraceEventType and check the return value, no event will be written."
                if (state is null)
                {
                    return this._logger.IsEnabled(level);
                }
                if (!this._logger.IsEnabled(level))
                {
                    return false;
                }

                var logEvent = LogEventInfo.Create(level, _logger.Name, exception, (IFormatProvider)null, formatter(state, exception));
                if (eventId != 0)
                {
                    logEvent.Properties["EventId"] = GetEventId(eventId);
                }

                _logger.Log(typeof(Microsoft.Owin.Logging.ILogger), logEvent);
                return true;
            }

            private static object GetEventId(int eventId)
            {
                if (eventId > 0 && eventId < EventIdMapper.Length)
                    return EventIdMapper[eventId];
                else
                    return eventId;
            }
        }
    }
}