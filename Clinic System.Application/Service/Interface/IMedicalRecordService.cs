namespace Clinic_System.Application.Service.Interface
{
    public interface IMedicalRecordService
    {
        Task<MedicalRecord> CreateMedicalRecordAsync(Appointment appointment ,string Diagnosis ,
            string Description , List<PrescriptionDto> prescriptionDto, CancellationToken cancellationToken = default);
    }
}
