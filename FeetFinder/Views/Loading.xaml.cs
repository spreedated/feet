using FeetFinder.ViewModels;
using System.Windows.Controls;

namespace FeetFinder.Views
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class Loading : Page
    {
        public Loading()
        {
            this.InitializeComponent();
            ((LoadingViewModel)this.DataContext).Instance = this;
        }
    }
}
