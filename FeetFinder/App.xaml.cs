using FeetFinder.Logic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace FeetFinder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string directory = Path.GetDirectoryName(typeof(App).Assembly.Location);
            RuntimeStorage.DownloadService = new(Path.Combine(directory, "downloads.json"));

            RuntimeStorage.DownloadService.LoadAsync();
        }
    }
}
