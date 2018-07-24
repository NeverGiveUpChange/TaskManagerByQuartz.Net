
using Quartz.Net_Infrastructure.IPUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.Net_RemoteServer.Models
{
    //todo:建立合适的数据
    public class SchedulerExecutedCallBackModel
    {
        public Action<string,string,Exception> LogSchedulerAction { get; set; }
        public string Log4NetKey_Scheduler { get; set; }
        public static string LocalIP { get { return IPHelper.IpAddress; } private set { } }
        private static string _localIP = IPHelper.IpAddress;

        public static string QuartzServerName { get { return "Bh_Crm_QuartzServer"; }private set { } }
        public string OperateType { get; set; }
        public string OperateState { get; set; }
        //public string Body { get; set; }
        public List<string> ToMailAddressList { get; set; }
        public string Subject { get; set; }
        public List<string> CCMailAddressList { get; set; }
        public Exception Exception { get; set; }
    }
}
