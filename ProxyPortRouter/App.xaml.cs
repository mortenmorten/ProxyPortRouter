namespace ProxyPortRouter
{
    using System;
    using System.Windows;

    using ProxyPortRouter.Core.Web;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            ServiceProviderBuilder.SetupBackendService(false);
        }

        // ReSharper disable once AsyncConverter.AsyncMethodNamingHighlighting
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindowViewModel = new MainWindowViewModel(ServiceProviderBuilder.BuildServiceProvider());
            MainWindow = new MainWindow(mainWindowViewModel);
            MainWindow.Show();
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
        }
    }
}
