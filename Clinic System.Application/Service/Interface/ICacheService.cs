namespace Clinic_System.Application.Service.Interface
{
    public interface ICacheService
    {
        // بنجيب داتا من الكاش
        Task<T?> GetDataAsync<T>(string key);

        // بنحفظ داتا في الكاش، ونديها وقت وتموت (TTL)
        Task<bool> SetDataAsync<T>(string key, T value, TimeSpan expirationTime);

        // بنمسح الداتا من الكاش (لو حصل Update في الداتابيز)
        Task<bool> RemoveDataAsync(string key);
    }
}
