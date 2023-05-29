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
    public partial class FeetOfTheDay : Page
    {
        public FeetOfTheDay()
        {
            this.InitializeComponent();
            ((FeetOfTheDayViewModel)this.DataContext).Instance = this;
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FootPicture footp = ((FeetOfTheDayViewModel)this.DataContext).SelectedFotd;

            if (footp == null)
            {
                return;
            }

            PictureWindow pw = new(((FeetOfTheDayViewModel)this.DataContext).SelectedFotd)
            {
                Owner = Application.Current.MainWindow
            };
            pw.Show();
        }
    }
}
