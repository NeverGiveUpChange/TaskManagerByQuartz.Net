
using Quartz.Net_Infrastructure.IPUtil;
using Quartz.Simpl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.Net_RemoteServer.RemotingSchedulerExporterOverrided
{
  public  class RemotingSchedulerExporterOverrided: RemotingSchedulerExporter
    {
        protected override IDictionary CreateConfiguration()
        {
            IDictionary props = new Hashtable();
            props["port"] = Port;
            props["name"] = ChannelName;
            props["bindTo"] = IPHelper.IpAddress;
    
            if (RejectRemoteRequests)
            {
                props["rejectRemoteRequests"] = "true";
            }
            return props;
        }
    }
}
