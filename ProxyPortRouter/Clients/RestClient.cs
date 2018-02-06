using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProxyPortRouter.Clients
{
    public class RestClient : ISlaveClient, IDisposable
    {
        private readonly HttpClient client;

        public RestClient(Uri baseAddress)
        {
            client = new HttpClient {BaseAddress = baseAddress};
        }

        public async Task SetCurrentEntry(string name)
        {
            await client.PutAsync($"api/entry?name={name}", null);
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
