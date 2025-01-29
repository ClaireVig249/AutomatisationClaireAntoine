namespace int_db.scripts;

class Program
{
    static void Main(string[] args)
    {
        Database db = new Database
        {
            Host = "db", // "localhost" pour local, sinon le nom du container Docker ("db")
            Name = "calc",
            User = "root",
            Password = "password"
        };

        var minio = new MinioClientWrapper
        {
            Host = "minio", // "localhost" pour local, sinon le nom du container Docker ("minio")
            AccessKey = "minioadmin",
            SecretKey = "minioadmin"
        };
        
        Console.WriteLine("---- Connexion aux bases de données ----\n");

        db.OpenConnection();
        minio.OpenConnection();
        
        Console.WriteLine("\n--- MySQL ---\n");

        // Show databases
        db.Query = "SHOW DATABASES;";
        db.ExecuteQuery();

        // Create table
        db.Query = "CREATE TABLE IF NOT EXISTS `calc` (`id` INT NOT NULL AUTO_INCREMENT, `number` INT NOT NULL, `is_even` BOOLEAN NOT NULL, `is_prime` BOOLEAN NOT NULL, `is_perfect` BOOLEAN NOT NULL, PRIMARY KEY (`id`));";
        db.ExecuteQuery();

        // Select data
        db.Query = "SELECT * FROM `calc`;";
        db.ExecuteQuery();

        // Show tables
        db.Query = "SHOW TABLES;";
        db.ExecuteQuery();

        Console.WriteLine("\n--- MinIO ---\n");

        minio.ListBucketsAsync().Wait();

        Console.WriteLine("\n--- Fermeture des connexions ---\n");

        db.CloseConnection();
        minio.CloseConnection();
    }
}