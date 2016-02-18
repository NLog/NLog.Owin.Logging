using Microsoft.Owin;
using Microsoft.Owin.Logging;
using NLog;
using Owin;
using System;
using System.Diagnostics;

namespace Pysco68.Owin.Logging.NLogAdapter
{
    /// <summary>
    /// Extension class
    /// </summary>
    public static class NlogFactoryExtensions
    {
        /// <summary>
        /// Set the logger factory for this app builder to NLogFactory
        /// </summary>
        /// <param name="app"></param>
        public static void UseNLog(this IAppBuilder app)
        {
            app.SetLoggerFactory(new NLogFactory());
        }

        /// <summary>
        /// Set the logger factory for this app builder to NLogFactory
        /// </summary>
        /// <param name="app"></param>
        /// <param name="getLogLevel"></param>
        public static void UseNLog(this IAppBuilder app, Func<TraceEventType, LogLevel> getLogLevel)
        {
            app.SetLoggerFactory(new NLogFactory(getLogLevel));
        }
    }
}
