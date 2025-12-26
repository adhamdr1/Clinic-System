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
        Task<Appointment> CancelAppointment(int AppointmentId, int PatientId, CancellationToken cancellationToken = default);
        Task<Appointment> ConfirmAppointment(int AppointmentId, int PatientId, PaymentMethod? method = null
            , decimal? amount = null, CancellationToken cancellationToken = default);
        Task<Appointment> NoShowAppointment(int AppointmentId, int DoctorId, CancellationToken cancellationToken = default);
        Task<Appointment> CompleteAppointment(CompleteAppointmentCommand command, CancellationToken cancellationToken = default);
        Task<(List<Appointment> Items, int TotalCount)> GetAppointmentsByStatusForAdminAsync(AppointmentStatus status,
            int pageNumber,
            int pageSize,
            DateTime? Start = null,
            DateTime? End = null,
            CancellationToken cancellationToken = default);
        //Doctor
        Task<(List<Appointment> Items, int TotalCount)> GetAppointmentsByStatusForDoctorAsync(AppointmentStatus status,
            int doctorId,
            int pageNumber,
            int pageSize,
            DateTime? Start = null,
            DateTime? End = null,
            CancellationToken cancellationToken = default);

        Task<(List<Appointment> Items, int TotalCount)> GetPastAppointmentsForDoctorAsync(int doctorId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);

        Task<(List<Appointment> Items, int TotalCount)> GetPastAppointmentsForPatientAsync(int patientId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);

    }
}
