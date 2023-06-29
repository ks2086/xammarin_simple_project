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
	public partial class TemperaturUnitDetails : ContentPage
	{
		public TemperaturUnitDetails (TemperatureUnitDetailsViewModel viewModel)
		{
			InitializeComponent ();
            this.Title = "Szczegóły czujnika";
            this.BackgroundColor = Color.FromHex("#383b40");


            BindingContext = viewModel;
        }
	}
}