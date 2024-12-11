using Microsoft.UI.Xaml.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.Storage;
using Microsoft.UI.Xaml;
using System.Linq;
using WinRT.Interop;
using Windows.UI;
using Windows.UI.Popups;
using System.IO;
using System.Reflection;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;


namespace App2
{
    public class Singleton
    {
        private static Singleton instance;
        private MySqlConnection connection;

        private Singleton()
        {
            connection = new MySqlConnection("Server=cours.cegep3r.info;Database=a2024_420335ri_eq17;Uid=2121088;Pwd=2121088;");
        }
        

        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }
                return instance;
            }
        }

        // Retained GetAllActivities method
        public List<Activite> GetAllActivites()
        {
            List<Activite> activites = new List<Activite>();

            // SQL query to get all activities with their average ratings
            string query = "SELECT a.id_activite, a.nom, a.cout_organisation, a.prix_participation, t.nom AS type_name, " +
                           "AVG(p.note_appreciation) AS avg_rating " +
                           "FROM activite a " +
                           "JOIN type t ON a.id_type = t.id_type " +
                           "LEFT JOIN seance s ON a.id_activite = s.id_activite " +
                           "LEFT JOIN participer p ON s.id_seance = p.id_seance " +
                           "GROUP BY a.id_activite";

            MySqlCommand command = new MySqlCommand(query, connection);

            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Activite activite = new Activite
                {
                    IdActivite = reader.GetInt32("id_activite"),
                    Nom = reader.GetString("nom"),
                    CoutOrganisation = reader.GetDouble("cout_organisation"),
                    PrixParticipation = reader.GetDouble("prix_participation"),
                    TypeName = reader.GetString("type_name"),
                    AvgRating = Math.Round(reader.IsDBNull(reader.GetOrdinal("avg_rating")) ? 0 : reader.GetDouble("avg_rating"),2) // Default to 0 if null
                }; 
                activites.Add(activite);
            }

            connection.Close();
            return activites;
        }

        public List<Adherent> GetAllAdherents()
        {
            List<Adherent> adherents = new List<Adherent>();

            // SQL query to get all adherents
            string query = "SELECT * FROM adherent";

            MySqlCommand command = new MySqlCommand(query, connection);

            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Adherent adherent = new Adherent
                {
                    Num_identification = reader.GetString("num_identification"),
                    Nom = reader.GetString("nom"),
                    Prenom = reader.GetString("prenom"),
                    Adresse = reader.GetString("adresse"),
                    Date_naissance = reader.GetDateTime("date_naissance").ToString(),
                    Age = reader.GetInt32("age")
                };
                adherents.Add(adherent);
            }

            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }

            return adherents;
        }


        public async Task<(bool success, string message)> ExportActivitesToCsv(List<Activite> activites)
        {
            StringBuilder csvContent = new StringBuilder();

            // Add each activity to the CSV content
            foreach (var activite in activites)
            {
                csvContent.AppendLine($"{activite.IdActivite};{activite.Nom};{activite.CoutOrganisation};{activite.PrixParticipation};{activite.IdType};{activite.TypeName};{activite.AvgRating}");
            }

            // Define the manual path (e.g., in the project's root directory)
            string projectDirectory = @"C:\Users\avada\source\repos\WinUI3Project2\App2\App2\Assets";
            string exportFolder = Path.Combine(projectDirectory, "ExportedFiles");

            // Ensure the folder exists
            if (!Directory.Exists(exportFolder))
            {
                Directory.CreateDirectory(exportFolder);
            }

            // Define the full file path (activites.csv will be saved in the "ExportedFiles" folder)
            string filePath = Path.Combine(exportFolder, "activites.csv");

            try
            {
                // Write CSV content to the file asynchronously
                await File.WriteAllTextAsync(filePath, csvContent.ToString());
                return (true, "Fichier Exporté !");
            }
            catch (Exception ex)
            {
                return (false, "Erreur !!");
            }
        }

        public async Task<(bool success, string message)> ExportAdherentToCsv(List<Adherent> adherents)
        {
            StringBuilder csvContent = new StringBuilder();

            // Add each activity to the CSV content
            foreach (var adherent in adherents)
            {
                csvContent.AppendLine($"{adherent.Num_identification};{adherent.Nom},{adherent.Prenom};{adherent.Adresse};{adherent.Date_naissance};{adherent.Age}");
            }

            // Define the manual path (e.g., in the project's root directory)
            string projectDirectory = @"C:\Users\avada\source\repos\WinUI3Project2\App2\App2\Assets";
            string exportFolder = Path.Combine(projectDirectory, "ExportedFiles");

            // Ensure the folder exists
            if (!Directory.Exists(exportFolder))
            {
                Directory.CreateDirectory(exportFolder);
            }

            // Define the full file path (activites.csv will be saved in the "ExportedFiles" folder)
            string filePath = Path.Combine(exportFolder, "adherent.csv");

            try
            {
                // Write CSV content to the file asynchronously
                await File.WriteAllTextAsync(filePath, csvContent.ToString());
                return (true, "Fichier Exporté !");
            }
            catch (Exception ex)
            {
                return (false, "Erreur !!");
            }
        }








        // Consolidated statistics method
        public (int TotalMembers, int TotalActivities, List<string> MembersPerActivity, List<string> AverageRatings,
                int TotalSessions, int TotalParticipants, List<string> AverageSessionsPerActivity) GetAllStatistics()
        {
            int totalMembers = Convert.ToInt32(ExecuteScalar("SELECT COUNT(*) FROM adherent"));
            int totalActivities = Convert.ToInt32(ExecuteScalar("SELECT COUNT(*) FROM activite"));

            List<string> membersPerActivity = new List<string>();
            string membersPerActivityQuery = "SELECT a.nom, COUNT(p.id_adherent) AS member_count " +
                                             "FROM activite a " +
                                             "JOIN participer p ON a.id_activite = p.id_seance " +
                                             "GROUP BY a.nom";
            using (var reader = ExecuteReader(membersPerActivityQuery))
            {
                while (reader.Read())
                {
                    string activity = reader.GetString("nom");
                    int count = reader.GetInt32("member_count");
                    membersPerActivity.Add($"{activity} contient {count} adhérents");
                }
            }

            List<string> averageRatings = new List<string>();
            string averageRatingsQuery = "SELECT a.nom, AVG(CAST(p.note_appreciation AS DECIMAL(10,2))) AS avg_rating " +
                                         "FROM activite a " +
                                         "JOIN participer p ON a.id_activite = p.id_seance " +
                                         "GROUP BY a.nom";
            using (var reader = ExecuteReader(averageRatingsQuery))
            {
                while (reader.Read())
                {
                    string activity = reader.GetString("nom");
                    double avgRating = reader.GetDouble("avg_rating");
                    averageRatings.Add($"La note moyenne de {activity} est : {avgRating:F2}");
                }
            }

            int totalSessions = Convert.ToInt32(ExecuteScalar("SELECT COUNT(*) FROM seance"));
            int totalParticipants = Convert.ToInt32(ExecuteScalar("SELECT COUNT(*) FROM participer"));

            List<string> averageSessionsPerActivity = new List<string>();
            string avgSessionsQuery = "SELECT a.nom AS activite_nom, " +
                                       "COUNT(s.id_seance) AS total_seances " +
                                       "FROM activite a " +
                                       "LEFT JOIN seance s ON a.id_activite = s.id_activite " +
                                       "GROUP BY a.id_activite;";

            using (var reader = Singleton.Instance.ExecuteReader(avgSessionsQuery))
            {
                while (reader.Read())
                {
                    string activityName = reader.GetString("activite_nom");
                    int sessionCount = reader.GetInt32("total_seances");  // Renamed variable to avoid conflict
                    averageSessionsPerActivity.Add($"{activityName} à {sessionCount} seances");
                }
            }


            return (totalMembers, totalActivities, membersPerActivity, averageRatings, totalSessions, totalParticipants, averageSessionsPerActivity);
        }
        public bool SubmitRating(string numIdentification, int idSeance, double noteAppreciation, out string errorMessage)
        {
            try
            {
                // Check if the user has already rated this activity
                string checkQuery = "SELECT COUNT(*) FROM participer WHERE id_adherent = @id_adherent AND id_seance = @id_seance";
                MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@id_adherent", numIdentification);
                checkCommand.Parameters.AddWithValue("@id_seance", idSeance);

                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }

                int count = Convert.ToInt32(checkCommand.ExecuteScalar());
                if (count > 0)
                {
                    // If the user already rated, set error message and return false
                    errorMessage = "Vous avez déjà noté cette activité";
                    return false;
                }

                // Proceed to insert new rating
                string query = "INSERT INTO participer (id_adherent, id_seance, note_appreciation) " +
                               "VALUES (@id_adherent, @id_seance, @note_appreciation)";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id_adherent", numIdentification);
                command.Parameters.AddWithValue("@id_seance", idSeance);
                command.Parameters.AddWithValue("@note_appreciation", noteAppreciation);

                int result = command.ExecuteNonQuery();
                if (result > 0)
                {
                    errorMessage = string.Empty;  // No error
                    return true;  // Successful insertion
                }
                else
                {
                    errorMessage = "Erreur lors de la soumission de la note.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Erreur lors de la soumission de la note : {ex.Message}";
                return false;
            }
            finally
            {
                connection.Close();
            }
        }





        // Helper methods: ExecuteScalar and ExecuteReader (unchanged)
        public object ExecuteScalar(string query)
        {
            MySqlCommand command = new MySqlCommand(query, connection);

            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }

            object result = command.ExecuteScalar();
            connection.Close();
            return result;
        }

        public MySqlDataReader ExecuteReader(string query)
        {
            MySqlCommand command = new MySqlCommand(query, connection);

            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }

            return command.ExecuteReader();
        }
    }
}
