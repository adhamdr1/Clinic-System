namespace Clinic_System.API.Controllers
{
    [Authorize]
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : AppControllerBase
    {
        public PaymentController(IMediator mediator) : base(mediator)
        {
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("list")]
        public async Task<IActionResult> GetAllPaymentsAsync([FromQuery] GetPaymentsListQuery query, CancellationToken cancellationToken)
        {
            var payments = await mediator.Send(query, cancellationToken);
            return Ok(payments);
        }

        [Authorize(Roles = "Admin,Doctor,Patient")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var response = await mediator.Send(new GetPaymentDetailsByIdQuery { Id = id });
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("daily-revenue")]
        public async Task<IActionResult> GetDailyRevenueAsync([FromQuery] GetDailyRevenueQuery query, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(query, cancellationToken);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin,Doctor")]
        [HttpGet("doctor-revenue")]
        public async Task<IActionResult> GetDoctorRevenueAsync([FromQuery] GetDoctorRevenueQuery query, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(query, cancellationToken);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdatePaymentAsync([FromRoute] int id, [FromBody] UpdatePaymentCommand command, CancellationToken cancellationToken)
        {
            // التحقق المهم جداً
            if (id != command.PaymentId)
            {
                return BadRequest("Payment ID mismatch between route and body.");
            }

            var result = await mediator.Send(command, cancellationToken);
            return NewResult(result);
        }
    }
}
