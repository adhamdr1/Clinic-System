namespace Clinic_System.API.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class DoctorController : AppControllerBase
    {
        public DoctorController(IMediator mediator) : base(mediator)
        {
        }

        [Route("api/GetDoctorList")]
        [HttpGet]
        public async Task<IActionResult> GetDoctorList()
        {
            var response = await mediator.Send(new GetDoctorListQuery());
            return Ok(response);
        }

        [Route("api/GetDoctorListPaging")]
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

        [Route("api/GetDoctorWithAppointmentsById/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetDoctorWithAppointmentsById(int id)
        {
            var response = await mediator.Send(new GetDoctorWithAppointmentsByIdQuery
            {
                Id = id
            });
            return NewResult(response);
        }

        [Route("api/CreateDoctor")]
        [HttpPost]
        public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorCommand command)
        {
            var response = await mediator.Send(command);
            return NewResult(response);
        }
    }
}
