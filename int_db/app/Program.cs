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
            Host = "localhost",
            AccessKey = "minioadmin",
            SecretKey = "minioadmin",
            Port = "9000"
        };

        db.OpenConnection();
        minio.OpenConnection();

        db.CloseConnection();
        minio.CloseConnection();
    }
}