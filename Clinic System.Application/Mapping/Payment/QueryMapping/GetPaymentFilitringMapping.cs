namespace Clinic_System.Application.Mapping.Payments
{
    public partial class PaymentProfile
    {
        public void GetPaymentFilitringMapping()
        {
            CreateMap<Payment, PaymentDetailsDTO>()
                // تنسيق التاريخ
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.PaymentDate.HasValue ? src.PaymentDate.Value.ToString("yyyy-MM-dd HH:mm") : "N/A"))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.AdditionalNotes ?? "N/A"))

                .ForMember(dest => dest.PaymentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.AmountPaid))

                // جلب اسم المريض من خلال: Payment -> Appointment -> Patient -> FullName
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Appointment.Patient.FullName))

                // جلب اسم الدكتور من خلال: Payment -> Appointment -> Doctor -> FullName
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Appointment.Doctor.FullName))

                // تحويل الـ Enums لنصوص
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.ToString()))
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus.ToString()));
        }
    }
}
