namespace AllThingsAsync;

public static class AsyncMultithreading
{
    public static void BlockingThread()
    {
        var tasks = new Task[100];
        for(int i=0;i<100;++i)
        {
            var icopy = i;
            tasks[i] = Task.Run(() =>
            { 
                var startTime = DateTime.Now;
                Task.Delay(2000).ConfigureAwait(false).GetAwaiter().GetResult();
                Console.WriteLine($"Task [{icopy}] completed in {-startTime.Subtract(DateTime.Now).TotalMilliseconds} ms");
            });
        }
        Task.WaitAll(tasks);
    }
    
    public static void OpenThreadsUsingAsyncMultithreading()
    {
        var tasks = new Task[100];
        for(int i=0;i<100;++i)
        {
            var icopy = i;
            tasks[i] = Task.Run(async () =>
            { 
                var startTime = DateTime.Now;
                await Task.Delay(2000).ConfigureAwait(false);
                Console.WriteLine($"Task [{icopy}] completed in {-startTime.Subtract(DateTime.Now).TotalMilliseconds} ms");
            });
        }
        Task.WaitAll(tasks);
    }
    
    public static void ThreadRecycling()
    {
        async ValueTask<int> GetId(int id)
        {
            if (id % 2 == 0)
                return id;
        
            await Task.Delay(100).ConfigureAwait(false);
            return id;
        }
        
        var tasks = new Task<(bool IsEven, bool WasSameThread)>[100];
        for(var i=0;i<tasks.Length;++i)
        {
            var icopy = i;
            tasks[i] = Task.Run(async () =>
            {
                var initialThread = Thread.CurrentThread.ManagedThreadId;
                await GetId(icopy);
                var finalThread = Thread.CurrentThread.ManagedThreadId;
                return (icopy % 2 == 0, initialThread == finalThread);
            });
        }

        var results = Task.WhenAll(tasks).GetAwaiter().GetResult();

        var evenSameThreadCount = results.Count(x => x is { IsEven: true, WasSameThread: true });
        var oddSameThreadCount = results.Count(x => x is { IsEven: false, WasSameThread: true });
        
        Console.WriteLine($"Even Same Thread Count: {evenSameThreadCount}, Odd Same Thread Count: {oddSameThreadCount}");
    }
}