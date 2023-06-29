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
    public partial class BlindDetails : ContentPage
    {
        public BlindDetails (BlindDetailsViewModel viewModel)
		{
			InitializeComponent();
            this.Title = "Szczegóły rolety";
            this.BackgroundColor = Color.FromHex("#383b40");
            BindingContext = viewModel;
        }
}
}