namespace Clinic_System.Core.Entities
{
    public class Patient : Person
    {
        public virtual string ApplicationUserId { get; set; } = null!;

        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
