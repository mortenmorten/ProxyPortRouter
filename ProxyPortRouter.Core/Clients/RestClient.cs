namespace ProxyPortRouter.Core.Clients
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Threading.Tasks;

    using ProxyPortRouter.Core.Config;

    using Serilog;

    public class RestClient : ISlaveClientAsync, IDisposable
    {
        private readonly HttpClient client;

        public RestClient(Uri baseAddress)
            : this(baseAddress, null)
        {
        }

        internal RestClient(HttpMessageHandler handler)
            : this(new Uri("http://localhost:8080"), handler)
        {
        }

        private RestClient(Uri baseAddress, HttpMessageHandler handler)
        {
            Log.Debug("Initializing REST client on {BaseAddress}", baseAddress);
            client = handler == null ? new HttpClient() : new HttpClient(handler);
            client.BaseAddress = baseAddress;
            client.Timeout = TimeSpan.FromSeconds(5);
        }

        public async Task SetCurrentEntryAsync(string name)
        {
            Log.Debug("Syncing REST client entry: {Name}", name);
            try
            {
                await client.PutAsync("api/entry", new NameEntry(name), new JsonMediaTypeFormatter()).ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                Log.Error("Failed to sync REST client");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                client.Dispose();
            }
        }
    }
}
