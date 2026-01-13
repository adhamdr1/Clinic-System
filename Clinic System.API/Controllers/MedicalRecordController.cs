namespace Clinic_System.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalRecordController : AppControllerBase
    {
        public MedicalRecordController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateMedicalRecord(int id ,[FromBody] UpdateMedicalRecordCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("Mismatched MedicalRecord ID");
            }

            var response = await mediator.Send(command);
            return NewResult(response);
        }
    }
}
