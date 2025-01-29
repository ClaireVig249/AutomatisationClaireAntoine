using Microsoft.AspNetCore.Mvc;
using int_db.scripts;

namespace int_db.Controllers
{
    [Route("api/process")]
    [ApiController]
    public class ProcessController(Database db, MinioClientWrapper minio) : ControllerBase
    {
        [HttpPost]
        public IActionResult Process([FromBody] dynamic data)
        {
            int valeur = data.valeur;

            // Stocker la valeur dans la base de données
            db.OpenConnection();
            db.Query = $"INSERT INTO calc (number, is_even, is_prime, is_perfect) VALUES ({valeur}, {valeur % 2 == 0}, 0, 0);";
            db.ExecuteQuery();
            db.CloseConnection();

            return Ok(new { message = "Données stockées avec succès", result = valeur });
        }

        [HttpGet("buckets")]
        public async Task<IActionResult> ListBuckets()
        {
            await minio.ListBucketsAsync();
            return Ok(new { message = "Liste des buckets affichée dans la console" });
        }
    }
}