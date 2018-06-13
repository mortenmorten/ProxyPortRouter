namespace ProxyPortRouter.Core.Socket
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using System.Windows.Threading;
    using ProxyPortRouter.Core.Config;
    using Serilog;

    public class TextCommandListener : IDisposable
    {
        private readonly List<TextCommandClient> clients = new List<TextCommandClient>();
        private readonly byte[] buffer = new byte[1024];
        private readonly object sync = new object();

        public TextCommandListener(ILocalSettings settings)
        {
            var port = settings.SimulatorPort != 0 ? settings.SimulatorPort : 8081;

            // var address = new IPAddress(new byte[] { 127, 0, 0, 1 });
            var address = IPAddress.Any;
            var listener = new TcpListener(address, port);
            listener.Start();
            listener.BeginAcceptSocket(AcceptCallback, listener);
            Log.Information("WebIO Simulator listening on {Address} port {Port}", address, port);
        }

        public void CloseClient(TextCommandClient client)
        {
            Log.Debug("Closing client {RemoteEndPoint}", client.Socket.RemoteEndPoint);

            lock (sync)
            {
                clients.Remove(client);
            }

            client.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            Log.Information("Closing down WebIO Simulator");
            if (!disposing)
            {
                return;
            }

            lock (sync)
            {
                foreach (var client in clients)
                {
                    client.Dispose();
                }

                clients.Clear();
            }
        }

        private void AcceptCallback(IAsyncResult result)
        {
            var listener = (TcpListener)result.AsyncState;

            try
            {
                var socket = listener.EndAcceptSocket(result);
                var client = new TextCommandClient(this, socket);

                Log.Debug("Accepting connection from {RemoteEndPoint}", client.Socket.RemoteEndPoint);

                lock (sync)
                {
                    clients.Add(client);
                }

                listener.BeginAcceptSocket(AcceptCallback, listener);
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, client);
            }
            catch (SocketException ex)
            {
                Log.Error("Accept error", ex);
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
