using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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

        //[Authorize(Roles = "Patient")]
        [HttpPost("book")]
        public async Task<IActionResult> BookAppointment([FromBody] BookAppointmentCommand command)
        {
            // حقن الـ ID في الـ command قبل إرساله للـ Handler
            command.PatientId = 1;

            var response = await mediator.Send(command);
            return Ok(response);
        }

    }
}
