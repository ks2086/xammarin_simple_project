using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

using Projekt.Models;
using Projekt.Helpers;
using Projekt.Views.Temperature;
using System.Collections.ObjectModel;

namespace Projekt.ViewModels.TemperatureVM
{
    class TemperatureMainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        /*
        private List<Temperature> _temperatureUnits;
        public List<Temperature> TemperatureUnits
        {
            get { return _temperatureUnits; }
            set
            {
                _temperatureUnits = value;
                OnPropertyChanged(nameof(TemperatureUnits));
                //PropertyChanged(this, new PropertyChangedEventArgs("Name"));
            }
        }
        */
       // public string Name { get; set; }

        public ICommand NavigateToAddNewTemperatureUnitFormCommand { get; set; }

        public TemperatureMainPageViewModel()
        {
            NavigateToAddNewTemperatureUnitFormCommand = new Command(GoToAddNewTemperatureForm);
            //GetTemperatureUnitsList();
        }

        private async void GoToAddNewTemperatureForm()
        {
            await App.Current.MainPage.Navigation.PushAsync(new AddTemperatureUnit());
        }
        /*
        public async void GetTemperatureUnitsList()
        {
            TemperatureUnits = await FirebaseTemperatureHelper.GetAllTemperatureUnits();
            //Console.WriteLine("Zapisało do tablicy: " + TemperatureUnits.Count);
        }
        */
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
