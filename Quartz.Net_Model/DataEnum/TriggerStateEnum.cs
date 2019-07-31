using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.Net_Model.DataEnum
{
    public enum TriggerStateEnum
    {
        [Description("部署")]
        Deploy = 1,
        [Description("运行")]
        Run = 2,
        [Description("暂停")]
        Pause = 3,
        [Description("删除")]
        Deleted= 4,
        [Description("完成")]
        Complete = 5

    }
}
