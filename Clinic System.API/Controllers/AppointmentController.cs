namespace Clinic_System.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : AppControllerBase
    {
        public AppointmentController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("AvailableSlots")]
        public async Task<IActionResult> GetAvailableSlots([FromQuery] int doctorId , [FromQuery] DateTime date)
        {
            var response = await mediator.Send(new GetAvailableSlotQuery
            {
                DoctorId = doctorId,
                Date = date
            });

            return Ok(response);
        }

        [HttpPost("BookAppointment")]
        public async Task<IActionResult> BookAppointment([FromBody] BookAppointmentCommand command)
        {
            var response = await mediator.Send(command);
            return Ok(response);
        }

    }
}
