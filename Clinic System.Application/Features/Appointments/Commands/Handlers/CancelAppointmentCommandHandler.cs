namespace Clinic_System.Application.Features.Appointments.Commands.Handlers
{
    public class CancelAppointmentCommandHandler : ResponseHandler, IRequestHandler<CancelAppointmentCommand, Response<CaneclledAppointmentDTO>>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<CancelAppointmentCommandHandler> logger;
        public CancelAppointmentCommandHandler(
            IAppointmentService appointmentService,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<CancelAppointmentCommandHandler> logger)
        {
            this.appointmentService = appointmentService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<Response<CaneclledAppointmentDTO>> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting appointment rescheduling for PatientId: {PatientId}", request.PatientId);

            Appointment CancelAppointment = null;
            try
            {
                CancelAppointment = await appointmentService.CancelAppointmentAsync(request, cancellationToken);

                var CaneclledAppointmentDTO = mapper.Map<CaneclledAppointmentDTO>(CancelAppointment);

                var patient = await unitOfWork.PatientsRepository.GetByIdAsync(request.PatientId, cancellationToken);

                if (patient == null)
                {
                    logger.LogWarning("Patient with ID {PatientId} not found.", request.PatientId);
                    return BadRequest<CaneclledAppointmentDTO>("Patient account not found.");
                }

                CaneclledAppointmentDTO.PatientName = patient.FullName;

                var doctor = await unitOfWork.DoctorsRepository.GetByIdAsync(CancelAppointment.DoctorId, cancellationToken);

                if (doctor != null)
                {
                    CaneclledAppointmentDTO.DoctorName = doctor.FullName;
                    logger.LogInformation("Doctor found: {DoctorName} for DoctorId: {DoctorId}", doctor.FullName, CancelAppointment.DoctorId);
                }

                logger.LogInformation("Appointment Cancelled successfully for PatientId: {PatientId}, DoctorId: {DoctorId}", CancelAppointment.PatientId, CancelAppointment.DoctorId);

                return Success(CaneclledAppointmentDTO, "Appointment Cancelled successfully.");

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while Cancel appointment for PatientId: {PatientId}, DoctorId: {DoctorId}", CancelAppointment?.PatientId, CancelAppointment?.DoctorId);
                return BadRequest<CaneclledAppointmentDTO>("Error occurred while processing Cancelling: " + ex.Message);
            }
        }
    }
}