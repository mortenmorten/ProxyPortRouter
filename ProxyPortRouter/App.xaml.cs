namespace ProxyPortRouter
{
    using System;
    using System.Windows;

    using ProxyPortRouter.Core;
    using ProxyPortRouter.Core.Web;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IDisposable
    {
        public App()
        {
            ServiceProviderBuilder.SetupBackendService(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindowViewModel = new MainWindowViewModel(ServiceProviderBuilder.BuildServiceProvider());
            MainWindow = new MainWindow(mainWindowViewModel);
            MainWindow.Show();
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
        }

        private void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }
        }
    }
}
