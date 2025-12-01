namespace Clinic_System.Core.Entities
{
    public class Patients : Person
    {
        public virtual string ApplicationUserId { get; set; } = null!;

        public virtual ICollection<Appointments> Appointments { get; set; } = new List<Appointments>();
    }
}
