namespace Clinic_System.API.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class DoctorController : AppControllerBase
    {
        public DoctorController(IMediator mediator) : base(mediator)
        {
        }

        //Specialization
        [Route("GetDoctorList")]
        [HttpGet]
        public async Task<IActionResult> GetDoctorList()
        {
            var response = await mediator.Send(new GetDoctorListQuery());
            return Ok(response);
        }

        [Route("GetDoctorListBySpecialization")]
        [HttpGet]
        public async Task<IActionResult> GetDoctorListBySpecialization(string Specialization)
        {
            var response = await mediator.Send(new GetDoctorListBySpecializationQuery
            {
                Specialization = Specialization
            });
            return NewResult(response);
        }

        [Route("GetDoctorListPaging")]
        [HttpGet]
        public async Task<IActionResult> GetDoctorListPaging([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var response = await mediator.Send(new GetDoctorListPagingQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            });
            return Ok(response);
        }

        [Route("GetDoctorWithAppointmentsById/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetDoctorWithAppointmentsById(int id)
        {
            var response = await mediator.Send(new GetDoctorWithAppointmentsByIdQuery
            {
                Id = id
            });
            return NewResult(response);
        }

        [Route("GetDoctorById/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            var response = await mediator.Send(new GetDoctorByIdQuery
            {
                Id = id
            });
            return NewResult(response);
        }

        [Route("CreateDoctor")]
        [HttpPost]
        public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorCommand command)
        {
            var response = await mediator.Send(command);
            return NewResult(response);
        }

        [Route("UpdateDoctor/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] UpdateDoctorCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("Mismatched Doctor ID");
            }

            var response = await mediator.Send(command);
            return NewResult(response);
        }

        [Route("UpdateIdentityDoctor/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateIdentityDoctor(int id, [FromBody] UpdateIdentityDoctorCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("Mismatched Doctor ID");
            }

            var response = await mediator.Send(command);
            return NewResult(response);
        }

        [Route("SoftDeleteDoctor/{id}")]
        [HttpDelete]
        public async Task<IActionResult> SoftDeleteDoctor(int id)
        {
            var response = await mediator.Send(new SoftDeleteDoctorCommand
            {
                Id = id
            });
            return NewResult(response);
        }

        [Route("HardDeleteDoctor/{id}")]
        [HttpDelete]
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
