using System.Diagnostics;
using System.Net.Sockets;

var sw = Stopwatch.StartNew();

long limit = args.Length > 0 ? long.Parse(args[0]) : 100_000;

using var client = new TcpClient("localhost", 7777);

var stream = client.GetStream();

await stream.WriteAsync(BitConverter.GetBytes(limit));

var buffer = new byte[8];
int read = 0;
while (read < buffer.Length)
{
    int n = await stream.ReadAsync(buffer, read, buffer.Length - read);
    if (n == 0) break;
    read += n;
}

Console.WriteLine($"Result: {BitConverter.ToInt64(buffer)} (took {sw.ElapsedMilliseconds} ms)");