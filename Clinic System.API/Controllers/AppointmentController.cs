namespace Clinic_System.API.Controllers
{
    [Route("api/appointments")]
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

        [HttpGet("doctor")]
        public async Task<IActionResult> GetDoctorAppointments([FromQuery] GetDoctorAppointmentsQuery query)
        {
            var response = await mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("patient")]
        public async Task<IActionResult> GetPatientAppointments([FromQuery] GetPatientAppointmentsQuery query)
        {
            var response = await mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("statusforadmin")]
        public async Task<IActionResult> GetAppointmentsByStatusForAdmin([FromQuery] GetAppointmentsByStatusForAdminQuery query)
        {
            var response = await mediator.Send(query);

            return Ok(response);
        }

        [HttpGet("statusfordoctor")]
        public async Task<IActionResult> GetAppointmentsByStatusForDoctor([FromQuery] GetAppointmentsByStatusForDoctorQuery query)
        {
            var response = await mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("pastforpatient")]
        public async Task<IActionResult> GetPastAppointmentsForPatient([FromQuery] GetPastAppointmentsForPatientQuery query)
        {
            var response = await mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("pastfordoctor")]
        public async Task<IActionResult> GetPastAppointmentsForDoctor([FromQuery] GetPastAppointmentsForDoctorQuery query)
        {
            var response = await mediator.Send(query);
            return Ok(response);
        }

        [HttpPost("book")]
        public async Task<IActionResult> BookAppointment([FromBody] BookAppointmentCommand command)
        {
            // حقن الـ ID في الـ command قبل إرساله للـ Handler
            command.PatientId = 1;

            var response = await mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut("reschedule")]
        public async Task<IActionResult> RescheduleAppointment([FromBody] RescheduleAppointmentCommand command)
        {
            // حقن الـ ID في الـ command قبل إرساله للـ Handler
            command.PatientId = 1;

            var response = await mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut("cancel")]
        public async Task<IActionResult> CancelledAppointment([FromBody] CancelAppointmentCommand command)
        {
            // حقن الـ ID في الـ command قبل إرساله للـ Handler
            command.PatientId = 1;

            var response = await mediator.Send(command);
            return NewResult(response);
        }
    }
}