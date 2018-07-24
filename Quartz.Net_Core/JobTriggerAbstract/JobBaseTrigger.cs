
using Quartz.Net_Core.JobCommon;
using Quartz.Net_EFModel_MySql;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quartz.Net_Core.JobTriggerAbstract
{
    public abstract class JobBaseTrigger
    {
        protected IScheduler Scheduler { get; set; }
        public abstract bool RunJob(customer_quartzjobinfo jobInfo);
        public abstract bool ModifyJobCron(customer_quartzjobinfo jobInfo);
        public bool DeleteJob(customer_quartzjobinfo jobInfo) {

            var jobKey = KeyManager.CreateJobKey (jobInfo.JobName, jobInfo.JobGroupName);
            var triggerKey = KeyManager.CreateTriggerKey(jobInfo.TriggerName, jobInfo.TriggerGroupName);
            Scheduler.PauseTrigger(triggerKey);
            Scheduler.UnscheduleJob(triggerKey);
            Scheduler.DeleteJob(jobKey);
            return true;
        }

        public bool PauseJob(customer_quartzjobinfo jobInfo)
        {
            var jobKey = KeyManager.CreateJobKey(jobInfo.JobName, jobInfo.JobGroupName);
            Scheduler.PauseJob(jobKey);
            return true;
        }

        public bool ResumeJob(customer_quartzjobinfo jobInfo)
        {
            var jobKey = KeyManager.CreateJobKey(jobInfo.JobName, jobInfo.JobGroupName);
            Scheduler.ResumeJob(jobKey);
            return true;

        }

        [Obsolete("状态已经由服务列表提供", true)]
        /// <summary>
        /// 获取单个任务状态（从scheduler获取）
        /// </summary>
        /// <param name="jobInfo">任务信息</param>
        /// <returns></returns>
        private TriggerState _getTriggerState(string triggerName, string triggerGroupName)
        {

            TriggerKey triggerKey = KeyManager.CreateTriggerKey(triggerName, triggerGroupName);
            var triggerState = Scheduler.GetTriggerState(triggerKey);

            return triggerState;
        }
        [Obsolete("由于比较消耗性能，同时不利于分布式,未来可能改进，已转移到ICustomerJobInfoRepository下", true)]
        /// <summary>
        /// /获取任务列表
        /// </summary>
        /// <param name="jobStatus">当前任务状态</param>
        /// <param name="pageIndex">当前索引页</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        public object GetJobList(List<customer_quartzjobinfo> customerJobInfoList, int jobStatus, int pageIndex, int pageSize)
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
                JobStartTime = x.JobStartTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                CreateTime = x.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                x.Deleted,
                Customer_TriggerState = x.TriggerState,
                TriggerState = _changeType(_getTriggerState(x.TriggerName, x.TriggerGroupName))
            }).ToList();
            allJobList = jobStatus == 5 || jobStatus == -1 ? allJobList.Where(x => x.Customer_TriggerState == jobStatus).ToList() : allJobList.Where(x => x.TriggerState == jobStatus).ToList();
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
                //x.CreateTime
            });
        }
        [Obsolete("暂时用不到", true)]
        private int _changeType(TriggerState triggerState)
        {
            switch (triggerState)
            {
                case TriggerState.None: return -1;
                case TriggerState.Normal: return 6;
                case TriggerState.Paused: return 1;
                case TriggerState.Complete: return 2;
                case TriggerState.Error: return 3;
                case TriggerState.Blocked: return 4;
                default: return -1;
            }

        }


    }
}
