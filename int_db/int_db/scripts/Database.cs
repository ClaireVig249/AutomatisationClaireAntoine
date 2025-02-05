using MySqlConnector;
using System.Collections.Generic;

namespace int_db.scripts
{
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
                // Initialisation de la connexion MySQL
                string connectionString = $"Server={Host};Port={Port};Database={Name};Uid={User};Pwd={Password};";
                _connection = new MySqlConnection(connectionString);
                _connection.Open();
                Console.WriteLine($"Connexion à la base de données réussie avec {Host}:{Port}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la connexion à la base de données : {ex.Message}");
            }
        }
        
        /// <summary>
        /// Vérifie si la connexion à la base de données est ouverte.
        /// </summary>
        /// <returns></returns>
        public bool IsOpen()
        {
            return _connection.State == System.Data.ConnectionState.Open;
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
        /// Exécute une requête SQL et retourne les résultats sous forme de liste de dictionnaires.
        /// </summary>
        public List<Dictionary<string, object>> ExecuteQueryWithResults()
        {
            var results = new List<Dictionary<string, object>>();

            if (_connection == null || _connection.State != System.Data.ConnectionState.Open)
            {
                Console.WriteLine("Connexion à la base de données non ouverte. Veuillez ouvrir la connexion d'abord.");
                return results;
            }

            if (string.IsNullOrEmpty(Query))
            {
                Console.WriteLine("Requête SQL vide. Veuillez fournir une requête valide.");
                return results;
            }

            try
            {
                using (MySqlCommand cmd = new MySqlCommand(Query, _connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[reader.GetName(i)] = reader.GetValue(i);
                            }
                            results.Add(row);
                        }
                    }
                }
                Console.WriteLine("Requête exécutée avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'exécution de la requête : {ex.Message}");
            }

            return results;
        }

        /// <summary>
        /// Exécute une requête SQL sans retourner de résultats.
        /// </summary>
        public bool ExecuteQuery()
        {
            if (_connection == null || _connection.State != System.Data.ConnectionState.Open)
            {
                throw new Exception("Connexion à la base de données non ouverte. Veuillez ouvrir la connexion d'abord.");
            }

            if (string.IsNullOrEmpty(Query))
            {
                throw new Exception("Requête SQL vide. Veuillez fournir une requête valide.");
            }

            try
            {
                using (MySqlCommand cmd = new MySqlCommand(Query, _connection))
                {
                    cmd.ExecuteNonQuery();
                }
                Console.WriteLine("Requête exécutée avec succès.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'exécution de la requête : {ex.Message}");
                return false;
            }
        }
    }
}