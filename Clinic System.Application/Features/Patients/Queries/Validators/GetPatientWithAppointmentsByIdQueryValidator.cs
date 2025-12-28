namespace Clinic_System.Application.Features.Patients.Queries.Validators
{
    public class GetPatientWithAppointmentsByIdQueryValidator : AbstractValidator<GetPatientWithAppointmentsByIdQuery>
    {
        public GetPatientWithAppointmentsByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThanOrEqualTo(1).WithMessage("Patient Id must be at least 1.");
        }
    }
}
