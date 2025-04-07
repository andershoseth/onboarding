using Microsoft.AspNetCore.Mvc;
using onboarding_backend.Models.StandardImport;
using onboarding_backend.Services;

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


            var file = ExcelSingleSheetExporter.CreateSingleSheet(model);
            
            
            return File(
                fileContents: file,
                contentType: "application/xlsx",
                fileDownloadName: "StandardImport.xlsx"
            );
        }
    }
}


