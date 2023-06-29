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
    public class FirebaseWentHelper
    {
        public static FirebaseClient firebase = new FirebaseClient("https://smarthomeapp-70dc9-default-rtdb.firebaseio.com");

        public static async Task<bool> DeleteVentUnit()
        {
            try
            {
                if (Application.Current.Properties.TryGetValue("UUID", out object value))
                {
                    string UUID = (string)value;
                    await firebase.Child("Went").Child(UUID).DeleteAsync();
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

        public static async Task<bool> UpdateVentUnit(string field, string upValue) {
            try
            {
                if (Application.Current.Properties.TryGetValue("UUID", out object value))
                {
                    string UUID = (string)value;
                    var ventModelJson = JsonConvert.SerializeObject(upValue);
                    await firebase.Child("Went").Child(UUID).Child(field).PutAsync(ventModelJson);
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

        public static async Task<bool> StoreVentUnit(string model)
        {
            try 
            {
                if (Application.Current.Properties.TryGetValue("UUID", out object value))
                {
                    string UUID = (string)value;
                    string Id = Guid.NewGuid().ToString();
                    var ventModel = new Ventilation
                    {
                        Id = Id,
                        Model = model,
                        IsActive = false,
                        PowerLevel = 0,
                        MoistureRecoveryLevel = 0,
                    };

                    var ventModelJson = JsonConvert.SerializeObject(ventModel);
                    await firebase.Child("Went").Child(UUID).PutAsync(ventModelJson);
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

        public static async Task<Ventilation> GetUserVentilationUnit()
        {
           

            try
            {
                if (Application.Current.Properties.TryGetValue("UUID", out object value))
                {
                    string UUID = (string)value;
                    Ventilation ventUnit = await firebase.Child("Went").Child(UUID).OnceSingleAsync<Ventilation>();
                    return ventUnit;
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
