namespace Clinic_System.Model.HelperModel
{
    /// <summary>
    /// Interface for entities that need automatic CreatedAt and UpdatedAt tracking
    /// </summary>
    public interface IAuditable
    {
        DateTime CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
    }
}


