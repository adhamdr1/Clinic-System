namespace Clinic_System.Application.Features.Appointments.Commands.Handlers
{
    public class RescheduleAppointmentCommandHandler : ResponseHandler, IRequestHandler<RescheduleAppointmentCommand, Response<AppointmentDTO>>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<RescheduleAppointmentCommandHandler> logger;
        public RescheduleAppointmentCommandHandler(
            IAppointmentService appointmentService,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<RescheduleAppointmentCommandHandler> logger)
        {
            this.appointmentService = appointmentService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<Response<AppointmentDTO>> Handle(RescheduleAppointmentCommand request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Starting appointment rescheduling for PatientId: {PatientId}", request.PatientId);

            Appointment newAppointment=null;

            try
            {
                newAppointment = await appointmentService.RescheduleAppointmentAsync(request, cancellationToken);

                var appointmentDto = mapper.Map<AppointmentDTO>(newAppointment);

                var patient = await unitOfWork.PatientsRepository.GetByIdAsync(request.PatientId, cancellationToken);

                if (patient == null)
                {
                    logger.LogWarning("Patient with ID {PatientId} not found.", newAppointment.PatientId);
                    return BadRequest<AppointmentDTO>("Patient account not found.");
                }

                appointmentDto.PatientName = patient.FullName;

                var doctor = await unitOfWork.DoctorsRepository.GetByIdAsync(newAppointment.DoctorId, cancellationToken);

                if (doctor != null)
                {
                    appointmentDto.DoctorName = doctor.FullName;
                    logger.LogInformation("Doctor found: {DoctorName} for DoctorId: {DoctorId}", doctor.FullName, newAppointment.DoctorId);
                }

                logger.LogInformation("Appointment rescheduled successfully for PatientId: {PatientId}, DoctorId: {DoctorId}", newAppointment.PatientId, newAppointment.DoctorId);

                return Success(appointmentDto, "Appointment rescheduled successfully.");

            }
            catch (SlotAlreadyBookedException ex) when (ex.Message.Contains("not available"))
            {
                logger.LogWarning("Reschedule failed: {ErrorMessage}", ex.Message);
                return BadRequest<AppointmentDTO>(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while reschedule appointment for PatientId: {PatientId}, DoctorId: {DoctorId}", newAppointment?.PatientId, newAppointment?.DoctorId);
                return BadRequest<AppointmentDTO>("Error occurred while processing rescheduling: " + ex.Message);
            }
        }
    }
}