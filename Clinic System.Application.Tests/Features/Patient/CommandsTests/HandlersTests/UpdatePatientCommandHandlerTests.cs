namespace Clinic_System.Application.Tests.Features.Patients.CommandsTests.HandlersTests
{
    public class UpdatePatientCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IPatientService> _mockPatientService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<UpdatePatientCommandHandler>> _mockLogger;

        private readonly Mock<ICurrentUserService> _mockCurrentUserService;
        private readonly UpdatePatientCommandHandler _handler;
        public UpdatePatientCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockPatientService = new Mock<IPatientService>();
            _mockMapper = new Mock<IMapper>();
            _mockCurrentUserService = new Mock<ICurrentUserService>();
            _mockLogger = new Mock<ILogger<UpdatePatientCommandHandler>>();

            _handler = new UpdatePatientCommandHandler(
                _mockCurrentUserService.Object,
                _mockPatientService.Object,
                _mockMapper.Object,
                _mockUnitOfWork.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_PatientNotFound_ReturnsNotFoundResponse()
        {
            var Patient = (Patient?)null;

            _mockPatientService.Setup(s => s.GetPatientByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(Patient);

            var command = new UpdatePatientCommand { Id = 1 };

            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.False(result.Succeeded);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task Handle_SuccessfulUpdate_ReturnsSuccessResponse()
        {
            var Patient = new Patient
            {
                Id = 1,
                FullName = "Dr. Smith",
            };

            _mockPatientService.Setup(s => s.GetPatientByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(Patient);

            _mockMapper.Setup(m => m.Map(It.IsAny<UpdatePatientCommand>(), It.IsAny<Patient>()))
               .Callback<UpdatePatientCommand, Patient>((cmd, doc) =>
               {
                   doc.FullName = cmd.FullName;
               });

            _mockUnitOfWork.Setup(u => u.SaveAsync())
                .ReturnsAsync(1);

           

            _mockMapper.Setup(m => m.Map<UpdatePatientDTO>(It.IsAny<Patient>()))
                .Returns((Patient doc) => new UpdatePatientDTO
                {
                    FullName = doc.FullName,
                });

            var command = new UpdatePatientCommand
            {
                Id = 1,
                FullName = "Dr. John Smith",
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.Succeeded);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Dr. John Smith", result.Data.FullName);
        }

        [Fact]
        public async Task Handle_UpdateFails_ReturnsBadRequestResponse()
        {
            var Patient = new Patient
            {
                Id = 1,
                FullName = "Dr. Smith",
            };
            _mockPatientService.Setup(s => s.GetPatientByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(Patient);

            _mockMapper.Setup(m => m.Map(It.IsAny<UpdatePatientCommand>(), It.IsAny<Patient>()))
               .Callback<UpdatePatientCommand, Patient>((cmd, doc) =>
               {
                   doc.FullName = cmd.FullName;
               });

            _mockUnitOfWork.Setup(u => u.SaveAsync())
                .ReturnsAsync(0); // Simulate failure

            var command = new UpdatePatientCommand
            {
                Id = 1,
                FullName = "Dr. John Smith",
            };

            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.False(result.Succeeded);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
    }
}
