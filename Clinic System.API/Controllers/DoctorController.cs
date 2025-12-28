namespace Clinic_System.API.Controllers
{
    [Route("api/doctors")]
    [ApiController]
    public class DoctorController : AppControllerBase
    {
        public DoctorController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetDoctorList()
        {
            var response = await mediator.Send(new GetDoctorListQuery());
            return Ok(response);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetDoctorListPaging([FromQuery] GetDoctorListPagingQuery query)
        {
            var response = await mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("specializations/{specialization}")]
        public async Task<IActionResult> GetDoctorListBySpecialization(string specialization)
        {
            var response = await mediator.Send(new GetDoctorListBySpecializationQuery
            {
                Specialization = specialization
            });
            return NewResult(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDoctorById([FromQuery] int id)
        {
            var response = await mediator.Send(new GetDoctorByIdQuery
            {
                Id = id
            });
            return NewResult(response);
        }

        [HttpGet("{id:int}/appointments")]
        public async Task<IActionResult> GetDoctorWithAppointmentsById([FromQuery] int id)
        {
            var response = await mediator.Send(new GetDoctorWithAppointmentsByIdQuery
            {
                Id = id
            });
            return NewResult(response);
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetDoctorListByName(string name)
        {
            var response = await mediator.Send(new GetDoctorListByNameQuery
            {
                FullName = name
            });
            return NewResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorCommand command)
        {
            var response = await mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] UpdateDoctorCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("Mismatched Doctor ID");
            }

            var response = await mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut("{id:int}/identity")]
        public async Task<IActionResult> UpdateIdentityDoctor(int id, [FromBody] UpdateIdentityDoctorCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("Mismatched Doctor ID");
            }

            var response = await mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> SoftDeleteDoctor(int id)
        {
            var response = await mediator.Send(new SoftDeleteDoctorCommand
            {
                Id = id
            });
            return NewResult(response);
        }

        [HttpDelete("{id:int}/hard")]
        public async Task<IActionResult> HardDeleteDoctor(int id)
        {
            var response = await mediator.Send(new HardDeleteDoctorCommand
            {
                Id = id
            });
            return NewResult(response);
        }
    }
}
