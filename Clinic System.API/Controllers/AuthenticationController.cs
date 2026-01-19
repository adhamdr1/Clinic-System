namespace Clinic_System.API.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : AppControllerBase 
    {
        public AuthenticationController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var response = await mediator.Send(command);
            return NewResult(response);
        }
    }
}
