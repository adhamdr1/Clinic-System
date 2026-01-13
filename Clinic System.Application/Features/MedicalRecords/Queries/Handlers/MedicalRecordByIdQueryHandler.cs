namespace Clinic_System.Application.Features.Doctors.Queries.Handlers
{
    public class MedicalRecordByIdQueryHandler : ResponseHandler, IRequestHandler<GetMedicalRecordByIdQuery, Response<MedicalRecordDTO>>
    {
        private readonly IMedicalRecordService medicalRecordService;
        private readonly IMapper mapper;
        private readonly ILogger<MedicalRecordByIdQueryHandler> logger;

        public MedicalRecordByIdQueryHandler(
            IMedicalRecordService medicalRecordService,
            IMapper mapper,
            ILogger<MedicalRecordByIdQueryHandler> logger)
        {
            this.medicalRecordService = medicalRecordService;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Response<MedicalRecordDTO>> Handle(GetMedicalRecordByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var medicalRecord = await medicalRecordService.GetRecordByIdAsync(request.Id, cancellationToken);
                if (medicalRecord == null)
                {
                    return NotFound<MedicalRecordDTO>($"Medical record with ID {request.Id} not found.");
                }
                var medicalRecordDto = mapper.Map<MedicalRecordDTO>(medicalRecord);
                return Success(medicalRecordDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving medical record with ID {MedicalRecordId}", request.Id);
                return NotFound<MedicalRecordDTO>("An error occurred while processing your request.");
            }
        }
    }
}
