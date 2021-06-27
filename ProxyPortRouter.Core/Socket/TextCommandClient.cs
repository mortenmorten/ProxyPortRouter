namespace ProxyPortRouter.Core.Socket
{
    using System;
    using System.Net.Sockets;
    using System.Text;
    using Serilog;

    public class TextCommandClient : IDisposable
    {
        private readonly object sync = new object();
        private readonly TextCommandReader commandReader = new TextCommandReader();

        public TextCommandClient(TextCommandListener host, System.Net.Sockets.Socket socket)
        {
            Host = host;
            Socket = socket;

            commandReader.CommandReceived += (sender, args) => DoCommand(args.Command);
        }

        public TextCommandListener Host { get; private set; }

        public System.Net.Sockets.Socket Socket { get; private set; }

        public void Dispose()
        {
            Dispose(true);
        }

        public void Read(byte[] buffer, int count)
        {
            commandReader.Read(buffer, count);
        }

        private void DoCommand(Command command)
        {
            try
            {
                Log.Verbose("Processing command: {command}", command);
                switch (command.Path.ToLower())
                {
                    case "input":
                        Send("input;0000");
                        break;
                    case "outputaccess":
                        Send("output;0000");
                        break;
                }
            }
            catch (Exception exception)
            {
                Log.Warning(exception.Message);
                Send(exception.Message);
            }
        }

        private void Send(string message)
        {
            System.Net.Sockets.Socket socket;
            TextCommandListener host;

            lock (sync)
            {
                socket = Socket;
                host = Host;
            }

            if (socket == null)
            {
                return;
            }

            Log.Verbose("Returning {Message}", message);
            var buffer = Encoding.ASCII.GetBytes($"{message}\0");

            try
            {
                socket.Send(buffer, buffer.Length, SocketFlags.None);
            }
            catch (SocketException)
            {
                host?.CloseClient(this);
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                System.Net.Sockets.Socket socket;

                lock (sync)
                {
                    socket = Socket;

                    Socket = null;
                    Host = null;
                }

                if (socket != null)
                {
                    try
                    {
                        socket.Close();
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }
    }
}