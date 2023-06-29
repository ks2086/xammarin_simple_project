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
	public partial class CreateNewVentUnit : ContentPage
	{
		public CreateNewVentUnit ()
		{
			InitializeComponent ();
            this.Title = "Dodaj centralę wentylacyjną";
            this.BackgroundColor = Color.FromHex("#383b40");

            var vm = new CreateNewVentViewModel();
            BindingContext = vm;
        }
	}
}