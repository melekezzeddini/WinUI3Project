using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace App2
{
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }




        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            if (AuthenticateUser(username, password))
            {
                // Appeler OnLoginSuccess dans MainWindow après une connexion réussie
                MainWindow.Instance.OnLoginSuccess(); // Utilisation de la référence statique
            }
            else
            {
                // Affichage d'un message d'erreur
                ErrorTextBlock.Visibility = Visibility.Visible;
            }
        }



        private bool AuthenticateUser(string username, string password)
        {
            // Logique pour authentifier l'utilisateur (par exemple, vérifier la base de données)
            return Singleton.Instance.Authenticate(username, password);
        }
    }
}
