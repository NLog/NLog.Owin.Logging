using Microsoft.Owin;
using Microsoft.Owin.Logging;
using Owin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var path = context.Request.Path.Value;
            switch (path)
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
                case "":
                    {
                        //nothing
                        break;
                    }
                default:
                    TraceEventType traceEventType;
                    if (Enum.TryParse(path.Substring(1), true, out traceEventType))
                    {
                        WriteCore(traceEventType);
                    }
                    break;

            }

            await context.Response.WriteAsync("Cool!");
        }

        /// <summary>
        /// Write this traceEventType with <paramref name="traceEventType"/> to-stringed as message
        /// </summary>
        /// <param name="traceEventType"></param>
        /// <returns></returns>
        private bool WriteCore(TraceEventType traceEventType)
        {

            return _logger.WriteCore(traceEventType, 10, traceEventType.ToString(), null, (o, exception) => o.ToString());
        }
    }

}
