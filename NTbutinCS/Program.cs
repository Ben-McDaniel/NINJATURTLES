using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using Microsoft.Diagnostics.Tracing.Session;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Security.Permissions;
using System.Collections;
using System.Threading;

namespace NTbutinCS
{
    class Program
    {
       public static ArrayList log = new ArrayList();
        static void Main(string[] args)
        {
            

            //ETW 
            using (var kernelSession = new TraceEventSession(KernelTraceEventParser.KernelSessionName))
            {
                //exit code
                Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs e)
                {
                    //current problem, only writing pid, and seperating each char
                    
                    Console.WriteLine("STOPPED");
                    kernelSession.Dispose();
                    DateTime n = DateTime.Now;
                    String fn = "log_" + n.ToString().Replace(' ', '_').Replace(":", "-").Replace("/", "-") + ".csv";
                    //File.Create(@"D:\NINJATURTLES\NINJATURTLES\logs\" + fn);
                    Console.WriteLine("Log File Created");
                    Console.WriteLine();
                    foreach (String[] sa in log)
                    {

                        foreach (String str in sa)
                        {

                            Console.Write(str + ",");
                        }
                        Console.Write("\n");
                    }
                    using (StreamWriter sw = new StreamWriter(@"D:\NINJATURTLES\NINJATURTLES\logs\" + fn))
                    {

                        foreach(String[] sa in log)
                        {
                            foreach(String str in sa)
                            {
                                sw.Write(str + ",");
                            }
                            sw.Write("\n");
                        }
                       
                    }
                };

                //tracking kernel events
                kernelSession.EnableKernelProvider(
                    KernelTraceEventParser.Keywords.Process
                );

                
                //handle specific types of events



                kernelSession.Source.Kernel.ProcessStart += processStarted;
                //kernelSession.Source.Kernel.ProcessStop += processStopped;

                //start processing
                kernelSession.Source.Process();
            }

        }

        

       
        //can add code here and it will run with ever new process
        private static void processStarted(ProcessTraceData data)
        {
            
            
            int pID = data.ProcessID;
            check ck = new check(pID);
            log.Add(ck.NT());
            
            
        }

      
        //private static void processStopped(ProcessTraceData data) { }
    }
}
