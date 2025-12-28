namespace Clinic_System.Application.Features.Appointments.Queries.Validators
{
    public class GetDoctorAppointmentsQueryValidator : AbstractValidator<GetDoctorAppointmentsQuery>
    {
        public GetDoctorAppointmentsQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

            RuleFor(x => x.DoctorId)
                .GreaterThanOrEqualTo(1).WithMessage("Doctor Id must be at least 1.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");
        }
    }
}
