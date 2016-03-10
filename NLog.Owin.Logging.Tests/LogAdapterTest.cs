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


        [OneTimeSetUp]
        public void FixtureInit()
        {
            _server = TestServer.Create<StartupStandard>();

            // setup the debug target
            this._debugTarget = new DebugTarget();
            this._debugTarget.Layout = "${level} ${message}";

            SimpleConfigurator.ConfigureForTargetLogging(_debugTarget, LogLevel.Trace);
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
        public async Task TestLogmessages(string route, string expectedMessageEnd, string expectedLogLevel)
        {
            var result = await _server.CreateRequest(route).GetAsync();
            result.EnsureSuccessStatusCode();

            // note: the log messages end with watherver was logged thus we want to check for that
            Assert.That(_debugTarget.LastMessage, Does.StartWith(expectedLogLevel));
            Assert.That(_debugTarget.LastMessage, Does.EndWith(expectedMessageEnd));
        }
    }
}
