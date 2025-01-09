using IntDB;

class Program
{
    static void Main(string[] args)
    {
        Database db = new Database
        {
            Host = "localhost",
            Name = "calc",
            User = "root",
            Password = "password"
        };

        var minio = new MinioClientWrapper
        {
            Host = "localhost", // Remplacez par l'hôte de votre serveur MinIO
            AccessKey = "minioadmin", // Remplacez par votre clé d'accès
            SecretKey = "minioadmin", // Remplacez par votre clé secrète
            Port = "9000"
        };

        db.OpenConnection();
        minio.OpenConnection();

        db.CloseConnection();
        minio.CloseConnection();
    }
}