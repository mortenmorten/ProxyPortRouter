namespace ProxyPortRouter.Core.Clients
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class RestClient : ISlaveClientAsync, IDisposable
    {
        private readonly HttpClient client;

        public RestClient(Uri baseAddress)
        {
            client = new HttpClient { BaseAddress = baseAddress };
        }

        public Task SetCurrentEntryAsync(string name)
        {
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
