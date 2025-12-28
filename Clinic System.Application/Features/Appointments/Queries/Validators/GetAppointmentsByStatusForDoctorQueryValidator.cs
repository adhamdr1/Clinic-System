namespace Clinic_System.Application.Features.Appointments.Queries.Validators
{
    public class GetAppointmentsByStatusForDoctorQueryValidator : AbstractValidator<GetAppointmentsByStatusForDoctorQuery>
    {
        public GetAppointmentsByStatusForDoctorQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

            RuleFor(x => x.DoctorId)
                .GreaterThanOrEqualTo(1).WithMessage("Doctor Id must be at least 1.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid appointment status.");

            // التأكد من أن تاريخ البداية ليس بعد تاريخ النهاية
            RuleFor(x => x)
                .Must(x => !(x.Start.HasValue && x.End.HasValue && x.Start > x.End))
                .WithMessage("Start date cannot be later than the end date.")
                .WithName("DateRange");
        }
    }
}
