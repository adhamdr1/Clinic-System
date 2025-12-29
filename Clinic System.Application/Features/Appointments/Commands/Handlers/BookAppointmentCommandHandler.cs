namespace Clinic_System.Application.Features.Appointments.Commands.Handlers
{
    public class BookAppointmentCommandHandler : ResponseHandler, IRequestHandler<BookAppointmentCommand, Response<AppointmentDTO>>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<BookAppointmentCommandHandler> logger;
        public BookAppointmentCommandHandler(
            IAppointmentService appointmentService,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<BookAppointmentCommandHandler> logger)
        {
            this.appointmentService = appointmentService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<Response<AppointmentDTO>> Handle(BookAppointmentCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling BookAppointmentCommand for PatientId: {PatientId}, DoctorId: {DoctorId}", request.PatientId, request.DoctorId);
            try
            {
                var patient = await unitOfWork.PatientsRepository.GetByIdAsync(request.PatientId, cancellationToken);

                if (patient == null)
                {
                    logger.LogWarning("Patient with ID {PatientId} not found.", request.PatientId);
                    return BadRequest<AppointmentDTO>("Patient account not found.");
                }

                var newAppointment = await appointmentService.BookAppointmentAsync(request, cancellationToken);

                var appointmentDto = mapper.Map<AppointmentDTO>(newAppointment);

                appointmentDto.PatientName = patient.FullName;

                var doctor = await unitOfWork.DoctorsRepository.GetByIdAsync(request.DoctorId, cancellationToken);
                
                if (doctor != null)
                {
                    appointmentDto.DoctorName = doctor.FullName;
                    logger.LogInformation("Doctor found: {DoctorName} for DoctorId: {DoctorId}", doctor.FullName, request.DoctorId);
                }

                logger.LogInformation("Appointment booked successfully for PatientId: {PatientId}, DoctorId: {DoctorId}", request.PatientId, request.DoctorId);

                return Success(appointmentDto, "Appointment booked successfully.");
            }
            catch (SlotAlreadyBookedException ex) when (ex.Message.Contains("not available"))
            {
                logger.LogWarning("Booking failed: {ErrorMessage}", ex.Message);
                return BadRequest<AppointmentDTO>(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while booking appointment for PatientId: {PatientId}, DoctorId: {DoctorId}", request.PatientId, request.DoctorId);
                return BadRequest<AppointmentDTO>("Error occurred while processing booking: " + ex.Message);
            }
        }
    }
}