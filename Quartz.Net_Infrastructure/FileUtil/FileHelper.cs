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
        static string batFormatWorkingDirectory = @"E:\{ 0}\WindowsServerBats";
        static string batFormatFileName = "{0}_{1}.bat";
        static string formatFilePath = @"{0}\{1}";
        public static bool Exists(string filePath)
        {
            return File.Exists(filePath);
        }

        public static (string, string) CreateFile(string schedulerInstanceId, string opName)
        {
            string fileFromPath = $@"E:\TaskManagerByQuartz.Net\Quartz.Net_RemoteServer";
            string fielToPath = $@"E:\{schedulerInstanceId}\Code";
            string batWorkingDirectory = string.Format(batFormatWorkingDirectory, schedulerInstanceId);
            string batFileName = string.Format(batFormatFileName, schedulerInstanceId, opName);
            string filePath = string.Format(formatFilePath, batWorkingDirectory, batFileName);
            File.Copy(fileFromPath, fielToPath, true);
            if (!Exists(filePath))
            {
                using (FileStream fs1 = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs1))
                    {
                        if (opName == "install")
                        {
                            sw.Write($@"E:\{schedulerInstanceId}\Code\bin\Release\Quartz.Net_RemoteServer.exe install 
  pause");
                        }
                        else if (opName == "delete")
                        {
                            sw.Write($"sc delete {schedulerInstanceId}");
                        }

                    }
                }
            }
            return (batWorkingDirectory, batFileName);
        }
    }
}
