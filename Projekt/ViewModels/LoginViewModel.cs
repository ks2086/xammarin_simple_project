using Projekt.Helpers;
using Projekt.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Projekt.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
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

        public ICommand LoginFormSubmitCommand { get; protected set; }

        public ICommand RegisterFormNavigateCommand { get; protected set; }

        public LoginViewModel()
        {
            LoginFormSubmitCommand = new Command(OnSubmit);
            RegisterFormNavigateCommand = new Command(NavigateToRegister);
        }

        public async void NavigateToRegister()
        {
            await App.Current.MainPage.Navigation.PushAsync(new Register());
        }

        public void OnSubmit()
        {
            Login();
        }

        private async void Login() 
        {
            if(string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert("Błąd danych", "Formularz zawiera niepoprawne wartości", "OK");
            }
            else
            {
                var user = await FirebaseUserHelper.GetUserByEmail(Email);
                if(user != null )
                {
                    if(!PasswordHasher.VerifyPassword(Password, user.Password))
                    {
                        await App.Current.MainPage.DisplayAlert("Błąd danych", "Podane hasło jest niepoprawne", "OK");
                    }
                    else
                    {
                        Application.Current.Properties["UUID"] = user.Uuid;
                        await Application.Current.SavePropertiesAsync();

                        await App.Current.MainPage.Navigation.PushAsync(new Panel());
                    }
                    

                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Błąd danych", "Konto o takich danych nie istnieje", "OK");
                }
            }
        }


    }
}
