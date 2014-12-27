using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Logging;

namespace Pysco68.Owin.Logging.NLogAdapter
{
    public static class NlogFactoryExtensions
    {
        /// <summary>
        /// Set the logger factory for this app builder to NLogFactory
        /// </summary>
        /// <param name="app"></param>
        public static void UseNLog(this IAppBuilder app)
        {
            app.SetLoggerFactory(new NLogFactory());
        }


    }
}
