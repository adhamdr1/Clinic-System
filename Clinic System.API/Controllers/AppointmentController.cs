namespace Clinic_System.API.Controllers
{
    [Authorize]
    [Route("api/appointments")]
    [ApiController]
    public class AppointmentController : AppControllerBase
    {
        public AppointmentController(IMediator mediator) : base(mediator)
        {
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("stats")]
        public async Task<IActionResult> GetAppointmentsStats([FromQuery] GetAdminAppointmentsStatsQuery query)
        {
            var response = await mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("AvailableSlots")]
        public async Task<IActionResult> GetAvailableSlots([FromQuery] GetAvailableSlotQuery query)
        {
            var response = await mediator.Send(query);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin,Doctor")]
        [HttpGet("doctor")]
        public async Task<IActionResult> GetDoctorAppointments([FromQuery] GetDoctorAppointmentsQuery query)
        {
            var response = await mediator.Send(query);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin,Patient")]
        [HttpGet("patient")]
        public async Task<IActionResult> GetPatientAppointments([FromQuery] GetPatientAppointmentsQuery query)
        {
            var response = await mediator.Send(query);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("statusforadmin")]
        public async Task<IActionResult> GetAppointmentsByStatusForAdmin([FromQuery] GetAppointmentsByStatusForAdminQuery query)
        {
            var response = await mediator.Send(query);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin,Doctor")]
        [HttpGet("statusfordoctor")]
        public async Task<IActionResult> GetAppointmentsByStatusForDoctor([FromQuery] GetAppointmentsByStatusForDoctorQuery query)
        {
            var response = await mediator.Send(query);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin,Patient")]
        [HttpGet("pastforpatient")]
        public async Task<IActionResult> GetPastAppointmentsForPatient([FromQuery] GetPastAppointmentsForPatientQuery query)
        {
            var response = await mediator.Send(query);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin,Doctor")]
        [HttpGet("pastfordoctor")]
        public async Task<IActionResult> GetPastAppointmentsForDoctor([FromQuery] GetPastAppointmentsForDoctorQuery query)
        {
            var response = await mediator.Send(query);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin,Patient")]
        [HttpPost("book")]
        public async Task<IActionResult> BookAppointment([FromBody] BookAppointmentCommand command)
        {
            var response = await mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin,Patient")]
        [HttpPut("confirm")]
        public async Task<IActionResult> ConfirmAppointment([FromBody] ConfirmAppointmentCommand command)
        {
            var response = await mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin,Doctor")]
        [HttpPut("complete")]
        public async Task<IActionResult> CompleteAppointment([FromBody] CompleteAppointmentCommand command)
        {
            // حقن الـ ID في الـ command قبل إرساله للـ Handler
            command.DoctorId = 1;

            var response = await mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin,Patient")]
        [HttpPut("reschedule")]
        public async Task<IActionResult> RescheduleAppointment([FromBody] RescheduleAppointmentCommand command)
        {
            var response = await mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin,Doctor")]
        [HttpPut("noshow")]
        public async Task<IActionResult> NoShowAppointment([FromBody] NoShowAppointmentCommand command)
        {
            var response = await mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin,Patient")]
        [HttpPut("cancel")]
        public async Task<IActionResult> CancelledAppointment([FromBody] CancelAppointmentCommand command)
        {
            var response = await mediator.Send(command);
            return NewResult(response);
        }
    }
}