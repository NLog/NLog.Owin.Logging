using System.Threading.Tasks;
using NLog.Config;
using NLog.Targets;
using NUnit.Framework;

namespace NLog.Owin.Logging.Tests
{
    [TestFixture]
    public class OwinDisabledLoggerTest : OwinLoggerTestBase
    {
        private DebugTarget _debugTargetOnlyFatal;

        [OneTimeSetUp]
        public void InitConfig()
        {
            // setup the debug target
            _debugTargetOnlyFatal = new DebugTarget { Layout = "${level} ${message}" };

            var loggingConfiguration = new LoggingConfiguration();

            loggingConfiguration.AddTarget("debug-fatal-only", _debugTargetOnlyFatal);
            loggingConfiguration.LoggingRules.Add(new LoggingRule("*", LogLevel.Fatal, _debugTargetOnlyFatal));

            LogManager.Configuration = loggingConfiguration;
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

    }
}