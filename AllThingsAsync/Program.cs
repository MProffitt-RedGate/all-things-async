using AllThingsAsync;

Console.WriteLine("Threading!");
BadThreadingDemo.RunBad();
BadThreadingDemo.RunSlightlyBetter();
BadThreadingDemo.RunSafelyWithLock();
BadThreadingDemo.RunATimedLock();
_ = Task.Run(BadThreadingDemo.RunALikelyDeadlock);

await Task.Run(async () =>
{
    for (int i = 0; i < 3; i++)
    {
        Console.WriteLine($"[RunALikelyDeadlock] Waiting for deadlock... {i}");
        await Task.Delay(1000);
    }
    
    Console.WriteLine("[RunALikelyDeadlock] It's definitely stuck, moving on.");
    await Task.Delay(1000);
});

// Thread Sync

await ThreadSynchronization.RunBadWaitingThread();
ThreadSynchronization.RunGoodWaitingThread();

// Thread Properties

ThreadProperties.RunLocaleThread();