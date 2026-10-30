using System.Net;
using System.Net.Sockets;

namespace AllThingsAsync;

public class Server
{
    private readonly Socket _listenSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    
    public async Task BeginListening(CancellationToken cancellationToken = default)
    {
        _listenSocket.Bind(new IPEndPoint(IPAddress.Any, 7777));
        _listenSocket.Listen(50);
        
        while (true)
        {
            var connection = await _listenSocket.AcceptAsync(cancellationToken);
            await Task.Run(async () =>
            {
                var buffer = new byte[8];
                int read = 0;
                while (read < buffer.Length)
                {
                    int n = await connection.ReceiveAsync(buffer.AsMemory(read), cancellationToken);
                    if (n == 0) return;
                    read += n;
                }

                long limit = BitConverter.ToInt64(buffer);
                long res = await Task.Run(() => CpuBoundTask.CountPrimes(limit), cancellationToken);

                try
                {
                    await connection.SendAsync(new List<ArraySegment<byte>>()
                    {
                        new(BitConverter.GetBytes(res)),
                    });
                }
                finally
                {
                    connection.Shutdown(SocketShutdown.Both);
                    connection.Dispose();
                }
            }, cancellationToken);
        }
    }
}