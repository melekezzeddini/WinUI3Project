using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Windows.Storage;
using System;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Windows.Storage.Pickers;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Pickers.Provider;
using Windows.UI.ViewManagement;
using static Microsoft.UI.Win32Interop; 


namespace App2
{
    public sealed partial class ListeActivitees : Page
    {
        public List<Activite> ListeActivites { get; set; }

        public ListeActivitees()
        {
            this.InitializeComponent();
            ListeActivites = Singleton.Instance.GetAllActivites();
            ActivitesListView.ItemsSource = ListeActivites;
        }

        private void ActivitesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ActivitesListView.SelectedItem is Activite selectedActivite)
            {
                this.Frame.Navigate(typeof(PageActivite), selectedActivite);
            }
        }

        private async void ExporterButton_Click(object sender, RoutedEventArgs e)
        {
            // Call the ExportActivitesToCsv method and get the returned result
            var result = await Singleton.Instance.ExportActivitesToCsv(ListeActivites);

            // Update the TextBlock with the message
            err.Text = result.message;

            // Set the color based on the success value
            if (result.success)
            {
                err.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Green); // Success (green)
            }
            else
            {
                err.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Red); // Failure (red)
            }
        }



    }



}

