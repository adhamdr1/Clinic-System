namespace Clinic_System.Application.Features.Appointments.Commands.Handlers
{
    public class CompleteAppointmentCommandHandler : ResponseHandler, IRequestHandler<CompleteAppointmentCommand, Response<CompleteAppointmentDTO>>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<CompleteAppointmentCommandHandler> logger;
        public CompleteAppointmentCommandHandler(
            IAppointmentService appointmentService,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<CompleteAppointmentCommandHandler> logger)
        {
            this.appointmentService = appointmentService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<Response<CompleteAppointmentDTO>> Handle(CompleteAppointmentCommand request, CancellationToken cancellationToken)
        {
            Appointment CompleteAppointment = null;
            try
            {
                CompleteAppointment = await appointmentService.CompleteAppointmentAsync(request, cancellationToken);

                logger.LogInformation("Appointment completed successfully for PatientId: {PatientId}, DoctorId: {DoctorId}", CompleteAppointment.PatientId, CompleteAppointment.DoctorId);

                var CompleteAppointmentDTO = mapper.Map<CompleteAppointmentDTO>(CompleteAppointment);

                logger.LogInformation("Appointment Completeed successfully for PatientId: {PatientId}, DoctorId: {DoctorId}", CompleteAppointment.PatientId, CompleteAppointment.DoctorId);

                return Success(CompleteAppointmentDTO, "Appointment Completeed successfully.");

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while Complete appointment for PatientId: {PatientId}, DoctorId: {DoctorId}", CompleteAppointment?.PatientId, CompleteAppointment?.DoctorId);
                return BadRequest<CompleteAppointmentDTO>("Error occurred while processing Completing: " + ex.Message);
            }
        }
    }
}