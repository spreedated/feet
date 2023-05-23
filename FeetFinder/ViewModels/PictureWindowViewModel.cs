using neXn.Lib.Wpf.ViewLogic;
using System.Windows;
using System.Windows.Input;

namespace FeetFinder.ViewModels
{
    public class PictureWindowViewModel : ViewModelBase
    {
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

        private FeetScraper.Models.FootPicture _FootPicture;
        public FeetScraper.Models.FootPicture FootPicture
        {
            get
            {
                return this._FootPicture;
            }
            set
            {
                this._FootPicture = value;
                base.OnPropertyChanged(nameof(this.FootPicture));
            }
        }
        #endregion
    }
}
