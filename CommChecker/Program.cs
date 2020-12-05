using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter working Directory : ");
            string workingDirectory = Console.ReadLine();
            Console.WriteLine("Enter comments filename : ");
            string commentFileName = Console.ReadLine();
            CheckController cc = new CheckController(workingDirectory, commentFileName);
            cc.Run();
            Console.WriteLine("Program finished. Press any key....");
            Console.ReadLine();
        }
    }
}
