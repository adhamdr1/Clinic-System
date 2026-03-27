namespace Clinic_System.Infrastructure.MessageBroker
{
    // الكلاس ده بيطبق العقد اللي عملناه في الـ Application
    public class MessagePublisher : IMessagePublisher
    {
        // IPublishEndpoint دي جاية من مكتبة MassTransit
        private readonly IPublishEndpoint _publishEndpoint;

        public MessagePublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
        {
            // هنا بنرمي الورقة (الرسالة) للـ RabbitMQ فعلياً!
            await _publishEndpoint.Publish(message, cancellationToken);
        }
    }
}
