namespace Clinic_System.Application.Common
{
    public interface IBackgroundJobService
    {
        void Enqueue(Expression<Func<Task>> job);
    }
}
