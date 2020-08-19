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

            BigPointNumber x = new BigPointNumber("113457645684678.453457684568467854");
            BigPointNumber y = new BigPointNumber("234.4545685678567567");
            //Console.WriteLine(x + y);
            Console.WriteLine(x - y);
            Console.ReadKey();
        }
    }
}
