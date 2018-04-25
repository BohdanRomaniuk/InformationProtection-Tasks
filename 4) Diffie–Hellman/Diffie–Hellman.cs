using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Information
{
    class A
    {
        private long p, a;
        public long x;
        public A(long _p, long _a, long _x=7)
        {
            p = _p;
            a = _a;
            x = _x;
        }

        public long CalculateX()
        {
            return (long)(Math.Pow(a,x)%p);
        }

        public long CalculateK(long YFromB)
        {
            return (long)(Math.Pow(YFromB, x) % p);
        }
    }

    class B
    {
        private long p, a;
        public long y;
        public B(long _p, long _a, long _y=4)
        {
            p = _p;
            a = _a;
            y = _y;
        }

        public long CalculateY()
        {
            return (long)(Math.Pow(a,y)%p);
        }

        public long CalculateK(long XFromA)
        {
            return (long)(Math.Pow(XFromA, y) % p);
        }
    }

    class DiffieHellman
    {
        public static bool IsPrimeNumber(long p)
        {
            for (long i = 2; i < p; ++i)
            {
                if (p % i == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public static long Power(long x, long y, long p)
        {
            long res = 1;
            x = x % p;
            while (y > 0)
            {
                if (y % 2 != 0)
                    res = (res * x) % p;
                y = y >> 1;
                x = (x * x) % p;
            }
            return res;
        }

        public static long FindPrimitiveRoot(long p)
        {
            HashSet<long> s = new HashSet<long>(); ;
            long phi = p - 1;

            //findPrimefactorsBegin
            while (phi % 2 == 0)
            {
                s.Add(2);
                phi = phi / 2;
            }
            for (long i = 3; i <= Math.Sqrt(phi); i = i + 2)
            {
                while (phi % i == 0)
                {
                    s.Add(i);
                    phi = phi / i;
                }
            }
            if (phi > 2)
            {
                s.Add(phi);
            } 
            //findPrimefactorsBeginEnd

            for (long r = 2; r <= phi; r++)
            {
                bool flag = false;
                foreach(var elem in s)
                {
                    if (Power(r, phi / (elem), p) == 1)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag == false)
                {
                    return r;
                } 
            }
            return -1;
        }

        static void Main(string[] args)
        {
            try
            {
                Console.Write("Input p=");
                long p = Convert.ToInt64(Console.ReadLine());
                if(!IsPrimeNumber(p))
                {
                    throw new ArgumentException("P must be prime number!");
                }
                long a = FindPrimitiveRoot(p);
                Console.WriteLine("Primitive root a: " + a);
                Random rand = new Random();
                A first = new A(p, a, rand.Next(10));
                B second = new B(p, a, rand.Next(10));
                //Console.WriteLine("\nx: {0}  y:{1}", first.x, second.y);
                long xfroma = first.CalculateX();
                long yfromb = second.CalculateY();
                Console.WriteLine("\nX from A: " + xfroma);
                Console.WriteLine("Y from B: " + yfromb);
                long kfroma = first.CalculateK(yfromb);
                long kfromb = second.CalculateK(xfroma);
                Console.WriteLine("K from A: " + kfroma);
                Console.WriteLine("K from B: " + kfromb);
                Console.WriteLine("K from A & K from B " + ((kfroma == kfromb) ? "EQUALS!!!" : "NOT equals!!!"));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error!!! " + ex.Message);
            }
            finally
            {
                Console.ReadKey();
            }
        }
    }
}
