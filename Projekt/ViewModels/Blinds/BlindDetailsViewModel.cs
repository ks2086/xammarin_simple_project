using Projekt.Helpers;
using Projekt.Models;
using Projekt.Views.Temperature;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;



namespace Projekt.ViewModels.Blinds
{
    public class BlindDetailsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _levelPercenage;

        public string LevelPercentage
        {
            get { return _levelPercenage; }
            set
            {
                _levelPercenage = value;
                PropertyChanged(this, new PropertyChangedEventArgs("LevelPercentage"));
            }
        }

        public string Description { get; set; }
        public string Id { get; set; }
        public bool IsClosed { get; set; }

        public ICommand UpdateBlindUnitCommand { get; set; }
        public ICommand OpenCloseBlindUnitCommand { get; set; }
        public ICommand DeleteBlindUnitCommand { get; set; }

        public BlindDetailsViewModel(Blind unit)
        {
            if (unit != null)
            {
                Description = unit.Description;
                IsClosed = unit.IsClosed;
                Id = unit.Id;
            }

            UpdateBlindUnitCommand = new Command(UpdateBlindUnitAction);
            OpenCloseBlindUnitCommand = new Command(OpenCloseBlindUnitAction);
            DeleteBlindUnitCommand = new Command(DeleteBlindUnitAction);
        }


        private async void UpdateBlindUnitAction() 
        {
            if (string.IsNullOrEmpty(Description))
            {
                await App.Current.MainPage.DisplayAlert("Błąd danych", "Formularz zawiera niepoprawne wartości", "OK");
            }
            else
            {
                if (await FirebaseBlindsHelper.UpdateBlindsUnit("Description", Description, Id))
                {
                    await App.Current.MainPage.DisplayAlert("Sukces", "Dane zostały zapisane", "OK");
                    OnPropertyChanged(nameof(Description));
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Wystąpił problem", "Dane nie zostały zapisane. Spróbuj ponownie później.", "OK");
                }
            }
        }
        private async void OpenCloseBlindUnitAction() 
        {
            int percent = 0;
            LevelPercentage = "0%";
            for (int i = 0; i < 100; i++) {
                await Task.Delay(100);
                percent++;
                LevelPercentage = percent + "%";
        
            }

            bool status = !IsClosed;
            if (await FirebaseBlindsHelper.UpdateBlindsUnit("IsClosed", status.ToString(), Id))
            {
                await App.Current.MainPage.DisplayAlert("Sukces", status ? "Roleta została podniesiona" : "Roleta została opuszczona", "OK");
            }

            IsClosed = status;
            OnPropertyChanged(nameof(IsClosed));
            LevelPercentage = "0%";
        }

        private async void DeleteBlindUnitAction() 
        {
            if (await FirebaseBlindsHelper.DeleteBlindUnit(Id))
            {
                await App.Current.MainPage.Navigation.PopAsync();
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
