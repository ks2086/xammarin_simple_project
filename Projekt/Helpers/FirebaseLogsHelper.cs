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
    public class FirebaseLogsHelper
    {
        public static FirebaseClient firebase = new FirebaseClient("https://smarthomeapp-70dc9-default-rtdb.firebaseio.com");

        public static async Task<List<Log>> GetAllLogs()
        {
            try
            {
                if (Application.Current.Properties.TryGetValue("UUID", out object value))
                {
                    Dictionary<string, string> logStatusList = (await firebase.Child("LogStatus").OnceAsync<LogStatus>()).ToDictionary(f => f.Key, f => f.Object.Description);

                    Console.WriteLine(logStatusList["units_sync"]);
                    string UUID = (string)value;
                    List<Log> items = (await firebase.Child("Logs")
                         .Child(UUID)
                         .OnceAsync<Log>()).Select(f => new Log
                         {
                             Id = f.Object.Id,
                             LogStatusId = logStatusList[f.Object.LogStatusId] != null ? logStatusList[f.Object.LogStatusId] : "Brak danych",
                             Module = f.Object.Module,
                             ModuleUnitId = f.Object.ModuleUnitId,
                             Created_at = f.Object.Created_at,

                         }).OrderByDescending(log => log.Created_at).Take(100).ToList();

                    return items;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Wystąpił błąd: " + ex.Message);

            }
            return new List<Log>();
        }

        public static async Task<bool> CreateLog(string logStatus, string module, string moduleUnitId)
        {
            try
            {
                if (Application.Current.Properties.TryGetValue("UUID", out object value))
                {
                    string UUID = (string)value;
                    string Id = Guid.NewGuid().ToString();
                    var logsModel = new Log
                    {
                        Id = Id,
                        LogStatusId = logStatus,
                        Module = module,
                        ModuleUnitId = moduleUnitId,
                        Created_at = DateTime.Now
                    };

                    var logsModelJson = JsonConvert.SerializeObject(logsModel);
                    await firebase.Child("Logs").Child(UUID).Child(Id).PutAsync(logsModelJson);
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

        public static async Task<bool> MakeLogsStatusTable()
        {
            try
            {

                var logModel = new LogStatus { Description = "Urządzenie zostało uruchomione." };
                var modelJson = JsonConvert.SerializeObject(logModel);
                await firebase.Child("LogStatus").Child("device_turn_on").PutAsync(modelJson);

                logModel = new LogStatus { Description = "Urządzenie zostało wyłączone." };
                modelJson = JsonConvert.SerializeObject(logModel);
                await firebase.Child("LogStatus").Child("device_turn_off").PutAsync(modelJson);

                logModel = new LogStatus { Description = "Synchronizacja czujników." };
                modelJson = JsonConvert.SerializeObject(logModel);
                await firebase.Child("LogStatus").Child("units_sync").PutAsync(modelJson);

                logModel = new LogStatus { Description = "Zwiększenie temperatury czynnika grzewczego" };
                modelJson = JsonConvert.SerializeObject(logModel);
                await firebase.Child("LogStatus").Child("inc_temp").PutAsync(modelJson);

                logModel = new LogStatus { Description = "Zminiejszenie temperatury czynnika grzewczego" };
                modelJson = JsonConvert.SerializeObject(logModel);
                await firebase.Child("LogStatus").Child("dec_temp").PutAsync(modelJson);

                return true;
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
