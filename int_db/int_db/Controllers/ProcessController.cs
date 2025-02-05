using Microsoft.AspNetCore.Mvc;
using int_db.scripts;
using int_db.Models;

namespace int_db.Controllers
{
    [Route("api/process")]
    [ApiController]
    public class ProcessController(Database db, MinioClientWrapper minio) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Process([FromBody] ProcessRequest? data)
        {
            try
            {
                Console.WriteLine($"Données reçues : {System.Text.Json.JsonSerializer.Serialize(data)}");
                
                if (data == null)
                {
                    throw new Exception("Les données reçues sont invalides");
                }

                // Récupérer les données
                int number = data.Number;
                bool isEven = data.IsEven;
                bool isPrime = data.IsPrime;
                bool isPerfect = data.IsPerfect;
                string syracuse = data.Syracuse;
                
                string bucketName = "syracuse";
                string objectName = $"syracuse-{number}.txt";
                
                // --- MySQL ---
                
                // Créer la table calc si elle n'existe pas
                db.OpenConnection();
                db.Query = "CREATE TABLE IF NOT EXISTS `calc` (`id` INT NOT NULL AUTO_INCREMENT, `number` INT NOT NULL, `is_even` BOOLEAN NOT NULL, `is_prime` BOOLEAN NOT NULL, `is_perfect` BOOLEAN NOT NULL, PRIMARY KEY (`id`));";
                if (!db.ExecuteQuery())
                {
                    throw new Exception("Erreur lors de la création de la table");
                }

                // Stocker les données dans la base de données
                db.Query = $"INSERT INTO calc (number, is_even, is_prime, is_perfect) VALUES ({number}, {isEven}, {isPrime}, {isPerfect});";
                if (!db.ExecuteQuery())
                {
                    throw new Exception("Erreur lors de l'insertion des données");
                }
                db.CloseConnection();
                
                // --- MinIO ---
                
                // Créer le bucket syracuse s'il n'existe pas
                minio.OpenConnection();
                if (!minio.CreateBucket(bucketName))
                {
                    throw new Exception("Erreur lors de la création du bucket");
                }
                
                // Stocker la suite de Syracuse dans un bucket MinIO
                if (await minio.ObjectExists(bucketName, objectName) == false)
                {
                    await minio.UploadSyracuse(bucketName, objectName, syracuse);
                }
                
                minio.CloseConnection();

                return Ok(new { message = "Données stockées avec succès", result = new { number, isEven, isPrime, isPerfect, syracuse } });
            }
            catch (Exception ex)
            {
                if (db.IsOpen())
                {
                    db.CloseConnection();
                }
                
                if (minio.IsOpen())
                {
                    minio.CloseConnection();
                }
                return StatusCode(500, new { message = "Une erreur interne s'est produite", error = ex.Message });
            }
        }

        /// <summary>
        /// Récupère la liste des tables de la base de données.
        /// Chemin : /api/process/tables
        /// </summary>
        /// <returns>Liste des tables.</returns>
        [HttpGet("tables")]
        public IActionResult ListTables()
        {
            db.OpenConnection();
            db.Query = "SHOW TABLES;";
            var results = db.ExecuteQueryWithResults();
            db.CloseConnection();
            return Ok(new { tables = results });
        }

        /// <summary>
        /// Récupère la liste des données stockées dans la base de données.
        /// Chemin : /api/process/datas
        /// </summary>
        /// <returns>Liste des données.</returns>
        [HttpGet("datas")]
        public IActionResult ListDatas()
        {
            db.OpenConnection();
            db.Query = "SELECT * FROM calc;";
            var results = db.ExecuteQueryWithResults();
            db.CloseConnection();
            return Ok(new { datas = results });
        }
        
        /// <summary>
        /// Récupère la liste des buckets MinIO.
        /// Chemin : /api/process/buckets
        /// </summary>
        /// <returns>Liste des buckets.</returns>
        [HttpGet("buckets")]
        public IActionResult ListBuckets()
        {
            try 
            {
                minio.OpenConnection();
                var buckets = minio.ListBuckets();
                minio.CloseConnection();
                
                if (buckets.Result.Count == 0)
                {
                    return StatusCode(500, new { message = "Aucun bucket trouvé" });
                }

                return Ok(new { buckets.Result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Une erreur interne s'est produite", error = ex.Message });
            }
            
        }
    }
}