using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobManagerByQuartz.CommonService;
using JobManagerByQuartz.Factories;
using Quartz.Net_Core.JobCommon;
using Quartz.Net_RepositoryInterface;

namespace JobManagerByQuartz.Controllers
{
    public class SchedulerController : ManageController
    {
        public SchedulerController(ICustomerJobInfoRepository _customerJobInfoRepository, IServiceGetter _serviceGetter) : base(_customerJobInfoRepository, _serviceGetter)
        {
        }
        /// <summary>
        /// 获取每个节点当前的任务统计信息
        /// </summary>
        /// <param name="searchWhere"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetSchedulerStatistics(string searchWhere)
        {
            var tempCustomerJobInfoList = _customerJobInfoRepository.LoadSchedulerInfoes(x => x.CurrentSchedulerHost == searchWhere || x.CurrentSchedulerHostName == searchWhere || x.CurrentSchedulerInstanceId == searchWhere);
            var schedulerStatistics = tempCustomerJobInfoList.GroupBy(x => x.CurrentSchedulerHost).Select(x => new
            {
                SchedulerHost = x.Key,
                SchedulerHostName = x.FirstOrDefault().CurrentSchedulerHostName,
                SchedulerInstanceId = x.FirstOrDefault().CurrentSchedulerInstanceId,
                IsNormalScheduler = true,
                TotalCount = x.Count(),
                NotStarttingCount = x.Where(s => s.TriggerState == 1).Count(),
                StarttingCount = x.Where(s => s.TriggerState == 2).Count(),
                PauseCount = x.Where(s => s.TriggerState == 3).Count(),
                DeletedCount = x.Where(s => s.TriggerState == 4).Count()
            }).ToList();
            var ajaxResponseData = ResponseDataFactory.CreateAjaxResponseData("1", "获取成功", new { SchedulerStatistics = schedulerStatistics });
            return Json(ajaxResponseData, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DeleteSchedulers(string schedulerInstanceId)
        {

            throw new Exception();
        }
        public JsonResult AddScheduler(string ip, string port, string schedulerInstanceId)
        {
            throw new Exception();
        }

        public JsonResult ShutDownSchedulers(List<string> schedulerInstanceIdList, bool waitForJobsToComplete)
        {
            foreach (var item in schedulerInstanceIdList)
            {
                var scheduler = SchedulerManager.ConnectionCache[item];
                if (!scheduler.IsShutdown)
                {
                    scheduler.Shutdown(waitForJobsToComplete);
                }
            }
            throw new Exception();

        }

        public JsonResult StartDelayedSchedulers(List<string> schedulerInstanceIdList, int delayedSeconds)
        {
            foreach (var item in schedulerInstanceIdList)
            {
                var scheduler = SchedulerManager.ConnectionCache[item];
                if (scheduler.IsShutdown) {
                    scheduler.StartDelayed(TimeSpan.FromSeconds(delayedSeconds));
                }
            }
            throw new Exception();
        }
    }
}