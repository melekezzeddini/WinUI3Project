using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace App2
{
    public sealed partial class MainWindow : Window
    {
        private bool isAuthenticated = false; // Variable pour suivre l'état de l'authentification
        public static MainWindow Instance { get; private set; } // Ajout d'une instance globale


        public MainWindow()
        {
            this.InitializeComponent();
            Instance = this; // Référence à l'instance actuelle
            // Naviguer vers la page de connexion au démarrage
            mainFrame.Navigate(typeof(LoginPage));
            nav.Visibility = Visibility.Collapsed; // Cacher le menu jusqu'à l'authentification
        }



        // Méthode appelée après une connexion réussie
        public void OnLoginSuccess()
        {
            nav.Visibility = Visibility.Visible;
            mainFrame.Navigate(typeof(ListeActivitees)); // Naviguer vers une page après la connexion
        }



        private void nav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (!isAuthenticated)
            {
                mainFrame.Navigate(typeof(LoginPage)); 
            if (args.SelectedItem != null)
            {
                var item = (NavigationViewItem)args.SelectedItem;

                switch (item.Name)
                {
                    case "listeAc":
                        mainFrame.Navigate(typeof(ListeActivitees));
                        break;
                    case "listeAd":
                        mainFrame.Navigate(typeof(ListeAdherents));
                        break;
                    case "Stat":
                        mainFrame.Navigate(typeof(PageStatistique));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
