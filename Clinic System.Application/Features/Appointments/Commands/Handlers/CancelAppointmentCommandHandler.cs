namespace Clinic_System.Application.Features.Appointments.Commands.Handlers
{
    public class CancelAppointmentCommandHandler : AppRequestHandler<CancelAppointmentCommand, CaneclledAndNoShowAppointmentDTO>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IMapper mapper;
        private readonly IPatientService patientService;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<CancelAppointmentCommandHandler> logger;
       
        public CancelAppointmentCommandHandler(
            IAppointmentService appointmentService,
            IMapper mapper,
            IPatientService patientService,
            IUnitOfWork unitOfWork,
            ILogger<CancelAppointmentCommandHandler> logger,
            ICurrentUserService currentUserService) : base(currentUserService)
        {
            this.appointmentService = appointmentService;
            this.mapper = mapper;
            this.patientService = patientService;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public override async Task<Response<CaneclledAndNoShowAppointmentDTO>> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
        {
            var patient = await patientService.GetPatientByUserIdAsync(CurrentUserId);
            if (patient == null) return Unauthorized<CaneclledAndNoShowAppointmentDTO>("Access denied.");

            // 2. املأ الـ ID
            request.PatientId = patient.Id;

            logger.LogInformation("Starting appointment rescheduling for PatientId: {PatientId}", request.PatientId);

            Appointment CancelAppointment = null;
            try
            {
                CancelAppointment = await appointmentService.CancelAppointmentAsync(request, cancellationToken);

                var CaneclledAndNoShowAppointmentDTO = mapper.Map<CaneclledAndNoShowAppointmentDTO>(CancelAppointment);

                logger.LogInformation("Appointment Cancelled successfully for PatientId: {PatientId}, DoctorId: {DoctorId}", CancelAppointment.PatientId, CancelAppointment.DoctorId);

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