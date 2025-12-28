namespace Clinic_System.Application.Features.Patients.Queries.Validators
{
    public class GetPatientByIdQueryValidator : AbstractValidator<GetPatientByIdQuery>
    {
        public GetPatientByIdQueryValidator()
        {
            RuleFor(x =>x.Id)
                .GreaterThanOrEqualTo(1).WithMessage("Patient Id must be at least 1.");
        }
    }
}
