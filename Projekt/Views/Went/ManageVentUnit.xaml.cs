using Projekt.ViewModels.Went;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Projekt.Views.Went
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ManageVentUnit : ContentPage
	{
		public ManageVentUnit ()
		{
            InitializeComponent();
            this.Title = "Zarządzaj wentylacją";
            this.BackgroundColor = Color.FromHex("#383b40");

            var vm = new ManageVentUnitViewModel();
            BindingContext = vm;
        }
	}
}