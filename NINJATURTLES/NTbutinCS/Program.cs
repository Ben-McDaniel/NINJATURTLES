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
                    Console.WriteLine("LogFileCreated");
                    using (StreamWriter sw = new StreamWriter(@"D:\NINJATURTLES\NINJATURTLES\logs\" + fn))
                    {
                        foreach(String[] i in log)
                        {
                            for(int k = 0; k < i.Length; k++)
                            {
                                sw.Write(i[k] + ",");
                            }
                            
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

        private static void MonitorDirectory()
        {
            String path = @"c:\ProgramData\Microsoft\Windows\Start Menu\Programs\StartUp";
            FileSystemWatcher fsw = new FileSystemWatcher();
            fsw.Path = path;
            fsw.Created += FileSystemWatcher_Created;
        }

        private static void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("File created");
        }

       
        //can add code here and it will run with ever new process
        private static void processStarted(ProcessTraceData data)
        {
            
            
            int pID = data.ProcessID;
            check ck = new check(pID);

            Thread thr = new Thread(new ThreadStart(ck.NT));
            Console.WriteLine("here");
            thr.Start();
            
            try
            {
                
                
                //detect module import
                //currently only printing a single module

                
                
                //Console.WriteLine("[{0}]", string.Join(", ", target.Modules));
                //Console.WriteLine(target.Modules);
                //String[] add = { pID.ToString(), target.Modules.ToString(), "0", "0" };
                //log.Add(add);
                //detect file creation
                //log.AddRange(add);
            }
            catch
            {
                
                //String[] add = { pID.ToString(), "0", "0", "0" };
                //log.Add(add);
                //log.AddRange(add);
            }
            
            
        }

      
        //private static void processStopped(ProcessTraceData data) { }
    }
}
