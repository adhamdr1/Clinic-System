namespace Clinic_System.Application.Service.Interface
{
    public interface IAppointmentService
    {
        Task<List<Appointment>> GetBookedAppointmentsAsync(int doctorId, DateTime date, CancellationToken cancellationToken = default);
        Task<List<TimeSpan>> GetAvailableSlotsAsync(int doctorId, DateTime date, CancellationToken cancellationToken = default);
        Task<Appointment> BookAppointmentAsync(BookAppointmentCommand command,CancellationToken cancellationToken = default);
        Task<Appointment> RescheduleAppointmentAsync(RescheduleAppointmentCommand command, CancellationToken cancellationToken = default);
        Task<Appointment> CancelAppointment(int AppointmentId, int PatientId, CancellationToken cancellationToken = default);
        Task<Appointment> ConfirmAppointment(int AppointmentId, int PatientId, PaymentMethod? method = null
            , decimal? amount = null, CancellationToken cancellationToken = default);
        Task<Appointment> NoShowAppointment(int AppointmentId, int DoctorId, CancellationToken cancellationToken = default);
        Task<Appointment> CompleteAppointment(CompleteAppointmentCommand command, CancellationToken cancellationToken = default);
    }
}
