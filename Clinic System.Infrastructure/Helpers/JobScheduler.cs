namespace Clinic_System.Infrastructure.Helpers
{
    public static class JobScheduler
    {
        public static void ScheduleRecurringJobs(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

            recurringJobManager.AddOrUpdate<IAppointmentService>(
                "cancel-overdue-appointments",
                service => service.CancelOverdueAppointmentsAsync(),
                Cron.Hourly
            );
        }
    }

}
