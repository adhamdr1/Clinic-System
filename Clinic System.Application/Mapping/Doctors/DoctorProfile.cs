namespace Clinic_System.Application.Mapping.Doctors
{
    public partial class DoctorProfile : Profile
    {
        public DoctorProfile()
        {
            GetDoctorListMapping();
            GetDoctorWithAppointmentsByIdMapping();
            GetDoctorByIdMapping();
            GetDoctorBasicInfoMapping();
            CreateDoctorMapping();
            UpdateDoctorMapping();
        }
    }
}
