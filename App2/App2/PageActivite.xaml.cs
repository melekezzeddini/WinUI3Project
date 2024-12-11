using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;

namespace App2
{
    public sealed partial class PageActivite : Page
    {
        public Activite SelectedActivite { get; set; }
        public double AverageRating { get; set; }

        public PageActivite()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Activite activite)
            {
                SelectedActivite = activite;
            }
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string numIdentification = "AB-1977-727";  // Ensure this is a valid ID from the adherent table
            int idSeance = 3;  // Ensure this is a valid ID from the seance table (replace 1 with a valid value)
            double noteAppreciation = Math.Round(slider.Value, 2);  // Round the slider value to 2 decimals

            string errorMessage = string.Empty;  

            bool isSuccess = Singleton.Instance.SubmitRating(numIdentification, idSeance, noteAppreciation, out errorMessage);

            if (!isSuccess)
            {
                StatusTextBlock.Text = errorMessage;  // Show error message
                StatusTextBlock.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Red);
            }
            else
            {
                StatusTextBlock.Text = "Note soumise avec succès";
                StatusTextBlock.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Green);
            }
        }







        private void RatingSlider_ValueChanged(object sender, Microsoft.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            if (SliderValueText != null)
                SliderValueText.Text = slider.Value.ToString("F1");
        }
    }



}
