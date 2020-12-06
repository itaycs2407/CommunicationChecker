using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessStartInfo psi = new ProcessStartInfo(@"cmd");
            psi.UseShellExecute = false;
            psi.CreateNoWindow = false;
            psi.RedirectStandardInput = true;
            var proc = Process.Start(psi);

            StreamWriter writer = proc.StandardInput;
            writer.WriteLine(@"cd C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\Common7\Tools");
            writer.WriteLine("VsDevCmd.bat");
            writer.WriteLine(@"cl C:\Users\itayco\Desktop\ITAY\client.cpp -o C:\Users\itayco\Desktop\ITAY\momo.exe");
            Console.ReadLine();
            // Process p = new Process("cmd",);

        }
    }
}
