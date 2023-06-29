using Projekt.Helpers;
using Projekt.ViewModels.TemperatureVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projekt.Views.Temperature;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Projekt.Views.Temperature
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
    { 
        public MainPage ()
		{
			InitializeComponent ();
			this.Title = "Sterowanie temperaturą";
            this.BackgroundColor = Color.FromHex("#383b40");

			var vm = new TemperatureMainPageViewModel();

            _ = GetAllTemperatureUnitsAsync();


            BindingContext = vm;
        }

        public async Task GetAllTemperatureUnitsAsync() {
            var temperatureUnits = await FirebaseTemperatureHelper.GetAllTemperatureUnits();

            if (temperatureUnits != null)
            {
                var TemperatureUnits = temperatureUnits;
                TemperatureUnitsList.ItemsSource = TemperatureUnits;
            }
            else
            {
                TemperatureUnitsList.ItemsSource = null;
            }
        }

        private async void LisViewItemTapped (object sender, ItemTappedEventArgs e)
        {

            if(e.Item is Models.Temperature tappedItem)
            {
                TemperatureUnitDetailsViewModel detailsPageVM = new TemperatureUnitDetailsViewModel(tappedItem);
                await Navigation.PushAsync(new TemperaturUnitDetails(detailsPageVM));
            }

        }





    }
}