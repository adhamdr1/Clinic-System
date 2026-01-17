namespace Clinic_System.Application.Mapping.Patients
{
    public partial class PatientProfile : Profile
    {
        public PatientProfile()
        {
            GetPatientListMapping();
            GetPatientWithAppointmentsByIdMapping();
            GetPatientByIdMapping();
            CreatePatientMapping();
            UpdatePatientMapping();
            UpdateIdentityPatientMapping();
        }
    }
}
