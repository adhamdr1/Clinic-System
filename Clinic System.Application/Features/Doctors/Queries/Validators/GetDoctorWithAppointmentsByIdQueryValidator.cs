namespace Clinic_System.Application.Features.Doctors.Queries.Validators
{
    public class GetDoctorWithAppointmentsByIdQueryValidator : AbstractValidator<GetDoctorWithAppointmentsByIdQuery>
    {
        public GetDoctorWithAppointmentsByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThanOrEqualTo(1).WithMessage("Doctor Id must be at least 1.");
        }
    }
}
