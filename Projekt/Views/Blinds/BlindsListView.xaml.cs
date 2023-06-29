using Projekt.Helpers;
using Projekt.ViewModels.Blinds;
using Projekt.ViewModels.TemperatureVM;
using Projekt.Views.Temperature;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Projekt.Views.Blinds
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BlindsListView : ContentPage
    {

        public ICommand OpenCloseBlindUnitCommand { get; set; }

        public BlindsListView()
        {
            InitializeComponent();
            this.Title = "Sterowanie roletami / żaluzjami";
            this.BackgroundColor = Color.FromHex("#383b40");

            var vm = new BlindsListViewModel();
            _ = GetAllBlindUnitsAsync();

      
            this.BindingContext = vm;
        }

        public async Task GetAllBlindUnitsAsync()
        {
            var units = await FirebaseBlindsHelper.GetAllBlindUnits();

            if (units != null)
            {
                var BlindUnits = units;
                BlindUnitsList.ItemsSource = BlindUnits;
            }
            else
            {
                BlindUnitsList.ItemsSource = null;
            }
        }

        private async void LisViewItemTapped(object sender, ItemTappedEventArgs e)
        {

            if (e.Item is Models.Blind tappedItem)
            {
                BlindDetailsViewModel detailsPageVM = new BlindDetailsViewModel(tappedItem);
                await Navigation.PushAsync(new BlindDetails(detailsPageVM));
            }

        }

    }

    
}