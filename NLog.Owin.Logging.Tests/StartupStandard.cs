using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit;
using Owin;
using Microsoft.Owin.Logging;
using Microsoft.Owin;

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
}
