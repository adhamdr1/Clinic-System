namespace Clinic_System.Application.Features.Appointments.Commands.Handlers
{
    public class CompleteAppointmentCommandHandler : AppRequestHandler<CompleteAppointmentCommand, CompleteAppointmentDTO>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IDoctorService doctorService; 
        private readonly IMapper mapper;
        private readonly ILogger<CompleteAppointmentCommandHandler> logger;

        public CompleteAppointmentCommandHandler(
            ICurrentUserService currentUserService,
            IAppointmentService appointmentService,
            IDoctorService doctorService, // New
            IMapper mapper,
            ILogger<CompleteAppointmentCommandHandler> logger) : base(currentUserService)
        {
            this.appointmentService = appointmentService;
            this.doctorService = doctorService;
            this.mapper = mapper;
            this.logger = logger;
        }

        public override async Task<Response<CompleteAppointmentDTO>> Handle(CompleteAppointmentCommand request, CancellationToken cancellationToken)
        {
            var doctor = await doctorService.GetDoctorByUserIdAsync(CurrentUserId);

            if (doctor == null)
                return Unauthorized<CompleteAppointmentDTO>("User is not registered as a doctor.");

            // 2. املأ الـ Command
            request.DoctorId = doctor.Id;

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