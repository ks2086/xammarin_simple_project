using Projekt.ViewModels.Heat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Projekt.Views.Heat
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreateNewHeatPumpUnit : ContentPage
	{
		public CreateNewHeatPumpUnit ()
		{
            InitializeComponent();
            this.Title = "Dodaj pompę ciepła";
            this.BackgroundColor = Color.FromHex("#383b40");

            var vm = new CreateNewHeatPumpUnitViewModel();
            BindingContext = vm;
        }
	}
}