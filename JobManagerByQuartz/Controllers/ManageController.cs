using JobManagerByQuartz.CommonService;
using JobManagerByQuartz.Factories;
using JobManagerByQuartz.Models;
using Models;
using Quartz.Net_Core.JobTriggerAbstract;
using Quartz.Net_RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobManagerByQuartz.Controllers
{
    public class ManageController : Controller
    {

        public ICustomerJobInfoRepository _customerJobInfoRepository;
        public IServiceGetter _serviceGetter;
        public JobBaseTrigger _triggerBase;

        public ManageController(ICustomerJobInfoRepository _customerJobInfoRepository, IServiceGetter _serviceGetter)
        {
            this._customerJobInfoRepository = _customerJobInfoRepository;
            this._serviceGetter = _serviceGetter;
        }
        // GET: Manage
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 操作任务
        /// </summary>
        /// <param name="jobId">任务编号</param>
        /// <param name="operateJobFunc">具体操作任务的委托</param>
        /// <returns></returns>
        public AjaxResponseData _operateJob(int jobId, Func<custom_job_infoes, bool> operateJobFunc)
        {
            AjaxResponseData ajaxResponseData = null;
            var jobDetail = _customerJobInfoRepository.LoadCustomerInfo(jobId);
            if (jobDetail == null)
            {
                ajaxResponseData = ResponseDataFactory.CreateAjaxResponseData("0", "无此任务", jobDetail);
            }
            else
            {
                _setSpecificTrigger(jobDetail.TriggerType);
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
        private void _setSpecificTrigger(string triggerType)
        {
            _triggerBase = _serviceGetter.GetByKeyed<JobBaseTrigger, string>(triggerType);
        }
    }
}