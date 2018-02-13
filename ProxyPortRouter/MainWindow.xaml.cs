namespace ProxyPortRouter
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private async void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel model)
            {
                await model.InitializeAsync().ConfigureAwait(false);
            }
        }
    }
}
