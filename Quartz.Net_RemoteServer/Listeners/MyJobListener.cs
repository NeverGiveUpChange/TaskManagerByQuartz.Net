
using Quartz;
using Quartz.Net_Infrastructure.LogUtil;
using Quartz.Net_RemoteServer.Events;
using Quartz.Net_RemoteServer.Models;
using System;

namespace Quartz.Net_RemoteServer.Listeners
{
    //TODOThink:log4netkey是否可以动态得到？
    //TODOThink:是否开始异步 减少监听器本身运行的时间
    internal class MyJobListener : SubjectBase, IJobListener
    {
        //const string log4netInfoKey = "logJobInfo";
        //const string log4netErrorKey = "logJobError";

        public MyJobListener()
        {
            new Observer(this);
        }
        public string Name
        {
            get
            {
                return "customerJobListener";
            }
        }

        public void JobExecutionVetoed(IJobExecutionContext context)
        {
        }

        public void JobToBeExecuted(IJobExecutionContext context)
        {
        }

        public void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException)
        {
            var jobName = context.JobDetail.Key.Name;
            var jobState = 6;
            var operateType = "运行";
            //try
            //{
            //    HttpClientHelper httpClient = new HttpClientHelper();
            string exceptionMessage = jobException == null ? null : jobException.Message;

            //var reuslt = httpClient.PostAsync(new { JobName = jobName, JobState = jobState, Exception = exceptionMessage, PreTime = context.Trigger.GetPreviousFireTimeUtc().HasValue ? context.Trigger.GetPreviousFireTimeUtc().Value.LocalDateTime as DateTime? : null, NextTime = context.Trigger.GetNextFireTimeUtc().HasValue ? context.Trigger.GetNextFireTimeUtc().Value.LocalDateTime as DateTime? : null }).Result;
            this.NotifyAsync(new JobExcutedCallBackModel { IsJobDeleted = false, JobName = jobName, JobState = jobState, Log4NetKey_JobError = Log4NetKeys.Log4netJobErrorKey, Log4NetKey_JobInfo = Log4NetKeys.Log4netJobInfoKey, OperateType = operateType, RequestUrl = "", RequestBody = new { JobName = jobName, JobState = jobState, Exception = exceptionMessage, PreTime = context.Trigger.GetPreviousFireTimeUtc().HasValue ? context.Trigger.GetPreviousFireTimeUtc().Value.LocalDateTime as DateTime? : null, NextTime = context.Trigger.GetNextFireTimeUtc().HasValue ? context.Trigger.GetNextFireTimeUtc().Value.LocalDateTime as DateTime? : null } });
            CustomerLogUtil.Info(Log4NetKeys.Log4netJobInfoKey, CustomerLogFormatUtil.LogJobMsgFormat(jobName, jobState, operateType));
            //}
            //catch (Exception ex)
            //{

            //    CustomerLog.Error(log4netErrorKey,CustomerLogFormatUtil.LogJobMsgFormat(jobName, jobState, operateType), ex);
            //}
        }

    }
}
