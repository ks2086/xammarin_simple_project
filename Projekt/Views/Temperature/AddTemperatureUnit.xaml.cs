using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projekt.ViewModels.TemperatureVM;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Projekt.Views.Temperature
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddTemperatureUnit : ContentPage
	{
		public AddTemperatureUnit ()
		{
			InitializeComponent ();
            this.Title = "Dodaj nową pozycję";
            this.BackgroundColor = Color.FromHex("#383b40");

            var vm = new TempratureAddNewUnitViewModel();
            BindingContext = vm;
        }
	}
}