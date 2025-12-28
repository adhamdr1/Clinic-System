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
        public async Task<IActionResult> GetAvailableSlots([FromQuery] GetAvailableSlotQuery query)
        {
            var response = await mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("DoctorAppointments")]
        public async Task<IActionResult> GetDoctorAppointments([FromQuery] GetDoctorAppointmentsQuery query)
        {
            var response = await mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("PatientAppointments")]
        public async Task<IActionResult> GetPatientAppointments([FromQuery] GetPatientAppointmentsQuery query)
        {
            var response = await mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("AppointmentsByStatusForAdmin")]
        public async Task<IActionResult> GetAppointmentsByStatusForAdmin([FromQuery] GetAppointmentsByStatusForAdminQuery query)
        {
            var response = await mediator.Send(query);

            return Ok(response);
        }

        [HttpGet("AppointmentsByStatusForDoctor")]
        public async Task<IActionResult> GetAppointmentsByStatusForDoctor([FromQuery] GetAppointmentsByStatusForDoctorQuery query)
        {
            var response = await mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("PastAppointmentsForPatient")]
        public async Task<IActionResult> GetPastAppointmentsForPatient([FromQuery] GetPastAppointmentsForPatientQuery query)
        {
            var response = await mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("PastAppointmentsForDoctor")]
        public async Task<IActionResult> GetPastAppointmentsForDoctor([FromQuery] GetPastAppointmentsForDoctorQuery query)
        {
            var response = await mediator.Send(query);
            return Ok(response);
        }

        //[Authorize(Roles = "Patient")]
        [HttpPost("Book")]
        public async Task<IActionResult> BookAppointment([FromBody] BookAppointmentCommand command)
        {
            // حقن الـ ID في الـ command قبل إرساله للـ Handler
            command.PatientId = 1;

            var response = await mediator.Send(command);
            return NewResult(response);
        }
    }
}
