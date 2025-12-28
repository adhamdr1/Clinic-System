namespace Clinic_System.Application.Features.Patients.Queries.Validators
{
    public class GetPatientListByNameQueryValidator : AbstractValidator<GetPatientListByNameQuery>
    {
        public GetPatientListByNameQueryValidator()
        {
            RuleFor(x => x.FullName)
                .NotNull()
                .NotEmpty()
                .WithMessage("Name is required");
        }
    }
}
