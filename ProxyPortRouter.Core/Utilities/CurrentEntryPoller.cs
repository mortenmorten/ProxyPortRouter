namespace ProxyPortRouter.Core.Utilities
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    public class CurrentEntryPoller : IDisposable
    {
        private readonly Timer timer;

        private string lastCurrent;

        public CurrentEntryPoller(IBackendAsync backend, double secondsInterval = 5)
        {
            var interval = TimeSpan.FromSeconds(secondsInterval);
            timer = new Timer(OnInterval, backend, interval, interval);
        }

        public event EventHandler CurrentChanged;

        public void Dispose()
        {
            Dispose(true);
        }

        private void OnInterval([NotNull] object state)
        {
            OnIntervalAsync(state as IBackendAsync).ConfigureAwait(false);
        }

        private async Task OnIntervalAsync(IBackendAsync backend)
        {
            var current = await backend.GetCurrentAsync().ConfigureAwait(false);
            if (lastCurrent != current?.Name)
            {
                CurrentChanged?.Invoke(this, EventArgs.Empty);
                lastCurrent = current?.Name;
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                timer?.Dispose();
            }
        }
    }
}
