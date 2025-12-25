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
            string Description, List<PrescriptionDto> prescriptionDto, CancellationToken cancellationToken = default)
        {
            var record = new MedicalRecord
            {
                Appointment = appointment,
                Diagnosis = Diagnosis,
                DescriptionOfTheVisit = Description,
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
    }
}
