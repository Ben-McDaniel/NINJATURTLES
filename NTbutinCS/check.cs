using Microsoft.Diagnostics.Tracing.Parsers.Clr;
using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftWindowsWPF;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Security.Cryptography;
using System.Net;
using System.Net.Sockets;

namespace NTbutinCS
{
    internal class check
    {
        private bool usePing = false;
        private static bool mdb = true;
        private int pID;
        private Process target;
        private bool tgt = true;
        //array setup, [time, pid, processOrNot, importsCryptography, pings]
        private String[] result = new String[5];

        public check(int pID)
        {
            this.pID = pID;
            try
            {
                Process target = Process.GetProcessById(pID);
                this.target = target;
            }
            catch
            {
                Console.WriteLine("Couldnt parse");
                tgt = false;
            } 
        }

        public String[] NT()
        {
            Console.WriteLine("Process Started: " + pID);
            DateTime n = DateTime.Now;
            String fn = "log_" + n.ToString().Replace(' ', '_').Replace(":", "-").Replace("/", "-");
            result[0] = fn;
            result[1] = pID.ToString();
            result[2] = "false";
            result[3] = "false";
            result[4] = "false";
            if (tgt)
            {
                
                foreach (ProcessModule mds in target.Modules)
                {
                    Console.WriteLine(mds);

                    //figure out what the dll im looking for is and implement it here
                    if (mds.ModuleName.Equals("Cryptography.dll")) 
                    {
                        result[3] = "true";
                    }

                    if (mds.ModuleName.Equals("Ping.dll"))
                    {
                        usePing = true;
                    }

                }
                result[2] = "true";
                //open threads to poll each part of the attack
                //monitor startup folder, if gets all other results, will set value to false in result array
                Thread sfm = new Thread(new ThreadStart(monitorDir));
                sfm.Start();

                //look for ping
                Thread mp = new Thread(new ThreadStart(monitorPing));
                
                //close both threads
                if (usePing)
                {
                    mp.Start();
                }
                System.Threading.Thread.Sleep(5000);
                sfm.Abort();
                mp.Abort();
            }
            
            /*foreach (var item in result)
            {
                Console.WriteLine(item);
            }*/

            return result;
        }


        //LISTEN FOR PING

        public void monitorPing()
        {
            IPEndPoint ipMyEndPoint = new IPEndPoint(IPAddress.Any, 0);
            EndPoint myEndPoint = (ipMyEndPoint);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp);
            socket.Bind(myEndPoint);
            while (true)
            {
                Byte[] ReceiveBuffer = new Byte[256];
                var nBytes = socket.ReceiveFrom(ReceiveBuffer, 256, 0, ref myEndPoint);
                if (ReceiveBuffer[20] == 3)// ICMP type = Delivery failed
                {
                    result[4] = "true";
                }
                else
                {
                    
                }
            }
        }

        //WATCH STARTUP FOLDER
        public void monitorDir()
        {
            while (mdb)
            {
                String path = @"c:\ProgramData\Microsoft\Windows\Start Menu\Programs\StartUp";
                FileSystemWatcher fsw = new FileSystemWatcher();
                fsw.Path = path;
                fsw.Created += FileSystemWatcher_Created;
            }
        }

        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            mdb = false;
            Console.WriteLine("File created");
            result[1] = "true";
            
        }
    }
}