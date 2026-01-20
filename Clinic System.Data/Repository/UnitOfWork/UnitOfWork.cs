using Clinic_System.Core.Interfaces;

namespace Clinic_System.Data.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly AppDbContext context;

        public UnitOfWork(AppDbContext context)
        {
            this.context = context;
        }
        IPatientRepository PatientsRepo;

        IDoctorRepository DoctorsRepo;

        IAppointmentRepository AppointmentsRepo;

        IMedicalRecordRepository MedicalRecordsRepo;

        IPaymentRepository PaymentsRepo;

        IPrescriptionRepository PrescriptionsRepo;

        IRefreshTokenRepository RefreshTokensRepo;

        public IRefreshTokenRepository RefreshTokensRepository
        {
            get
            {
                if (RefreshTokensRepo == null)
                {
                    RefreshTokensRepo = new RefreshTokenRepository(context);
                }
                return RefreshTokensRepo;
            }
        }

        public IPrescriptionRepository PrescriptionsRepository
        {
            get
            {
                if (PrescriptionsRepo == null)
                {
                    PrescriptionsRepo = new PrescriptionRepository(context);
                }
                return PrescriptionsRepo;
            }
        }

        public IPatientRepository PatientsRepository
        {
            get
            {
                if (PatientsRepo == null)
                {
                    PatientsRepo = new PatientRepository(context);
                }
                return PatientsRepo;
            }
        }

        public IDoctorRepository DoctorsRepository
        {
            get
            {
                if (DoctorsRepo == null)
                {
                    DoctorsRepo = new DoctorRepository(context);
                }
                return DoctorsRepo;
            }
        }

        public IAppointmentRepository AppointmentsRepository
        {
            get
            {
                if (AppointmentsRepo == null)
                {
                    AppointmentsRepo = new AppointmentRepository(context);
                }
                return AppointmentsRepo;
            }
        }

        public IMedicalRecordRepository MedicalRecordsRepository
        {
            get
            {
                if (MedicalRecordsRepo == null)
                {
                    MedicalRecordsRepo = new MedicalRecordRepository(context);
                }
                return MedicalRecordsRepo;
            }
        }

        public IPaymentRepository PaymentsRepository
        {
            get
            {
                if (PaymentsRepo == null)
                {
                    PaymentsRepo = new PaymentRepository(context);
                }
                return PaymentsRepo;
            }
        }

        public void Dispose() => context.Dispose();

        public Task<int> SaveAsync() => context.SaveChangesAsync();

    }
}
