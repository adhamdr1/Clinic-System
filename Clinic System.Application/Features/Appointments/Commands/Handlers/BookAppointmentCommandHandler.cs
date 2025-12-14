namespace Clinic_System.Application.Features.Appointments.Commands.Handlers
{
    public class BookAppointmentCommandHandler : ResponseHandler, IRequestHandler<BookAppointmentCommand, Response<CreateAppointmentDTO>>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        public BookAppointmentCommandHandler(
            IAppointmentService appointmentService,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            this.appointmentService = appointmentService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Response<CreateAppointmentDTO>> Handle(BookAppointmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var newAppointment = await appointmentService.BookAppointmentAsync(request, cancellationToken);

                var appointmentDto = mapper.Map<CreateAppointmentDTO>(newAppointment);

                var doctor = await unitOfWork.DoctorsRepository.GetByIdAsync(request.DoctorId, cancellationToken);
                var patient = await unitOfWork.PatientsRepository.GetByIdAsync(request.PatientId, cancellationToken);

                if (patient != null)
                {
                    appointmentDto.PatientName = patient.FullName;
                }
                if (doctor != null)
                {
                    appointmentDto.DoctorName = doctor.FullName;
                }

                return Success(appointmentDto, "Appointment booked successfully.");
            }
            catch (Exception ex) when (ex.Message.Contains("not available"))
            {
                return BadRequest<CreateAppointmentDTO>(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest<CreateAppointmentDTO>("Error occurred while processing booking: " + ex.Message);
            }
        }
    }
}