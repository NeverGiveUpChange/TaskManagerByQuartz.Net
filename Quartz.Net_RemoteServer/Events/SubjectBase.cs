
using Quartz.Net_RemoteServer.Models;


namespace Quartz.Net_RemoteServer.Events
{

    public abstract class SubjectBase
    {
        public SubjectBase() { }
        public delegate void JobExecutedCallBack(JobExcutedCallBackModel jobExcutedCallBackModel);
        public delegate void SchedulerExecutedCallBack(SchedulerExecutedCallBackModel schedulerExecutedCallBackModel);
        private JobExecutedCallBack _jobExecutedCallBack;
        private SchedulerExecutedCallBack _schedulerExecutedCallBack;
        public event JobExecutedCallBack JobExecutedCallBackEvent
        {
            add
            {
                _jobExecutedCallBack += value;
            }
            remove
            {
                if (_jobExecutedCallBack != null)
                {
                    _jobExecutedCallBack -= value;
                }
            }
        }
        public event SchedulerExecutedCallBack SchedulerExecutedCallBackEvent
        {

            add
            {
                _schedulerExecutedCallBack += value;
            }
            remove
            {
                if (_schedulerExecutedCallBack != null)
                {
                    _schedulerExecutedCallBack -= value;
                }

            }
        }
        protected void NotifyAsync(JobExcutedCallBackModel jobExcutedCallBackModel)
        {
            _jobExecutedCallBack.BeginInvoke(jobExcutedCallBackModel, null, null);


        }
        protected void NotifyAsync(SchedulerExecutedCallBackModel schedulerExecutedCallBackModel)
        {
            _schedulerExecutedCallBack.BeginInvoke(schedulerExecutedCallBackModel, null, null);
           
        }

    }
}
