using Quartz.Net_Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.Net_Infrastructure.ConfigHelper
{
    public class ConfigurationHelper
    {
        static string formatConfigPath = @"E:\{0}\Code\RemoteServer.config";
        public static void SetConfig(AddSchedulerViewModel addSchedulerViewModel)
        {

            var configPath = string.Format(formatConfigPath, addSchedulerViewModel.SchedulerInstanceId);
            Configuration config = ConfigurationManager.OpenExeConfiguration(configPath);

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
            config.AppSettings.Settings.Add("localIp", addSchedulerViewModel.Ip);
            config.AppSettings.Settings.Add("port", addSchedulerViewModel.Port);
            config.AppSettings.Settings.Add("schedulerInstanceId", addSchedulerViewModel.SchedulerInstanceId);
            config.AppSettings.Settings.Add("threadCount", addSchedulerViewModel.ThreadCount);
            config.Save(ConfigurationSaveMode.Modified);
        }

    }
}
