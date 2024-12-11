
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace App2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ListeAdherents : Page
    {
        public List<Adherent> ListeAdherent { get; set; }

        public ListeAdherents()
        {
            this.InitializeComponent();
            ListeAdherent = Singleton.Instance.GetAllAdherents();
            ActivitesListView.ItemsSource = ListeAdherent;
        }
        private async void ExporterButton_Click(object sender, RoutedEventArgs e)
        {
            // Call the ExportActivitesToCsv method and get the returned result
            var result = await Singleton.Instance.ExportAdherentToCsv(ListeAdherent);

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
