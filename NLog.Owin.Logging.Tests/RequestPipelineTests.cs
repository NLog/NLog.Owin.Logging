using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;
using Microsoft.Owin.Testing;
using Owin;

namespace NLog.Owin.Logging.Tests
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            appBuilder.UseNLog();
        }
    }


    [TestFixture]
    public class MyAppIntegrationTests
    {
        private TestServer _server;

        [TestFixtureSetUp]
        public void FixtureInit()
        {
            _server = TestServer.Create<Startup>();
        }

        [TestFixtureTearDown]
        public void FixtureDispose()
        {
            _server.Dispose();
        }

        [Test]
        public void WebApiGetAllTest()
        {
            //_server.

            //var response = _server.HttpClient.GetAsync("/api/test").Result;

        }


    }
}
