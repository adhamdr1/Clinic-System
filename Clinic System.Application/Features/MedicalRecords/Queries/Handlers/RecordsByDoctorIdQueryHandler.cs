namespace Clinic_System.Application.Features.Doctors.Queries.Handlers
{
    public class RecordsByDoctorIdQueryHandler : AppRequestHandler<GetRecordsByDoctorIdQuery, PagedResult<MedicalRecordDoctorDTO>>
    {
        private readonly IMedicalRecordService medicalRecordService;
        private readonly IMapper mapper;
        private readonly ILogger<RecordsByDoctorIdQueryHandler> logger;

        public RecordsByDoctorIdQueryHandler(
            ICurrentUserService currentUserService, // 2. إضافة الـ CurrentUser عشان الـ Base
            IMedicalRecordService medicalRecordService,
            IMapper mapper,
            ILogger<RecordsByDoctorIdQueryHandler> logger) : base(currentUserService)
        {
            this.medicalRecordService = medicalRecordService;
            this.mapper = mapper;
            this.logger = logger;
        }

        public override async Task<Response<PagedResult<MedicalRecordDoctorDTO>>> Handle(GetRecordsByDoctorIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var (authorizedDoctorId, errorResponse) = await GetAuthorizedDoctorId(request.DoctorId);

                if (errorResponse != null) return errorResponse;

                request.DoctorId = authorizedDoctorId;

                var medicalRecord = await medicalRecordService.GetRecordsByDoctorIdAsync(request.PageNumber, request.PageSize, request.DoctorId,
                    request.StartDate, request.EndDate, cancellationToken);

                if (medicalRecord == null || !medicalRecord.Items.Any())
                {
                    // رجع PagedResult فاضي
                    return Success(new PagedResult<MedicalRecordDoctorDTO>(new List<MedicalRecordDoctorDTO>(), 0, request.PageNumber, request.PageSize));
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
