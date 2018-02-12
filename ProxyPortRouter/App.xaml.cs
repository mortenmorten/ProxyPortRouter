namespace ProxyPortRouter
{
    using System;
    using System.Windows;

    using ProxyPortRouter.Core;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IDisposable
    {
        private Main main;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            main = new Main();
            main.Start(e.Args);

            var mainWindowViewModel = new MainWindowViewModel(main.Services);
            MainWindow = new MainWindow(mainWindowViewModel);
            MainWindow.Show();
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            main.Stop();
        }

        private void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            main.Dispose();
        }
    }
}
