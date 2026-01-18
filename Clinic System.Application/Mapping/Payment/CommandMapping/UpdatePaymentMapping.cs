namespace Clinic_System.Application.Mapping.Payments
{
    public partial class PaymentProfile
    {
        public void UpdatePaymentMapping()
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
        }
    }
}
