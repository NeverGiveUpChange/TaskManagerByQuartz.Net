using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.Net_Model.ViewModels
{
    public class UpdateJobInfoViewModel:IValidatableObject
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        public string JobName { get; set; }
        /// <summary>
        /// 任务状态
        /// </summary>
        public int JobState { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool Deleted { get; set; }
        /// <summary>
        /// 异常信息
        /// </summary>
        public string Exception { get; set; }
        /// <summary>
        /// 上次执行时间
        /// </summary>
        public DateTime? PreTime { get; set; }
        /// <summary>
        /// 下次执行时间
        /// </summary>
        public DateTime? NextTime { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResult = new List<ValidationResult>();
            if (string.IsNullOrWhiteSpace(JobName))
            {
                validationResult.Add(new ValidationResult("JobName为必填参数"));
            }
            if (JobState == 0)
            {
                validationResult.Add(new ValidationResult("JobState为必填参数"));
            }
            return validationResult;
        }
    }
}
