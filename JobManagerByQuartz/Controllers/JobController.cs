﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobManagerByQuartz.CommonService;
using JobManagerByQuartz.Factories;
using Models;
using Quartz.Net_Model.ViewModels;
using Quartz.Net_RepositoryInterface;

namespace JobManagerByQuartz.Controllers
{
    public class JobController :ManageController
    {
        public JobController(ICustomerJobInfoRepository _customerJobInfoRepository, IServiceGetter _serviceGetter) : base(_customerJobInfoRepository, _serviceGetter)
        {
        }

        [HttpPost]
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="addJobViewModel">添加任务模型</param>
        /// <returns></returns>

        public JsonResult AddJob(AddJobViewModel addJobViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Json(ResponseDataFactory.CreateAjaxResponseData("-10001", "添加失败", "输入参数错误:" + ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "))));
            }
            var values = addJobViewModel.SchedulerHost.Split('/');
            var host = values[0];
            var name = string.Empty;
            var instanceId = string.Empty;
            if (host.Length > 1 && host.Length <= 2)
            {
                name = addJobViewModel.SchedulerHost.Split('/')[1];
            }
            else
            {
                instanceId = addJobViewModel.SchedulerHost.Split('/')[2];
            }
            var customeJobInfo = new custom_job_infoes() { CreateTime = DateTime.Now, Cron = addJobViewModel.CronJob.Cron, CurrentSchedulerHost = host, CurrentSchedulerHostName = name, CurrentSchedulerInstanceId = instanceId, Cycle = addJobViewModel.SimpleJob.Cycle, Deleted = 0, Description = addJobViewModel.JobDescription, DllName = "Quartz.Net_Core.dll", JobName = addJobViewModel.JobName, FullJobName = "Quartz.Net_Core.JobExcute.JobItem", OriginSchedulerHost = host, OriginSchedulerHostName = name, OriginSchedulerInstanceId = instanceId, RepeatCount = addJobViewModel.SimpleJob.RepeatCount, RequestUrl = addJobViewModel.RequestUrl, TriggerState = 0, TriggerType = addJobViewModel.TriggerType };
            var jobId = _customerJobInfoRepository.AddCustomerJobInfo(customeJobInfo);
            return Json(ResponseDataFactory.CreateAjaxResponseData("1", "添加成功", jobId));

        }
        [HttpPost]
        /// <summary>
        /// 更改任务执行周期（任务运行中）
        /// </summary>
        /// <param name="jobId">任务编号</param>
        /// <param name="cron">任务执行表达式</param>
        /// <returns></returns>
        public JsonResult ModifyJobCron(int jobId, string cron)
        {
            var ajaxResponseData = _operateJob(jobId, (jobDetail) => { jobDetail.Cron = cron; _customerJobInfoRepository.UpdateCustomerJobInfo(x => new custom_job_infoes { Cron = cron }, x => x.Id == jobId); return _triggerBase.ModifyJobCron(jobDetail); });
            return Json(ajaxResponseData);

        }
        /// <summary>
        /// 提供服务调用更改jobInfo接口
        /// </summary>
        /// <param name="updateJobInfoViewModel"></param>
        /// <returns></returns>
        public JsonResult UpdateJobInfo(UpdateJobInfoViewModel updateJobInfoViewModel)
        {

            var jobName = _customerJobInfoRepository.UpdateCustomerJobInfo(x => new custom_job_infoes { JobName = updateJobInfoViewModel.JobName, Deleted = 1, TriggerState = updateJobInfoViewModel.JobState, PreTime = updateJobInfoViewModel.PreTime, NextTime = updateJobInfoViewModel.NextTime }, x => x.JobName == updateJobInfoViewModel.JobName);
            return Json(ResponseDataFactory.CreateAjaxResponseData("1", "操作成功", jobName));

        }
        [HttpPost]
        /// <summary>
        /// 启动任务
        /// </summary>
        /// <param name="jobId">任务编号</param>
        /// <returns></returns>
        public JsonResult RunJob(int jobId)
        {
            var ajaxResponseData = _operateJob(jobId, (jobDetail) => { return _triggerBase.RunJob(jobDetail); });
            return Json(ajaxResponseData);
        }
        [HttpPost]
        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="jobId">任务编号</param>
        /// <returns></returns>
        public JsonResult DeleteJob(int jobId)
        {
            var ajaxResponseData = _operateJob(jobId, (jobDetail) => { return _triggerBase.DeleteJob(jobDetail); });
            return Json(ajaxResponseData);
        }
        [HttpPost]
        /// <summary>
        /// 暂停任务
        /// </summary>
        /// <param name="jobId">任务编号</param>
        /// <returns></returns>
        public JsonResult PauseJob(int jobId)
        {
            var ajaxResponseData = _operateJob(jobId, (jobDetail) => { return _triggerBase.PauseJob(jobDetail); });
            return Json(ajaxResponseData);
        }
        [HttpPost]
        /// <summary>
        /// 恢复任务
        /// </summary>
        /// <param name="jobId">任务编号</param>
        /// <returns></returns>
        public JsonResult ResumeJob(int jobId)
        {
            var ajaxResponseData = _operateJob(jobId, (jobDetail) => { return _triggerBase.ResumeJob(jobDetail); });
            return Json(ajaxResponseData);

        }
        [HttpGet]
        /// <summary>
        /// 获取任务
        /// </summary>
        /// <param name="jobStatus">任务状态</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页数量</param>
        /// <returns></returns>

        public JsonResult GetJobList(int jobStatus, int pageIndex, int pageSize)
        {

            var jobQueryable = _customerJobInfoRepository.LoadPageCustomerInfoes(x => x.TriggerState == jobStatus, x => new { x.Id }, false, pageIndex, pageSize);
            var JobList = jobQueryable.Item1.ToList().Select(x => new
            {
                x.Id,
                x.JobName,
                TriggerType = x.TriggerType == "JobCronTrigger" ? "复杂任务" : "简单任务",
                x.Description,
                x.Cron,
                PreTime = x.PreTime.HasValue ? x.PreTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                NextTime = x.NextTime.HasValue ? x.NextTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                JobStartTime = x.JobStartTime.HasValue ? x.JobStartTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                CreateTime = x.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),

            }).ToList();
            var ajaxResponseData = ResponseDataFactory.CreateAjaxResponseData("1", "获取成功", new { JobList = JobList, TotalCount = jobQueryable.Item2 });
            return Json(ajaxResponseData, JsonRequestBehavior.AllowGet);

        }
    }
}