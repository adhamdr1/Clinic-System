namespace Clinic_System.Model
{
    public class Patients : Person
    {
        public virtual string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<Appointments> Appointments { get; set; }
    }
}
