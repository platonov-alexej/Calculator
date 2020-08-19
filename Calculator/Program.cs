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
            //BigNumber a = new BigNumber("123456");
            //BigNumber b = new BigNumber("654321");
            //Console.WriteLine(a + b);

            BigPointNumber x = new BigPointNumber("12.5");
            BigPointNumber y = new BigPointNumber("80");
            //Console.WriteLine(x + y);
            Console.WriteLine(x * y);
            Console.ReadKey();
        }
    }
}
