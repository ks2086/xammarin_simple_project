using Projekt.Helpers;
using Projekt.Models;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Timers;
using Projekt.Views;

namespace Projekt.ViewModels.Heat
{
    public class ManageHeatPumpUnitViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly Timer timer;
        private bool isTimerRunning = false;

        private string _model;
        private decimal _compressorPower;
        private decimal _heaterPower;
        private decimal _co_heatingTemperature;
        private decimal _cwu_heatingTemperature;

        private decimal _currentCoWaterTemperature;
        private decimal _currentCwuWaterTemperature;
        private decimal _currentPowerConsumption;


        public string Model
        {
            get { return _model; }
            set
            {
                _model = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Model"));
            }
        }

        public decimal CompressorPower
        {
            get { return _compressorPower; }
            set
            {
                _compressorPower = value;
                PropertyChanged(this, new PropertyChangedEventArgs("CompressorPower"));
            }
        }

        public decimal HeaterPower
        {
            get { return _heaterPower; }
            set
            {
                _heaterPower = value;
                PropertyChanged(this, new PropertyChangedEventArgs("HeaterPower"));
            }
        }

        public decimal COHeatingTemperature
        {
            get { return _co_heatingTemperature; }
            set
            {
                _co_heatingTemperature = value;
                PropertyChanged(this, new PropertyChangedEventArgs("COHeatingTemperature"));
            }
        }

        public decimal CWUHeatingTemperature
        {
            get { return _cwu_heatingTemperature; }
            set
            {
                _cwu_heatingTemperature = value;
                PropertyChanged(this, new PropertyChangedEventArgs("CWUHeatingTemperature"));
            }
        }

        public decimal CurrentCoWaterTemperature
        {
            get { return _currentCoWaterTemperature; }
            set
            {
                _currentCoWaterTemperature = value;
                PropertyChanged(this, new PropertyChangedEventArgs("CurrentCoWaterTemperature"));
            }
        }

        public decimal CurrentCwuWaterTemperature
        {
            get { return _currentCwuWaterTemperature; }
            set
            {
                _currentCwuWaterTemperature = value;
                PropertyChanged(this, new PropertyChangedEventArgs("CurrentCwuWaterTemperature"));
            }
        }

        public decimal CurrenPowerConsumption
        {
            get { return _currentPowerConsumption; }
            set
            {
                _currentPowerConsumption = value;
                PropertyChanged(this, new PropertyChangedEventArgs("CurrenPowerConsumption"));
            }
        }

        public ICommand IncCoWaterTempCommand { get; set; }
        public ICommand DecCoWaterTempCommand { get; set; }
        public ICommand IncCwuWaterTempCommand { get; set; }
        public ICommand DecCwuWaterTempCommand { get; set; }
        public ICommand RemoveHeatPumpUnitCommand { get; set; }


        public ManageHeatPumpUnitViewModel()
        {
            IncCoWaterTempCommand = new Command(IncCoWaterTempAction);
            DecCoWaterTempCommand = new Command(DecCoWaterTempAction);
            IncCwuWaterTempCommand = new Command(IncCwuWaterTempAction);
            DecCwuWaterTempCommand = new Command(DecCwuWaterTempAction);
            RemoveHeatPumpUnitCommand = new Command(RemoveHeatPumpUnitAction);

            GetMyHeatPump();

            timer = new Timer();
            timer.Elapsed += TimerElapsed;
            timer.Interval = TimeSpan.FromSeconds(5).TotalMilliseconds;

            
        }

        public void StartQuery()
        {
            isTimerRunning = true;
            timer.Start();
        }

        public void StopQuery()
        {
            isTimerRunning = false;
            timer.Stop();
        }

        private async void TimerElapsed(object sender, ElapsedEventArgs e) 
        {
            CurrenPowerConsumption = CompressorPower + HeaterPower;

            if(CurrentCoWaterTemperature < COHeatingTemperature)
            {
                CurrentCoWaterTemperature++;
                IncPower();
            }
            else
            {
                CurrentCoWaterTemperature--;
                DecPower();
            }

            if (CurrentCwuWaterTemperature < CWUHeatingTemperature)
            {
                CurrentCwuWaterTemperature++;
                IncPower();
            }
            else
            {
                CurrentCwuWaterTemperature--;
                DecPower();
            }


            if((CurrentCoWaterTemperature == COHeatingTemperature) && (CurrentCwuWaterTemperature == CWUHeatingTemperature))
            {
                StopQuery();
            }

        }

        private void IncPower()
        {
            if (CompressorPower < 5)
            {
                CompressorPower += (decimal)0.1;
            }
            else if (CompressorPower == 5 && HeaterPower < 5)
            {
                HeaterPower += (decimal)0.1;
            }
        }

        private void DecPower()
        {
            if (CompressorPower == 5 && (HeaterPower <= 5 && HeaterPower > 0))
            {
                HeaterPower -= (decimal)0.1;
            }
            else if (CompressorPower <= 5 && HeaterPower == 0)
            {
                CompressorPower -= (decimal)0.1;
            }
        }

        private async void GetMyHeatPump()
        {
            HeatPump hp = await FirebaseHeatHelper.GetUserHeatUnit();
            if (hp != null)
            {
                Model = hp.Model;
                COHeatingTemperature = hp.CO_HeatingTemperature;
                CWUHeatingTemperature = hp.CWU_HeatingTemperature;
                CompressorPower = 0;
                HeaterPower = 0;
                CurrentCoWaterTemperature = hp.CO_HeatingTemperature;
                CurrentCwuWaterTemperature = hp.CWU_HeatingTemperature;
                CurrenPowerConsumption = 0;

                setPower(hp);
            }
        }

        private void setPower(HeatPump hp)
        {
            decimal sumOfTemperature = (decimal)hp.CO_HeatingTemperature + (decimal)hp.CWU_HeatingTemperature;
            if(sumOfTemperature > 50)
            {
                CompressorPower = 5;
                HeaterPower = (decimal)(sumOfTemperature - 50) * (decimal)0.1;
            }
            else if(sumOfTemperature >= 0)
            {
                CompressorPower = (decimal) sumOfTemperature * (decimal)0.1;
                HeaterPower = 0;
            }
            

            CurrenPowerConsumption = sumOfTemperature * (decimal)0.1;
        }

        private async void IncCoWaterTempAction()
        {
            if(COHeatingTemperature < 50)
            {

                COHeatingTemperature++;
                UpdateValues("CO_HeatingTemperature", COHeatingTemperature.ToString());
            }
        }

        private async void DecCoWaterTempAction()
        {
            if (COHeatingTemperature > 0)
            {
                COHeatingTemperature--;
                UpdateValues("CO_HeatingTemperature", COHeatingTemperature.ToString());
            }
        }

        private async void IncCwuWaterTempAction()
        {
            if (CWUHeatingTemperature < 50)
            {
                CWUHeatingTemperature++;
                UpdateValues("CWU_HeatingTemperature", CWUHeatingTemperature.ToString());
            }
        }

        private async void DecCwuWaterTempAction()
        {
            if (CWUHeatingTemperature > 0)
            {
                CWUHeatingTemperature--;
                UpdateValues("CWU_HeatingTemperature", CWUHeatingTemperature.ToString());
            }
        }

        private async void UpdateValues(string field, string value)
        {
            if (!isTimerRunning)
            {
                StartQuery();
            }

            await FirebaseHeatHelper.UpdateHeatPumpUnit(field, value);
        }

        private async void RemoveHeatPumpUnitAction()
        {
            if (await FirebaseHeatHelper.DeleteHeatPumpUnit())
            {
                StopQuery();
                await App.Current.MainPage.Navigation.PushAsync(new Panel());
            }
        }
    }
}
