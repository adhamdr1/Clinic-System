namespace Clinic_System.Application.Features.Doctors.Queries.Handlers
{
    public class RecordsByDoctorIdQueryHandler : ResponseHandler, IRequestHandler<GetRecordsByDoctorIdQuery, Response<PagedResult<MedicalRecordDoctorDTO>>>
    {
        private readonly IMedicalRecordService medicalRecordService;
        private readonly IMapper mapper;
        private readonly ILogger<RecordsByDoctorIdQueryHandler> logger;

        public RecordsByDoctorIdQueryHandler(
            IMedicalRecordService medicalRecordService,
            IMapper mapper,
            ILogger<RecordsByDoctorIdQueryHandler> logger)
        {
            this.medicalRecordService = medicalRecordService;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Response<PagedResult<MedicalRecordDoctorDTO>>> Handle(GetRecordsByDoctorIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var medicalRecord = await medicalRecordService.GetRecordsByDoctorIdAsync(request.PageNumber, request.PageSize, request.DoctorId,
                    request.StartDate, request.EndDate, cancellationToken);

                if (medicalRecord == null)
                {
                    return NotFound<PagedResult<MedicalRecordDoctorDTO>>($"No medical records found for doctor ID {request.DoctorId}.");
                }

                var medicalRecordDto = mapper.Map<List<MedicalRecordDoctorDTO>>(medicalRecord.Items);
                var pagedResult = new PagedResult<MedicalRecordDoctorDTO>(medicalRecordDto, medicalRecord.TotalCount, medicalRecord.CurrentPage, medicalRecord.PageSize);

                return Success(pagedResult);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving medical records for doctor ID {DoctorId}", request.DoctorId);
                return NotFound<PagedResult<MedicalRecordDoctorDTO>>("An error occurred while processing your request.");
            }
        }
    }
}
