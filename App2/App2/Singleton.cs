using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace App2
{
    public class Singleton
    {
        private static Singleton instance;
        private MySqlConnection connection;

        private Singleton()
        {
            connection = new MySqlConnection("Server=cours.cegep3r.info;Database=a2024_420335ri_eq17;Uid=6221863;Pwd=6221863;");
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

            string query = "SELECT a.id_activite, a.nom, a.cout_organisation, a.prix_participation, t.nom AS type_name " +
                           "FROM activite a " +
                           "JOIN type t ON a.id_type = t.id_type";

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
                    TypeName = reader.GetString("type_name")
                };
                activites.Add(activite);
            }

            connection.Close();
            return activites;
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
