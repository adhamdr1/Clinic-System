namespace Clinic_System.Application.Features.Appointments.Commands.Validators
{
    public class RescheduleAppointmentCommandValidator : AbstractValidator<RescheduleAppointmentCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public RescheduleAppointmentCommandValidator(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;

            ApplyRules();
        }

        private void ApplyRules()
        {
            RuleFor(x => x.AppointmentId)
                .GreaterThan(0)
                .MustAsync(AppointmentExists)
                .WithMessage("Appointment not found");

            RuleFor(x => x)
               .MustAsync(NewDateTimeDifferentFromOld)
               .WithMessage("New appointment date and time must be different from the current appointment")
               .When(x => x.AppointmentDate >= DateTime.Today);

            RuleFor(x => x.AppointmentDate)
               .GreaterThanOrEqualTo(DateTime.Today)
               .WithMessage("Appointment date cannot be in the past");

            RuleFor(x => x.AppointmentTime)
            .NotEmpty()
            .WithMessage("Appointment time is required")
            // تحقق من أن الوقت يقع بين 12:00:00 و 22:00:00
            .Must(BeWithinServiceHours)
            .WithMessage("The appointment time must be between 12:00PM and 10:00PM.");
        }

        private async Task<bool> AppointmentExists(int appointmentId, CancellationToken cancellationToken)
        {
            var appointment = await unitOfWork.AppointmentsRepository
                .GetByIdAsync(appointmentId, cancellationToken);

            return appointment != null;
        }
        private async Task<bool> NewDateTimeDifferentFromOld(
                    RescheduleAppointmentCommand command,
                    CancellationToken cancellationToken)
        {
            var appointment = await unitOfWork.AppointmentsRepository
                .GetByIdAsync(command.AppointmentId, cancellationToken);

            if (appointment == null)
                return true;

            var existingDateTime = appointment.AppointmentDate;
            var newDateTime = command.AppointmentDate.Date + command.AppointmentTime;

            return existingDateTime != newDateTime;
        }

        // >> الميثود المساعدة الجديدة في Validator
        private bool BeWithinServiceHours(TimeSpan appointmentTime)
        {
            var DefaultStartTime = new TimeSpan(12, 0, 0); // 12:00 PM
            var DefaultEndTime = new TimeSpan(22, 0, 0);  // 10:00 PM

            // يجب أن يكون الوقت >= بداية الخدمة و < نهاية الخدمة (لأن الـ Slot Duration يجب أن يُحسب)
            return appointmentTime >= DefaultStartTime && appointmentTime < DefaultEndTime;
        }
    }
}
