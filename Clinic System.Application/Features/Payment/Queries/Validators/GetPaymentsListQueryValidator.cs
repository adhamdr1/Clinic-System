namespace Clinic_System.Application.Features.Payment.Queries.Validators
{
    public class GetPaymentsListQueryValidator : AbstractValidator<GetPaymentsListQuery>
    {
        public GetPaymentsListQueryValidator()
        {
            // 1. Pagination Validation
            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("PageNumber must be at least 1.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("PageSize must be at least 1.")
                .LessThanOrEqualTo(100).WithMessage("PageSize cannot exceed 100."); // يفضل وضع حد أقصى عشان الأداء

            // 2. IDs Validation (Only if provided)
            RuleFor(x => x.DoctorId)
                .GreaterThan(0).WithMessage("DoctorId must be greater than 0.")
                .When(x => x.DoctorId.HasValue);

            RuleFor(x => x.PatientId)
                .GreaterThan(0).WithMessage("PatientId must be greater than 0.")
                .When(x => x.PatientId.HasValue);

            // 3. Date Range Logic (أهم قاعدة هنا)
            // لازم "إلى تاريخ" يكون بعد أو يساوي "من تاريخ"
            RuleFor(x => x.ToDate)
                .GreaterThanOrEqualTo(x => x.FromDate.Value)
                .When(x => x.FromDate.HasValue && x.ToDate.HasValue)
                .WithMessage("ToDate must be greater than or equal to FromDate.");

            // 4. Enums Validation
            // التأكد إن القيمة المبعوتة موجودة فعلاً جوه الـ Enum
            RuleFor(x => x.Method)
                .IsInEnum().WithMessage("Invalid Payment Method.")
                .When(x => x.Method.HasValue);

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid Payment Status.")
                .When(x => x.Status.HasValue);
        }
    }
}
