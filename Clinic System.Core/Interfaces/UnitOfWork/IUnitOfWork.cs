namespace Clinic_System.Core.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IPatientRepository PatientsRepository { get; }
        IDoctorRepository DoctorsRepository { get; }
        IAppointmentsRepository AppointmentsRepository { get; }
        IMedicalRecordRepository MedicalRecordsRepository { get; }
        IPaymentRepository PaymentsRepository { get; }

        Task<int> SaveAsync();
    }
}
