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

        db.OpenConnection();

        // db.Query = "SELECT * FROM example_table;";
        // db.ExecuteQuery();

        db.CloseConnection();
    }
}