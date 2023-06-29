using Projekt.ViewModels.Blinds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Projekt.Views.Blinds
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddBlindsUnit : ContentPage
    {
        public AddBlindsUnit()
        {
            InitializeComponent();
            this.Title = "Dodaj nową roletę / żaluzję";
            this.BackgroundColor = Color.FromHex("#383b40");

            var vm = new AddBlindsUnitViewModel();


            BindingContext = vm;
        }
    }
}