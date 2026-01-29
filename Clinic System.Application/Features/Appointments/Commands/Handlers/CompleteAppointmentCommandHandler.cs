using Clinic_System.Application.Service.Interface;
using Clinic_System.Core.Entities;

namespace Clinic_System.Application.Features.Appointments.Commands.Handlers
{
    public class CompleteAppointmentCommandHandler : AppRequestHandler<CompleteAppointmentCommand, CompleteAppointmentDTO>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<CompleteAppointmentCommandHandler> logger;
        public CompleteAppointmentCommandHandler(
            ICurrentUserService currentUserService,
            IAppointmentService appointmentService,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<CompleteAppointmentCommandHandler> logger) : base(currentUserService)
        {
            this.appointmentService = appointmentService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public override async Task<Response<CompleteAppointmentDTO>> Handle(CompleteAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await unitOfWork.AppointmentsRepository.GetByIdAsync(request.AppointmentId);

            if (appointment == null)
            {
                return NotFound<CompleteAppointmentDTO>("Appointment not found.");
            }

            var authResult = await ValidateDoctorAccess(appointment.DoctorId);
            if (authResult != null)
                return authResult;

            request.DoctorId = appointment.DoctorId;

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