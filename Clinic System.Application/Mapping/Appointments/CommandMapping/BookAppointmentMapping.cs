namespace Clinic_System.Application.Mapping.Appointments
{
    public partial class AppointmentProfile
    {
        public void BookAppointmentMapping()
        {
            // قاعدة المابينج من كيان Appointment إلى DTO النهائي
            CreateMap<Appointment, CreateAppointmentDTO>()
                .ForMember(dest => dest.AppointmentDateTime, opt => opt.MapFrom(src => src.AppointmentDate.ToString("dd/MM/yyyy-HH:mm")))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))

                // تعيين القيمة الثابتة (Duration)
                .ForMember(dest => dest.AppointmentDuration, opt => opt.MapFrom(src => "15 Minutes"))

                // تجاهل الأسماء (سيتم ملؤها يدوياً في الـ Handler)
                .ForMember(dest => dest.DoctorName, opt => opt.Ignore())
                .ForMember(dest => dest.PatientName, opt => opt.Ignore());

            // هذا Mapping غير مستخدم في الـ Handler، ولكنه مطلوب إذا كان الـ Service يقبل كيان Appointment كمدخل
            CreateMap<BookAppointmentCommand, Appointment>()
                // دمج التاريخ والوقت لتكوين حقل AppointmentDate في الكيان
                .ForMember(dest => dest.AppointmentDate, opt => opt.MapFrom(src => src.AppointmentDate.Date.Add(src.AppointmentTime)))
                // تعيين الحالة الأولية
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AppointmentStatus.Pending));

            // تأكد من تعيين ID إذا كان المطلوب
        }
    }
}
