using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.Net_RemoteServer.Models
{
    //todo:建立合适的数据
    public class JobExcutedCallBackModel
    {
        public string Log4NetKey_JobInfo { get; set; }
        public string Log4NetKey_JobError { get; set; }
        public string JobName { get; set; }
        public int JobState { get; set; }
        public bool IsJobDeleted { get; set; }
        public string OperateType { get; set; }
        public string  RequestUrl { get; set; }
        public object RequestBody { get; set; }
    }
}
