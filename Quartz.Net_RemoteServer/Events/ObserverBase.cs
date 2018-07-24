
using Quartz.Net_RemoteServer.Models;

namespace Quartz.Net_RemoteServer.Events
{
    public abstract class ObserverBase
    {
        
        public ObserverBase(SubjectBase childModel) {
            childModel.JobExecutedCallBackEvent += new SubjectBase.JobExecutedCallBack(Post);
            childModel.SchedulerExecutedCallBackEvent += new SubjectBase.SchedulerExecutedCallBack(SendMail);
   
        }
        public abstract void Post(JobExcutedCallBackModel jobExcutedCallBackModel);
        public abstract void SendMail(SchedulerExecutedCallBackModel schedulerExecutedCallBackModel);
    }
}
