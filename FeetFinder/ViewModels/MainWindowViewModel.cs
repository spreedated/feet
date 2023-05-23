using FeetFinder.Views;
using MahApps.Metro.IconPacks;
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
        public ICommand SearchCommand { get; } = new RelayCommand<MainWindow>((w) =>
        {
            
        });
        public ICommand HomeCommand { get; } = new RelayCommand<MainWindow>((w) =>
        {
            MainWindowViewModel vm = (MainWindowViewModel)w.DataContext;
            vm.MainFramePage = new Home();
        });
        public ICommand MaximizeCommand { get; } = new RelayCommand<MainWindow>((w) =>
        {
            if (w.WindowState == WindowState.Maximized)
            {
                w.WindowState = WindowState.Normal;
                return;
            }
            w.WindowState = WindowState.Maximized;
        });
        public ICommand MinimizeCommand { get; } = new RelayCommand<MainWindow>((w) =>
        {
            w.WindowState = WindowState.Minimized;
        });
        #endregion

        #region BindableProperties
        private Visibility _WindowIcon = Visibility.Visible;
        public Visibility WindowIcon
        {
            get
            {
                return this._WindowIcon;
            }
            set
            {
                this._WindowIcon = value;
                base.OnPropertyChanged(nameof(this.WindowIcon));
            }
        }

        private Visibility _Loading = Visibility.Visible;
        public Visibility Loading
        {
            get
            {
                return this._Loading;
            }
            set
            {
                this._Loading = value;
                base.OnPropertyChanged(nameof(this.Loading));
                this.MainFramePage = new Home();
            }
        }

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
