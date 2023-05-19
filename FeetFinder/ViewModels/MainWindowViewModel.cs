using neXn.Lib.Wpf.ViewLogic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FeetFinder.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Commands
        public ICommand CloseCommand { get; } = new RelayCommand<Window>((w) => w?.Close());
        #endregion

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

        private Page _MainFramePage;
        public Page MainFramePage
        {
            get
            {
                return this._MainFramePage;
            }
            set
            {
                this._MainFramePage = value;
                base.OnPropertyChanged(nameof(this.MainFramePage));
            }
        }
        #endregion
    }
}
