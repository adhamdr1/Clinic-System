namespace Clinic_System.Application.Features.Doctors.Commands.Handlers
{
    public class UpdateDoctorCommandHandler : ResponseHandler, IRequestHandler<UpdateDoctorCommand, Response<UpdateDoctorDTO>>
    {
        private readonly IDoctorService doctorService;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public UpdateDoctorCommandHandler(IDoctorService doctorService
            , IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.doctorService = doctorService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Response<UpdateDoctorDTO>> Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
        {

            var doctor = await doctorService.GetDoctorByIdAsync(request.Id);

            if (doctor == null)
            {
                return NotFound<UpdateDoctorDTO>($"Doctor with Id {request.Id} not found");
            }

            mapper.Map(request, doctor);

            await doctorService.UpdateDoctor(doctor, cancellationToken);

            var result = await unitOfWork.SaveAsync();

            if (result == 0)
            {
                return BadRequest<UpdateDoctorDTO>("Failed to update doctor profile in the database.");
            }

            var doctorsMapper = mapper.Map<UpdateDoctorDTO>(doctor);

            return Success<UpdateDoctorDTO>(doctorsMapper, "Doctor updated successfully");
        }
    }
}