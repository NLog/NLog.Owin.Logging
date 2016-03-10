using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;
using Microsoft.Owin.Testing;
using Owin;
using Microsoft.Owin.Logging;
using Microsoft.Owin;
using NLog.Config;
using NLog.Targets;

namespace NLog.Owin.Logging.Tests
{
    public class StartupStandard
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNLog();
            app.Use<TestMiddleware>(app);
        }
    }

    [TestFixture]
    public class OwinLoggerTest
    {
        private TestServer _server;
        private DebugTarget _debugTarget;
        private DebugTarget _debugTargetOnlyFatal;


        [OneTimeSetUp]
        public void FixtureInit()
        {
            _server = TestServer.Create<StartupStandard>();

            // setup the debug target
            _debugTarget = new DebugTarget { Layout = "${level} ${message}" };
            _debugTargetOnlyFatal = new DebugTarget { Layout = "${level} ${message}" };

            var loggingConfiguration = new LoggingConfiguration();

            loggingConfiguration.AddTarget("debug", _debugTarget);
            loggingConfiguration.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, _debugTarget));
            loggingConfiguration.AddTarget("debug-fatal-only", _debugTargetOnlyFatal);
            loggingConfiguration.LoggingRules.Add(new LoggingRule("*", LogLevel.Fatal, _debugTargetOnlyFatal));


            LogManager.Configuration = loggingConfiguration;
        }

        [OneTimeTearDown]
        public void FixtureDispose()
        {
            _server.Dispose();
        }

        [TestCase("/critical", "critical", "Fatal")]
        [TestCase("/error", "error", "Error")]
        [TestCase("/warning", "warning", "Warn")]
        [TestCase("/information", "information", "Info")]
        [TestCase("/verbose", "verbose", "Trace")]
        [TestCase("/start", "Start", "Debug")]
        [TestCase("/stop", "Stop", "Debug")]
        [TestCase("/suspend", "Suspend", "Debug")]
        [TestCase("/Resume", "Resume", "Debug")]
        [TestCase("/Transfer", "Transfer", "Debug")]
        public async Task TestLogmessages(string route, string expectedMessageEnd, string expectedLogLevel)
        {
            await CallRoute(route);

            // note: the log messages end with watherver was logged thus we want to check for that
            Assert.That(_debugTarget.LastMessage, Does.StartWith(expectedLogLevel));
            Assert.That(_debugTarget.LastMessage, Does.EndWith(expectedMessageEnd));
        }

        /// <summary>
        /// Test logger which is only enabled for fatal/critical.
        /// </summary>
        /// <param name="route"></param>
        /// <param name="counterChange"></param>
        /// <returns></returns>
        [TestCase("/critical", 1)]
        [TestCase("/error", 0)]
        [TestCase("/warning", 0)]
        public async Task TestDisabledLogger(string route, int counterChange)
        {
            //also before empty
            var oldCounter = _debugTargetOnlyFatal.Counter;
            int callCount = 5;
            for (int i = 0; i < callCount; i++)
            {
                await CallRoute(route);
            }

            Assert.AreEqual(oldCounter + (counterChange * callCount), _debugTargetOnlyFatal.Counter);
        }

        private async Task CallRoute(string route)
        {
            var result = await _server.CreateRequest(route).GetAsync();
            result.EnsureSuccessStatusCode();
        }
    }
}
