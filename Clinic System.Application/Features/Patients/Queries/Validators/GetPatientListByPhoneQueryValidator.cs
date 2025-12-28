namespace Clinic_System.Application.Features.Patients.Queries.Validators
{
    public class GetPatientListByPhoneQueryValidator : AbstractValidator<GetPatientByPhoneQuery>
    {
        public GetPatientListByPhoneQueryValidator()
        {
            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage("Phone is required")
                .Matches(@"^\+?[0-9]{10,15}$")
                .WithMessage("Phone is invalid");
        }
    }
}