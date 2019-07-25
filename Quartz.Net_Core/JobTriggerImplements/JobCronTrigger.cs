
using System;
using Quartz.Net_Core.JobTriggerAbstract;
using Quartz.Net_Core.JobCommon;
using Quartz.Net_Core.JobExcute;
using Models;

namespace Quartz.Net_Core.JobTriggerImplements
{
    public class JobCronTrigger : JobBaseTrigger
    {
        public JobCronTrigger() {

            Scheduler = SchedulerManager.Instance;
        }
        public override bool ModifyJobCron(custom_job_infoes jobInfo)
        {
            CronScheduleBuilder scheduleBuilder = CronScheduleBuilder.CronSchedule(jobInfo.Cron);
            var triggerKey = KeyManager.CreateTriggerKey(jobInfo.TriggerName, jobInfo.TriggerGroupName);
            ITrigger trigger = TriggerBuilder.Create().StartAt(DateTimeOffset.Now.AddYears(-1))
                    .WithIdentity(jobInfo.TriggerName, jobInfo.TriggerGroupName)
                   .WithSchedule(scheduleBuilder.WithMisfireHandlingInstructionDoNothing())
                    .Build();
            Scheduler.RescheduleJob(triggerKey, trigger);
            return true;
        }

        public override bool RunJob(custom_job_infoes jobInfo)
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
                CronScheduleBuilder scheduleBuilder = CronScheduleBuilder.CronSchedule(jobInfo.Cron);
                ITrigger trigger = TriggerBuilder.Create().StartAt(DateTimeOffset.Now.AddYears(-1))
                    .WithIdentity(jobInfo.TriggerName, jobInfo.TriggerGroupName)
                    .ForJob(jobKey)
                    .WithSchedule(scheduleBuilder.WithMisfireHandlingInstructionDoNothing())
                    .Build();
                Scheduler.ScheduleJob(job, trigger);
            }
            return true;
        }
    }
}
