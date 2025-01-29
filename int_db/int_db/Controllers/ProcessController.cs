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
        public IActionResult Process([FromBody] ProcessRequest data)
        {
            try
            {
                if (data == null || data.Valeur == 0)
                {
                    return BadRequest(new { message = "Valeur invalide" });
                }

                int valeur = data.Valeur;

                // Stocker la valeur dans la base de données
                db.OpenConnection();
                db.Query = $"INSERT INTO calc (number, is_even, is_prime, is_perfect) VALUES ({valeur}, {valeur % 2 == 0}, 0, 0);";
                db.ExecuteQuery();
                db.CloseConnection();

                return Ok(new { message = "Données stockées avec succès", result = valeur });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Une erreur interne s'est produite", error = ex.Message });
            }
        }

        [HttpGet("buckets")]
        public async Task<IActionResult> ListBuckets()
        {
            await minio.ListBucketsAsync();
            return Ok(new { message = "Liste des buckets affichée dans la console" });
        }
    }
}