namespace ProxyPortRouter.Core
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

    using ProxyPortRouter.Core.Config;
    using ProxyPortRouter.Core.Utilities;

    public class RestBackend : IBackendAsync, IDisposable
    {
        private readonly HttpClient httpClient;
        private readonly CurrentEntryPoller poller;

        [UsedImplicitly]
        public RestBackend()
            : this(null)
        {
        }

        internal RestBackend(HttpMessageHandler handler)
        {
            httpClient = handler == null ? new HttpClient() : new HttpClient(handler);
            httpClient.BaseAddress = new Uri("http://localhost:8080");
            poller = new CurrentEntryPoller(this);
            poller.CurrentChanged += (sender, args) => CurrentChanged?.Invoke(this, args);
        }

        public event EventHandler CurrentChanged;

        public void Dispose()
        {
            Dispose(true);
        }

        public Task<string> GetListenAddressAsync()
        {
            return GetContentAsync<string>("api/entry/listen");
        }

        public Task<CommandEntry> GetCurrentAsync()
        {
            return GetContentAsync<CommandEntry>("api/entry");
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
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

            poller?.Dispose();
            httpClient?.Dispose();
        }

        private async Task<T> GetContentAsync<T>(string endpoint)
            where T : class
        {
            var rtrn = default(T);
            var response = await httpClient.GetAsync(endpoint).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                if (typeof(T) == typeof(string))
                {
                    rtrn = await response.Content.ReadAsStringAsync().ConfigureAwait(false) as T;
                }
                else
                {
                    rtrn = await response.Content.ReadAsAsync<T>().ConfigureAwait(false);
                }
            }

            return rtrn;
        }
    }
}
