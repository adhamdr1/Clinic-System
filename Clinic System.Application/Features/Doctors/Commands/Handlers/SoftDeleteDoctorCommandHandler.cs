namespace Clinic_System.Application.Features.Doctors.Commands.Handlers
{
    public class SoftDeleteDoctorCommandHandler : ResponseHandler, IRequestHandler<SoftDeleteDoctorCommand, Response<Doctor>>
    {
        private readonly IDoctorService doctorService;
        private readonly IIdentityService identityService;
        private readonly IUnitOfWork unitOfWork;

        public SoftDeleteDoctorCommandHandler(IDoctorService doctorService
            , IIdentityService identityService, IUnitOfWork unitOfWork)
        {
            this.doctorService = doctorService;
            this.identityService = identityService;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Response<Doctor>> Handle(SoftDeleteDoctorCommand request, CancellationToken cancellationToken)
        {

            var doctor = await doctorService.GetDoctorByIdAsync(request.Id);

            if (doctor == null)
            {
                return NotFound<Doctor>($"Doctor with Id {request.Id} not found");
            }

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await doctorService.SoftDeleteDoctor(doctor, cancellationToken);

                    var result = await unitOfWork.SaveAsync();
                    if (result == 0)
                    {
                        return BadRequest<Doctor>("Failed to Deleted doctor");
                    }

                    var IsDeletedUser = await identityService.SoftDeleteUserAsync(doctor.ApplicationUserId, cancellationToken);

                    if (!IsDeletedUser)
                    {
                        return BadRequest<Doctor>("Failed to Deleted associated user");
                    }

                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    return BadRequest<Doctor>($"Doctor deletion failed: {ex.Message}");
                }
            }


            return Deleted<Doctor>("Doctor Deleted successfully");
        }
    }
}
