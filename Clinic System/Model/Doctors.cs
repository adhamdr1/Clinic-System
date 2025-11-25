namespace Clinic_System.Model
{
    public class Doctors : Person
    {
        public virtual string Specialization { get; set; }
        public virtual string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<Appointments> Appointments { get; set; }
    }
}
