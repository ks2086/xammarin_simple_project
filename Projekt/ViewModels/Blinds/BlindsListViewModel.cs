using Projekt.Views.Blinds;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Projekt.ViewModels.Blinds
{
    public class BlindsListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand NavigateToAddNewBlindsUnitFormCommand { get; set; }
        

        public BlindsListViewModel()
        {
            NavigateToAddNewBlindsUnitFormCommand = new Command(NavigateToAddNewBlindsUnitAction);
 
        }

        private async void NavigateToAddNewBlindsUnitAction()
        {
            await App.Current.MainPage.Navigation.PushAsync(new AddBlindsUnit());
        }

        


    }
}
