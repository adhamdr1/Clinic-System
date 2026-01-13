namespace Clinic_System.Application.Features.Doctors.Queries.Handlers
{
    public class PatientHistoryQueryHandler : ResponseHandler, IRequestHandler<GetPatientHistoryQuery, Response<PagedResult<MedicalRecordPatientHistoryDTO>>>
    {
        private readonly IMedicalRecordService medicalRecordService;
        private readonly IMapper mapper;
        private readonly ILogger<PatientHistoryQueryHandler> logger;

        public PatientHistoryQueryHandler(
            IMedicalRecordService medicalRecordService,
            IMapper mapper,
            ILogger<PatientHistoryQueryHandler> logger)
        {
            this.medicalRecordService = medicalRecordService;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Response<PagedResult<MedicalRecordPatientHistoryDTO>>> Handle(GetPatientHistoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var medicalRecord = await medicalRecordService.GetPatientHistoryAsync(request.PageNumber, request.PageSize, request.PatientId, cancellationToken);
                if (medicalRecord == null)
                {
                    return NotFound<PagedResult<MedicalRecordPatientHistoryDTO>>($"No medical history found for patient ID {request.PatientId}.");
                }
                var medicalRecordDto = mapper.Map<List<MedicalRecordPatientHistoryDTO>>(medicalRecord.Items);
                var pagedResult = new PagedResult<MedicalRecordPatientHistoryDTO>(medicalRecordDto, medicalRecord.TotalCount, medicalRecord.CurrentPage, medicalRecord.PageSize);

                return Success(pagedResult);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving medical history for patient ID {PatientId}", request.PatientId);
                return NotFound<PagedResult<MedicalRecordPatientHistoryDTO>>("An error occurred while processing your request.");
            }
        }
    }
}
