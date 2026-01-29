namespace Clinic_System.Application.Features.Appointments.Commands.Validators
{
    public class BookAppointmentCommandValidator : AbstractValidator<BookAppointmentCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public BookAppointmentCommandValidator(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;

            ApplyRules();
        }

        private void ApplyRules()
        {
            RuleFor(x => x.DoctorId)
                .GreaterThan(0)
                .MustAsync(DoctorExists)
                .WithMessage("Doctor not found");

            RuleFor(x => x.PatientId)
                .MustAsync(async (command, patientId, ct) =>
                {
                    var roles = await currentUserService.GetCurrentUserRolesAsync();

                    // أ. لو مريض: عدي (رجع true) حتى لو الـ ID بـ 0
                    // لأن الـ Handler هيجيب الـ ID الحقيقي من التوكن بعدين
                    if (roles.Contains("Patient")) return true;

                    // ب. لو أدمن (أو أي حد تاني): لازم يكون باعت ID وقيمته > 0
                    return patientId > 0;
                })
                .WithMessage("Patient Id is required for Admin booking.")
                .DependentRules(() => // القاعدة دي هتشتغل بس لو اللي فوق عدت
                {
                    RuleFor(x => x.PatientId)
                        .MustAsync(async (command, patientId, ct) =>
                        {
                            var roles = await currentUserService.GetCurrentUserRolesAsync();
                            if (roles.Contains("Patient")) return true; // المريض ميتحصش هنا

                            return await PatientExists(patientId, ct); // الأدمن نتأكد إن المريض اللي باعته موجود
                        })
                        .WithMessage("Patient not found in database.");
                });

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
