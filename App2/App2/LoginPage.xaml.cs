using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MySql.Data.MySqlClient;
using System;

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
                // Navigation vers la page principale
                this.Frame.Navigate(typeof(MainWindow));
            }
            else
            {
                // Affichage d'un message d'erreur
                ErrorTextBlock.Visibility = Visibility.Visible;
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM administrateur WHERE nom_usager = @username AND mot_de_passe = @password";

                using (MySqlConnection connection = Singleton.Instance.connection)
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);

                    connection.Open();
                    int result = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();

                    return result > 0;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
