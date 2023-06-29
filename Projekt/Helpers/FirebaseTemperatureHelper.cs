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
    public class FirebaseTemperatureHelper
    {
        public static FirebaseClient firebase = new FirebaseClient("https://smarthomeapp-70dc9-default-rtdb.firebaseio.com");

        public static async Task<bool> DeleteTemperatureUnit(string id)
        {
            try
            {
                if (Application.Current.Properties.TryGetValue("UUID", out object value))
                {
                    string UUID = (string)value;
                    await firebase.Child("TemperatureUnits").Child(UUID).Child(id).DeleteAsync();
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

        public static async Task<Temperature> GetUnitById(string id)
        {
            try
            {
                if (Application.Current.Properties.TryGetValue("UUID", out object value))
                {
                    string UUID = (string)value;
                    Temperature unit = await firebase.Child("TemperatureUnits").Child(UUID).Child(id).OnceSingleAsync<Temperature>();
                    return unit;
                }
                    
            }
            catch (Exception ex)
            {
                Console.WriteLine("Wystąpił błąd: " + ex.Message);
            }
            return null;
        }

        public static async Task<bool> SyncTemperatureUnit(decimal temp, decimal humidity, string id)
        {
            try
            {
                if (Application.Current.Properties.TryGetValue("UUID", out object value)) 
                {
                    string UUID = (string)value;
                    await firebase.Child("TemperatureUnits").Child(UUID).Child(id).Child("TemperatureDeg").PutAsync(JsonConvert.SerializeObject(temp));
                    await firebase.Child("TemperatureUnits").Child(UUID).Child(id).Child("Humidity").PutAsync(JsonConvert.SerializeObject(humidity));
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

        public static async Task<bool> UpdateTemperatureUnit(string description, string id) 
        {
            try
            {
                if (Application.Current.Properties.TryGetValue("UUID", out object value))
                {
                    string UUID = (string)value;
                    await firebase.Child("TemperatureUnits").Child(UUID).Child(id).Child("Description").PutAsync(JsonConvert.SerializeObject(description));
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

        public static async Task<List<Temperature>> GetAllTemperatureUnits()
        {
            try
            {
                if (Application.Current.Properties.TryGetValue("UUID", out object value))
                {
                    string UUID = (string)value;
                    List<Temperature> items = (await firebase.Child("TemperatureUnits")
                         .Child(UUID)
                         .OnceAsync<Temperature>()).Select(f => new Temperature
                         {
                             Id = f.Object.Id,
                             Description = f.Object.Description,
                             Uid = f.Object.Uid,
                             IsConnected = f.Object.IsConnected,
                             Name = f.Object.Name,
                             TemperatureDeg = f.Object.TemperatureDeg,
                             Humidity = f.Object.Humidity,

                         }).ToList();

                    return items;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Wystąpił błąd: " + ex.Message);
                
            }
            return new List<Temperature>();
        }

        public static async Task<bool> SaveTemperatureUnit(string name, string description)
        {
            try
            {
                if (Application.Current.Properties.TryGetValue("UUID", out object value))
                { 
                    string UUID = (string) value;
                    string Id = Guid.NewGuid().ToString();
                    var temperatureModel = new Temperature
                    {
                        Id = Id,
                        Name = name,
                        Description = description,
                        TemperatureDeg = 0,
                        Humidity = 0,
                        IsConnected = false,
                    };

                    var temperatureUnitJson = JsonConvert.SerializeObject(temperatureModel);
                    await firebase.Child("TemperatureUnits").Child(UUID).Child(Id).PutAsync(temperatureUnitJson);

                    return true;
                }
                return false;

               
            }
            catch (Exception ex)
            {
                Console.WriteLine("Wystąpił błąd: " + ex.Message);
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "ok");
                return false;
            }
        }

    }
}
