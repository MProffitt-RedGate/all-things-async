namespace AllThingsAsync;

public static class ThreadSynchronization 
{
    public static void RunBadWaitingThread()
    {
        BoxedInt? mustBeSet = null;

        var valueSettingThread = new Thread(() =>
            {
                Thread.Sleep(3000);
                mustBeSet = new BoxedInt(0);
            }
        );

        var valueReadingThread = new Thread(() =>
            {
                try
                {
                    Console.WriteLine($"This will cause an Exception");
                    var read = mustBeSet!.Value;
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine("NullReferenceException was thrown and caught");
                }
            }
        );
        
        valueSettingThread.Start();
        valueReadingThread.Start();
    }
    
    public static void RunGoodWaitingThread()
    {
        var myEvent = new ManualResetEventSlim(false);
        BoxedInt? mustBeSet = null;

        var valueSettingThread = new Thread(() =>
            {
                Thread.Sleep(3000);
                mustBeSet = new BoxedInt(0);
                myEvent.Set();
            }
        );

        var valueReadingThread = new Thread(() =>
            {
                Console.WriteLine("Waiting for event to open");
                myEvent.Wait();
                var read = mustBeSet!.Value;
                
                Console.WriteLine($"Successfully wait for value: {read}");
            }
        );
        
        valueSettingThread.Start();
        valueReadingThread.Start();
    }
}

internal record BoxedInt(int Value);