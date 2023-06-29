using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using Projekt.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Projekt.Helpers
{
    public class FirebaseUserHelper
    {

        public static FirebaseClient firebase = new FirebaseClient("https://smarthomeapp-70dc9-default-rtdb.firebaseio.com");

        //  Pobierz wszystkich userów
        public static async Task<List<User>> GetAllUsers()
        {
            try
            {
                var usersSnapshot = await firebase.Child("Users").OnceAsync<User>();
                List<User> usersList = new List<User>();

                foreach (var userSnapshot in usersSnapshot)
                {
                    User user = new User
                    {
                        Email = userSnapshot.Object.Email,
                        Password = userSnapshot.Object.Password
                    };

                    usersList.Add(user);
                }

                return usersList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Wystąpił błąd: " + ex.Message);
                return null;
            }
        }

        //  Pobierz usera
        public static async Task<User> GetUserByEmail(string email)
        {
            try
            {
                string emailReq = Regex.Replace(email, @"[^\w\-\.]", "_at_");
                emailReq = Regex.Replace(emailReq, @"\.", "_dot_");
                var usersSnapshot = await firebase.Child("Users").OnceAsync<User>();

                foreach (var userSnapshot in usersSnapshot)
                {
                    User user = userSnapshot.Object;

                    if (user.Email == emailReq)
                    {
                        return user;
                    }
                }

                return null; // Użytkownik o podanym adresie e-mail nie został znaleziony
            }
            catch (Exception ex)
            {
                Console.WriteLine("Wystąpił błąd: " + ex.Message);
                return null;
            }
        }

        //  Zapisz usera
        public static async Task<bool> SaveUser(string email, string password)
        {
            try
            {
                string emailReq = Regex.Replace(email, @"[^\w\-\.]", "_at_");
                emailReq = Regex.Replace(emailReq, @"\.", "_dot_");
                var user = new User
                {
                    Email = emailReq,
                    Password = password,
                    Uuid = Guid.NewGuid().ToString(),
                };

                var userJson = JsonConvert.SerializeObject(user);
                await firebase.Child("Users").Child(emailReq).PutAsync(userJson);

                return true; // Zapisano użytkownika pomyślnie
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
