using FeetFinder.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace FeetFinder.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            ((MainWindowViewModel)this.DataContext).Instance = this;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Window_StateChanged(object sender, System.EventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                ((MainWindowViewModel)this.DataContext).WindowIcon = Visibility.Collapsed;
                return;
            }
            ((MainWindowViewModel)this.DataContext).WindowIcon = Visibility.Visible;
        }
    }
}
