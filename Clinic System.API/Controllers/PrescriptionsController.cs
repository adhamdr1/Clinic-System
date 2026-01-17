namespace Clinic_System.API.Controllers
{
    [Route("api/prescription")]
    [ApiController]
    public class PrescriptionController : AppControllerBase
    {
        public PrescriptionController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        public async Task<IActionResult> CreatePrescription([FromBody] CreatePrescriptionCommand command)
        {
            var response = await mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePrescription([FromQuery]int id ,[FromBody] UpdatePrescriptionCommand command)
        {
            if (id != command.PrescriptionId)
            {
                return BadRequest("Prescription ID mismatch.");
            }

            var response = await mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePrescription([FromQuery] DeletePrescriptionCommand command)
        {
            var response = await mediator.Send(command);
            return NewResult(response);
        }
    }
}
