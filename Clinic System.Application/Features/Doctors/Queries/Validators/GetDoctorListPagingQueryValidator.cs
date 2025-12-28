namespace Clinic_System.Application.Features.Doctors.Queries.Validators
{
    public class GetDoctorListPagingQueryValidator : AbstractValidator<GetDoctorListPagingQuery>
    {
        public GetDoctorListPagingQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");

        }
    }
}
