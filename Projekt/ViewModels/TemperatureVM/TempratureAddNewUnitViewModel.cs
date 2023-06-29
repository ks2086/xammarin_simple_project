using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Projekt.Helpers;
using Projekt.Models;
using System.Threading.Tasks;

namespace Projekt.ViewModels.TemperatureVM
{
    public class TempratureAddNewUnitViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _model;
        private string _description; 

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Description"));
            }
        }

        public string Model
        {
            get { return _model; }
            set
            {
                _model = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Model"));
            }
        }

        public ICommand SaveNewTemperatureUnitCommand { get; set; }

        public TempratureAddNewUnitViewModel()
        {
            SaveNewTemperatureUnitCommand = new Command(OnSubmit);
        }

        private async void OnSubmit()
        {

            if (string.IsNullOrEmpty(Model) || string.IsNullOrEmpty(Description))
            {
                await App.Current.MainPage.DisplayAlert("Błąd danych", "Formularz zawiera niepoprawne wartości", "OK");
            }
            else
            {
                if(await FirebaseTemperatureHelper.SaveTemperatureUnit(Model, Description))
                {
                    //await App.Current.MainPage.DisplayAlert("Sukces", "Dane zostały zapisane", "OK");
                    await App.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Wystąpił problem", "Dane nie zostały zapisane. Spróbuj ponownie później.", "OK");
                }
            }
        }


    }
}
