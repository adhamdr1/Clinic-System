namespace Clinic_System.Application.Service.Implemention
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly IUnitOfWork unitOfWork;

        public MedicalRecordService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<MedicalRecord> CreateMedicalRecordAsync(Appointment appointment, string Diagnosis,
            string Description, List<PrescriptionDto> prescriptionDto, string? AdditionalNotes = null, CancellationToken cancellationToken = default)
        {
            var record = new MedicalRecord
            {
                Appointment = appointment,
                Diagnosis = Diagnosis,
                DescriptionOfTheVisit = Description,
                AdditionalNotes = AdditionalNotes,
                Prescriptions = prescriptionDto
                    .Select(dto => new Prescription
                    {
                        Dosage = dto.Dosage,
                        MedicationName = dto.MedicationName,
                        SpecialInstructions = dto.SpecialInstructions,
                        Frequency = dto.Frequency,
                        StartDate = dto.StartDate,
                        EndDate = dto.EndDate
                    })
                    .ToList()
            };

            await unitOfWork.MedicalRecordsRepository.AddAsync(record, cancellationToken);

            return record;
        }

        public async Task<PagedResult<MedicalRecord>> GetPatientHistoryAsync(int pageNumber, int pageSize, int patientId, CancellationToken cancellationToken = default)
        {
            var (item , total) = await unitOfWork.MedicalRecordsRepository
                .GetPatientMedicalHistoryAsync(patientId, pageNumber,pageSize, cancellationToken);

            return new PagedResult<MedicalRecord>(item, total, pageNumber, pageSize);
        }

        public async Task<MedicalRecord> GetRecordByIdAsync(int recordId, CancellationToken cancellationToken = default)
        {
            var record = await unitOfWork.MedicalRecordsRepository.GetMedicalRecordDetailsAsync(recordId, cancellationToken);
            if (record == null)
            {
                throw new NotFoundException($"Medical record with ID {recordId} not found.");
            }
            return record;
        }

        public async Task<PagedResult<MedicalRecord>> GetRecordsByDateRangeAsync(DateTime start, DateTime end, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var (item, total) = await unitOfWork.MedicalRecordsRepository
                .GetRecordsByDateRangeAsync(start, end, pageNumber, pageSize, cancellationToken);
            return new PagedResult<MedicalRecord>(item, total, pageNumber, pageSize);
        }

        public async Task<PagedResult<MedicalRecord>> GetRecordsByDoctorIdAsync(int pageNumber, int pageSize, int patientId, CancellationToken cancellationToken = default)
        {
            var (item, total) = await unitOfWork.MedicalRecordsRepository
                .GetRecordsByDoctorAsync(patientId, pageNumber, pageSize, cancellationToken);
            return new PagedResult<MedicalRecord>(item, total, pageNumber, pageSize);
        }

        public async Task<MedicalRecord> UpdateAsync(int recordId,
        string? diagnosis,string? description,string? notes,CancellationToken cancellationToken = default)
        {
            var record = await unitOfWork.MedicalRecordsRepository.GetMedicalRecordForUpdateAsync(recordId, cancellationToken);

            if (record == null)
                throw new NotFoundException("Medical record not found");

            record.Update(diagnosis, description, notes);

            var result = await unitOfWork.SaveAsync();

            if (result <= 0)
                throw new DatabaseSaveException("Failed to update medical record");

            return record;
        }
    }
}
