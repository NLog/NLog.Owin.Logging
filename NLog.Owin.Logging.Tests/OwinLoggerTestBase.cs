using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Owin.Testing;
using NUnit.Framework;

namespace NLog.Owin.Logging.Tests
{
    public class OwinLoggerTestBase
    {
        private TestServer _server;

        [OneTimeSetUp]
        public void StartServer()
        {
            _server = TestServer.Create<StartupStandard>();
        }

        [OneTimeTearDown]
        public void StopServer()
        {
            _server.Dispose();
        }

        protected async Task CallRoute(string route)
        {
            var result = await _server.CreateRequest(route).GetAsync();
            result.EnsureSuccessStatusCode();
        }
    }
}