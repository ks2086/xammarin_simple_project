using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
    public class FirebaseBlindsHelper
    {
        public static FirebaseClient firebase = new FirebaseClient("https://smarthomeapp-70dc9-default-rtdb.firebaseio.com");

        public static async Task<bool> DeleteBlindUnit(string id)
        {
            try
            {
                if (Application.Current.Properties.TryGetValue("UUID", out object value))
                {
                    string UUID = (string)value;
                    await firebase.Child("Blinds").Child(UUID).Child(id).DeleteAsync();
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

        public static async Task<bool> UpdateBlindsUnit(string key, string upValue, string id)
        {
            try
            {
                if (Application.Current.Properties.TryGetValue("UUID", out object value))
                {
                    string UUID = (string)value;
                    await firebase.Child("Blinds").Child(UUID).Child(id).Child(key).PutAsync(JsonConvert.SerializeObject(upValue));
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

        public static async Task<List<Blind>> GetAllBlindUnits()
        {
            try
            {
                if (Application.Current.Properties.TryGetValue("UUID", out object value))
                {
                    string UUID = (string)value;
                    List<Blind> items = (await firebase.Child("Blinds")
                         .Child(UUID)
                         .OnceAsync<Blind>()).Select(f => new Blind
                         {
                             Id = f.Object.Id,
                             Description = f.Object.Description,
                             IsClosed = f.Object.IsClosed,
                             ActionLevel = f.Object.ActionLevel,

                         }).ToList();

                    return items;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Wystąpił błąd: " + ex.Message);

            }
            return new List<Blind>();
        }

        public static async Task<bool> StoreBlindsUnit(string description, bool isClosed)
        {
            try
            {
                if (Application.Current.Properties.TryGetValue("UUID", out object value))
                {
                    string UUID = (string)value;
                    string Id = Guid.NewGuid().ToString();
                    var blindsModel = new Blind
                    {
                        Id = Id,
                        Description = description,
                        IsClosed = isClosed,
                        ActionLevel = 100

                    };

                    var blindsModelJson = JsonConvert.SerializeObject(blindsModel);
                    await firebase.Child("Blinds").Child(UUID).Child(Id).PutAsync(blindsModelJson);
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
    }
}
