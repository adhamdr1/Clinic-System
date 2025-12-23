namespace Clinic_System.API.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class PatientController : AppControllerBase
    {
        public PatientController(IMediator mediator) : base(mediator)
        {
        }

        [Route("GetPatientList")]
        [HttpGet]
        public async Task<IActionResult> GetPatientList()
        {
            var response = await mediator.Send(new GetPatientListQuery());
            return Ok(response);
        }

        [Route("GetPatientListPaging")]
        [HttpGet]
        public async Task<IActionResult> GetPatientListPaging([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var response = await mediator.Send(new GetPatientListPagingQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            });
            return Ok(response);
        }

        [Route("GetPatientById/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetPatientById(int id)
        {
            var response = await mediator.Send(new GetPatientByIdQuery
            {
                Id = id
            });
            return NewResult(response);
        }

        [Route("GetPatientWithAppointmentsById/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetPatientWithAppointmentsById(int id)
        {
            var response = await mediator.Send(new GetPatientWithAppointmentsByIdQuery
            {
                Id = id
            });
            return NewResult(response);
        }

        [Route("CreatePatient")]
        [HttpPost]
        public async Task<IActionResult> CreatePatient([FromBody] CreatePatientCommand command)
        {
            var response = await mediator.Send(command);
            return NewResult(response);
        }

        [Route("UpdatePatient/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] UpdatePatientCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("Mismatched Patient ID");
            }

            var response = await mediator.Send(command);
            return NewResult(response);
        }

        [Route("UpdateIdentityPatient/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateIdentityPatient(int id, [FromBody] UpdateIdentityPatientCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("Mismatched Patient ID");
            }

            var response = await mediator.Send(command);
            return NewResult(response);
        }

        [Route("SoftDeletePatient/{id}")]
        [HttpDelete]
        public async Task<IActionResult> SoftDeletePatient(int id)
        {
            var response = await mediator.Send(new SoftDeletePatientCommand
            {
                Id = id
            });
            return NewResult(response);
        }

        [Route("HardDeletePatient/{id}")]
        [HttpDelete]
        public async Task<IActionResult> HardDeletePatient(int id)
        {
            var response = await mediator.Send(new HardDeletePatientCommand
            {
                Id = id
            });
            return NewResult(response);
        }
    }
}
