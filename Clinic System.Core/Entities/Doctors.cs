namespace Clinic_System.Core.Entities
{
    public class Doctor : Person
    {
        public virtual string Specialization { get; set; } = null!;
        public virtual string ApplicationUserId { get; set; } = null!;

        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
