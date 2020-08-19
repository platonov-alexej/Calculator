using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class BigPointNumber
    {
        private readonly List<byte> digits = new List<byte>();
        public Sign Sign { get; private set; } = Sign.Plus;
        public int PointPosition { get; private set; }



        public BigPointNumber(List<byte> bytes, Sign sign, int pointPosition)
        {
            digits = bytes.ToList();
            Sign = sign;
            PointPosition = pointPosition;
            RemoveNulls();
        }

        //public BigPointNumber(Sign sign, List<byte> bytes)
        //{
        //    Sign = sign;
        //    digits = bytes;
        //    RemoveNulls();
        //}

        public BigPointNumber()
        {

        }


        public BigPointNumber(string s)
        {
            if (s.StartsWith("-"))
            {
                Sign = Sign.Minus;
                s = s.Substring(1);
            }
            PointPosition = s.IndexOf(".");
            if (PointPosition >= 0) s = s.Remove(PointPosition, 1);

            foreach (var c in s.Reverse())
            {
                digits.Add(Convert.ToByte(c.ToString()));
            }

            RemoveNulls();
        }

        public BigPointNumber(double x) : this(x.ToString())
        {
        }


        //private List<byte> GetBytes(uint num)
        //{
        //    var bytes = new List<byte>();
        //    while (num > 0)
        //    {
        //        bytes.Add((byte)(num % 10));
        //        num /= 10;
        //    }

        //    return bytes;
        //}

        private void RemoveNulls()
        {
            for (var i = digits.Count - 1; i > 0; i--)
            {
                if (digits[i] == 0)
                {
                    int x = digits.Count - i - 1;
                    if (PointPosition - x > 1)
                    {
                        if (PointPosition != -1) PointPosition--;
                        digits.RemoveAt(i);
                    }

                }
                else
                {
                    break;
                }
            }
            for (var i = 0; i < digits.Count - 1; i++)
            {
                if (digits[i] == 0 && PointPosition != -1 && PointPosition != digits.Count)
                {
                    digits.RemoveAt(i);
                    i--;
                }
                else
                {
                    break;
                }
            }
        }

        public static BigPointNumber Zero => new BigPointNumber(0);

        public static BigPointNumber One => new BigPointNumber(1);

        //длина числа
        public int Size => digits.Count;

        //знак числа


        //получение цифры по индексу
        //если индекс больше длины возвращает 0
        public byte GetByte(int i) => i < Size ? digits[i] : (byte)0;

        //установка цифры по индексу

        public void addDigit(int pos, byte b)
        {
            digits.Insert(pos, b);
        }

        public void SetByte(int i, byte b)
        {
            while (digits.Count <= i)
            {
                digits.Add(0);
            }

            digits[i] = b;
        }

        public int SizeAfterPoint
        {
            get
            {
                return PointPosition == -1 ? 0 : digits.Count - PointPosition;
            }
        }


        public int SizeBeforePoint
        {
            get
            {
                return PointPosition == -1 ? digits.Count : PointPosition;
            }
        }

        public override string ToString()
        {
            var s = new StringBuilder("");

            for (var i = digits.Count - 1; i >= 0; i--)
            {
                s.Append(Convert.ToString(digits[i]));
            }
            if (PointPosition >= 0 & PointPosition != digits.Count)
                s.Insert(PointPosition, ".");
            s.Insert(0, Sign == Sign.Minus ? "-" : "");

            return s.ToString();
        }

        private static void EqualizationDigitsCount(ref BigPointNumber a, ref BigPointNumber b)
        {
            int deltaBefore = Math.Abs(a.SizeBeforePoint - b.SizeBeforePoint);
            int deltaAfter = Math.Abs(a.SizeAfterPoint - b.SizeAfterPoint);
            if (a.SizeAfterPoint > b.SizeAfterPoint)
                for (int i = 0; i < deltaAfter; i++)
                {
                    if (b.PointPosition == -1) b.PointPosition = b.digits.Count;
                    b.addDigit(0, 0);

                }
            else
                for (int i = 0; i < deltaAfter; i++)
                    a.addDigit(0, 0);


            if (a.SizeBeforePoint > b.SizeBeforePoint)
                for (int i = 0; i < deltaBefore; i++)
                {
                    b.addDigit(b.digits.Count, 0);
                    b.PointPosition++;
                }

            else
                for (int i = 0; i < deltaBefore; i++)
                {
                    a.addDigit(a.digits.Count, 0);
                    a.PointPosition++;
                }
        }


        private static BigPointNumber Add(BigPointNumber a, BigPointNumber b)
        {
            BigPointNumber tmp_a = a;
            BigPointNumber tmp_b = b;

            EqualizationDigitsCount(ref tmp_a, ref tmp_b);

            var digits = new List<byte>();

            byte t = 0;
            for (int i = 0; i < tmp_a.Size; i++)
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

            int tmp_PointPos = tmp_a.PointPosition > tmp_b.PointPosition ? tmp_a.PointPosition : tmp_b.PointPosition;
            return new BigPointNumber(digits, tmp_a.Sign, tmp_PointPos);
        }


        //-------------------------------------------------------------------
        private static int Comparison(BigPointNumber a, BigPointNumber b, bool ignoreSign = false)
        {
            return CompareSign(a, b, ignoreSign);
        }

        private static int CompareSign(BigPointNumber a, BigPointNumber b, bool ignoreSign = false)
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

        private static int CompareSize(BigPointNumber a, BigPointNumber b)
        {
            if (a.SizeBeforePoint < b.SizeBeforePoint)
            {
                return -1;
            }
            else if (a.SizeBeforePoint > b.SizeBeforePoint)
            {
                return 1;
            }

            return CompareDigits(a, b);
        }

        private static int CompareDigits(BigPointNumber a, BigPointNumber b)
        {
            BigPointNumber tmp_a = a;
            BigPointNumber tmp_b = b;

            EqualizationDigitsCount(ref tmp_a, ref tmp_b);

            for (var i = tmp_a.Size; i >= 0; i--)
            {
                if (tmp_a.GetByte(i) < tmp_b.GetByte(i))
                {
                    return -1;
                }
                else if (tmp_a.GetByte(i) > tmp_b.GetByte(i))
                {
                    return 1;
                }
            }

            return 0;
        }


        private static BigPointNumber Substract(BigPointNumber a, BigPointNumber b)
        {
            BigPointNumber tmp_a = a;
            BigPointNumber tmp_b = b;


            var digits = new List<byte>();

            BigPointNumber max = Zero;
            BigPointNumber min = Zero;

            //сравниваем числа игнорируя знак
            var compare = Comparison(tmp_a, tmp_b, ignoreSign: true);

            switch (compare)
            {
                case -1:
                    min = tmp_a;
                    max = tmp_b;
                    break;
                case 0:
                    //если числа равны возвращаем 0
                    return Zero;
                case 1:
                    min = tmp_b;
                    max = tmp_a;
                    break;
            }

            //из большего вычитаем меньшее
            EqualizationDigitsCount(ref tmp_a, ref tmp_b);

            var t = 0;
            for (var i = 0; i < tmp_a.Size; i++)
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
            int tmp_PointPos = tmp_a.PointPosition > tmp_b.PointPosition ? tmp_a.PointPosition : tmp_b.PointPosition;
            return new BigPointNumber(digits, max.Sign, tmp_PointPos);
        }

        private static BigPointNumber Multiply(BigPointNumber a, BigPointNumber b)
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
            int delta = 0;
            delta += a.PointPosition != -1 ? a.SizeAfterPoint : 0;
            delta += b.PointPosition != -1 ? b.SizeAfterPoint : 0;
            retValue.PointPosition = retValue.digits.Count - delta;
            if (retValue.PointPosition == retValue.digits.Count) retValue.PointPosition = -1;
            retValue.RemoveNulls();
            return retValue;
        }

        public static BigPointNumber operator -(BigPointNumber a)
        {
            a.Sign = a.Sign == Sign.Plus ? Sign.Minus : Sign.Plus;
            return a;
        }

        public static BigPointNumber operator +(BigPointNumber a, BigPointNumber b) => a.Sign == b.Sign
                ? Add(a, b)
                : Substract(a, b);

        public static BigPointNumber operator -(BigPointNumber a, BigPointNumber b) => a + -b;
        public static BigPointNumber operator *(BigPointNumber a, BigPointNumber b) => Multiply(a, b);
    }

}
