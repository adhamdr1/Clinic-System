namespace Clinic_System.API.Controllers
{
    [Authorize]
    [Route("api/patients")]
    [ApiController]
    public class PatientController : AppControllerBase
    {
        public PatientController(IMediator mediator) : base(mediator)
        {
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetPatientList()
        {
            var response = await mediator.Send(new GetPatientListQuery());
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("paging")]
        public async Task<IActionResult> GetPatientListPaging([FromQuery] GetPatientListPagingQuery query)
        {
            var response = await mediator.Send(query);
            return Ok(response);
        }

        [Authorize(Roles = "Admin,Doctor,Patient")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPatientById(int id)
        {
            var response = await mediator.Send(new GetPatientByIdQuery
            {
                Id = id
            });
            return NewResult(response);
        }

        [Authorize(Roles = "Admin,Doctor")]
        [HttpGet("phone/{phone}")]
        public async Task<IActionResult> GetPatientByPhone(string phone)
        {
            var response = await mediator.Send(new GetPatientByPhoneQuery
            {
                Phone = phone
            });
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetPatientListByName(string name)
        {
            var response = await mediator.Send(new GetPatientListByNameQuery
            {
                FullName = name
            });
            return NewResult(response);
        }

        [Authorize(Roles = "Admin,Patient")]
        [HttpGet("{id:int}/appointments")]
        public async Task<IActionResult> GetPatientWithAppointmentsById(int id)
        {
            var response = await mediator.Send(new GetPatientWithAppointmentsByIdQuery
            {
                Id = id
            });
            return NewResult(response);
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> CreatePatient([FromBody] CreatePatientCommand command)
        {
            command.BaseUrl = $"{Request.Scheme}://{Request.Host.Value}";
            var response = await mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin,Patient")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] UpdatePatientCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("Mismatched Patient ID");
            }

            var response = await mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin,Patient")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> SoftDeletePatient(int id)
        {
            var response = await mediator.Send(new SoftDeletePatientCommand
            {
                Id = id
            });
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}/hard")]
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
