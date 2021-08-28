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
            
           // GetInputMainMenu();
        }

        public static void GetInputMainMenu()
        {
            printMenu();

            string inputStr = Console.ReadLine(); ;
            int input;
            while (!(int.TryParse(inputStr, out input) && checkLimit(input, 1, 4)))
            {
                printMenu();
                inputStr = Console.ReadLine();
            }/*
            switch (input)
            {
                case 1: extractAllFiles();
                    break;
                case 2: compileFiles();
                    break;
                case 3: copyComments();
                    break;
                case 4: Environment.Exit(0);
                    break;
                                
            }*/



        }

        private static bool checkLimit(int i_Value, int i_Low, int i_High)
        {
            return i_Value >= i_Low && i_Value <= i_High;
        }

        private static void printMenu()
        {
            Console.WriteLine("MainMenu");
            Console.WriteLine("******************");
            Console.WriteLine("1. extract all files.");
            Console.WriteLine("2. compile (cpp) files using cl compiler.");
            Console.WriteLine("3. copy comments file.");
            Console.WriteLine("4. exit.");
            Console.WriteLine("Enter choise : ");
        }
    }
}
