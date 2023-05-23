using FeetFinder.ViewModels;
using FeetScraper.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FeetFinder.Views
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            this.InitializeComponent();
            ((HomeViewModel)this.DataContext).Instance = this;
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FootPicture footp = ((HomeViewModel)this.DataContext).SelectedFotd;

            if (footp == null)
            {
                return;
            }

            PictureWindow pw = new(((HomeViewModel)this.DataContext).SelectedFotd)
            {
                Owner = Application.Current.MainWindow
            };
            pw.Show();
        }
    }
}
