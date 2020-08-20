using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            //BigNumber a = new BigNumber("4");
            //BigNumber b = new BigNumber("11");
            //Console.WriteLine(a % b);

            //BigPointNumber x = new BigPointNumber("4");
            //BigPointNumber y = new BigPointNumber("13");
            //Console.WriteLine(x / y);

            //Stopwatch stopwatch = Stopwatch.StartNew();
            //var res1 = BigPointNumber.Factorial(20000).ToString();
            //stopwatch.Stop();
            //var time = stopwatch.Elapsed;
            //Console.WriteLine($"{res1}  -  Len: {res1.Length}");
            //Console.WriteLine();

            Console.Write("Введите выражение: "); 
            Console.WriteLine(RPN.Calculate(Console.ReadLine())); 

            Console.ReadKey();
        }


    }
}
