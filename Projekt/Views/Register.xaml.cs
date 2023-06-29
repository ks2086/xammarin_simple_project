using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Projekt.ViewModels;

namespace Projekt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Register : ContentPage
    {
        public Register()
        {
            InitializeComponent();
            this.Title = "Wróć do logowania";
            this.BackgroundColor = Color.FromHex("#383b40");
            var vm = new RegisterViewModel();
            BindingContext = vm;
        }
    }
}