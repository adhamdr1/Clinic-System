using Clinic_System.Application.Features.Appointments.Queries.Models;

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

        [HttpGet("DoctorAppointments")]
        public async Task<IActionResult> GetDoctorAppointments([FromQuery] int doctorId,[FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10 , [FromQuery] DateTime? date = null)
        {
            var response = await mediator.Send(new GetDoctorAppointmentsQuery
            {
                doctorId = doctorId,
                pageNumber = pageNumber,
                pageSize = pageSize,
                dateTime = date
            });

            return Ok(response);
        }

        [HttpGet("PatientAppointments")]
        public async Task<IActionResult> GetPatientAppointments([FromQuery] int patientId, [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10, [FromQuery] DateTime? date = null)
        {
            var response = await mediator.Send(new GetPatientAppointmentsQuery
            {
                patientId = patientId,
                pageNumber = pageNumber,
                pageSize = pageSize,
                dateTime = date
            });

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
