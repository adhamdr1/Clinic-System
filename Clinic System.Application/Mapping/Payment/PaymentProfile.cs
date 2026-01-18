namespace Clinic_System.Application.Mapping.Payments
{
    public partial class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            GetPaymentFilitringMapping();
            UpdatePaymentMapping();
        }
    }
}
