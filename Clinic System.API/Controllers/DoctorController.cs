namespace Clinic_System.API.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IMediator mediator;

        public DoctorController(IMediator mediator)
        {
            this.mediator = mediator;
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
            return Ok(response);
        }
    }
}
