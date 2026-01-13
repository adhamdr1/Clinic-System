namespace Clinic_System.Application.Features.MedicalRecords.Commands.Handlers
{
    public class UpdateMedicalRecordCommandHandler : ResponseHandler, IRequestHandler<UpdateMedicalRecordCommand, Response<UpdateMedicalRecordDTO>>
    {
        private readonly IMedicalRecordService medicalRecord;
        private readonly IMapper mapper;
        private readonly ILogger<UpdateMedicalRecordCommandHandler> logger;

        public UpdateMedicalRecordCommandHandler(IMedicalRecordService medicalRecord
            , IMapper mapper, ILogger<UpdateMedicalRecordCommandHandler> logger)
        {
            this.medicalRecord = medicalRecord;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Response<UpdateMedicalRecordDTO>> Handle(UpdateMedicalRecordCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling UpdateMedicalRecordCommand for RecordId: {RecordId}", request.Id);

            var record = await medicalRecord.UpdateAsync(
            request.Id,
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