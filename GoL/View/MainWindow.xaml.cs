using System.Windows;
using GoL.ViewModel;

namespace GoL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.DataContext = new MainWindowViewModel();
            this.InitializeComponent(); 
        }
    }
}
