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
	public partial class ManageHeatPumpUnit : ContentPage
	{
		public ManageHeatPumpUnit ()
		{
            InitializeComponent();
            this.Title = "Zarządzaj pompą ciepła";
            this.BackgroundColor = Color.FromHex("#383b40");

            var vm = new ManageHeatPumpUnitViewModel();
            BindingContext = vm;
        }
	}
}