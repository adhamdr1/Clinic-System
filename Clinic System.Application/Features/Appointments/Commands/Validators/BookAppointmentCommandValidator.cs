namespace Clinic_System.Application.Features.Appointments.Commands.Validators
{
    public class BookAppointmentCommandValidator : AbstractValidator<BookAppointmentCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public BookAppointmentCommandValidator(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;

            ApplyRules();
        }

        private void ApplyRules()
        {
            RuleFor(x => x.DoctorId)
                .GreaterThan(0)
                .MustAsync(DoctorExists)
                .WithMessage("Doctor not found");

            RuleFor(x => x.PatientId)
                .GreaterThan(0)
                .MustAsync(PatientExists)
                .WithMessage("Patient not found");

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

        private async Task<bool> DoctorExists(int doctorId, CancellationToken cancellationToken)
        {
            var doctor = await unitOfWork.DoctorsRepository
                .GetByIdAsync(doctorId, cancellationToken);

            return doctor != null;
        }

        private async Task<bool> PatientExists(int patientId, CancellationToken cancellationToken)
        {
            var patient = await unitOfWork.PatientsRepository
                .GetByIdAsync(patientId, cancellationToken);

            return patient != null;
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
