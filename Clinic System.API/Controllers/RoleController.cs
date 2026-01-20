namespace Clinic_System.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/role")]
    [ApiController]
    public class RoleController : AppControllerBase
    {
        public RoleController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("promote-doctor")]
        public async Task<IActionResult> PromoteDoctorToAdmin([FromBody] PromoteDoctorToAdminCommand command)
        {
            var response = await mediator.Send(command);
            return NewResult(response);
        }
    }
}
