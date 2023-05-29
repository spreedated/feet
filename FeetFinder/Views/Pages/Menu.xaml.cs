using FeetFinder.ViewModels.Pages;
using System.Windows.Controls;

namespace FeetFinder.Views.Pages
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Page
    {
        public Menu()
        {
            this.InitializeComponent();
            ((MenuViewModel)this.DataContext).Instance = this;
        }
    }
}
