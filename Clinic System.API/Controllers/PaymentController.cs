namespace Clinic_System.API.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : AppControllerBase
    {
        public PaymentController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllPaymentsAsync([FromQuery] GetPaymentsListQuery query, CancellationToken cancellationToken)
        {
            var payments = await mediator.Send(query, cancellationToken);
            return Ok(payments);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var response = await mediator.Send(new GetPaymentDetailsByIdQuery { Id = id });
            return NewResult(response);
        }

        [HttpGet("daily-revenue")]
        public async Task<IActionResult> GetDailyRevenueAsync([FromQuery] GetDailyRevenueQuery query, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(query, cancellationToken);
            return NewResult(response);
        }

        [HttpGet("doctor-revenue")]
        public async Task<IActionResult> GetDoctorRevenueAsync([FromQuery] GetDoctorRevenueQuery query, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(query, cancellationToken);
            return NewResult(response);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdatePaymentAsync([FromBody] UpdatePaymentCommand command, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);
            return NewResult(result);
        }
    }
}
