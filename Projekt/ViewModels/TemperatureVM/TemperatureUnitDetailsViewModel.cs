using Projekt.Helpers;
using Projekt.Models;
using Projekt.Views.Temperature;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Projekt.ViewModels.Base;

namespace Projekt.ViewModels.TemperatureVM
{
    public class TemperatureUnitDetailsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        public string Description { get; set; }
        public string Id { get; set; }
        public bool IsConnected { get; set; }
        public decimal TemperatureDeg { get; set; }
        public decimal Humidity { get; set; }

        public ICommand UpdateUnitCommand { get; set; }
        public ICommand SyncUnitCommand { get; set; }
        public ICommand DeleteUnitCommand { get; set; }

        public TemperatureUnitDetailsViewModel(Temperature unit)
        {
            if (unit != null)
            {
                Description = unit.Description;
                Id = unit.Id;
                IsConnected = unit.IsConnected;
                TemperatureDeg = unit.TemperatureDeg;
                Humidity = unit.Humidity;
            }

            UpdateUnitCommand = new Command(UpdateAction);
            SyncUnitCommand = new Command(SyncUnitAction);
            DeleteUnitCommand = new Command(DeleteUnitAction);
        }

        private async void refresh()
        {
            Temperature temperature =  await FirebaseTemperatureHelper.GetUnitById(Id);
            if(temperature != null)
            {
                TemperatureDeg = temperature.TemperatureDeg;
                OnPropertyChanged(nameof(TemperatureDeg));
                Humidity = temperature.Humidity;
                OnPropertyChanged(nameof(Humidity));
                Description = temperature.Description;
                OnPropertyChanged(nameof(Description));
            }

        }

        private async void UpdateAction() 
        {
            if (string.IsNullOrEmpty(Description))
            {
                await App.Current.MainPage.DisplayAlert("Błąd danych", "Formularz zawiera niepoprawne wartości", "OK");
            }
            else
            {
                if (await FirebaseTemperatureHelper.UpdateTemperatureUnit(Description, Id))
                {
                    await App.Current.MainPage.DisplayAlert("Sukces", "Dane zostały zapisane", "OK");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Wystąpił problem", "Dane nie zostały zapisane. Spróbuj ponownie później.", "OK");
                }
            }
        }

        private async void SyncUnitAction()
        {
            Random random = new Random();

            decimal randomTemp = Math.Round((decimal)(random.NextDouble() * (23 - 19) + 19), 2);
            decimal randomHumidity = Math.Round((decimal)(random.NextDouble() * (70 - 40) + 40), 2);

            if (await FirebaseTemperatureHelper.SyncTemperatureUnit(randomTemp, randomHumidity, Id))
            {
                refresh();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Wystąpił problem", "Dane nie zostały zapisane. Spróbuj ponownie później.", "OK");
            }
        }

        private async void DeleteUnitAction()
        {
            if(await FirebaseTemperatureHelper.DeleteTemperatureUnit(Id))
            {
                await App.Current.MainPage.Navigation.PushAsync(new MainPage());
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Wystąpił problem", "Dane nie zostały usunięte. Spróbuj ponownie później.", "OK");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
