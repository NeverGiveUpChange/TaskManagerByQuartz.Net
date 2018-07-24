
using Quartz.Net_Infrastructure.HttpClientUtil;
using Quartz.Net_Infrastructure.LogUtil;
using Quartz.Net_Infrastructure.MailUtil;
using Quartz.Net_RemoteServer.Models;
using System;

namespace Quartz.Net_RemoteServer.Events
{
    public class Observer : ObserverBase
    {
        public Observer(SubjectBase childBase) : base(childBase) { }
        public override void Post(JobExcutedCallBackModel jobExcutedCallBackModel)
        {
            try
            {
                CustomerLogUtil.Info(jobExcutedCallBackModel.Log4NetKey_JobInfo, CustomerLogFormatUtil.LogJobMsgFormat(jobExcutedCallBackModel.JobName, jobExcutedCallBackModel.JobState, jobExcutedCallBackModel.OperateType));
                var _httpClient = new HttpClientHelper();
                var result = _httpClient.PostAsync(jobExcutedCallBackModel.RequestBody, jobExcutedCallBackModel.RequestUrl).Result;
            }
            catch (Exception ex)
            {
                CustomerLogUtil.Error(jobExcutedCallBackModel.Log4NetKey_JobError, CustomerLogFormatUtil.LogJobMsgFormat(jobExcutedCallBackModel.JobName, jobExcutedCallBackModel.JobState, jobExcutedCallBackModel.OperateType), ex);

            }
        }

        public override void SendMail(SchedulerExecutedCallBackModel schedulerExecutedCallBackModel)
        {
            schedulerExecutedCallBackModel.LogSchedulerAction(schedulerExecutedCallBackModel.Log4NetKey_Scheduler, CustomerLogFormatUtil.LogSchedulerMsgFormat(SchedulerExecutedCallBackModel.LocalIP, SchedulerExecutedCallBackModel.QuartzServerName, schedulerExecutedCallBackModel.OperateType, schedulerExecutedCallBackModel.OperateState), schedulerExecutedCallBackModel.Exception);
            var mailClient =new MailClient();
            mailClient.SendMail(new MailMessageConfigurationInfo { Body = CustomerLogFormatUtil.LogSchedulerMsgFormat(SchedulerExecutedCallBackModel.LocalIP, SchedulerExecutedCallBackModel.QuartzServerName, schedulerExecutedCallBackModel.OperateType, schedulerExecutedCallBackModel.OperateState), Subject = schedulerExecutedCallBackModel.Subject, ToMailAddressList = schedulerExecutedCallBackModel.ToMailAddressList });

        }
    }
}
