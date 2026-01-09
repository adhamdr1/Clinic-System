namespace Clinic_System.Infrastructure.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        // استخدام IOptions هو الطريقة الاحترافية في الـ .NET
        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();

            // اسم المرسل وإيميله
            message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.FromEmail));

            // إيميل المستلم
            message.To.Add(MailboxAddress.Parse(to));

            message.Subject = subject;

            var bodybuilder = new BodyBuilder
            {
                HtmlBody = body, // هنا بيتحط الـ Template اللي فيه الألوان والجداول (HTML)
                TextBody = body  // نسخة احتياطية سادة في حال تعذر عرض الـ HTML
            };

            message.Body = bodybuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    // SecureSocketOptions.StartTls هو الأنسب للبورت 587
                    await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);

                    await client.AuthenticateAsync(_emailSettings.FromEmail, _emailSettings.Password);

                    await client.SendAsync(message);
                }
                catch (Exception ex)
                {
                    // هنا لازم تعمل Log للخطأ عشان لو الإيميل مفشلش تعرف السبب (باسورد غلط مثلاً)
                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                }
            }
        }
    }
}