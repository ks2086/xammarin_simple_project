using Projekt.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Projekt.ViewModels.Blinds
{
    public class AddBlindsUnitViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _description;
        private string _isClosedStr;
        private bool _isClosed;

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Description"));
            }
        }

        public string IsClosedStr
        {
            get { return _isClosedStr; }
            set
            {
                _isClosedStr = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsClosedStr"));
            }
        }

        public bool IsClosed
        {
            get { return _isClosed; }
            set
            {
                _isClosed = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsClosed"));
            }
        }

        public ICommand SaveNewBlindsUnitCommand { get; set; }

        public AddBlindsUnitViewModel()
        {
            SaveNewBlindsUnitCommand = new Command(SaveNewBlindsUnitAction);
        }

        private async void SaveNewBlindsUnitAction()
        {

            if (string.IsNullOrEmpty(Description))
            {
                await App.Current.MainPage.DisplayAlert("Błąd danych", "Formularz zawiera niepoprawne wartości", "OK");
            }
            else
            {
                IsClosed = IsClosedStr == "Opuszczona" ? true : false;

                if (await FirebaseBlindsHelper.StoreBlindsUnit(Description, IsClosed))
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
