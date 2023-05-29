using FeetFinder.Logic;
using FeetScraper;
using FeetScraper.Models;
using neXn.Lib.Wpf.ViewLogic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FeetFinder.ViewModels
{
    public class PictureWindowViewModel : ViewModelBase
    {
        public ICommand DownloadCommand { get; } = new RelayCommand<Window>((w) =>
        {
            PictureWindowViewModel vm = (PictureWindowViewModel)w.DataContext;

            Task.Factory.StartNew(() =>
            {
                vm.FootPicture.DownloadFootPictureAsync().Wait();
                RuntimeStorage.DownloadService.Add(vm.FootPicture);
                RuntimeStorage.DownloadService.Save();
                vm.CheckIfDownloaded();
            });
        });

        public ICommand CloseCommand { get; } = new RelayCommand<Window>((w) => w.Close());

        #region BindableProperties
        private Window _Instance;
        public Window Instance
        {
            get
            {
                return this._Instance;
            }
            set
            {
                this._Instance = value;
                base.OnPropertyChanged(nameof(this.Instance));
            }
        }

        private FootPicture _FootPicture;
        public FootPicture FootPicture
        {
            get
            {
                return this._FootPicture;
            }
            set
            {
                this._FootPicture = value;
                base.OnPropertyChanged(nameof(this.FootPicture));
                this.CheckIfDownloaded();
            }
        }

        private bool _IsDownloaded;
        public bool IsDownloaded
        {
            get
            {
                return this._IsDownloaded;
            }
            set
            {
                this._IsDownloaded = value;
                base.OnPropertyChanged(nameof(this.IsDownloaded));
            }
        }
        #endregion

        private void CheckIfDownloaded()
        {
            this.IsDownloaded = RuntimeStorage.DownloadService.Any(x => x.Id == this.FootPicture.Id);
        }
    }
}
