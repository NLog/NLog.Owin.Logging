using Microsoft.Owin;
using Microsoft.Owin.Logging;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLog.Owin.Logging.Tests
{
    /// <summary>
    /// This owin middleware does log messages matching the called route:
    /// 
    /// /critical       => "critical"
    /// /error          => "error"
    /// /warning        => "warning"
    /// /information    => "information"
    /// /verbose        => "verbose"
    /// 
    /// Calling these routes allows us to check the function of the 
    /// registered OWIN logger factory
    /// </summary>  
    public class TestMiddleware : OwinMiddleware
    {
        private readonly Microsoft.Owin.Logging.ILogger _logger;

        public TestMiddleware(OwinMiddleware next, IAppBuilder app) : base(next)
        {
            _logger = app.CreateLogger<TestMiddleware>();
        }

        public override async Task Invoke(IOwinContext context)
        {
            switch (context.Request.Path.Value)
            {
                case "/critical":
                    _logger.WriteCritical("critical"); ;
                    break;
                case "/error":
                    _logger.WriteError("error");
                    break;
                case "/warning":
                    _logger.WriteWarning("warning");
                    break;
                case "/information":
                    _logger.WriteInformation("information");
                    break;
                case "/verbose":
                    _logger.WriteVerbose("verbose");
                    break;
            }

            await context.Response.WriteAsync("Cool!");
        }
    }

}
