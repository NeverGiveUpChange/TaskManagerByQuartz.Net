using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.Net_Model
{
   public static class SchedulerData
    {
        public static Dictionary<string, IpPort> schedulerIdEquivalentIp = new Dictionary<string, IpPort>() {
            { "444444",new IpPort{ Ip="127.0.0.1", Port="999" }},
            { "333333",new IpPort{Ip="127.0.0.1" ,Port="998"}}
            
        };
    }
    public class IpPort {
        public string Ip { get; set; }
        public string Port { get; set; }
    }
}
