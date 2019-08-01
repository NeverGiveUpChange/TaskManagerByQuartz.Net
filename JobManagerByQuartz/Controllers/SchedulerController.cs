using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using JobManagerByQuartz.CommonService;
using JobManagerByQuartz.Factories;
using Newtonsoft.Json;
using Quartz.Net_Core.JobCommon;
using Quartz.Net_Infrastructure.WindowsServerUtil;
using Quartz.Net_Model;
using Quartz.Net_Model.DataEnum;
using Quartz.Net_Model.ViewModels;
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
                DeployCount = x.Where(s => s.TriggerState == (int)TriggerStateEnum.Deploy).Count(),
                RunCount = x.Where(s => s.TriggerState == (int)TriggerStateEnum.Run).Count(),
                PauseCount = x.Where(s => s.TriggerState == (int)TriggerStateEnum.Pause).Count(),
                DeletedCount = x.Where(s => s.TriggerState == (int)TriggerStateEnum.Deleted).Count(),
                CompleteCount = x.Where(s => s.TriggerState == (int)TriggerStateEnum.Complete).Count()
            }).ToList();
            var ajaxResponseData = ResponseDataFactory.CreateAjaxResponseData("1", "获取成功", new { SchedulerStatistics = schedulerStatistics });
            return Json(ajaxResponseData, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult DeleteSchedulers(List<string> schedulerInstanceIdList)
        {
            
            ServiceController serviceController = new ServiceController();
            WindowsServerHelper windowsServerHelper = new WindowsServerHelper();
            foreach (var schedulerInstanceId in schedulerInstanceIdList)
            {
                
                serviceController.ServiceName = schedulerInstanceId;
                serviceController.Stop();
                windowsServerHelper.DeleteWindowsServer(schedulerInstanceId, "delete");
                if (SchedulerData.schedulerInstanceIdEquivalentIp.ContainsKey(schedulerInstanceId))
                {
                    SchedulerData.schedulerInstanceIdEquivalentIp.Remove(schedulerInstanceId);

                }
                if (SchedulerManager.ConnectionCache.ContainsKey(schedulerInstanceId))
                {
                    SchedulerManager.ConnectionCache.Remove(schedulerInstanceId);
                }
            }
            var ajaxResponseData = ResponseDataFactory.CreateAjaxResponseData("1", "删除服务节点成功", JsonConvert.SerializeObject(schedulerInstanceIdList));
            return Json(ajaxResponseData);
        }
        [HttpPost]
        public JsonResult AddScheduler(AddSchedulerViewModel addSchedulerViewModel)
        {
            var windowsServerHelper = new WindowsServerHelper();
            windowsServerHelper.AddWindowsServer(addSchedulerViewModel, "install");
            if (!SchedulerData.schedulerInstanceIdEquivalentIp.ContainsKey(addSchedulerViewModel.SchedulerInstanceId))
            {
                SchedulerData.schedulerInstanceIdEquivalentIp.Add(addSchedulerViewModel.SchedulerInstanceId, new schedulerInstanceInfo { Ip = addSchedulerViewModel.Ip, Port = addSchedulerViewModel.Port, schedulerInstanceId = addSchedulerViewModel.SchedulerInstanceId });
            }
            var ajaxResponseData = ResponseDataFactory.CreateAjaxResponseData("1", "增加服务节点成功", JsonConvert.SerializeObject(addSchedulerViewModel));
            return Json(ajaxResponseData);

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
            //todo:参数修正
            return Json(ResponseDataFactory.CreateAjaxResponseData("1", "服务下线成功", 1));
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
            return Json(ResponseDataFactory.CreateAjaxResponseData("1", "服务上线成功", 1));
        }
    }
}