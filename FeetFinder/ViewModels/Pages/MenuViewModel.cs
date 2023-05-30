using FeetFinder.Views;
using neXn.Lib.Wpf.ViewLogic;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FeetFinder.ViewModels.Pages
{
    internal class MenuViewModel : ViewModelBase
    {
        #region Commands
        public ICommand HomeCommand { get; } = new RelayCommand<Page>((p) =>
        {
            MarkActiveButton(0, p);
        });
        public ICommand FeetOfTheDayCommand { get; } = new RelayCommand<Page>((p) =>
        {
            MarkActiveButton(1, p);
            MainWindowViewModel vm = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            vm.MainFramePage = new FeetOfTheDay();
        });
        public ICommand SearchCommand { get; } = new RelayCommand<Page>((p) =>
        {
            MarkActiveButton(2, p);
        });
        public ICommand DownloadsCommand { get; } = new RelayCommand<Page>((p) =>
        {
            MarkActiveButton(3, p);
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
        #endregion

        public static void MarkActiveButton(uint id, Page page)
        {
            StackPanel stack = (StackPanel)page.FindName("STP_PageMenu");

            if (stack == null)
            {
                return;
            }

            IEnumerable<Button> buttons = stack.Children.OfType<UIElement>().Where(x => x.GetType() == typeof(Button)).Cast<Button>();

            if (!buttons.Any())
            {
                return;
            }

            foreach (Button button in buttons)
            {
                button.Style = (Style)Application.Current.FindResource("MaterialDesignRaisedButton");
            }

            Button biq = buttons.FirstOrDefault(x => x.Uid == id.ToString());

            if (biq == null)
            {
                return;
            }

            biq.Style = (Style)Application.Current.FindResource("MaterialDesignRaisedAccentButton");
        }

        public static string GetActiveButton(Page page)
        {
            StackPanel stack = (StackPanel)page.FindName("STP_PageMenu");

            if (stack == null)
            {
                return null;
            }

            IEnumerable<Button> buttons = stack.Children.OfType<UIElement>().Where(x => x.GetType() == typeof(Button)).Cast<Button>();

            if (!buttons.Any())
            {
                return null;
            }

            return (string)buttons.FirstOrDefault(x => x.Style == (Style)Application.Current.FindResource("MaterialDesignRaisedAccentButton"))?.Content;
        }
    }
}
