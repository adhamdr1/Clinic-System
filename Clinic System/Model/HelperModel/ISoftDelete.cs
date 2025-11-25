namespace Clinic_System.Model.HelperModel
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
        DateTime? DeletedAt { get; set; }
    }
}
