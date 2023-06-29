using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using Projekt.Models;
using Xamarin.Forms;

namespace Projekt.Helpers
{
    public class FirebaseHeatHelper
    {
        public static FirebaseClient firebase = new FirebaseClient("https://smarthomeapp-70dc9-default-rtdb.firebaseio.com");

        public static async Task<bool> DeleteHeatPumpUnit()
        {
            try
            {
                if (Application.Current.Properties.TryGetValue("UUID", out object value))
                {
                    string UUID = (string)value;
                    await firebase.Child("HeatPumps").Child(UUID).DeleteAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Wystąpił błąd: " + ex.Message);
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "ok");

            }
            return false;
        }

        public static async Task<bool> UpdateHeatPumpUnit(string field, string upValue)
        {
            try
            {
                if (Application.Current.Properties.TryGetValue("UUID", out object value))
                {
                    string UUID = (string)value;
                    var ventModelJson = JsonConvert.SerializeObject(upValue);
                    await firebase.Child("HeatPumps").Child(UUID).Child(field).PutAsync(ventModelJson);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Wystąpił błąd: " + ex.Message);
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "ok");

            }
            return false;
        }

        public static async Task<bool> StoreHeatPumpUnit(string model)
        {
            try
            {
                if (Application.Current.Properties.TryGetValue("UUID", out object value))
                {
                    string UUID = (string)value;
                    string Id = Guid.NewGuid().ToString();
                    var heatPumpModel = new HeatPump
                    {
                        Id = Id,
                        Model = model,
                        CompressorPower = 0,
                        HeaterPower = 0,
                        CO_HeatingTemperature = 0,
                        CWU_HeatingTemperature = 0,
                    };

                    var heatPumpModelJson = JsonConvert.SerializeObject(heatPumpModel);
                    await firebase.Child("HeatPumps").Child(UUID).PutAsync(heatPumpModelJson);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Wystąpił błąd: " + ex.Message);
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "ok");
            }
            return false;
        }

        public static async Task<HeatPump> GetUserHeatUnit()
        {
            try
            {
                if (Application.Current.Properties.TryGetValue("UUID", out object value))
                {
                    string UUID = (string)value;
                    HeatPump heatPump = await firebase.Child("HeatPumps").Child(UUID).OnceSingleAsync<HeatPump>();
                    return heatPump;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Wystąpił błąd: " + ex.Message);
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "ok");
            }
            return null;
        }
    }
}
