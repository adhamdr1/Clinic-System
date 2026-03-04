namespace Clinic_System.Application.Features.Appointments.Commands.Handlers
{
    public class CancelAppointmentCommandHandler : AppRequestHandler<CancelAppointmentCommand, CaneclledAndNoShowAppointmentDTO>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IMapper mapper;
        private readonly ICacheService cacheService;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<CancelAppointmentCommandHandler> logger;
        public CancelAppointmentCommandHandler(
            IAppointmentService appointmentService,
            IMapper mapper,
            ICacheService cacheService,
            IUnitOfWork unitOfWork,
           ILogger<CancelAppointmentCommandHandler> logger,
            ICurrentUserService currentUserService) : base(currentUserService)
        {
            this.appointmentService = appointmentService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.cacheService = cacheService;
            this.logger = logger;
        }

        public override async Task<Response<CaneclledAndNoShowAppointmentDTO>> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting appointment rescheduling for PatientId: {PatientId}", request.PatientId);

            var appointment = await unitOfWork.AppointmentsRepository.GetByIdAsync(request.AppointmentId);

            if (appointment == null)
            {
                return NotFound<CaneclledAndNoShowAppointmentDTO>("Appointment not found.");
            }

            var authResult = await ValidatePatientAccess(appointment.PatientId);
            if (authResult != null)
                return authResult;

            request.PatientId = appointment.PatientId;

            Appointment CancelAppointment = null;
            try
            {
                CancelAppointment = await appointmentService.CancelAppointmentAsync(request, cancellationToken);

                var CaneclledAndNoShowAppointmentDTO = mapper.Map<CaneclledAndNoShowAppointmentDTO>(CancelAppointment);

                logger.LogInformation("Appointment Cancelled successfully for PatientId: {PatientId}, DoctorId: {DoctorId}", CancelAppointment.PatientId, CancelAppointment.DoctorId);

                await cacheService.RemoveByPrefixAsync(
                    $"UpcomingAppts_Patient_{CancelAppointment.PatientId}",
                    $"PastAppts_Patient_{CancelAppointment.PatientId}",      // مسحنا الماضي كمان لأنه ممكن يظهر فيه كـ ملغي
                    $"UpcomingAppts_Doctor_{CancelAppointment.DoctorId}",
                    $"PastAppts_Doctor_{CancelAppointment.DoctorId}",        // مسحنا الماضي
                    $"DoctorApptsByStatus_{CancelAppointment.DoctorId}",
                    "AdminApptsByStatus",
                    "AdminStats"
                );

                return Success(CaneclledAndNoShowAppointmentDTO, "Appointment Cancelled successfully.");

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while Cancel appointment for PatientId: {PatientId}, DoctorId: {DoctorId}", CancelAppointment?.PatientId, CancelAppointment?.DoctorId);
                return BadRequest<CaneclledAndNoShowAppointmentDTO>("Error occurred while processing Cancelling: " + ex.Message);
            }
        }
    }
}