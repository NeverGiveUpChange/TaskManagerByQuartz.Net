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
            Process process = new Process();
            ServiceController serviceController = new ServiceController();

            foreach (var schedulerInstanceId in schedulerInstanceIdList)
            {
                serviceController.ServiceName = schedulerInstanceId;
                serviceController.Stop();
                if (!System.IO.File.Exists($@"E:\{schedulerInstanceId}\WindowsServerBats\{schedulerInstanceId}_Delete.bat"))
                {
                    FileStream fs1 = new FileStream($@"E:\{schedulerInstanceId}\WindowsServerBats\{schedulerInstanceId}_Delete.bat", FileMode.Create, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs1);
                    sw.Write($"sc Delete {schedulerInstanceId}");

                }
                process.StartInfo.WorkingDirectory = @"E:\{ schedulerInstanceId}\WindowsServerBats";
                process.StartInfo.FileName = $"{schedulerInstanceId}_Delete.bat";
                process.StartInfo.Arguments = "1";
                process.Start();
                process.WaitForExit();
                if (SchedulerData.schedulerInstanceIdEquivalentIp.ContainsKey(schedulerInstanceId))
                {
                    SchedulerData.schedulerInstanceIdEquivalentIp.Remove(schedulerInstanceId);

                }
                if (SchedulerManager.ConnectionCache.ContainsKey(schedulerInstanceId))
                {
                    SchedulerManager.ConnectionCache.Remove(schedulerInstanceId);
                }
            }
            throw new Exception();
        }
        public JsonResult AddScheduler(string ip, string port, string schedulerInstanceId, string threadCount)
        {
            Process process = new Process();
            System.IO.File.Copy(@"E:\TaskManagerByQuartz.Net\Quartz.Net_RemoteServer", $@"E:\{schedulerInstanceId}\Code", true);
            if (!System.IO.File.Exists($@"E:\{schedulerInstanceId}\WindowsServerBats\{schedulerInstanceId}_install.bat"))
            {
                FileStream fs1 = new FileStream($@"E:\{schedulerInstanceId}\WindowsServerBats\{schedulerInstanceId}_install.bat", FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs1);
                sw.Write($@"E:\{schedulerInstanceId}\Code\bin\Release\Quartz.Net_RemoteServer.exe install 
  pause");
                Configuration config = ConfigurationManager.OpenExeConfiguration($@"E:\{schedulerInstanceId}\Code\RemoteServer.config");
                if (config.AppSettings.Settings["localIp"] != null)
                {
                    config.AppSettings.Settings.Remove("localIp");
                }
                if (config.AppSettings.Settings["port"] != null)
                {
                    config.AppSettings.Settings.Remove("port");
                }
                if (config.AppSettings.Settings["intanceId"] != null)
                {
                    config.AppSettings.Settings.Remove("intanceId");
                }

                if (config.AppSettings.Settings["threadCount"] != null)
                {
                    config.AppSettings.Settings.Remove("threadCount");
                }
                config.AppSettings.Settings.Add("localIp", ip);
                config.AppSettings.Settings.Add("port", port);
                config.AppSettings.Settings.Add("schedulerInstanceId", schedulerInstanceId);
                config.AppSettings.Settings.Add("threadCount", threadCount);
                config.Save(ConfigurationSaveMode.Modified);
                process.StartInfo.WorkingDirectory = $@"E:\{schedulerInstanceId}\WindowsServerBats";
                process.StartInfo.FileName = $"{schedulerInstanceId}_install.bat";
                process.StartInfo.Arguments = "1";
                process.Start();
                process.WaitForExit();
            }
            if (!SchedulerData.schedulerInstanceIdEquivalentIp.ContainsKey(schedulerInstanceId))
            {
                SchedulerData.schedulerInstanceIdEquivalentIp.Add(schedulerInstanceId,new schedulerInstanceInfo {  Ip=ip, Port=port, schedulerInstanceId= schedulerInstanceId });

            }
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