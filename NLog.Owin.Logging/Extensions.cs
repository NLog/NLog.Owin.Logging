namespace NLog.Owin.Logging
{
    using global::Owin;
    using Microsoft.Owin.Logging;
    using NLog;
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Extension class
    /// </summary>
    public static class NlogFactoryExtensions
    {
        /// <summary>
        /// Set the logger factory for this app builder to NLogFactory
        /// </summary>
        /// <param name="app"></param>
        public static IAppBuilder UseNLog(this IAppBuilder app)
        {
            InitSetup();
            app.SetLoggerFactory(new NLogFactory());
            return app;
        }

        /// <summary>
        /// Set the logger factory for this app builder to NLogFactory
        /// </summary>
        /// <param name="app"></param>
        /// <param name="getLogLevel"></param>
        public static IAppBuilder UseNLog(this IAppBuilder app, Func<TraceEventType, LogLevel> getLogLevel)
        {
            InitSetup();
            app.SetLoggerFactory(new NLogFactory(getLogLevel));
            return app;
        }
        
        private static void InitSetup()
        {
            LogManager.AddHiddenAssembly(typeof(NLogFactory).Assembly);//NLog.Owin.Logging
            LogManager.AddHiddenAssembly(typeof(LoggerExtensions).Assembly);//Microsoft.Owin.Logging
        }
    }
}
