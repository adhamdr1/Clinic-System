namespace Clinic_System.Application.Features.MedicalRecords.Commands.Handlers
{
    public class UpdateMedicalRecordCommandHandler : AppRequestHandler<UpdateMedicalRecordCommand, UpdateMedicalRecordDTO>
    {
        private readonly IMedicalRecordService medicalRecord;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<UpdateMedicalRecordCommandHandler> logger;

        public UpdateMedicalRecordCommandHandler(
            IMedicalRecordService medicalRecord,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<UpdateMedicalRecordCommandHandler> logger) : base(currentUserService)
        {
            this.medicalRecord = medicalRecord;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        public override async Task<Response<UpdateMedicalRecordDTO>> Handle(UpdateMedicalRecordCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling UpdateMedicalRecordCommand for RecordId: {RecordId}", request.Id);

            var existingRecord = await unitOfWork.MedicalRecordsRepository.GetMedicalRecordForUpdateAsync(request.Id);

            if (existingRecord == null)
            {
                return NotFound<UpdateMedicalRecordDTO>("Medical Record not found.");
            }

            var doctorId = existingRecord.Appointment?.DoctorId;

            if (doctorId.HasValue)
            {
                var authResult = await ValidateDoctorAccess(doctorId.Value);
                if (authResult != null) return authResult;
            }
            else
            {
                return BadRequest<UpdateMedicalRecordDTO>("Associated Appointment or Doctor information is missing.");
            }

            var record = await medicalRecord.UpdateAsync(
            existingRecord,
            request.Diagnosis,
            request.DescriptionOfTheVisit,
            request.AdditionalNotes,
            cancellationToken);

            var resultDto = mapper.Map<UpdateMedicalRecordDTO>(record);

            logger.LogInformation("Successfully updated MedicalRecord with RecordId: {RecordId}", request.Id);

            return Success<UpdateMedicalRecordDTO>(resultDto, "Updated Successfully");
        }
    }
}