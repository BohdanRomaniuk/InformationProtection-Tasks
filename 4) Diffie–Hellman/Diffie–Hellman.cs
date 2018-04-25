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

        public static long PowerMod(long x, long y, long p)
        {
            long res = 1;
            x = x % p;
            while (y > 0)
            {
                if (y % 2 != 0)
                {
                    res = (res * x) % p;
                }
                y = y >> 1;
                x = (x * x) % p;
            }
            return res;
        }

        public static long FindPrimitiveRoot(long p)
        {
            List<long> fact = new List<long>();
            long phi = p - 1, n = phi;
            for (long i = 2; i * i <= n; ++i)
            {
                if (n % i == 0)
                {
                    fact.Add(i);
                    while (n % i == 0)
                    {
                        n /= i;
                    }
                }
            }
            if (n > 1)
            {
                fact.Add(n);
            }

            for (long res = 2; res <= p; ++res)
            {
                bool ok = true;
                for (int i = 0; i < fact.Count && ok; ++i)
                {
                    ok &= PowerMod(res, phi / fact[i], p) != 1;
                }
                if (ok)
                {
                    return res;
                }
            }
            return -1;
        }

        static void Main(string[] args)
        {
            while(true)
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
                A first = new A(p, a, rand.Next(8));
                B second = new B(p, a, rand.Next(8));
                Console.WriteLine("\nx: {0}  y:{1}", first.x, second.y);
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
                //Console.ReadKey();
            }
        }
    }
}
