using FeetFinder.Views;
using neXn.Lib.Wpf.ViewLogic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FeetFinder.ViewModels.Pages
{
    internal class MenuViewModel : ViewModelBase
    {
        #region Commands
        public ICommand SearchCommand { get; } = new RelayCommand<Button>((b) =>
        {

        });
        public ICommand FeetOfTheDayCommand { get; } = new RelayCommand<Button>((b) =>
        {
            MainWindowViewModel vm = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            vm.MainFramePage = new FeetOfTheDay();
        });
        public ICommand CloseCommand { get; } = new RelayCommand(() => Application.Current.MainWindow?.Close());
        #endregion

        #region BindableProperties
        private Page _Instance;
        public Page Instance
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

        private string _SelectedButton = "0";
        public string SelectedButton
        {
            get
            {
                return this._SelectedButton;
            }
            set
            {
                this._SelectedButton = value;
                base.OnPropertyChanged(nameof(this.SelectedButton));
            }
        }
        #endregion
    }
}
