
using Quartz.Net_EFModel;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using System.ComponentModel.Composition;

namespace Quartz.Net_Web.Quartz.NetSchedulerManager
{
    [Export("JobHelper", typeof(JobHelper))]
    public class JobHelper
    {
        private readonly IScheduler _scheduler;
        public JobHelper()
        {
            _scheduler = SchedulerManager.Instance;
        }
        /// <summary>
        /// 运行任务
        /// </summary>
        /// <param name="jobInfo">任务信息</param>
        /// <returns></returns>
        public bool RunJob(Customer_JobInfo jobInfo)
        {
            Assembly assembly = Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + $"bin/{jobInfo.DLLName}");
            var type = assembly.GetType(jobInfo.FullJobName);
            JobKey jobKey = _createJobKey(jobInfo.JobName, jobInfo.JobGroupName);
            if (!_scheduler.CheckExists(jobKey))
            {
                IJobDetail job = JobBuilder.Create(type)
                    .WithIdentity(jobKey)
                    .UsingJobData(_createJobDataMap("jobId", jobInfo.Id))
                    .UsingJobData(_createJobDataMap("requestUrl",jobInfo.RequestUrl))//添加此任务请求地址附带到Context上下文中
                    .Build();

                CronScheduleBuilder scheduleBuilder = CronScheduleBuilder.CronSchedule(jobInfo.Cron);
                ITrigger trigger = TriggerBuilder.Create().StartNow()//StartAt(DateTime.SpecifyKind(jobInfo.JobStartTime, DateTimeKind.Local))
                    .WithIdentity(jobInfo.TriggerName, jobInfo.TriggerGroupName)
                    .ForJob(jobKey)
                    .WithSchedule(scheduleBuilder.WithMisfireHandlingInstructionDoNothing())
                    .Build();
                #region Quartz 任务miss之后三种操作
                /*
             withMisfireHandlingInstructionDoNothing
——不触发立即执行
——等待下次Cron触发频率到达时刻开始按照Cron频率依次执行

withMisfireHandlingInstructionIgnoreMisfires
——以错过的第一个频率时间立刻开始执行
——重做错过的所有频率周期后
——当下一次触发频率发生时间大于当前时间后，再按照正常的Cron频率依次执行

withMisfireHandlingInstructionFireAndProceed
——以当前时间为触发频率立刻触发一次执行
——然后按照Cron频率依次执行*/
                #endregion

                _scheduler.ScheduleJob(job, trigger);

            }
            return true;
        }
        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="jobInfo">任务信息</param>
        /// <returns></returns>

        public bool DeleteJob(Customer_JobInfo jobInfo)
        {
            var jobKey = _createJobKey(jobInfo.JobName, jobInfo.JobGroupName);
            var triggerKey = _createTriggerKey(jobInfo.TriggerName, jobInfo.TriggerGroupName);
            _scheduler.PauseTrigger(triggerKey);
            _scheduler.UnscheduleJob(triggerKey);
            _scheduler.DeleteJob(jobKey);
            return true;
        }

        /// <summary>
        /// 暂停任务
        /// </summary>
        /// <param name="jobInfo">任务信息</param>
        /// <returns></returns>
        public bool PauseJob(Customer_JobInfo jobInfo)
        {
            var jobKey = _createJobKey(jobInfo.JobName, jobInfo.JobGroupName);
            _scheduler.PauseJob(jobKey);
            return true;
        }

        /// <summary>
        /// 恢复任务
        /// </summary>
        /// <param name="jobInfo">任务信息</param>
        /// <returns></returns>
        public bool ResumeJob(Customer_JobInfo jobInfo)
        {
            var jobKey = _createJobKey(jobInfo.JobName, jobInfo.JobGroupName);
            _scheduler.ResumeJob(jobKey);
            return true;

        }
        /// <summary>
        /// 更改任务运行周期
        /// </summary>
        /// <param name="jobInfo">任务信息</param>
        /// <returns></returns>
        public bool ModifyJobCron(Customer_JobInfo jobInfo)
        {
            CronScheduleBuilder scheduleBuilder = CronScheduleBuilder.CronSchedule(jobInfo.Cron);
            var triggerKey = _createTriggerKey(jobInfo.TriggerName, jobInfo.TriggerGroupName);
            ITrigger trigger = TriggerBuilder.Create().StartNow()
                    .WithIdentity(jobInfo.TriggerName, jobInfo.TriggerGroupName)
                   .WithSchedule(scheduleBuilder.WithMisfireHandlingInstructionDoNothing())
                    .Build();
            _scheduler.RescheduleJob(triggerKey, trigger);
            return true;
        }
        /// <summary>
        /// 获取单个任务状态（从scheduler获取）
        /// </summary>
        /// <param name="jobInfo">任务信息</param>
        /// <returns></returns>
        private TriggerState _getTriggerState(string triggerName, string triggerGroupName)
        {

            TriggerKey triggerKey = _createTriggerKey(triggerName, triggerGroupName);
            var triggerState = _scheduler.GetTriggerState(triggerKey);

            return triggerState;
        }
        /// <summary>
        /// /获取任务列表
        /// </summary>
        /// <param name="jobStatus">当前任务状态</param>
        /// <param name="pageIndex">当前索引页</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        public object GetJobList(List<Customer_JobInfo> customerJobInfoList, int jobStatus, int pageIndex, int pageSize)
        {
            var allJobList = customerJobInfoList.Select(x => new
            {
                x.Id,
                x.JobName,
                x.JobGroupName,
                x.TriggerName,
                x.TriggerGroupName,
                x.Description,
                x.Cron,
                JobStartTime = x.JobStartTime.ToString("yyyy-MM-dd HH:mm:ss"),
                CreateTime = x.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                x.Deleted,
                Customer_TriggerState = x.TriggerState,
                TriggerState = _changeType(_getTriggerState(x.TriggerName, x.TriggerGroupName))
            }).ToList();
            allJobList = jobStatus == 5 || jobStatus == -1 ? allJobList.Where(x => x.Customer_TriggerState == jobStatus).ToList() : allJobList.Where(x => x.TriggerState == jobStatus ).ToList();
            return allJobList.Select(x => new
            {
                x.Id,
                x.JobName,
                x.JobGroupName,
                x.TriggerName,
                x.TriggerGroupName,
                x.Description,
                x.Cron,
                x.JobStartTime,
                x.CreateTime
            });
        }
        private JobKey _createJobKey(string jobName, string jobGroupName)
        {
            return new JobKey(jobName, jobGroupName);

        }
        private TriggerKey _createTriggerKey(string triggerName, string triggerGroupName)
        {
            return new TriggerKey(triggerName, triggerGroupName);
        }
        private int _changeType(TriggerState triggerState)
        {
            switch (triggerState)
            {
                case TriggerState.None: return -1;
                case TriggerState.Normal: return 0;
                case TriggerState.Paused: return 1;
                case TriggerState.Complete: return 2;
                case TriggerState.Error: return 3;
                case TriggerState.Blocked: return 4;
                default: return -1;
            }

        }
        private JobDataMap _createJobDataMap<T>(string propertyName, T propertyValue)
        {
            return new JobDataMap(new Dictionary<string, T>() { { propertyName, propertyValue } });
        }

    }
}