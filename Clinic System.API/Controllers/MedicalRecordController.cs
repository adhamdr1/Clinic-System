namespace Clinic_System.API.Controllers
{
    [Route("api/medicalrecords")]
    [ApiController]
    public class MedicalRecordController : AppControllerBase
    {
        public MedicalRecordController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetMedicalRecordById(int id)
        {
            // بنبعت الـ ID للـ Query constructor
            var response = await mediator.Send(new GetMedicalRecordByIdQuery
            {
                Id = id
            });

            return NewResult(response);
        }

        [HttpGet("patient")]
        public async Task<IActionResult> GetPatientHistory([FromQuery] GetPatientHistoryQuery query)
        {
            var response = await mediator.Send(query);
            return NewResult(response);
        }

        [HttpGet("doctor")]
        public async Task<IActionResult> GetRecordsByDoctorId([FromQuery] GetRecordsByDoctorIdQuery query)
        {
            var response = await mediator.Send(query);
            return NewResult(response);
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
