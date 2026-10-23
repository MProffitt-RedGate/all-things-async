using System.Globalization;

namespace AllThingsAsync;

public static class ThreadProperties
{
    public static void RunLocaleThread()
    {
        var dateTime = DateTime.Now;
        var ukThread = new Thread(() => 
            Console.WriteLine($"[RunLocaleThread] UK Locale: {dateTime.ToLocalTime()}"));
        var usThread = new Thread(() => 
            Console.WriteLine($"[RunLocaleThread] US Locale: {dateTime.ToLocalTime()}"));

        ukThread.CurrentCulture = new CultureInfo("en-GB");
        usThread.CurrentCulture = new CultureInfo("en-US");
        
        ukThread.Start();
        usThread.Start();
    }
}