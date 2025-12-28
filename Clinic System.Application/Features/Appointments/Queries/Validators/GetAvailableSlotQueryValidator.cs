namespace Clinic_System.Application.Features.Appointments.Queries.Validators
{
    public class GetAvailableSlotQueryValidator : AbstractValidator<GetAvailableSlotQuery>
    {
        public GetAvailableSlotQueryValidator()
        {
            RuleFor(x => x.DoctorId)
                .GreaterThanOrEqualTo(1).WithMessage("Doctor Id must be at least 1.");

            // التأكد من أن تاريخ البداية ليس بعد تاريخ النهاية
            RuleFor(x => x)
                .Must(x => !(x.Date.Date < DateTime.Today.Date))
                .WithMessage("Date cannot be in Past");
        }
    }
}
