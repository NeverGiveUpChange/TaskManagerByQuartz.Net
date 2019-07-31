using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.Net_Model.ViewModels
{
    public class AddSchedulerViewModel
    {
        /// <summary>
        /// 节点ip
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// 节点端口
        /// </summary>
        public string  Port { get; set; }
        /// <summary>
        /// 节点实例id
        /// </summary>
        public string SchedulerInstanceId { get; set; }
        /// <summary>
        /// 当前节点线程数
        /// </summary>
        public string ThreadCount { get; set; }

        
    }
}
