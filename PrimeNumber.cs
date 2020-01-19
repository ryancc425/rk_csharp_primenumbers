using System;
using System.Numerics;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
namespace rk_csharp_primenumbers
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "t")
            {
                // Tester Mode

                // Test CheckPrime Functions
                Console.WriteLine("Testing Normal Check Prime...");
                Debug.Assert(RK_PrimeNumber.CheckPrime(100) == false);
                Debug.Assert(RK_PrimeNumber.CheckPrime(19) == true);
                Debug.Assert(RK_PrimeNumber.CheckPrime(224) == false);

                Console.WriteLine("Testing Wilson Check Prime...");
                Debug.Assert(RK_PrimeNumber.CheckPrime_Wilson(100) == false);
                Debug.Assert(RK_PrimeNumber.CheckPrime_Wilson(19) == true);
                Debug.Assert(RK_PrimeNumber.CheckPrime_Wilson(224) == false);


                Console.WriteLine("Testing Prime Generate...");
                long[] primeNumbers = RK_PrimeNumber.GenerateN_PrimeNumbers(100);
                foreach (long n in primeNumbers)
                {
                    Debug.Assert(RK_PrimeNumber.CheckPrime(n));
                }
            }
        }
    }

    public static class RK_PrimeNumber{
        public static bool CheckPrime(long n)
        {
            // If there are any numbers that can divide the number betwee {2... Number-1}, 
            // It is not a prime number
            for (long i = 2; i < n-1; i++)
            {
                if (n % i == 0)
                {
                    return false;
                }
            }
            return true;
        }
        
        public static bool CheckPrime_Parallel(long n, int Threads = -1)
        {
            bool isPrime = true;
            Parallel.For(2, n-1, new ParallelOptions{MaxDegreeOfParallelism = Threads}, 
            (long i,  ParallelLoopState state)=> {
                if (n % i == 0)
                {
                    isPrime = false;
                    state.Stop();
                }
            });
            return isPrime;
        }

        public static bool CheckPrime_Wilson(long n)
        {
            // By Wilson's theorem, n+ 1 is prime iff n! mod (n+1) = n
            // Factorials easily create numbers that go beyond simple data types in C#
            // Using BigInteger to store Factorial results
            long n1 = n-1;
            BigInteger n1_Factorial = 1;
            long s1 = 2;
            while (s1 <= n1)
            {
                n1_Factorial = s1 * n1_Factorial;
                s1++;
            }
            return (n1_Factorial % n == n1);
        }

        public static long[] GenerateN_PrimeNumbers(long n)
        {
            List<long> primes = new List<long>();
            for (long i = 2; i < n; i++)
            {
                if (CheckPrime(i)) primes.Add(i);
            }
            return primes.ToArray();
        }

        
    }
}
