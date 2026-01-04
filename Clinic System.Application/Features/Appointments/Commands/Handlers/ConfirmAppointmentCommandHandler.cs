namespace Clinic_System.Application.Features.Appointments.Commands.Handlers
{
    public class ConfirmAppointmentCommandHandler : ResponseHandler, IRequestHandler<ConfirmAppointmentCommand, Response<ConfirmAppointmentDTO>>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<ConfirmAppointmentCommandHandler> logger;
        public ConfirmAppointmentCommandHandler(
            IAppointmentService appointmentService,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<ConfirmAppointmentCommandHandler> logger)
        {
            this.appointmentService = appointmentService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<Response<ConfirmAppointmentDTO>> Handle(ConfirmAppointmentCommand request, CancellationToken cancellationToken)
        {
            Appointment ConfirmAppointment = null;
            try
            {
                ConfirmAppointment = await appointmentService.ConfirmAppointmentAsync(request.AppointmentId , 
                    request.PatientId ,request.method,request.Notes ,request.amount ,cancellationToken);

                logger.LogInformation("Appointment Confirmd successfully for PatientId: {PatientId}, DoctorId: {DoctorId}", ConfirmAppointment.PatientId, ConfirmAppointment.DoctorId);

                var ConfirmAppointmentDTO = mapper.Map<ConfirmAppointmentDTO>(ConfirmAppointment);

                logger.LogInformation("Appointment Confirmed successfully for PatientId: {PatientId}, DoctorId: {DoctorId}", ConfirmAppointment.PatientId, ConfirmAppointment.DoctorId);

                return Success(ConfirmAppointmentDTO, "Appointment Confirmed successfully.");

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while Confirm appointment for PatientId: {PatientId}, DoctorId: {DoctorId}", ConfirmAppointment?.PatientId, ConfirmAppointment?.DoctorId);
                return BadRequest<ConfirmAppointmentDTO>("Error occurred while processing Completing: " + ex.Message);
            }
        }
    }
}