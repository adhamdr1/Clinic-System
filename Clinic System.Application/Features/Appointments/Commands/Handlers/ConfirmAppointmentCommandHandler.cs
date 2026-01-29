namespace Clinic_System.Application.Features.Appointments.Commands.Handlers
{
    public class ConfirmAppointmentCommandHandler : AppRequestHandler<ConfirmAppointmentCommand, ConfirmAppointmentDTO>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly IPatientService patientService;
        private readonly ILogger<ConfirmAppointmentCommandHandler> logger;

        public ConfirmAppointmentCommandHandler(
            ICurrentUserService currentUserService,
            IAppointmentService appointmentService,
            IPatientService patientService,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<ConfirmAppointmentCommandHandler> logger) : base(currentUserService)
        {
            this.appointmentService = appointmentService;
            this.patientService = patientService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }
        public override async Task<Response<ConfirmAppointmentDTO>> Handle(ConfirmAppointmentCommand request, CancellationToken cancellationToken)
        {
            var patient = await patientService.GetPatientByUserIdAsync(CurrentUserId);

            if (patient == null)
                return Unauthorized<ConfirmAppointmentDTO>("User is not registered as a patient.");

            // 2. املأ الـ Command بالـ ID الحقيقي
            request.PatientId = patient.Id;


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