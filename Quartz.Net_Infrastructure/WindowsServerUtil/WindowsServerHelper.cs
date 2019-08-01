using Quartz.Net_Infrastructure.ConfigHelper;
using Quartz.Net_Infrastructure.FileUtil;
using Quartz.Net_Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.Net_Infrastructure.WindowsServerUtil
{
    public class WindowsServerHelper
    {
        public static Process _process;
        public WindowsServerHelper()
        {
            _process = new Process();
        }
        public void AddWindowsServer(AddSchedulerViewModel addSchedulerViewModel, string opName)
        {
            var workingDirectoryAndFileName = FileHelper.CreateFile(addSchedulerViewModel.SchedulerInstanceId, opName);
            ConfigurationHelper.SetConfig(addSchedulerViewModel);
            _processExcute(workingDirectoryAndFileName.Item1, workingDirectoryAndFileName.Item2, "10");

        }

        public void DeleteWindowsServer(string schedulerInstanceId, string opName)
        {
            var workingDirectoryAndFileName = FileHelper.CreateFile(schedulerInstanceId, opName);
            _processExcute(workingDirectoryAndFileName.Item1, workingDirectoryAndFileName.Item2, "delete");
        }

        private void _processExcute(string workingDirectory, string fileName, string arguments)
        {
            _process.StartInfo.WorkingDirectory = workingDirectory;
            _process.StartInfo.FileName = fileName;
            _process.StartInfo.Arguments = arguments;
            _process.Start();
            _process.WaitForExit();
        }
    }
}
