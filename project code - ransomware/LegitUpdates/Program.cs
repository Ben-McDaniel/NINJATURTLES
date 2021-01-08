using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace LegitUpdates
{
    class Program
    {
        static void Main(string[] args)
        {
            var exitCode = HostFactory.Run(x =>
            {
                x.Service<Updates>(s =>
                {
                    s.ConstructUsing(update => new Updates());
                    s.WhenStarted(update => update.Start());
                    s.WhenStopped(update => update.Stop());
                });

                x.RunAsLocalSystem();

                x.SetServiceName("LegitUpdates");
                x.SetDisplayName("Adobe Updates or Something");
                x.SetDescription("This is totally not malware");
            });

            int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;
        }
    }
}
