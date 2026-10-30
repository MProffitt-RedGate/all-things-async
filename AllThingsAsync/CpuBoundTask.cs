using System.Diagnostics;

namespace AllThingsAsync;

public static class CpuBoundTask
{
    public static void RunCpuBoundTask()
    {
        CpuBoundTaskExample(10).GetAwaiter().GetResult();
    }
    
    public static async Task<IEnumerable<long>> CpuBoundTaskExample(int numOfOperations)
    {
        var sw = Stopwatch.StartNew();

        var tasks = new int[numOfOperations]
            .Select((i) => Task.Run(() => CountPrimes(5_000_000_0)))
            .ToArray();

        var results = await Task.WhenAll(tasks);

        Console.WriteLine($"{sw.ElapsedMilliseconds} ms");
        return results;
    }
    
    public static long CountPrimes(long limit)
    {
        long count = 0;
        for (long n = 2; n <= limit; n++)
        {
            if (IsPrime(n))
                count++;
        }
        Console.WriteLine($"{count} Primes");
        return count;
    }
    
    static bool IsPrime(long n)
    {
        if (n < 2) return false;
        if (n % 2 == 0) return n == 2;

        for (long i = 3; i * i <= n; i += 2)
        {
            if (n % i == 0)
                return false;
        }
        return true;
    }
}