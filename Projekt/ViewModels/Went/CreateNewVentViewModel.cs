using Projekt.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Projekt.ViewModels.Went
{
    public class CreateNewVentViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Model { get; set; }

        public ICommand SaveNewVentUnitCommand { get; set; }

        public CreateNewVentViewModel()
        {
            SaveNewVentUnitCommand = new Command(SaveNewVentUnitAction);
        }

        private async void SaveNewVentUnitAction() 
        {
            if (string.IsNullOrEmpty(Model))
            {
                await App.Current.MainPage.DisplayAlert("Błąd danych", "Formularz zawiera niepoprawne wartości", "OK");
            }
            else
            {
                if (await FirebaseWentHelper.StoreVentUnit(Model))
                {
                    await App.Current.MainPage.DisplayAlert("Sukces", "Dane zostały zapisane", "OK");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Wystąpił problem", "Dane nie zostały zapisane. Spróbuj ponownie później.", "OK");
                }
            }
        }

    }
}
