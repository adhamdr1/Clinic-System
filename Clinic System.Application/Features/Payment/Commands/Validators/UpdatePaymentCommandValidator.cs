namespace Clinic_System.Application.Features.Payment.Commands.Validators
{
    public class UpdatePaymentCommandValidator : AbstractValidator<UpdatePaymentCommand>
    {
        public UpdatePaymentCommandValidator()
        {
            RuleFor(x => x.PaymentId)
                .GreaterThan(0).WithMessage("Invalid Payment Id.");

            // المبلغ لو مبعوت، لازم يكون أكبر من صفر
            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .When(x => x.Amount.HasValue)
                .WithMessage("Amount must be greater than 0.");

            // الـ Method لو مبعوتة، لازم تكون في الـ Enum
            RuleFor(x => x.PaymentMethod)
                .IsInEnum()
                .When(x => x.PaymentMethod.HasValue)
                .WithMessage("Invalid Payment Method.");
        }
    }
}
