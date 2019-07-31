using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.Net_Infrastructure.FileUtil
{
    public class FileHelper
    {
        static string batFormatFilePath = @"E:\{ 0}\WindowsServerBats\{1}_{2}.bat";
        public bool Exists(string schedulerInstanceId,string opName)
        {
            string batFilePath = string.Format(batFormatFilePath, schedulerInstanceId,schedulerInstanceId, opName);
            return File.Exists(batFilePath);
        }

        public bool Copy(string schedulerInstanceId,string opName)
        {
            string fileFromPath = $@"E:\TaskManagerByQuartz.Net\Quartz.Net_RemoteServer";
            string fielToPath = $@"E:\{schedulerInstanceId}\Code";
            string batFilePath = string.Format(batFormatFilePath, schedulerInstanceId, schedulerInstanceId, opName);
            if (!Exists(schedulerInstanceId, opName)) {
                using (FileStream fs1 = new FileStream(batFilePath, FileMode.Create, FileAccess.Write)) {
                    using (StreamWriter sw = new StreamWriter(fs1))
                    {
                        sw.Write($@"E:\{schedulerInstanceId}\Code\bin\Release\Quartz.Net_RemoteServer.exe install 
  pause");
                    }
                }
            }
            throw new Exception();

        }
    }
}
