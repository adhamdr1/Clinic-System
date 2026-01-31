using Clinic_System.Application.Common.Bases;
using System.Data;

namespace Clinic_System.Application.Features.Doctors.Queries.Handlers
{
    public class PatientHistoryQueryHandler : AppRequestHandler<GetPatientHistoryQuery, PagedResult<MedicalRecordPatientHistoryDTO>>
    {
        private readonly IMedicalRecordService medicalRecordService;
        private readonly IMapper mapper;
        private readonly ILogger<PatientHistoryQueryHandler> logger;

        public PatientHistoryQueryHandler(
            ICurrentUserService currentUserService, 
            IMedicalRecordService medicalRecordService,
            IMapper mapper,
            ILogger<PatientHistoryQueryHandler> logger) : base(currentUserService)
        {
            this.medicalRecordService = medicalRecordService;
            this.mapper = mapper;
            this.logger = logger;
        }

        public override async Task<Response<PagedResult<MedicalRecordPatientHistoryDTO>>> Handle(GetPatientHistoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (CurrentDoctorId.HasValue)
                {
                    if (request.PatientId == null || request.PatientId == 0)
                    {
                        return BadRequest<PagedResult<MedicalRecordPatientHistoryDTO>>("PatientId is required for Doctors/Admins.");
                    }
                }
                else
                {
                    var (authorizedPatientId, errorResponse) = await GetAuthorizedPatientId(request.PatientId);

                    if (errorResponse != null) return errorResponse;

                    request.PatientId = authorizedPatientId;
                }

                var medicalRecord = await medicalRecordService.GetPatientHistoryAsync(request.PageNumber, request.PageSize, request.PatientId, cancellationToken);
                if (medicalRecord == null || !medicalRecord.Items.Any())
                {
                    return Success(new PagedResult<MedicalRecordPatientHistoryDTO>(new List<MedicalRecordPatientHistoryDTO>(), 0, request.PageNumber, request.PageSize));
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
