namespace Clinic_System.Application.Mapping.Appointments
{
    public partial class AppointmentProfile
    {
        public void ConfirmAppointmentMapping()
        {
            CreateMap<Payment, PaymentDTO>()
                .ForMember(dest => dest.PaymentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.AmountPaid, opt => opt.MapFrom(src => src.AmountPaid))
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus.ToString()))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.ToString()))
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src =>
                        src.PaymentDate.HasValue
                        ? src.PaymentDate.Value.ToString("yyyy-MM-dd HH:mm")
                        : null));

            // 2. Mapping for Appointment Entity to ConfirmAppointmentDTO
            CreateMap<Appointment, ConfirmAppointmentDTO>()
                .ForMember(dest => dest.AppointmentId, opt => opt.MapFrom(src => src.Id))
                // جلب اسم الطبيب والمريض من الخصائص الملاحية (Navigation Properties)
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.FullName))
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FullName))
                .ForMember(dest => dest.AppointmentDuration, opt => opt.MapFrom(src => "15 Minutes"))
                // تنسيق التاريخ والوقت
                .ForMember(dest => dest.AppointmentDateTime, opt => opt.MapFrom(src => src.AppointmentDate.ToString("yyyy-MM-dd HH:mm")))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                // ربط كائن الدفع (AutoMapper سيتعرف تلقائياً على PaymentDTO لوجود المابينج بالأعلى)
                .ForMember(dest => dest.Payment, opt => opt.MapFrom(src => src.Payment));
        }
    }
}