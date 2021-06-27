namespace ProxyPortRouter.Core.Socket
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using ProxyPortRouter.Core.Config;

    public class TextCommandListener : IHostedService
    {
        private readonly ILocalSettings settings;
        private readonly ILogger<TextCommandListener> logger;
        private readonly List<TextCommandClient> clients = new List<TextCommandClient>();
        private readonly byte[] buffer = new byte[1024];
        private readonly object sync = new object();

        public TextCommandListener(ILocalSettings settings, ILogger<TextCommandListener> logger)
        {
            this.settings = settings;
            this.logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var port = settings.SimulatorPort != 0 ? settings.SimulatorPort : 8081;

            // var address = new IPAddress(new byte[] { 127, 0, 0, 1 });
            var address = IPAddress.Any;
            var listener = new TcpListener(address, port);
            listener.Start();
            listener.BeginAcceptSocket(AcceptCallback, listener);
            logger.LogInformation("WebIO Simulator listening on {Address} port {Port}", address, port);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Closing down WebIO Simulator");
 
            lock (sync)
            {
                foreach (var client in clients)
                {
                    client.Dispose();
                }

                clients.Clear();
            }

            return Task.CompletedTask;
        }


        public void CloseClient(TextCommandClient client)
        {
            logger.LogDebug("Closing client {RemoteEndPoint}", client.Socket.RemoteEndPoint);

            lock (sync)
            {
                clients.Remove(client);
            }

            client.Dispose();
        }

        private void AcceptCallback(IAsyncResult result)
        {
            var listener = (TcpListener)result.AsyncState;

            try
            {
                var socket = listener.EndAcceptSocket(result);
                var client = new TextCommandClient(this, socket);

                logger.LogDebug("Accepting connection from {RemoteEndPoint}", client.Socket.RemoteEndPoint);

                lock (sync)
                {
                    clients.Add(client);
                }

                listener.BeginAcceptSocket(AcceptCallback, listener);
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, client);
            }
            catch (SocketException ex)
            {
                logger.LogError(ex, "Accept error");
            }
            catch (ObjectDisposedException)
            {
            }
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            var client = (TextCommandClient)result.AsyncState;
            var socket = client.Socket;

            if (socket == null)
            {
                return;
            }

            try
            {
                var count = socket.EndReceive(result);

                if (count > 0)
                {
                    client.Read(buffer, count);

                    socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, client);
                }
                else
                {
                    CloseClient(client);
                }
            }
            catch (SocketException)
            {
                CloseClient(client);
            }
            catch (ObjectDisposedException)
            {
            }
        }
    }
}
