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
using FeetFinder.Attributes;
using System.Reflection;

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
                Thread.Sleep(2400);

                if (this.StartLoading().All(x => x))
                {
                    Thread.Sleep(2400);
                    this.Instance.Dispatcher.Invoke(() => ((MainWindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).Loading = Visibility.Collapsed);
                }
            });
        }

        private IEnumerable<bool> StartLoading()
        {
            IEnumerable<MethodInfo> p = typeof(LoadingViewModel).GetMethods(BindingFlags.Public | BindingFlags.Instance)?.Where(x => x.GetCustomAttributes(false).Any(y => y.GetType() == typeof(PreloadAttribute)));

            if (p == null || !p.Any())
            {
                return Array.Empty<bool>();
            }

            List<bool> results = new();

            float step = (float)Math.Round(100 / (float)p.Count(), 2);

            foreach (MethodInfo pMethod in p)
            {
                if (pMethod.ReturnType == null)
                {
                    pMethod.Invoke(this, null);
                    this.ProgressbarValue += step;
                    results.Add(true);
                    continue;
                }

                bool result = (bool)pMethod.Invoke(this, null);
                results.Add(result);
                this.ProgressbarValue += step;
            }

            this.ProgressbarValue = 100.0d;

            return results;
        }

        [Preload]
        public bool CheckInternetAvailabilty()
        {
            Inspectable[] ins = new Inspectable[]
            {
                new Inspectable(FEET_URL, 0, Inspectable.Protocol.Http),
                new Inspectable(FEET_URL_MEN, 0, Inspectable.Protocol.Http),
                new Inspectable("http://google.com", 0, Inspectable.Protocol.Http)
            };

            Scan s = new(ins);

            s.Start();

            return s.Inspectables.All(x => x.Results.Values.All(y => y == Inspectable.Result.Available));
        }
    }
}
