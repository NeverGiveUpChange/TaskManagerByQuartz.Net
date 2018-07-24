
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Quartz.Net_RemoteServer
{
    class Program
    {
        static void Main(string[] args)

        {
            HostFactory.Run(x =>
            {
                x.Service<QuartzServer>();
                x.SetDescription(Configuration.Description);
                x.SetDisplayName(Configuration.DisplayName);
                x.SetServiceName(Configuration.ServiceName);
                x.EnablePauseAndContinue();

                x.RunAsLocalSystem();
            });

            Console.ReadKey();

        }
    }
}
