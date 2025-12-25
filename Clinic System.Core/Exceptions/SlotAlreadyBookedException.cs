namespace Clinic_System.Core.Exceptions
{
    public class SlotAlreadyBookedException : Exception
    {
        public SlotAlreadyBookedException() { }

        public SlotAlreadyBookedException(string message)
            : base(message) { }

        public SlotAlreadyBookedException(string message, Exception inner)
            : base(message, inner) { }
    }
}
