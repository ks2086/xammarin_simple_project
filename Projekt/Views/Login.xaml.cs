using Projekt.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Projekt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
          
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
            this.BackgroundColor = Color.FromHex("#383b40");

            var vm = new LoginViewModel();
            BindingContext = vm;

            Email.Completed += (object sender, EventArgs e) =>
            {
                Password.Focus();
            };

            Password.Completed += (object sender, EventArgs e) =>
            {
                vm.LoginFormSubmitCommand.Execute(null);
            };
        }
    }
}