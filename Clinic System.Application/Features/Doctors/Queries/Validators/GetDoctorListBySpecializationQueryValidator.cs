namespace Clinic_System.Application.Features.Doctors.Queries.Validators
{
    public class GetDoctorListBySpecializationQueryValidator : AbstractValidator<GetDoctorListBySpecializationQuery>
    {
        public GetDoctorListBySpecializationQueryValidator()
        {
            RuleFor(x => x.Specialization)
                .NotNull()
                .NotEmpty()
                .WithMessage("Specialization is required");
        }
    }
}
