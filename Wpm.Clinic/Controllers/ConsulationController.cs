using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wpm.Clinic.Application;

namespace Wpm.Clinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsulationController(ClinicApplicationService applicationService) : ControllerBase
    {
        [HttpPost("/start")]
        public async Task<IActionResult> Start([FromBody] StartConsulationCommand cmd) 
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await applicationService.Handle(cmd);
            return Ok(result);
        }

       
    }

    public record StartConsulationCommand(int PatientId);
   
}
