namespace Clinic_System.Application.Features.Appointments.Commands.Handlers
{
    public class ConfirmAppointmentCommandHandler : AppRequestHandler<ConfirmAppointmentCommand, ConfirmAppointmentDTO>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<ConfirmAppointmentCommandHandler> logger;
        public ConfirmAppointmentCommandHandler(
            ICurrentUserService currentUserService,
            IAppointmentService appointmentService,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<ConfirmAppointmentCommandHandler> logger) : base(currentUserService)
        {
            this.appointmentService = appointmentService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public override async Task<Response<ConfirmAppointmentDTO>> Handle(ConfirmAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await unitOfWork.AppointmentsRepository.GetByIdAsync(request.AppointmentId);

            if (appointment == null)
            {
                return NotFound<ConfirmAppointmentDTO>("Appointment not found.");
            }

            var authResult = await ValidatePatientAccess(appointment.PatientId);
            if (authResult != null)
                return authResult;

            request.PatientId = appointment.PatientId;


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