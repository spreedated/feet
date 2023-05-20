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

namespace FeetFinder.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
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
            }
        }
        #endregion

        public HomeViewModel()
        {
            Task.Factory.StartNew(() =>
            {
                this.Loading = Visibility.Visible;

                FeetOfTheDay fotd = new();
                fotd.RetrieveAsync().Wait();
                this.Girlname = fotd.FeetOfTheDayResponse.Name;

                FeetPage fp = new(this.Girlname);

                FeetScraper.Models.PageResponse r = fp.RetrieveAsync().Result;

                this.FotdFeet = new(r.Feet);

                this.Loading = Visibility.Collapsed;
            });
        }
    }
}
