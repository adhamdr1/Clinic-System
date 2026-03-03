namespace Clinic_System.Infrastructure.Services
{
    public class HangfireBackgroundJobService : IBackgroundJobService
    {
        private readonly IBackgroundJobClient _backgroundJobClient;

        public HangfireBackgroundJobService(IBackgroundJobClient backgroundJobClient)
        {
            _backgroundJobClient = backgroundJobClient;
        }

        public void Enqueue(Expression<Func<Task>> job)
        {
            _backgroundJobClient.Enqueue(job);
        }
    }
}
