namespace Clinic_System.Application.Features.Doctors.Queries.Validators
{
    public class GetDoctorByIdQueryValidator : AbstractValidator<GetDoctorByIdQuery>
    {
        public GetDoctorByIdQueryValidator()
        {
            RuleFor(x =>x.Id)
                .GreaterThanOrEqualTo(1).WithMessage("Doctor Id must be at least 1.");
        }
    }
}
