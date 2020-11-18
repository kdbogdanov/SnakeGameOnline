using System.Windows;
using System.Windows.Controls;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        public MainWindowView()
        {
            InitializeComponent();
        }

        private void ExitCommand(object sender, System.Windows.RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
