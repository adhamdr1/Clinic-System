namespace Clinic_System.Application.Features.Appointments.Queries.Validators
{
    public class GetPatientAppointmentsQueryValidator : AbstractValidator<GetPatientAppointmentsQuery>
    {
        public GetPatientAppointmentsQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

            RuleFor(x => x.PatientId)
                .GreaterThanOrEqualTo(1).WithMessage("Patient Id must be at least 1.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");
        }
    }
}
