using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.Net_Model
{
   public static class SchedulerData
    {
        public static Dictionary<string, schedulerInstanceInfo> schedulerInstanceIdEquivalentIp = new Dictionary<string, schedulerInstanceInfo>() {
            { "444444",new schedulerInstanceInfo{ Ip="localhost", Port="999" , schedulerInstanceId="444444"}},
            { "333333",new schedulerInstanceInfo{Ip="localhost" ,Port="998", schedulerInstanceId="333333"}}
            
        };
    }
    public class schedulerInstanceInfo
    {
        public string schedulerInstanceId { get; set; }
        public string Ip { get; set; }
        public string Port { get; set; }
    }
}
