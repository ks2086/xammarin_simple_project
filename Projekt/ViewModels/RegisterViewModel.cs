using Projekt.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Projekt.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _email;
        private string _password;

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Email"));
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Password"));
            }
        }

        public ICommand RegisterFormSubmitCommand { get; protected set; }

        public RegisterViewModel()
        {
            RegisterFormSubmitCommand = new Command(OnSubmit);
        }

        public void OnSubmit()
        {
            Register();
        }

        private async void Register()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert("Błąd danych", "Formularz zawiera niepoprawne wartości", "OK");
            }
            else
            {
                var user = await FirebaseUserHelper.GetUserByEmail(Email);
                if (user != null)
                {
                    await App.Current.MainPage.DisplayAlert("Błąd danych", "Konto o takim adresie e-mail już istnieje", "OK");
                }
                else
                {
                    if(await FirebaseUserHelper.SaveUser(Email, PasswordHasher.HashPassword(Password)))
                    {
                        await App.Current.MainPage.DisplayAlert("Nowe konto utworzone", "Uzytkownik został zapisany, możesz już się zalogować.", "OK");
                    }
                }
            }
        }
    }
}
