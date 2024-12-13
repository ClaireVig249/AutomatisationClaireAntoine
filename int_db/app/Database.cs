using System;
using MySql.Data.MySqlClient; // Nécessaire pour interagir avec MySQL

namespace IntDB
{
    /// <summary>
    /// Classe permettant de :
    /// - Se connecter à la base de données MySQL
    /// - Exécuter des requêtes SQL
    /// - Récupérer les résultats de ces requêtes
    /// - Fermer la connexion à la base de données
    /// </summary>
    public class Database
    {
        private MySqlConnection _connection;

        public string Name { get; set; }
        public string Host { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Port { get; set; } = "3306"; // Port par défaut pour MySQL
        public string Query { get; set; }

        /// <summary>
        /// Initialise la connexion avec les paramètres spécifiés.
        /// </summary>
        public void OpenConnection()
        {
            try
            {
                string connectionString = $"Server={Host};Port={Port};Database={Name};Uid={User};Pwd={Password};";
                _connection = new MySqlConnection(connectionString);
                _connection.Open();
                Console.WriteLine("Connexion à la base de données réussie.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la connexion à la base de données : {ex.Message}");
            }
        }

        /// <summary>
        /// Ferme la connexion à la base de données.
        /// </summary>
        public void CloseConnection()
        {
            if (_connection != null && _connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
                Console.WriteLine("Connexion à la base de données fermée.");
            }
        }

        /// <summary>
        /// Exécute une requête SQL et affiche les résultats (si applicable).
        /// </summary>
        public void ExecuteQuery()
        {
            if (_connection == null || _connection.State != System.Data.ConnectionState.Open)
            {
                Console.WriteLine("Connexion à la base de données non ouverte. Veuillez ouvrir la connexion d'abord.");
                return;
            }

            if (string.IsNullOrEmpty(Query))
            {
                Console.WriteLine("Requête SQL vide. Veuillez fournir une requête valide.");
                return;
            }

            try
            {
                using (MySqlCommand cmd = new MySqlCommand(Query, _connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.Write($"{reader.GetName(i)}: {reader.GetValue(i)}\t");
                            }
                            Console.WriteLine();
                        }
                    }
                }
                Console.WriteLine("Requête exécutée avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'exécution de la requête : {ex.Message}");
            }
        }
    }
}
