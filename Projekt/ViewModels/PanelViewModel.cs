using Projekt.Helpers;
using Projekt.Models;
using Projekt.Views.Temperature;
using Projekt.Views.Went;
using Projekt.Views.Heat;
using Projekt.Views.Blinds;

using System;
using System.Collections.Generic;
using System.Timers;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Projekt.Views;
using Projekt.Services;

namespace Projekt.ViewModels
{
    public class PanelViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly Timer timer;

        private decimal _temperature;
        private decimal _humidity;

        public decimal TemperatureDeg 
        {
            get { return _temperature; }
            set
            {
                _temperature = value;
                PropertyChanged(this, new PropertyChangedEventArgs("TemperatureDeg"));
            }
        }
        public decimal Humidity
        {
            get { return _humidity; }
            set
            {
                _humidity = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Humidity"));
            }
        }


        public ICommand GotoTeperatureModuleCommand { get; set; }
        public ICommand GotoWentModuleCommand { get; set; }
        public ICommand GotoHeatModuleCommand { get; set; }
        public ICommand GotoBlindsModuleCommand { get; set; }
        public ICommand GotoProfileModuleCommand { get; set; }
        public ICommand GotoLogsModuleCommand { get; set; }

        public PanelViewModel() {

            timer = new Timer();
            timer.Elapsed += TimerElapsed;
            timer.Interval = TimeSpan.FromSeconds(5).TotalMilliseconds;


            GotoTeperatureModuleCommand = new Command(NavigateToTemperatureModule);
            GotoWentModuleCommand = new Command(NavigateToWentModule);
            GotoHeatModuleCommand = new Command(NavigateToHeatModule);
            GotoBlindsModuleCommand = new Command(NavigateToBlindsModule);
            GotoProfileModuleCommand = new Command(NavigateProfileModule);
            GotoLogsModuleCommand = new Command(NavigateLogsModule);

            StartQuery();

        }

        public void StartQuery()
        {
            timer.Start();
        }

        public void StopQuery()
        {
            timer.Stop();
        }

        private async void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            List<Temperature> _temperatureUnits = await FirebaseTemperatureHelper.GetAllTemperatureUnits();
            if( _temperatureUnits != null )
            {
                int unitsCounts = _temperatureUnits.Count;
                decimal tempertatureSum = 0;
                decimal humiditySum = 0;

                Random random = new Random();

                foreach (Temperature temperature in _temperatureUnits )
                {
                    tempertatureSum += temperature.TemperatureDeg;
                    humiditySum += temperature.Humidity;

                    decimal randomTemp = Math.Round((decimal)(random.NextDouble() * (23 - 19) + 19), 2);
                    decimal randomHumidity = Math.Round((decimal)(random.NextDouble() * (70 - 40) + 40), 2);
                    await FirebaseTemperatureHelper.SyncTemperatureUnit(randomTemp, randomHumidity, temperature.Id);
                }

                TemperatureDeg = unitsCounts > 0 ? Math.Round((decimal) (tempertatureSum / unitsCounts), 2) : 0;
                Humidity = unitsCounts > 0 ? Math.Round((decimal)(humiditySum / unitsCounts), 2) : 0;

                _ = LogsHandler.CreateLog("units_sync", "Main", null);
            }
        }

        public async void NavigateToTemperatureModule()
        {
            await App.Current.MainPage.Navigation.PushAsync(new MainPage());
        }

        public async void NavigateToWentModule()
        {
            Ventilation ventUnit = await FirebaseWentHelper.GetUserVentilationUnit();
            if (ventUnit == null)
            {
                await App.Current.MainPage.Navigation.PushAsync(new CreateNewVentUnit());
            }
            else
            {
                await App.Current.MainPage.Navigation.PushAsync(new ManageVentUnit());
            }
        }

        public async void NavigateToHeatModule()
        {
            HeatPump hp = await FirebaseHeatHelper.GetUserHeatUnit();
            if(hp == null)
            {
                await App.Current.MainPage.Navigation.PushAsync(new CreateNewHeatPumpUnit());
            }
            else
            {
                await App.Current.MainPage.Navigation.PushAsync(new ManageHeatPumpUnit());
            }
        }

        public async void NavigateToBlindsModule()
        {
            await App.Current.MainPage.Navigation.PushAsync(new BlindsListView());
        }

        public async void NavigateProfileModule() 
        {
            await App.Current.MainPage.Navigation.PushAsync(new Profile());
        }

        public async void NavigateLogsModule()
        {
            await App.Current.MainPage.Navigation.PushAsync(new Logs());
        }
    }
}
