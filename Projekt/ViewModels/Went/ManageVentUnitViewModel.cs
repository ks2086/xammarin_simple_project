using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Projekt.ViewModels.Base;
using Projekt.Helpers;
using Projekt.Models;
using Projekt.Views;
using Projekt.Services;

namespace Projekt.ViewModels.Went
{
    public class ManageVentUnitViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _model;
        private int _powerLevel;
        private decimal _moistureRecoveryLevel;

        public string Id { get; set; }
        public string Model
        {
            get { return _model; }
            set
            {
                _model = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Model"));
            }
        }
        public int PowerLevel
        {
            get { return _powerLevel; }
            set
            {
                _powerLevel = value;
                PropertyChanged(this, new PropertyChangedEventArgs("PowerLevel"));
            }
        }

        public decimal MoistureRecoveryLevel
        {
            get { return _moistureRecoveryLevel; }
            set
            {
                _moistureRecoveryLevel = value;
                PropertyChanged(this, new PropertyChangedEventArgs("MoistureRecoveryLevel"));
            }
        }




        public ICommand IncVentUnitPowerCommand { get; set; }
        public ICommand DecVentUnitPowerCommand { get; set; }

        public ICommand IncVentUnitMoistureRecoveryLevelCommand { get; set; }
        public ICommand DecVentUnitMoistureRecoveryLevelCommand { get; set; }

        public ICommand RemoveVentUnitCommand { get; set; }
        public ManageVentUnitViewModel() {
            IncVentUnitPowerCommand = new Command(IncVentUnitPowerAction);
            DecVentUnitPowerCommand = new Command(DecVentUnitPowerAction);

            IncVentUnitMoistureRecoveryLevelCommand = new Command(IncVentUnitMoistureRecoveryLevelAction);
            DecVentUnitMoistureRecoveryLevelCommand = new Command(DecVentUnitMoistureRecoveryLevelAction);

            RemoveVentUnitCommand = new Command(RemoveVentUnitAction);

            GetMyVent();

        }

        private async void GetMyVent()
        {
            Ventilation ventilation = await FirebaseWentHelper.GetUserVentilationUnit();
            if(ventilation != null)
            {
                Id = ventilation.Id;
                Model = ventilation.Model;
                PowerLevel = ventilation.PowerLevel;
                MoistureRecoveryLevel = ventilation.MoistureRecoveryLevel;
            }
        }

        private async void IncVentUnitPowerAction() {
            if(PowerLevel < 500)
            {

                if(PowerLevel == 0)
                {
                    _ = LogsHandler.CreateLog("device_turn_on", "Went", Id);
                }

                PowerLevel += 100;
                UpdateVent("PowerLevel", PowerLevel.ToString());
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Informacja systemowa", "Osiągnięto maksymalny poziom mocy dla tego urządzenia.", "OK");
            }
        }
        private async void DecVentUnitPowerAction() {
            if (PowerLevel > 0)
            {
                PowerLevel -= 100;
                UpdateVent("PowerLevel", PowerLevel.ToString());
                if(PowerLevel == 0)
                {
                    await App.Current.MainPage.DisplayAlert("Informacja systemowa", "Urządzenie zostało wyłączone.", "OK");
                    _ = LogsHandler.CreateLog("device_turn_off", "Went", Id);
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Informacja systemowa", "Urządzenie jest wyłączone.", "OK");
            }
        }

       

        private async void IncVentUnitMoistureRecoveryLevelAction() 
        {
            if (MoistureRecoveryLevel < 100)
            {
                MoistureRecoveryLevel += 25;
                UpdateVent("MoistureRecoveryLevel", MoistureRecoveryLevel.ToString());
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Informacja systemowa", "Osiągnięto maksymalny poziom odzysku wilgoci dla tego urządzenia.", "OK");
            }
        }
        private async void DecVentUnitMoistureRecoveryLevelAction() 
        {
            if (MoistureRecoveryLevel >= 25)
            {
                MoistureRecoveryLevel -= 25;
                UpdateVent("MoistureRecoveryLevel", MoistureRecoveryLevel.ToString());
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Informacja systemowa", "Osiągnięto minimalny poziom odzysku wilgoci dla tego urządzenia.", "OK");
            }
        }
        private async void RemoveVentUnitAction() {
            if(await FirebaseWentHelper.DeleteVentUnit())
            {
                await App.Current.MainPage.Navigation.PushAsync(new Panel());
            }
        }

        private async void UpdateVent(string field, string value)
        {
            if (await FirebaseWentHelper.UpdateVentUnit(field, value))
            {
                Ventilation ventUnit = await FirebaseWentHelper.GetUserVentilationUnit();
                if (ventUnit != null)
                {
                    Refresh(ventUnit);
                }
            }
        }

        private async void Refresh(Ventilation ventilationUnit) {
            PowerLevel = ventilationUnit.PowerLevel;
            OnPropertyChanged(nameof(PowerLevel));
            MoistureRecoveryLevel = ventilationUnit.MoistureRecoveryLevel;
            OnPropertyChanged(nameof(MoistureRecoveryLevel));
        }
    }
}
