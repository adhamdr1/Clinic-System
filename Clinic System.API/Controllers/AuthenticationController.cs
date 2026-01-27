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

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            var response = await mediator.Send(command);
            return NewResult(response);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailQuery query)
        {
            var response = await mediator.Send(query);
            return NewResult(response);
        }

        [HttpPost("send-reset-password")]
        public async Task<IActionResult> SendResetPassword([FromBody] SendResetPasswordCommand command)
        {
            command.BaseUrl = $"{Request.Scheme}://{Request.Host}";

            var response = await mediator.Send(command);
            return NewResult(response);
        }

        // 2. تنفيذ تغيير الباسورد (Reset Password)
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            var response = await mediator.Send(command);
            return NewResult(response);
        }
    }
}
