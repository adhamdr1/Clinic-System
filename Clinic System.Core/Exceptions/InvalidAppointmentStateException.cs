namespace Clinic_System.Core.Exceptions
{
    public class InvalidAppointmentStateException : Exception
    {
        public InvalidAppointmentStateException() { }

        public InvalidAppointmentStateException(string message)
            : base(message) { }

        public InvalidAppointmentStateException(string message, Exception inner)
            : base(message, inner) { }
    }
}
