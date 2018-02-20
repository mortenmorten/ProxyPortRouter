namespace ProxyPortRouter.Core.Clients
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Serilog;

    public class RestClient : ISlaveClientAsync, IDisposable
    {
        private readonly HttpClient client;

        public RestClient(Uri baseAddress)
        {
            Log.Debug("Initializing REST client on {BaseAddress}", baseAddress);
            client = new HttpClient { BaseAddress = baseAddress, Timeout = TimeSpan.FromSeconds(5) };
        }

        public async Task SetCurrentEntryAsync(string name)
        {
            Log.Debug("Syncing REST client entry: {Name}", name);
            try
            {
                await client.PutAsync($"api/entry?name={name}", null).ConfigureAwait(false);
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
