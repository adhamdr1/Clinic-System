namespace Clinic_System.Application.Features.Appointments.Queries.Handlers
{
    public class AvailableSlotQueryHandler : ResponseHandler, IRequestHandler<GetAvailableSlotQuery, Response<List<AvailableSlotDTO>>>
    {
        private readonly IAppointmentService appointmentService;
        private readonly IMapper mapper;

        public AvailableSlotQueryHandler(
            IAppointmentService appointmentService,
            IMapper mapper)
        {
            this.appointmentService = appointmentService;
            this.mapper = mapper;
        }

        public async Task<Response<List<AvailableSlotDTO>>> Handle(GetAvailableSlotQuery request, CancellationToken cancellationToken)
        {
            if (request.DoctorId <= 0 || request.Date.Date < DateTime.Today.Date)
            {
                return BadRequest<List<AvailableSlotDTO>>("Invalid doctor ID or date provided.");
            }

            try
            {
                var availableSlots = await appointmentService
                    .GetAvailableSlotsAsync(request.DoctorId, request.Date, cancellationToken);

                var availableSlotDTOs = mapper.Map<List<AvailableSlotDTO>>(availableSlots);

                return Success(availableSlotDTOs, "Available slots retrieved successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest<List<AvailableSlotDTO>>("Error occurred while fetching available slots: " + ex.Message);
            }
        }
    }
}
