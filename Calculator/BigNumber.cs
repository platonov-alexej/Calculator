using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public enum Sign
        {
            Minus = -1,
            Plus = 1
        }
    
    class BigNumber
    {
        private readonly List<byte> digits = new List<byte>();
        public Sign Sign { get; private set; } = Sign.Plus;



        public BigNumber(List<byte> bytes)
        {
            digits = bytes.ToList();
            RemoveNulls();
        }

        public BigNumber(Sign sign, List<byte> bytes)
        {
            Sign = sign;
            digits = bytes;
            RemoveNulls();
        }

        public BigNumber(string s)
        {
            if (s.StartsWith("-"))
            {
                Sign = Sign.Minus;
                s = s.Substring(1);
            }

            foreach (var c in s.Reverse())
            {
                digits.Add(Convert.ToByte(c.ToString()));
            }

            RemoveNulls();
        }

        public BigNumber(uint x) => digits.AddRange(GetBytes(x));

        public BigNumber(int x)
        {
            if (x < 0)
            {
                Sign = Sign.Minus;
            }

            digits.AddRange(GetBytes((uint)Math.Abs(x)));
        }

        private List<byte> GetBytes(uint num)
        {
            var bytes = new List<byte>();
            while (num > 0)
            {
                bytes.Add((byte)(num % 10));
                num /= 10;
            }

            return bytes;
        }

        private void RemoveNulls()
        {
            for (var i = digits.Count - 1; i > 0; i--)
            {
                if (digits[i] == 0)
                {
                    digits.RemoveAt(i);
                }
                else
                {
                    break;
                }
            }
        }

        public static BigNumber Zero => new BigNumber(0);

        public static BigNumber One => new BigNumber(1);

        //длина числа
        public int Size => digits.Count;

        //знак числа
        

        //получение цифры по индексу
        //если индекс больше длины возвращает 0
        public byte GetByte(int i) => i < Size ? digits[i] : (byte)0;

        //установка цифры по индексу
        public void SetByte(int i, byte b)
        {
            while (digits.Count <= i)
            {
                digits.Add(0);
            }

            digits[i] = b;
        }

        public static BigNumber Exp(byte val, int exp)
        {
            var bigInt = Zero;
            bigInt.SetByte(exp, val);
            bigInt.RemoveNulls();
            return bigInt;
        }

        public override string ToString()
        {
            var s = new StringBuilder(Sign == Sign.Plus ? "" : "-");

            for (var i = digits.Count - 1; i >= 0; i--)
            {
                s.Append(Convert.ToString(digits[i]));
            }

            return s.ToString();
        }

        private static int Comparison(BigNumber a, BigNumber b, bool ignoreSign = false)
        {
            return CompareSign(a, b, ignoreSign);
        }

        private static int CompareSign(BigNumber a, BigNumber b, bool ignoreSign = false)
        {
            if (!ignoreSign)
            {
                if (a.Sign < b.Sign)
                {
                    return -1;
                }
                else if (a.Sign > b.Sign)
                {
                    return 1;
                }
            }

            return CompareSize(a, b);
        }

        private static int CompareSize(BigNumber a, BigNumber b)
        {
            if (a.Size < b.Size)
            {
                return -1;
            }
            else if (a.Size > b.Size)
            {
                return 1;
            }

            return CompareDigits(a, b);
        }

        private static int CompareDigits(BigNumber a, BigNumber b)
        {
            var maxLength = Math.Max(a.Size, b.Size);
            for (var i = maxLength; i >= 0; i--)
            {
                if (a.GetByte(i) < b.GetByte(i))
                {
                    return -1;
                }
                else if (a.GetByte(i) > b.GetByte(i))
                {
                    return 1;
                }
            }

            return 0;
        }

        public static bool operator <(BigNumber a, BigNumber b) => Comparison(a, b) < 0;

        public static bool operator >(BigNumber a, BigNumber b) => Comparison(a, b) > 0;

        public static bool operator <=(BigNumber a, BigNumber b) => Comparison(a, b) <= 0;

        public static bool operator >=(BigNumber a, BigNumber b) => Comparison(a, b) >= 0;

        public static bool operator ==(BigNumber a, BigNumber b) => Comparison(a, b) == 0;

        public static bool operator !=(BigNumber a, BigNumber b) => Comparison(a, b) != 0;

        public override bool Equals(object obj) => !(obj is BigNumber) ? false : this == (BigNumber)obj;

        private static BigNumber Add(BigNumber a, BigNumber b)
        {
            var digits = new List<byte>();

            var maxLength = Math.Max(a.Size, b.Size);
            byte t = 0;
            for (int i = 0; i < maxLength; i++)
            {
                byte sum = (byte)(a.GetByte(i) + b.GetByte(i) + t);
                if (sum > 10)
                {
                    sum -= 10;
                    t = 1;
                }
                else
                {
                    t = 0;
                }

                digits.Add(sum);
            }

            if (t > 0)
            {
                digits.Add(t);
            }

            return new BigNumber(a.Sign, digits);
        }

        private static BigNumber Substract(BigNumber a, BigNumber b)
        {
            var digits = new List<byte>();

            BigNumber max = Zero;
            BigNumber min = Zero;

            //сравниваем числа игнорируя знак
            var compare = Comparison(a, b, ignoreSign: true);

            switch (compare)
            {
                case -1:
                    min = a;
                    max = b;
                    break;
                case 0:
                    //если числа равны возвращаем 0
                    return Zero;
                case 1:
                    min = b;
                    max = a;
                    break;
            }

            //из большего вычитаем меньшее
            var maxLength = Math.Max(a.Size, b.Size);

            var t = 0;
            for (var i = 0; i < maxLength; i++)
            {
                var s = max.GetByte(i) - min.GetByte(i) - t;
                if (s < 0)
                {
                    s += 10;
                    t = 1;
                }
                else
                {
                    t = 0;
                }

                digits.Add((byte)s);
            }
            return new BigNumber(max.Sign, digits);
        }

        private static BigNumber Multiply(BigNumber a, BigNumber b)
        {
            var retValue = Zero;

            for (var i = 0; i < a.Size; i++)
            {
                for (int j = 0, carry = 0; (j < b.Size) || (carry > 0); j++)
                {
                    var cur = retValue.GetByte(i + j) + a.GetByte(i) * b.GetByte(j) + carry;
                    retValue.SetByte(i + j, (byte)(cur % 10));
                    carry = cur / 10;
                }
            }

            retValue.Sign = a.Sign == b.Sign ? Sign.Plus : Sign.Minus;
            return retValue;
        }

        private static BigNumber Div(BigNumber a, BigNumber b)
        {
            var retValue = Zero;
            var curValue = Zero;

            for (var i = a.Size - 1; i >= 0; i--)
            {
                curValue += Exp(a.GetByte(i), i);

                var x = 0;
                var l = 0;
                var r = 10;
                while (l <= r)
                {
                    var m = (l + r) / 2;
                    var cur = b * Exp((byte)m, i);
                    if (cur <= curValue)
                    {
                        x = m;
                        l = m + 1;
                    }
                    else
                    {
                        r = m - 1;
                    }
                }

                retValue.SetByte(i, (byte)(x % 10));
                var t = b * Exp((byte)x, i);
                curValue = curValue - t;
            }

            retValue.RemoveNulls();

            retValue.Sign = a.Sign == b.Sign ? Sign.Plus : Sign.Minus;
            return retValue;
        }

        private static BigNumber Mod(BigNumber a, BigNumber b)
        {
            var retValue = Zero;

            for (var i = a.Size - 1; i >= 0; i--)
            {
                retValue += Exp(a.GetByte(i), i);

                var x = 0;
                var l = 0;
                var r = 10;

                while (l <= r)
                {
                    var m = (l + r) >> 1;
                    var cur = b * Exp((byte)m, i);
                    if (cur <= retValue)
                    {
                        x = m;
                        l = m + 1;
                    }
                    else
                    {
                        r = m - 1;
                    }
                }

                retValue -= b * Exp((byte)x, i);
            }

            retValue.RemoveNulls();

            retValue.Sign = a.Sign == b.Sign ? Sign.Plus : Sign.Minus;
            return retValue;
        }

        public static BigNumber operator -(BigNumber a)
        {
            a.Sign = a.Sign == Sign.Plus ? Sign.Minus : Sign.Plus;
            return a;
        }

        //сложение
        public static BigNumber operator +(BigNumber a, BigNumber b) => a.Sign == b.Sign
                ? Add(a, b)
                : Substract(a, b);

        //вычитание
        public static BigNumber operator -(BigNumber a, BigNumber b) => a + -b;

        //умножение
        public static BigNumber operator *(BigNumber a, BigNumber b) => Multiply(a, b);

        //целочисленное деление(без остатка)
        public static BigNumber operator /(BigNumber a, BigNumber b) => Div(a, b);

        //остаток от деления
        public static BigNumber operator %(BigNumber a, BigNumber b) => Mod(a, b);


    }
}
