namespace Clinic_System.Application.Features.MedicalRecords.Queries.Validators
{
    public class GetRecordsByDoctorIdQueryValidator : AbstractValidator<GetRecordsByDoctorIdQuery>
    {
        public GetRecordsByDoctorIdQueryValidator()
        {
            RuleFor(x => x.DoctorId)
                .GreaterThanOrEqualTo(1).WithMessage("Doctor Id must be at least 1.");

            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");

            RuleFor(x => x.StartDate)
                .LessThanOrEqualTo(x => x.EndDate).When(x => x.StartDate.HasValue && x.EndDate.HasValue)
                .WithMessage("Start date must be less than or equal to end date.");
        }
    }
}
