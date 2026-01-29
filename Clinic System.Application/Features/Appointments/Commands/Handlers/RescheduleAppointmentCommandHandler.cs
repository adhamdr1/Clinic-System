namespace Clinic_System.Application.Features.Appointments.Commands.Handlers
{
    public class RescheduleAppointmentCommandHandler : AppRequestHandler<RescheduleAppointmentCommand, AppointmentDTO>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly IPatientService patientService;
        private readonly ILogger<RescheduleAppointmentCommandHandler> logger;

        public RescheduleAppointmentCommandHandler(
            ICurrentUserService currentUserService,
            IAppointmentService appointmentService,
            IPatientService patientService,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<RescheduleAppointmentCommandHandler> logger) : base(currentUserService)
        {
            this.appointmentService = appointmentService;
            this.patientService = patientService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public override async Task<Response<AppointmentDTO>> Handle(RescheduleAppointmentCommand request, CancellationToken cancellationToken)
        {
            var patient = await patientService.GetPatientByUserIdAsync(CurrentUserId);

            if (patient == null)
                return Unauthorized<AppointmentDTO>("User is not registered as a patient.");

            // 2. املأ الـ Command بالـ ID الحقيقي
            request.PatientId = patient.Id;

            logger.LogInformation("Starting appointment rescheduling for PatientId: {PatientId}", request.PatientId);

            Appointment newAppointment=null;

            try
            {
                newAppointment = await appointmentService.RescheduleAppointmentAsync(request, cancellationToken);

                var appointmentDto = mapper.Map<AppointmentDTO>(newAppointment);
               
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