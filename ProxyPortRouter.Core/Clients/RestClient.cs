namespace ProxyPortRouter.Core.Clients
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    using ProxyPortRouter.Core.Config;

    using Serilog;

    public class RestClient : ISlaveClientAsync, IDisposable
    {
        private readonly HttpClient client;

        public RestClient(HttpClient httpClient)
        {
            Log.Debug("Initializing REST client on {BaseAddress}", httpClient.BaseAddress);
            client = httpClient;
        }

        public async Task SetCurrentEntryAsync(string name)
        {
            Log.Debug("Syncing REST client entry: {Name}", name);
            try
            {
                await client.PutAsJsonAsync("api/entry", new NameEntry(name)).ConfigureAwait(false);
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
