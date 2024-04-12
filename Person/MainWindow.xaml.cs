using Person.Stores;
using Person.ViewModels;
using System.Windows;

namespace PersonApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly NavigationStore _navigationStore;

        public MainWindow()
        {
            _navigationStore = new NavigationStore();
            _navigationStore.CurrentViewModel = new ShowUserDataViewModel(_navigationStore);
            DataContext = new MainViewModel(_navigationStore);
            InitializeComponent();
        }
    }
}
