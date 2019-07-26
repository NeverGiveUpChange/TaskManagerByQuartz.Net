using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Quartz.Net_Model.ViewModels
{
    public class AddJobViewModel: IValidatableObject
    {
        private string _jobName;
        /// <summary>
        /// 任务名称
        /// </summary>
        public string JobName
        {
            get { return _jobName; }
            set { _jobName = value; }
        }
        private string _jobGroupName;
        /// <summary>
        /// 任务组名称
        /// </summary>
        public string JobGroupName
        {
            get { return _jobGroupName; }
            set { _jobGroupName = "JobGroupNameFor_" + _jobName; }
        }

        private string _triggerName;
        /// <summary>
        /// 触发器名称
        /// </summary>                                      
        public string TriggerName
        {
            get { return _triggerName; }
            set { _triggerName = "TriggerNameFor_" + _jobName; }
        }
        private string _triggerGroupName;
        /// <summary>
        /// 触发器组名称
        /// </summary>
        public string TriggerGroupName
        {
            get { return _triggerGroupName; }
            set { _triggerGroupName = "TriggerGroupNameFor_" + _jobName; }
        }
        /// <summary>
        /// 触发器类型
        /// </summary>
        public string TriggerType { get; set; }
        /// <summary>
        /// 任务描述
        /// </summary>
        public string JobDescription { get; set; }
        /// <summary>
        /// 请求业务地址
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// Cron模型
        /// </summary>
        public CronJob CronJob { get; set; }
        /// <summary>
        /// Simple模型
        /// </summary>
        public SimpleJob SimpleJob { get; set; }
        /// <summary>
        /// 节点所在ip/name组合
        /// </summary>

        public string SchedulerHost { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResult = new List<ValidationResult>();
            if (string.IsNullOrEmpty(JobName) || !Regex.IsMatch(JobName, @"^[a-zA-Z\d_]+$"))
            {
                validationResult.Add(new ValidationResult("JobName为必填参数且是字母数字或者_组成"));
            }
            else
            {
                JobGroupName = "";
                TriggerName = "";
                TriggerGroupName = "";

            }
            if (TriggerType == "JobCronTrigger")
            {
                if (CronJob == null || string.IsNullOrWhiteSpace(CronJob.Cron))
                {
                    validationResult.Add(new ValidationResult("Cron为必填参数"));
                }
            }
            else
            {
                if (SimpleJob == null || !SimpleJob.RepeatCount.HasValue || SimpleJob.RepeatCount.Value < 1)
                {
                    validationResult.Add(new ValidationResult("当为简单任务时，重复次数是必须的且不小1"));
                }
                if (SimpleJob != null && SimpleJob.RepeatCount.HasValue && SimpleJob.RepeatCount.Value > 1 && !SimpleJob.Cycle.HasValue)
                {

                    validationResult.Add(new ValidationResult("当为简单任务时且重复次数大于1时，重复周期必填"));
                }
            }


            if (string.IsNullOrWhiteSpace(JobDescription))
            {
                validationResult.Add(new ValidationResult("JobDescription为必填参数"));
            }
            if (string.IsNullOrWhiteSpace(RequestUrl))
            {
                validationResult.Add(new ValidationResult("RequestUrl为必填参数"));
            }
            if (string.IsNullOrWhiteSpace(SchedulerHost))
            {
                validationResult.Add(new ValidationResult("SchedulerHost为必填参数"));
            }
            else if (SchedulerHost.Split('/').Length < 1) {
                validationResult.Add(new ValidationResult("节点ip必须拥有"));
            }
            return validationResult;
        }


    }
    public class CronJob
    {
        /// <summary>
        /// 运行周期表达式
        /// </summary>
        public string Cron { get; set; }
    }

    public class SimpleJob
    {
        /// <summary>
        /// 执行周期
        /// </summary>
        public int? Cycle { get; set; }
        /// <summary>
        /// 重复次数
        /// </summary>
        public int? RepeatCount { get; set; }
    }
}
