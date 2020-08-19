using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            //BigNumber a = new BigNumber("12345645");
            //BigNumber b = new BigNumber("654321");
            //Console.WriteLine(a / b);

            BigPointNumber x = new BigPointNumber("123.5");
            BigPointNumber y = new BigPointNumber("2.5");
            Console.WriteLine(x / y);
            Console.ReadKey();
        }
    }
}
