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
            client = new HttpClient { BaseAddress = baseAddress };
        }

        public Task SetCurrentEntryAsync(string name)
        {
            Log.Debug("Syncing REST client entry: {Name}", name);
            return client.PutAsync($"api/entry?name={name}", null);
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
