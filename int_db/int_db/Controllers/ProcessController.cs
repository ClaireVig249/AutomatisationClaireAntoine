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
        public IActionResult Process([FromBody] ProcessRequest? data)
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
            var buckets = await minio.ListBucketsAsync();
            return Ok(new { buckets });
        }

        [HttpGet("tables")]
        public IActionResult ListTables()
        {
            db.OpenConnection();
            db.Query = "SHOW TABLES;";
            var results = db.ExecuteQueryWithResults();
            db.CloseConnection();
            return Ok(results);
        }

        [HttpGet("datas")]
        public IActionResult ListDatas()
        {
            db.OpenConnection();
            db.Query = "SELECT * FROM calc;";
            var results = db.ExecuteQueryWithResults();
            db.CloseConnection();
            return Ok(results);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromBody] UploadRequest request)
        {
            if (string.IsNullOrEmpty(request.BucketName) || string.IsNullOrEmpty(request.ObjectName) || string.IsNullOrEmpty(request.FilePath))
            {
                return BadRequest(new { message = "Paramètres invalides" });
            }

            var result = await minio.UploadFileAsync(request.BucketName, request.ObjectName, request.FilePath);
            return Ok(new { message = result });
        }

        [HttpPost("download")]
        public async Task<IActionResult> DownloadFile([FromBody] DownloadRequest request)
        {
            if (string.IsNullOrEmpty(request.BucketName) || string.IsNullOrEmpty(request.ObjectName) || string.IsNullOrEmpty(request.FilePath))
            {
                return BadRequest(new { message = "Paramètres invalides" });
            }

            var result = await minio.DownloadFileAsync(request.BucketName, request.ObjectName, request.FilePath);
            return Ok(new { message = result });
        }
    }
}