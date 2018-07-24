using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.Net_RemoteServer
{
   public class Configuration
    {
        public static string Description { get { return ConfigurationManager.AppSettings["Description"]; } }

        public static string DisplayName { get { return ConfigurationManager.AppSettings["DisplayName"]; } }

        public static string ServiceName { get { return ConfigurationManager.AppSettings["ServiceName"]; } }
    }
}
