using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projekt.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Projekt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Panel : ContentPage
    {
        public Panel()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            this.BackgroundColor = Color.FromHex("#383b40");

            var vm = new PanelViewModel();
            BindingContext = vm;
        }
    }
}