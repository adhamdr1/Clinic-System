namespace Clinic_System.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IMediator mediator;

        public DoctorController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetDoctorList()
        {
            var response = await mediator.Send(new GetDoctorListQuery());
            return Ok(response);
        }
    }
}
