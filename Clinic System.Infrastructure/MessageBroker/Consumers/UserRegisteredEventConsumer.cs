namespace Clinic_System.Infrastructure.MessageBroker.Consumers
{
    public class UserRegisteredEventConsumer : IConsumer<UserRegisteredEvent>
    {
        private readonly IIdentityNotificationService _notificationService;

        public UserRegisteredEventConsumer(IIdentityNotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
        {
            var data = context.Message;

            // بننقل كل البيانات من الـ Event للخدمة اللي بتبعت الإيميل
            await _notificationService.SendEmailConfirmationAsync(
                data.UserId,
                data.FullName,
                data.UserName,
                data.Email,
                data.ConfirmationLink,
                data.UserRole,
                data.Specialty
            );
        }
    }
}
