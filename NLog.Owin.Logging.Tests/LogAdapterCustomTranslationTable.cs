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
    public class StartupCustomTranslationTable
    {
        public void Configuration(IAppBuilder app)
        {
            // map all messages to Fatal log level
            app.UseNLog(tl => LogLevel.Fatal);

            app.Use<TestMiddleware>(app);
        }
    }

    [TestFixture]
    public class OwinLoggerCustomTranslationTableTest
    {
        private TestServer _server;
        private DebugTarget _debugTarget;


        [OneTimeSetUp]
        public void FixtureInit()
        {
            _server = TestServer.Create<StartupCustomTranslationTable>();

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

        [TestCase("/critical", "critical")]
        [TestCase("/error", "error")]
        [TestCase("/warning", "warning")]
        [TestCase("/information", "information")]
        [TestCase("/verbose", "verbose")]
        public async Task TestLogmessages(string route, string expectedMessageEnd)
        {
            var result = await _server.CreateRequest(route).GetAsync();
            result.EnsureSuccessStatusCode();

            // in this setup we should be redirecting all log messages to "Fatal"....
            Assert.That(_debugTarget.LastMessage, Does.StartWith("Fatal"));

            // note: the log messages end with watherver was logged thus we want to check for that
            Assert.That(_debugTarget.LastMessage, Does.EndWith(expectedMessageEnd));
        }
    }
}
