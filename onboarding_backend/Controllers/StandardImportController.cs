using Microsoft.AspNetCore.Mvc;
using onboarding_backend.Models.StandardImport;

namespace onboarding_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StandardImportController : ControllerBase
    {
        // POST: api/StandardImport
        [HttpPost("import")]
        public IActionResult ImportData([FromBody] Standardimport model)
        {   

            if (model == null)
                return BadRequest("No data provided.");
            
            

            
            
            return Ok(new { message = "Data imported successfully", model });
        }
    }
}


