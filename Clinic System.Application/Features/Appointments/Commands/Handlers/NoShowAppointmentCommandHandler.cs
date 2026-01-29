namespace Clinic_System.Application.Features.Appointments.Commands.Handlers
{
    public class NoShowAppointmentCommandHandler : AppRequestHandler<NoShowAppointmentCommand, CaneclledAndNoShowAppointmentDTO>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IDoctorService doctorService;
        private readonly IMapper mapper;
        private readonly ILogger<NoShowAppointmentCommandHandler> logger;

        public NoShowAppointmentCommandHandler(
            ICurrentUserService currentUserService,
            IAppointmentService appointmentService,
            IDoctorService doctorService, // New
            IMapper mapper,
            ILogger<NoShowAppointmentCommandHandler> logger) : base(currentUserService)
        {
            this.appointmentService = appointmentService;
            this.doctorService = doctorService;
            this.mapper = mapper;
            this.logger = logger;
        }

        public override async Task<Response<CaneclledAndNoShowAppointmentDTO>> Handle(NoShowAppointmentCommand request, CancellationToken cancellationToken)
        {
            var doctor = await doctorService.GetDoctorByUserIdAsync(CurrentUserId);

            if (doctor == null)
                return Unauthorized<CaneclledAndNoShowAppointmentDTO>("User is not registered as a doctor.");

            // 2. املأ الـ Command
            request.DoctorId = doctor.Id;

            Appointment NoShowAppointment = null;
            try
            {
                NoShowAppointment = await appointmentService.NoShowAppointmentAsync(request, cancellationToken);

                logger.LogInformation("Starting No-Show process for AppointmentId: {AppointmentId}, PatientId: {PatientId}", request.AppointmentId, NoShowAppointment.PatientId);


                var CaneclledAndNoShowAppointmentDTO = mapper.Map<CaneclledAndNoShowAppointmentDTO>(NoShowAppointment);

                logger.LogInformation("Appointment NoShowed successfully for PatientId: {PatientId}, DoctorId: {DoctorId}", NoShowAppointment.PatientId, NoShowAppointment.DoctorId);

                return Success(CaneclledAndNoShowAppointmentDTO, "Appointment NoShowed successfully.");

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while NoShow appointment for PatientId: {PatientId}, DoctorId: {DoctorId}", NoShowAppointment?.PatientId, NoShowAppointment?.DoctorId);
                return BadRequest<CaneclledAndNoShowAppointmentDTO>("Error occurred while processing No Showing: " + ex.Message);
            }
        }
    }
}