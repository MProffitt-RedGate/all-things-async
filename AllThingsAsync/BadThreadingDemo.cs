namespace AllThingsAsync;

public static class BadThreadingDemo
{
    public static void RunBad()
    {
        var threads = new Thread[10];
        var myVal = 0;

        for (int i = 0; i < threads.Length; i++)
        {
            threads[i] = new Thread(() =>
            {
                for (int j = 0; j < 500000; j++)
                {
                    ++myVal;
                }
            });
    
            threads[i].Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }  

        Console.WriteLine($"[RunBad] Final value {myVal} - It's probably wrong");
    }
    
    public static void RunSlightlyBetter()
    {
        var threads = new Thread[10];
        var myVal = 0;

        for (int i = 0; i < threads.Length; i++)
        {
            threads[i] = new Thread(() =>
            {
                var localVal = 0;
                for (int j = 0; j < 500000; j++)
                {
                    ++localVal;
                }
                myVal += localVal;
            });
    
            threads[i].Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }  

        Console.WriteLine($"[RunSlightlyBetter] Final value {myVal} - It's likely correct, but still not very safe");
    }
    
    public static void RunSafelyWithLock()
    {
        var threads = new Thread[10];
        var myVal = 0;
        var myLock = new Lock();

        for (int i = 0; i < threads.Length; i++)
        {
            threads[i] = new Thread(() =>
            {
                for (int j = 0; j < 500000; j++)
                {
                    lock (myLock)
                    {
                        ++myVal;
                    }
                }
            });
    
            threads[i].Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }  

        Console.WriteLine($"[RunSafelyWithLock] Final value {myVal} - This is 100% correct, always");
    }
    
    public static void RunATimedLock()
    {
        Console.WriteLine($"[RunATimedLock] Running {DateTime.Now.ToString("hh:mm:ss")}...");
        var threads = new Thread[10];
        var myVal = 0;
        var myLock = new Lock();

        Task.Run(() =>
        {
            lock (myLock)
            {
                Thread.Sleep(3000);
            }
            
            Console.WriteLine($"[RunATimedLock] Released");
        });

        for (int i = 0; i < threads.Length; i++)
        {
            threads[i] = new Thread(() =>
            {
                for (int j = 0; j < 500000; j++)
                {
                    lock (myLock)
                    {
                        ++myVal;
                    }
                }
            });
    
            threads[i].Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }  

        Console.WriteLine($"[RunATimedLock] Finished {DateTime.Now.ToString("hh:mm:ss")}... Value = {myVal}");
    }

    public static void RunALikelyDeadlock()
    {
        Console.WriteLine("[RunALikelyDeadlock] You'll probably never see the next message from this method");
        var threads = new Thread[10];
        var myVal = 0;
        var lockA = new Lock();
        var lockB = new Lock();

        for (int i = 0; i < threads.Length; i++)
        {
            threads[i] = new Thread(() =>
            {
                for (int j = 0; j < 500000; j++)
                {
                    if (j % 2 == 0)
                    {
                        lock (lockA)
                        {
                            lock (lockB)
                            {
                                ++myVal;
                            }
                        }
                    }
                    else
                    {
                        lock (lockB)
                        {
                            lock (lockA)
                            {
                                ++myVal;
                            }
                        }
                    }
                }
            });
            threads[i].IsBackground = true; // This will prevent the application from never ending
            threads[i].Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }  

        Console.WriteLine("[RunALikelyDeadlock] Potatoes");
    }
}