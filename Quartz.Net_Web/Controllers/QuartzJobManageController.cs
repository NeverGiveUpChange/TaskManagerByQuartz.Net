
using Quartz.Net_EFModel_MySql;
using Quartz.Net_RepositoryInterface;
using Quartz.Net_Web.Factories;
using Quartz.Net_Web.Models;
using Quartz.Net_Web.Quartz.NetSchedulerManager;
using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quartz.Net_Web.Controllers
{
    [Export]
    public class QuartzJobManageController : Controller
    {

        [Import("CustomerJobInfoRepository")]
        public ICustomerJobInfoRepository _customerJobInfoRepository { get; set; }
        [Import("JobHelper")]
        private JobHelper _jobHelper { get; set; }
        public QuartzJobManageController()
        {
        }
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }
        // GET: Default/Create
        public ActionResult Test()
        {
            return View();
        }
        // GET: Default/Edit/5
        public ActionResult Test1()
        {
            return View();
        }
        [HttpPost]
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="jobName">任务名称</param>
        /// <param name="jobGroupName">任务所在组名称</param>
        /// <param name="triggerName">触发器名称</param>
        /// <param name="triggerGroupName">触发器所在的组名称</param>
        /// <param name="cron">执行周期表达式</param>
        /// <param name="jobDescription">任务描述</param>
        /// <param name="requestUrl">请求地址</param>
        /// <returns></returns>

        public JsonResult AddJob(string jobName, string jobGroupName, string triggerName, string triggerGroupName, string cron,  string jobDescription,string requestUrl)
        {
            var jobId = _customerJobInfoRepository.AddCustomerJobInfo(jobName, jobGroupName, triggerName, triggerGroupName, cron, jobDescription, requestUrl);
            return Json(ResponseDataFactory.CreateAjaxResponseData("1", "添加成功", jobId));

        }

        /// <summary>
        /// 更改任务执行周期（任务运行中）
        /// </summary>
        /// <param name="jobId">任务编号</param>
        /// <param name="cron">任务执行表达式</param>
        /// <returns></returns>
        public JsonResult ModifyJobCron(int jobId, string cron)
        {

            var ajaxResponseData = _operateJob(jobId, (jobDetail) => { jobDetail.Cron = cron; _customerJobInfoRepository.UpdateCustomerJobInfo(jobDetail); return _jobHelper.ModifyJobCron(jobDetail); });
            return Json(ajaxResponseData);
        }
        [HttpPost]
        /// <summary>
        /// 启动任务
        /// </summary>
        /// <param name="jobId">任务编号</param>
        /// <returns></returns>
        public JsonResult RunJob(int jobId)
        {
            var ajaxResponseData = _operateJob(jobId, (jobDetail) => { jobDetail.TriggerState = 0; _customerJobInfoRepository.UpdateCustomerJobInfo(jobDetail); return _jobHelper.RunJob(jobDetail); });
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
            var ajaxResponseData = _operateJob(jobId, (jobDetail) => { jobDetail.Deleted = true; jobDetail.TriggerState = 5; _customerJobInfoRepository.UpdateCustomerJobInfo(jobDetail); return _jobHelper.DeleteJob(jobDetail); });
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
            var ajaxResponseData = _operateJob(jobId, (jobDetail) => { jobDetail.TriggerState = 1; _customerJobInfoRepository.UpdateCustomerJobInfo(jobDetail); return _jobHelper.PauseJob(jobDetail); });
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
            var ajaxResponseData = _operateJob(jobId, (jobDetail) => { jobDetail.TriggerState = 0; _customerJobInfoRepository.UpdateCustomerJobInfo(jobDetail); return _jobHelper.ResumeJob(jobDetail); });
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
            var jobQueryable = _customerJobInfoRepository.LoadCustomerInfoes(x => x.TriggerState == jobStatus, x => x.Id, false, pageIndex, pageSize);
            var JobList = jobQueryable.Item1.ToList().Select(x => new
            {
                x.Id,
                x.JobName,
                x.JobGroupName,
                x.TriggerName,
                x.TriggerGroupName,
                x.Description,
                x.Cron,
                JobStartTime = x.JobStartTime.HasValue ? x.JobStartTime.Value.ToString("yyyy-MM-dd HH:mm:ss"):"",
                CreateTime = x.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                x.Exception
            }).ToList();
            var ajaxResponseData = ResponseDataFactory.CreateAjaxResponseData("1", "获取成功", new { JobList = JobList, TotalCount = jobQueryable.Item2 });
            return Json(ajaxResponseData, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 操作任务
        /// </summary>
        /// <param name="jobId">任务编号</param>
        /// <param name="operateJobFunc">具体操作任务的委托</param>
        /// <returns></returns>
        private AjaxResponseData _operateJob(int jobId, Func<customer_quartzjobinfo, bool> operateJobFunc)
        {
            AjaxResponseData ajaxResponseData = null;
            var jobDetail = _customerJobInfoRepository.LoadCustomerInfo(x => x.Id == jobId);
            if (jobDetail == null)
            {
                ajaxResponseData = ResponseDataFactory.CreateAjaxResponseData("0", "无此任务", jobDetail);
            }
            else
            {
                var isSuccess = operateJobFunc(jobDetail);
                if (isSuccess)
                {
                    ajaxResponseData = ResponseDataFactory.CreateAjaxResponseData("1", "操作成功", jobDetail);
                }
                else
                {
                    ajaxResponseData = ResponseDataFactory.CreateAjaxResponseData("-10001", "操作失败", jobDetail);
                }
            }
            return ajaxResponseData;
        }
     
    }
}
