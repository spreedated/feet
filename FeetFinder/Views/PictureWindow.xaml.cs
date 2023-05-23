using FeetFinder.ViewModels;
using FeetScraper.Models;
using System.Windows;
using System.Windows.Input;

namespace FeetFinder.Views
{
    /// <summary>
    /// Interaction logic for PictureWindow.xaml
    /// </summary>
    public partial class PictureWindow : Window
    {
        public PictureWindow()
        {
            this.InitializeComponent();
            ((PictureWindowViewModel)this.DataContext).Instance = this;
        }

        public PictureWindow(FootPicture footPicture) : this()
        {
            ((PictureWindowViewModel)this.DataContext).FootPicture = footPicture;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
