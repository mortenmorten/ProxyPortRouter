namespace ProxyPortRouter.Core
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Mime;
    using System.Text;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

    using ProxyPortRouter.Core.Config;

    public class RestBackend : IBackend, IBackendAsync, IDisposable
    {
        private readonly HttpClient httpClient;

        [UsedImplicitly]
        public RestBackend()
            : this(new HttpClient { BaseAddress = new Uri("http://localhost:8080") })
        {
        }

        internal RestBackend(HttpClient client)
        {
            httpClient = client;
        }

        public event EventHandler CurrentChanged;

        public void Dispose()
        {
            Dispose(true);
        }

        public string GetListenAddress()
        {
            return GetListenAddressAsync().Result;
        }

        public Task<string> GetListenAddressAsync()
        {
            return GetContentAsync<string>("api/entry/listen");
        }

        public CommandEntry GetCurrent()
        {
            return GetCurrentAsync().Result;
        }

        public Task<CommandEntry> GetCurrentAsync()
        {
            return GetContentAsync<CommandEntry>("api/entry");
        }

        public void SetCurrent(string name)
        {
            SetCurrentAsync(name).Wait();
        }

        public async Task SetCurrentAsync(string name)
        {
            var previousEntry = (await GetCurrentAsync().ConfigureAwait(false))?.Name;
            var response = await httpClient.PutAsJsonAsync("api/entry", new NameEntry(name)).ConfigureAwait(false);
            var newEntry = string.Empty;
            if (response.IsSuccessStatusCode)
            {
                newEntry = (await response.Content.ReadAsAsync<CommandEntry>().ConfigureAwait(false))?.Name;
            }

            if (previousEntry != null && previousEntry != newEntry)
            {
                CurrentChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public IEnumerable<CommandEntry> GetEntries()
        {
            return GetEntriesAsync().Result;
        }

        public Task<IEnumerable<CommandEntry>> GetEntriesAsync()
        {
            return GetContentAsync<IEnumerable<CommandEntry>>("api/entry/list");
        }

        private void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            httpClient?.Dispose();
        }

        private async Task<T> GetContentAsync<T>(string endpoint)
        {
            var rtrn = default(T);
            var response = await httpClient.GetAsync(endpoint).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                rtrn = await response.Content.ReadAsAsync<T>().ConfigureAwait(false);
            }

            return rtrn;
        }
    }
}
