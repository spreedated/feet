using neXn.Lib.Wpf.ViewLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using neXn.Lib.Availability;
using neXn.Lib.Availability.Models;
using FeetFinder.Views;
using System.Windows;
using System.Windows.Controls;
using static FeetScraper.Logic.Constants;
using System.Threading;

namespace FeetFinder.ViewModels
{
    public class LoadingViewModel : ViewModelBase
    {
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

        private double _ProgressbarValue;
        public double ProgressbarValue
        {
            get
            {
                return this._ProgressbarValue;
            }
            set
            {
                this._ProgressbarValue = value;
                base.OnPropertyChanged(nameof(this.ProgressbarValue));
            }
        }
        #endregion

        public LoadingViewModel()
        {
            Task.Factory.StartNew(() =>
            {
                this.ProgressbarValue += 10.0d;
                Thread.Sleep(5000);
                this.ProgressbarValue += 10.0d;
                if (this.CheckInternetAvailabilty().All(x => x.Results.Values.All(y => y == Inspectable.Result.Available)))
                {
                    this.Instance.Dispatcher.Invoke(() =>
                    {
                        ((MainWindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).MainFramePage = null;
                    });
                }
            });
        }

        private IEnumerable<Inspectable> CheckInternetAvailabilty()
        {
            Inspectable[] ins = new Inspectable[]
            {
                new Inspectable(FEET_URL, 0, Inspectable.Protocol.Http),
                new Inspectable(FEET_URL_MEN, 0, Inspectable.Protocol.Http),
                new Inspectable("http://google.com", 0, Inspectable.Protocol.Http)
            };

            Scan s = new(ins);

            s.Start();

            return s.Inspectables;
        }
    }
}
