namespace Clinic_System.API.Controllers
{
    [Authorize(Roles = "Admin,Doctor")]
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

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdatePrescription([FromBody] UpdatePrescriptionCommand command)
        {
            var response = await mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePrescription([FromRoute] int id)
        {
            var response = await mediator.Send(new DeletePrescriptionCommand { PrescriptionId = id });
            return NewResult(response);
        }
    }
}
