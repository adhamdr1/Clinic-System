namespace Clinic_System.API.Controllers
{
    [Route("api/appointments")]
    [ApiController]
    public class AppointmentController : AppControllerBase
    {
        public AppointmentController(IMediator mediator) : base(mediator)
        {
        }

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
            return Ok(response);
        }

        [HttpGet("doctor")]
        public async Task<IActionResult> GetDoctorAppointments([FromQuery] GetDoctorAppointmentsQuery query)
        {
            var response = await mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("today-schedule")]
        public async Task<IActionResult> GetTodaySchedule()
        {
            var query = new GetDoctorAppointmentsQuery
            {
                DoctorId = 1,
                DateTime = DateTime.Today, // اليوم الحالي فقط
                PageNumber = 1,
                PageSize = 100 // لضمان عرض كل مواعيد اليوم في قائمة واحدة
            };

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
            command.PatientId = 8;

            var response = await mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut("confirm")]
        public async Task<IActionResult> ConfirmAppointment([FromBody] ConfirmAppointmentCommand command)
        {
            command.PatientId = 8;

            var response = await mediator.Send(command);
            return Ok(response);
        }
    
        [HttpPut("complete")]
        public async Task<IActionResult> CompleteAppointment([FromBody] CompleteAppointmentCommand command)
        {
            // حقن الـ ID في الـ command قبل إرساله للـ Handler
            command.DoctorId = 1;

            var response = await mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut("reschedule")]
        public async Task<IActionResult> RescheduleAppointment([FromBody] RescheduleAppointmentCommand command)
        {
            // حقن الـ ID في الـ command قبل إرساله للـ Handler
            command.PatientId = 8;

            var response = await mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut("noshow")]
        public async Task<IActionResult> NoShowAppointment([FromBody] NoShowAppointmentCommand command)
        {
            // حقن الـ ID في الـ command قبل إرساله للـ Handler
            command.DoctorId = 1;

            var response = await mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut("cancel")]
        public async Task<IActionResult> CancelledAppointment([FromBody] CancelAppointmentCommand command)
        {
            // حقن الـ ID في الـ command قبل إرساله للـ Handler
            command.PatientId = 8;

            var response = await mediator.Send(command);
            return NewResult(response);
        }
    }
}