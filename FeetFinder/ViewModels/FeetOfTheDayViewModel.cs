using FeetFinder.Logic;
using FeetScraper.Logic;
using neXn.Lib.Wpf.ViewLogic;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FeetFinder.ViewModels
{
    public class FeetOfTheDayViewModel : ViewModelBase
    {
        public ICommand NextPage { get; } = new RelayCommand<Page>((w) =>
        {
            FeetOfTheDayViewModel vm = ((FeetOfTheDayViewModel)w.DataContext);

            if (vm.PageIndex + 1 > vm.PagesMax)
            {
                return;
            }

            vm.PageIndex++;
            vm.FotdFeet = new(RuntimeStorage.FeetofthedayResponse.Feet.Skip((vm.PageIndex - 1) * 55).Take(55));
        });

        public ICommand PrevPage { get; } = new RelayCommand<Page>((w) =>
        {
            FeetOfTheDayViewModel vm = ((FeetOfTheDayViewModel)w.DataContext);

            if (vm.PageIndex - 1 < 1)
            {
                return;
            }

            vm.PageIndex--;
            vm.FotdFeet = new(RuntimeStorage.FeetofthedayResponse.Feet.Skip((vm.PageIndex - 1) * 55).Take(55));
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

                if (this._PageIndex >= this._PagesMax)
                {
                    this.NextPageEnabled = false;
                    this.PrevPageEnabled = true;
                }

                if (this._PageIndex <= 1)
                {
                    this.NextPageEnabled = true;
                    this.PrevPageEnabled = false;
                }
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

        private bool _NextPageEnabled = true;
        public bool NextPageEnabled
        {
            get
            {
                return this._NextPageEnabled;
            }
            set
            {
                this._NextPageEnabled = value;
                base.OnPropertyChanged(nameof(this.NextPageEnabled));
            }
        }

        private bool _PrevPageEnabled;
        public bool PrevPageEnabled
        {
            get
            {
                return this._PrevPageEnabled;
            }
            set
            {
                this._PrevPageEnabled = value;
                base.OnPropertyChanged(nameof(this.PrevPageEnabled));
            }
        }

        private float _AverageRating;
        public float AverageRating
        {
            get
            {
                return this._AverageRating;
            }
            set
            {
                this._AverageRating = value;
                base.OnPropertyChanged(nameof(this.AverageRating));
            }
        }

        private float? _Shoesize;
        public float? Shoesize
        {
            get
            {
                return this._Shoesize;
            }
            set
            {
                this._Shoesize = value;
                base.OnPropertyChanged(nameof(this.Shoesize));
            }
        }

        private string _Birthplace;
        public string Birthplace
        {
            get
            {
                return this._Birthplace;
            }
            set
            {
                this._Birthplace = value;
                base.OnPropertyChanged(nameof(this.Birthplace));
            }
        }

        private DateTime? _Birthdate;
        public DateTime? Birthdate
        {
            get
            {
                return this._Birthdate;
            }
            set
            {
                this._Birthdate = value;
                base.OnPropertyChanged(nameof(this.Birthdate));
            }
        }
        #endregion

        public FeetOfTheDayViewModel()
        {
            Task.Factory.StartNew(() =>
            {
                this.Loading = Visibility.Visible;

                if (RuntimeStorage.Feetoftheday == null || RuntimeStorage.Feetoftheday.Date != DateTime.Now.Date)
                {
                    FeetScraper.Logic.FeetOfTheDay fotd = new();
                    RuntimeStorage.Feetoftheday = fotd.RetrieveAsync().Result;

                    FeetPage fp = new(RuntimeStorage.Feetoftheday.Name);
                    RuntimeStorage.FeetofthedayResponse = fp.RetrieveAsync().Result;
                }

                this.Girlname = RuntimeStorage.FeetofthedayResponse.Name;
                this.AverageRating = RuntimeStorage.FeetofthedayResponse.RatingStats.FiveStarRating;
                this.Birthdate = RuntimeStorage.FeetofthedayResponse.Birthday;
                this.Birthplace = RuntimeStorage.FeetofthedayResponse.Birthplace;
                this.Shoesize = RuntimeStorage.FeetofthedayResponse.ShoeSize;
                this.FeetpictureCount = RuntimeStorage.FeetofthedayResponse.Feet.Count();
                this.FotdFeet = new(RuntimeStorage.FeetofthedayResponse.Feet.Skip(0).Take(55));

                this.Loading = Visibility.Collapsed;
            });
        }
    }
}
