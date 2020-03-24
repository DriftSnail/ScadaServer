using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Topshelf;

namespace ScadaServerHost
{
    public class Program
    {
        public static void Main()
        {
            var rc = HostFactory.Run(x =>                                   //1
            {
                x.Service<MainEntry>(s =>                                   //2
                {
                    s.ConstructUsing(name => new MainEntry());                //3
                    s.WhenStarted(tc => tc.Start());                         //4
                    s.WhenStopped(tc => tc.Stop());                          //5
                });
                x.RunAsLocalSystem();                                       //6

                x.SetDescription("Sample Topshelf Host");                     //服务描述
                x.SetDisplayName("Scada");                                  //8
                x.SetServiceName("Scada");                                  //9

                x.StartAutomaticallyDelayed();

                // 设置服务失败后的操作，分别对应第一次、第二次、后续
                x.EnableServiceRecovery(t =>
                {
                    t.RestartService(0);

                    t.RestartService(0);

                    t.RestartService(0);

                    t.OnCrashOnly();
                });

            });

            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());  //11
            Environment.ExitCode = exitCode;
        }
    }
}
