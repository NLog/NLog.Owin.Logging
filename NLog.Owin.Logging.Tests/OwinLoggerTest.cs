using System.Threading.Tasks;
using NLog.Config;
using NLog.Targets;
using NUnit.Framework;

namespace NLog.Owin.Logging.Tests
{
    [TestFixture]
    public class OwinLoggerTest : OwinLoggerTestBase
    {
        private DebugTarget _debugTarget;

        [OneTimeSetUp]
        public void InitConfig()
        {
            // setup the debug target
            _debugTarget = new DebugTarget { Layout = "${level} ${message}" };

            var loggingConfiguration = new LoggingConfiguration();

            loggingConfiguration.AddTarget("debug", _debugTarget);
            loggingConfiguration.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, _debugTarget));

            LogManager.Configuration = loggingConfiguration;
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


        [Test]
        public async Task NullShouldNotWrite()
        {
            var oldCounter = _debugTarget.Counter;
            await CallRoute("/null");
            Assert.AreEqual(oldCounter, _debugTarget.Counter);
        }
    }
}