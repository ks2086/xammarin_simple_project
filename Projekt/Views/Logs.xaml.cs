using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Projekt.Services;
using Projekt.ViewModels;
using Projekt.Helpers;

namespace Projekt.Views.Went
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Logs : ContentPage
    {
        public Logs()
        {
            InitializeComponent();

            this.Title = "Logi systemowe";
            this.BackgroundColor = Color.FromHex("#383b40");

            var vm = new LogsViewModel();
            BindingContext = vm;
            //LogsHandler.CreateLogTableMigration();

            _ = GetAllLogsAsync();
        }

        public async Task GetAllLogsAsync()
        {
            var logs = await FirebaseLogsHelper.GetAllLogs();
            if (logs != null)
            {
                LogsList.ItemsSource = logs;
            }
            else
            {
                LogsList.ItemsSource = null;
            }
        }
    }
}