namespace Clinic_System.Application.Common
{
    public class EmailSettings
    {
        public string Host { get; set; }        // مثلاً smtp.gmail.com
        public int Port { get; set; }           // مثلاً 587
        public string FromEmail { get; set; }   // إيميل العيادة
        public string Password { get; set; }    // App Password من جوجل
        public string SenderName { get; set; }  // اسم العيادة اللي هيظهر للمريض
    }
}
