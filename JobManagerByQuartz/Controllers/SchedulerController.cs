using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using JobManagerByQuartz.CommonService;
using JobManagerByQuartz.Factories;
using Quartz.Net_Core.JobCommon;
using Quartz.Net_Model;
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
        public JsonResult DeleteSchedulers(List<string> schedulerInstanceIdList)
        {
            throw new Exception();
        }
        public JsonResult AddScheduler(string ip, string port, string schedulerInstanceId)
        {
            throw new Exception();
        }
        /// <summary>
        /// 关闭节点 不可以恢复
        /// </summary>
        /// <param name="schedulerInstanceId"></param>
        /// <param name="waitForJobsToComplete"></param>
        /// <returns></returns>
        public JsonResult ShutDownSchedulers(List<string> schedulerInstanceIdList, bool waitForJobsToComplete)
        {
            foreach (var schedulerInstanceId in schedulerInstanceIdList)
            {
                var scheduler = SchedulerManager.ConnectionCache[schedulerInstanceId];
                if (scheduler != null && !scheduler.IsShutdown)
                {
                    scheduler.Shutdown(waitForJobsToComplete);
                    SchedulerManager.ConnectionCache.Remove(schedulerInstanceId);//删除可用节点连接
                    if (SchedulerData.schedulerInstanceIdEquivalentIp.ContainsKey(schedulerInstanceId))
                    {
                        SchedulerData.schedulerInstanceIdEquivalentIp.Remove(schedulerInstanceId);//删除服务节点
                    }
                    Task.Run(() =>
                    {
                        ServiceController service = new ServiceController(schedulerInstanceId);
                        while (true)
                        {
                            if (scheduler.IsShutdown)
                            {
                                service.Stop();
                                break;
                            }
                            else
                            {
                                service.Stop();
                                break;
                            }
                        }
                    });
                }
            }
            throw new Exception();
        }

        public JsonResult StartSchedulers(List<string> schedulerInstanceIdList)
        {
            foreach (var schedulerInstanceId in schedulerInstanceIdList)
            {
                ServiceController service = new ServiceController(schedulerInstanceId);
                if (service.Status == ServiceControllerStatus.Stopped)
                {
                    service.Start();
                    if (SchedulerData.schedulerInstanceIdEquivalentIp.ContainsKey(schedulerInstanceId))
                    {
                        SchedulerManager.GetScheduler(schedulerInstanceId);
                        //SchedulerData.schedulerInstanceIdEquivalentIp.Add(schedulerInstanceId,new schedulerInstanceInfo { });//增加服务节点
                    }
                }
            }


            throw new Exception();
        }
    }
}