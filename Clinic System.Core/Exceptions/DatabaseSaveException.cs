namespace Clinic_System.Core.Exceptions
{
    public class DatabaseSaveException : Exception
    {
        public DatabaseSaveException() { }

        public DatabaseSaveException(string message)
            : base(message) { }

        public DatabaseSaveException(string message, Exception inner)
            : base(message, inner) { }
    }
}
