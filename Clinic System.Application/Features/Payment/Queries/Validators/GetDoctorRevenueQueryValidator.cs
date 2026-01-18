namespace Clinic_System.Application.Features.Payment.Queries.Validators
{
    public class GetDoctorRevenueQueryValidator : AbstractValidator<GetDoctorRevenueQuery>
    {
        public GetDoctorRevenueQueryValidator()
        {
            // 1. التأكد من أن معرف الطبيب موجود وصحيح
            RuleFor(x => x.DoctorId)
                .NotEmpty().WithMessage("Doctor ID is required.")
                .GreaterThan(0).WithMessage("Doctor ID must be a positive number.");

            // 2. التحقق من منطقية التواريخ في حال تم إرسالها
            // يجب أن يكون تاريخ النهاية بعد تاريخ البداية
            RuleFor(x => x.ToDate)
                .GreaterThanOrEqualTo(x => x.FromDate.Value)
                .When(x => x.FromDate.HasValue && x.ToDate.HasValue)
                .WithMessage("End date (ToDate) cannot be earlier than start date (FromDate).");

            // 3. قاعدة إضافية: منع البحث في تواريخ مستقبلية بعيدة جداً (اختياري)
            RuleFor(x => x.ToDate)
                .LessThanOrEqualTo(DateTime.Now.AddDays(1))
                .When(x => x.ToDate.HasValue)
                .WithMessage("Future dates are not allowed for revenue reports.");
        }
    }
}
