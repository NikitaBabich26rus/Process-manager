using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace UI
{
    public class Server
    {
        private TcpListener listener;

        private const int port = 8888;

        private ViewModel viewModel;

        private const string host = "127.0.0.1";

        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public Server(ViewModel viewModel)
        {
            listener = new TcpListener(IPAddress.Parse(host), port);
            this.viewModel = viewModel;
        }
        
        public async Task Start()
        {
            listener.Start();
            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                var client = await listener.AcceptTcpClientAsync();
                await Task.Run(() => GetTheNewProcessList(client));
            }
            listener.Stop();
        }

        public void Stop() => cancellationTokenSource.Cancel();

        private async Task GetTheNewProcessList(TcpClient client)
        {
            using var stream = client.GetStream();
            var reader = new StreamReader(stream);
            try
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    viewModel.Processes.Clear();
                });
                var size = Convert.ToInt32(await reader.ReadLineAsync());
                for (int i = 0; i < size; i++)
                {
                    Application.Current.Dispatcher.Invoke((Action)async delegate
                    {
                        viewModel.Processes.Add(await reader.ReadLineAsync() + " " + await reader.ReadLineAsync());
                    });
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            stream.Dispose();
            reader.Dispose();
        }
    }
}
