using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftWindowsWPF;
using System;
using System.Diagnostics;

namespace NTbutinCS
{
    internal class check
    {
        private int pID;
        private Process target;
        private bool tgt = true;

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

        public void NT()
        {
            Console.WriteLine("Process Started: " + pID);
            
            if (tgt)
            {
                Console.WriteLine("[{0}]", string.Join(", ", target.Modules));
                Console.WriteLine(target.Modules);
            }

            
        }
    }
}