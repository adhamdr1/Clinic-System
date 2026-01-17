namespace Clinic_System.Core.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IPatientRepository PatientsRepository { get; }
        IDoctorRepository DoctorsRepository { get; }
        IAppointmentRepository AppointmentsRepository { get; }
        IMedicalRecordRepository MedicalRecordsRepository { get; }
        IPaymentRepository PaymentsRepository { get; }
        IPrescriptionRepository PrescriptionsRepository { get; }

        Task<int> SaveAsync();
    }
}
