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

            double dres = 0;
            BigPointNumber res = new BigPointNumber(0);
            for (int i = 0; i <= 100000; i++)
            {
                BigPointNumber fraction = new BigPointNumber(4) / new BigPointNumber(2 * i + 1);
                double dfraction = 4.0 / (2 * i + 1);
                if (i % 2 == 0)
                {
                    res = res + fraction;
                    dres = dres + 4.0 / (2 * i + 1);
                    //Console.WriteLine("                               " + res);
                }
                else
                {
                    res = res - fraction;
                    dres = dres - 4.0 / (2 * i + 1);
                    //Console.WriteLine(res);
                }
            }
            Console.WriteLine(res);

            //double res = 0;
            //for (int i = 0; i <= 1000; i++)
            //{
            //    if (i % 2 == 0)
            //    {
            //        res = res + 4.0 / (2 * i + 1);
            //        Console.WriteLine("     " + res);
            //    }

            //    else
            //    {
            //        res = res - 4.0 / (2 * i + 1);
            //        Console.WriteLine(res);
            //    }

            //}
            //Console.WriteLine(res);


            //for (int i = 2; i <= 1500; i++)
            //{
            //    var res = BigPointNumber.Factorial(i).ToString();
            //    Console.WriteLine($"{res}  -  Len: {res.Length}");
            //    Console.WriteLine();
            //}

            Console.ReadKey();
        }


    }
}
