namespace Clinic_System.Application.Features.Appointments.Commands.Handlers
{
    public class BookAppointmentCommandHandler : AppRequestHandler<BookAppointmentCommand, AppointmentDTO>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly IPatientService patientService;
        private readonly ILogger<BookAppointmentCommandHandler> logger;

        public BookAppointmentCommandHandler(
            ICurrentUserService currentUserService, 
            IAppointmentService appointmentService,
            IPatientService patientService, 
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<BookAppointmentCommandHandler> logger) : base(currentUserService)
        {
            this.appointmentService = appointmentService;
            this.patientService = patientService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public override async Task<Response<AppointmentDTO>> Handle(BookAppointmentCommand request, CancellationToken cancellationToken)
        {
            var patient = await patientService.GetPatientByUserIdAsync(CurrentUserId);

            if (patient == null)
                return Unauthorized<AppointmentDTO>("User is not registered as a patient.");

            // 2. املأ الـ Command بالـ ID الحقيقي
            request.PatientId = patient.Id;


            logger.LogInformation("Handling BookAppointmentCommand for PatientId: {PatientId}, DoctorId: {DoctorId}", request.PatientId, request.DoctorId);
            
            try
            {
                var newAppointment = await appointmentService.BookAppointmentAsync(
                                request.PatientId,
                                request.DoctorId,
                                request.AppointmentDate,
                                request.AppointmentTime,
                                cancellationToken
                            );

                var appointmentDto = mapper.Map<AppointmentDTO>(newAppointment);
    
                logger.LogInformation("Appointment booked successfully for PatientId: {PatientId}, DoctorId: {DoctorId}", request.PatientId, request.DoctorId);

                return Created(appointmentDto, "Appointment booked successfully.");
            }
            catch (SlotAlreadyBookedException ex) when (ex.Message.Contains("not available"))
            {
                logger.LogWarning("Booking failed: {ErrorMessage}", ex.Message);
                return BadRequest<AppointmentDTO>(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while booking appointment for PatientId: {PatientId}, DoctorId: {DoctorId}", request.PatientId, request.DoctorId);
                return BadRequest<AppointmentDTO>("Error occurred while processing booking: " + ex.Message);
            }
        }
    }
}