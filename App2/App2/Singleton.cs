using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace App2
{
    class Singleton
    {
        private static Singleton instance;
        private MySqlConnection connection;

        // Private constructor to prevent instantiation from outside
        private Singleton()
        {
            // Set up the MySQL database connection
            connection = new MySqlConnection("Server=cours.cegep3r.info;Database=a2024_420335ri_eq17;Uid=6221863;Pwd=6221863;");
        }

        // Public property to access the singleton instance
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

        // Example method to retrieve all activities from the database
        public List<Activite> GetAllActivites()
        {
            List<Activite> activites = new List<Activite>();

            // Updated query with a JOIN to fetch the type name from the type table
            string query = "SELECT a.id_activite, a.nom, a.cout_organisation, a.prix_participation, t.nom AS type_name " +
                           "FROM activite a " +
                           "JOIN type t ON a.id_type = t.id_type";

            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Activite activite = new Activite
                {
                    IdActivite = reader.GetInt32("id_activite"),
                    Nom = reader.GetString("nom"),
                    CoutOrganisation = reader.GetDouble("cout_organisation"),
                    PrixParticipation = reader.GetDouble("prix_participation"),
                    TypeName = reader.GetString("type_name") // Assign the type name
                };
                activites.Add(activite);
            }

            connection.Close();
            return activites;
        }


        // Example method to add a new activity
        public void AddActivite(Activite activite)
        {
            string query = "INSERT INTO activite (nom, coût_organisation, prix_participation, id_type) VALUES (@nom, @coût_organisation, @prix_participation, @id_type)";
            MySqlCommand command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@nom", activite.Nom);
            command.Parameters.AddWithValue("@coût_organisation", activite.CoutOrganisation);
            command.Parameters.AddWithValue("@prix_participation", activite.PrixParticipation);
            command.Parameters.AddWithValue("@id_type", activite.IdType);  // Assuming IdType exists

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

    }
}
