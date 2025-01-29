using Microsoft.AspNetCore.Mvc;

namespace IntDB
{
    [Route("api/process")]
    [ApiController]
    public class ProcessController : ControllerBase
    {
        [HttpPost]
        public IActionResult Process([FromBody] dynamic data)
        {
            int valeur = data.valeur;
            return Ok(new { resultat = valeur + 10 });
        }
    }
}