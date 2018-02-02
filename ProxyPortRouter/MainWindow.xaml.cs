using System.Windows;

namespace ProxyPortRouter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainWindowViewModel();
        }

        private MainWindowViewModel ViewModel
        {
            get => viewModel;
            set
            {
                viewModel = value;
                DataContext = viewModel;
            }
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Load("entries.json");
        }
    }
}
