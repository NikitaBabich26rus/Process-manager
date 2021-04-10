using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Service
{
    public class Client
    {
        private const int port = 8888;

        private const string host = "127.0.0.1";

        public async Task SendTheNewProcessList(List<(int, string)> process)
        {
            var client = new TcpClient(host, port);
            using var stream = client.GetStream();
            var writer = new StreamWriter(stream) { AutoFlush = true };
            await writer.WriteLineAsync(process.Count.ToString());
            foreach (var item in process)
            {
                await writer.WriteLineAsync(item.Item1.ToString());
                await writer.WriteLineAsync(item.Item2);
            }
            client.Dispose();
            stream.Dispose();
            writer.Dispose();
        }
    }
}
