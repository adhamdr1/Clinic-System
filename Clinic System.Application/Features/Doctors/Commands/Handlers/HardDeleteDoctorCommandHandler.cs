namespace Clinic_System.Application.Features.Doctors.Commands.Handlers
{
    public class HardDeleteDoctorCommandHandler : ResponseHandler, IRequestHandler<HardDeleteDoctorCommand, Response<string>>
    {
        private readonly IDoctorService doctorService;
        private readonly IIdentityService identityService;
        private readonly IUnitOfWork unitOfWork;

        public HardDeleteDoctorCommandHandler(IDoctorService doctorService
            , IIdentityService identityService, IUnitOfWork unitOfWork)
        {
            this.doctorService = doctorService;
            this.identityService = identityService;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Response<string>> Handle(HardDeleteDoctorCommand request, CancellationToken cancellationToken)
        {

            var doctor = await doctorService.GetDoctorByIdAsync(request.Id);

            if (doctor == null)
            {
                return NotFound<string>($"Doctor with Id {request.Id} not found");
            }

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await doctorService.HardDeleteDoctor(doctor, cancellationToken);

                    var result = await unitOfWork.SaveAsync();
                    if (result == 0)
                    {
                        return BadRequest<string>("Failed to Deleted doctor");
                    }

                    var IsDeletedUser = await identityService.HardDeleteUserAsync(doctor.ApplicationUserId, cancellationToken);

                    if (!IsDeletedUser)
                    {
                        return BadRequest<string>("Failed to Deleted associated user");
                    }

                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    return BadRequest<string>($"Doctor deletion failed: {ex.Message}");
                }
            }

            return Deleted<string>("Doctor Deleted successfully");
        }
    }
}
