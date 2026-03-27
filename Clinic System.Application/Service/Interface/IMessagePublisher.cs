namespace Clinic_System.Application.Service.Interface
{
    public interface IMessagePublisher
    {
        // الدالة دي بتاخد أي نوع من أنواع الـ Events (عشان تبقى Generic)
        Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class;
    }
}
