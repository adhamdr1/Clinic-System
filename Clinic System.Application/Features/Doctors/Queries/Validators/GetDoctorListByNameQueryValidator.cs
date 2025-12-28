namespace Clinic_System.Application.Features.Doctors.Queries.Validators
{
    public class GetDoctorListByNameQueryValidator : AbstractValidator<GetDoctorListByNameQuery>
    {
        public GetDoctorListByNameQueryValidator()
        {
            RuleFor(x => x.FullName)
                .NotNull()
                .NotEmpty()
                .WithMessage("Name is required");
        }
    }
}
