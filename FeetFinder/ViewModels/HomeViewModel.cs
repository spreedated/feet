using neXn.Lib.Wpf.ViewLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using FeetScraper.Logic;
using System.Collections.ObjectModel;
using FeetFinder.Logic;
using System.Windows.Input;

namespace FeetFinder.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public ICommand NextPage { get; } = new RelayCommand<Page>((w) =>
        {
            HomeViewModel vm = ((HomeViewModel)w.DataContext);
            vm.PageIndex += 1;
            vm.FotdFeet = new(RuntimeStorage.FeetofthedayResponse.Feet.Skip(vm.PageIndex * 50).Take(50));
        });

        #region BindableProperties
        private string _Girlname;
        public string Girlname
        {
            get
            {
                return this._Girlname;
            }
            set
            {
                this._Girlname = value;
                base.OnPropertyChanged(nameof(this.Girlname));
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
            }
        }

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

        private ObservableCollection<FeetScraper.Models.FootPicture> _FotdFeet;
        public ObservableCollection<FeetScraper.Models.FootPicture> FotdFeet
        {
            get
            {
                return this._FotdFeet;
            }
            set
            {
                this._FotdFeet = value;
                base.OnPropertyChanged(nameof(this.FotdFeet));
                this.PagesMax = (int)Math.Ceiling((double)RuntimeStorage.FeetofthedayResponse.Feet.Count() / 50);
            }
        }

        private FeetScraper.Models.FootPicture _SelectedFotd;
        public FeetScraper.Models.FootPicture SelectedFotd
        {
            get
            {
                return this._SelectedFotd;
            }
            set
            {
                this._SelectedFotd = value;
                base.OnPropertyChanged(nameof(this.SelectedFotd));
            }
        }

        private int _FeetpictureCount;
        public int FeetpictureCount
        {
            get
            {
                return this._FeetpictureCount;
            }
            set
            {
                this._FeetpictureCount = value;
                base.OnPropertyChanged(nameof(this.FeetpictureCount));
            }
        }

        private int _PageIndex = 1;
        public int PageIndex
        {
            get
            {
                return this._PageIndex;
            }
            set
            {
                this._PageIndex = value;
                base.OnPropertyChanged(nameof(this.PageIndex));
            }
        }

        private int _PagesMax = 1;
        public int PagesMax
        {
            get
            {
                return this._PagesMax;
            }
            set
            {
                this._PagesMax = value;
                base.OnPropertyChanged(nameof(this.PagesMax));
                this.PageControlsVisibility = this._PagesMax > 1 ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private Visibility _PageControlsVisibility = Visibility.Collapsed;
        public Visibility PageControlsVisibility
        {
            get
            {
                return this._PageControlsVisibility;
            }
            set
            {
                this._PageControlsVisibility = value;
                base.OnPropertyChanged(nameof(this.PageControlsVisibility));
            }
        }
        #endregion

        public HomeViewModel()
        {
            Task.Factory.StartNew(() =>
            {
                this.Loading = Visibility.Visible;

                if (RuntimeStorage.Feetoftheday == null || RuntimeStorage.Feetoftheday.Date != DateTime.Now.Date)
                {
                    FeetOfTheDay fotd = new();
                    fotd.RetrieveAsync().Wait();
                    this.Girlname = fotd.FeetOfTheDayResponse.Name;

                    //FeetPage fp = new(this.Girlname);
                    FeetPage fp = new("Jeri Ryan");

                    RuntimeStorage.FeetofthedayResponse = fp.RetrieveAsync().Result;
                    RuntimeStorage.Feetoftheday = fotd.FeetOfTheDayResponse;
                }

                this.FeetpictureCount = RuntimeStorage.FeetofthedayResponse.Feet.Count();
                this.FotdFeet = new(RuntimeStorage.FeetofthedayResponse.Feet.Skip(0).Take(50));

                this.Loading = Visibility.Collapsed;
            });
        }
    }
}
