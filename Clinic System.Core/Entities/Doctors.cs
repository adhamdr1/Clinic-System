namespace Clinic_System.Core.Entities
{
    public class Doctors : Person
    {
        public virtual string Specialization { get; set; } = null!;
        public virtual string ApplicationUserId { get; set; } = null!;

        public virtual ICollection<Appointments> Appointments { get; set; } = new List<Appointments>();
    }
}
