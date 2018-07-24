
using Quartz;
using Quartz.Net_Core.JobCommon;
using Quartz.Net_Core.JobExcute;
using Quartz.Net_Core.JobTriggerAbstract;
using Quartz.Net_EFModel_MySql;
using System;
using System.Reflection;

namespace Quartz.Net_Core.JobTriggerImplements
{
    internal class JobSimpleTrigger : JobBaseTrigger
    {
        public JobSimpleTrigger()
        {
            Scheduler = SchedulerManager.Instance;
        }
        public override bool ModifyJobCron(customer_quartzjobinfo jobInfo)
        {
            var triggerKey = KeyManager.CreateTriggerKey(jobInfo.TriggerName, jobInfo.TriggerGroupName);
            ITrigger trigger = TriggerBuilder.Create().StartAt(DateTimeOffset.Now)
                    .WithIdentity(jobInfo.TriggerName, jobInfo.TriggerGroupName)
                   .WithSimpleSchedule(x => x.WithIntervalInSeconds(jobInfo.Cycle.HasValue? jobInfo.Cycle.Value:1).WithRepeatCount(jobInfo.RepeatCount.Value-1))
                    .Build();
            Scheduler.RescheduleJob(triggerKey, trigger);
            return true;
        }

        public override bool RunJob(customer_quartzjobinfo jobInfo)
        {
            //Assembly assembly = Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + $"bin/{jobInfo.DLLName}");
            //var type = assembly.GetType(jobInfo.FullJobName);
            JobKey jobKey = KeyManager.CreateJobKey(jobInfo.JobName, jobInfo.JobGroupName);
            if (!Scheduler.CheckExists(jobKey))
            {
                IJobDetail job = JobBuilder.Create<JobItem>()
                    .WithIdentity(jobKey)
                    .UsingJobData(KeyManager.CreateJobDataMap("requestUrl", jobInfo.RequestUrl))
                    .Build();
                ITrigger trigger = TriggerBuilder.Create().StartAt(DateTimeOffset.Now)
                    .WithIdentity(jobInfo.TriggerName, jobInfo.TriggerGroupName)
                    .ForJob(jobKey)
            .WithSimpleSchedule(x => x.WithIntervalInSeconds(jobInfo.Cycle.HasValue? jobInfo.Cycle.Value:1).WithRepeatCount(jobInfo.RepeatCount.Value-1))
                    .Build();
                Scheduler.ScheduleJob(job, trigger);
            }
            return true;
        }
    }
}
