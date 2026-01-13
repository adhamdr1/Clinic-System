namespace Clinic_System.Application.Mapping.Doctors
{
    public partial class MedicalRecordProfile : Profile
    {
        public MedicalRecordProfile()
        {
            GetDoctorListMapping();
            GetDoctorWithAppointmentsByIdMapping();
            GetDoctorByIdMapping();
            CreateDoctorMapping();
            UpdateDoctorMapping();
            UpdateIdentityDoctorMapping();
        }
    }
}
