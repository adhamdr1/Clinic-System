namespace Clinic_System.Infrastructure.MessageBroker.Consumers
{
    public class PasswordResetRequestedEventConsumer : IConsumer<PasswordResetRequestedEvent>
    {
        private readonly IIdentityNotificationService _notificationService;

        public PasswordResetRequestedEventConsumer(IIdentityNotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Consume(ConsumeContext<PasswordResetRequestedEvent> context)
        {
            var data = context.Message;

            // إرسال إيميل إعادة تعيين كلمة المرور
            await _notificationService.SendResetPasswordAsync(data.Email, data.ResetLink);
        }
    }
}
