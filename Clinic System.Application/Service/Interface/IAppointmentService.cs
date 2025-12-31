namespace Clinic_System.Application.Service.Interface
{
    public interface IAppointmentService
    {
        Task<PagedResult<Appointment>> GetDoctorAppointmentsAsync(GetDoctorAppointmentsQuery doctorAppointmentsQuery,
            CancellationToken cancellationToken = default);
        Task<PagedResult<Appointment>> GetPatientAppointmentsAsync(GetPatientAppointmentsQuery patientAppointmentsQuery,
            CancellationToken cancellationToken = default);
        Task<List<Appointment>> GetBookedAppointmentsAsync(int doctorId, DateTime date, CancellationToken cancellationToken = default);
        Task<List<TimeSpan>> GetAvailableSlotsAsync(int doctorId, DateTime date, CancellationToken cancellationToken = default);
        Task<Appointment> BookAppointmentAsync(BookAppointmentCommand command,CancellationToken cancellationToken = default);
        Task<Appointment> RescheduleAppointmentAsync(RescheduleAppointmentCommand command, CancellationToken cancellationToken = default);
        Task<Appointment> CancelAppointmentAsync(CancelAppointmentCommand command, CancellationToken cancellationToken = default);
        Task<Appointment> ConfirmAppointmentAsync(int AppointmentId, int PatientId, PaymentMethod? method = null
            , decimal? amount = null, CancellationToken cancellationToken = default);
        Task<Appointment> NoShowAppointmentAsync(NoShowAppointmentCommand command, CancellationToken cancellationToken = default);
        Task<Appointment> CompleteAppointmentAsync(CompleteAppointmentCommand command, CancellationToken cancellationToken = default);
        Task<PagedResult<Appointment>> GetAppointmentsByStatusForAdminAsync(GetAppointmentsByStatusForAdminQuery appointmentsByStatusForAdminQuery,
            CancellationToken cancellationToken = default);
        Task<PagedResult<Appointment>> GetAppointmentsByStatusForDoctorAsync(GetAppointmentsByStatusForDoctorQuery appointmentsByStatusForDoctorQuery,
            CancellationToken cancellationToken = default);

        Task<PagedResult<Appointment>> GetPastAppointmentsForDoctorAsync(GetPastAppointmentsForDoctorQuery appointmentsForDoctorQuery,
            CancellationToken cancellationToken = default);

        Task<PagedResult<Appointment>> GetPastAppointmentsForPatientAsync(GetPastAppointmentsForPatientQuery appointmentsForPatientQuery,
            CancellationToken cancellationToken = default);

    }
}
