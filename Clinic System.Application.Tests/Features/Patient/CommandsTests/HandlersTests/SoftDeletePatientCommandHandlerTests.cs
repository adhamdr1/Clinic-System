namespace Clinic_System.Application.Tests.Features.Patients.CommandsTests.HandlersTests
{
    public class SoftDeletePatientCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IPatientService> _mockPatientService;
        private readonly Mock<IIdentityService> _mockIdentityService;
        private readonly Mock<ILogger<SoftDeletePatientCommandHandler>> _mockLogger;
        private readonly SoftDeletePatientCommandHandler _handler;

        private readonly Mock<ICurrentUserService> _mockCurrentUserService;
        public SoftDeletePatientCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockPatientService = new Mock<IPatientService>();
            _mockIdentityService = new Mock<IIdentityService>();
            _mockLogger = new Mock<ILogger<SoftDeletePatientCommandHandler>>();
            _mockCurrentUserService = new Mock<ICurrentUserService>();
            _handler = new SoftDeletePatientCommandHandler(_mockCurrentUserService.Object, _mockPatientService.Object,
                _mockIdentityService.Object,
                _mockUnitOfWork.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_PatientNotFound_ReturnsNotFoundResponse()
        {
            var Patient = (Patient?)null;
            _mockPatientService.Setup(s => s.GetPatientByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(Patient);
            var command = new SoftDeletePatientCommand { Id = 1 };
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.False(result.Succeeded);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task Handle_SuccessfulSoftDeletion_ReturnsSuccessResponse()
        {
            var Patient = new Patient
            {
                Id = 1,
                ApplicationUserId = "user-123"
            };
            _mockPatientService.Setup(s => s.GetPatientByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(Patient);

            _mockPatientService.Setup(s => s.SoftDeletePatient(It.IsAny<Patient>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.SaveAsync())
                .ReturnsAsync(1);

            _mockIdentityService.Setup(i => i.SoftDeleteUserAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var command = new SoftDeletePatientCommand { Id = 1 };
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.True(result.Succeeded);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task Handle_IdentityServiceFails_ReturnsErrorResponse()
        {
            var Patient = new Patient
            {
                Id = 1,
                ApplicationUserId = "user-123"
            };
            _mockPatientService.Setup(s => s.GetPatientByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(Patient);

            _mockPatientService.Setup(s => s.SoftDeletePatient(It.IsAny<Patient>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.SaveAsync())
                .ReturnsAsync(1);

            _mockIdentityService.Setup(i => i.SoftDeleteUserAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var command = new SoftDeletePatientCommand { Id = 1 };
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Succeeded);

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task Handle_UnitOfWorkSaveFails_ReturnsErrorResponse()
        {
            var Patient = new Patient
            {
                Id = 1,
                ApplicationUserId = "user-123"
            };
            _mockPatientService.Setup(s => s.GetPatientByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(Patient);
            _mockPatientService.Setup(s => s.SoftDeletePatient(It.IsAny<Patient>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveAsync())
                .ReturnsAsync(0);
            var command = new SoftDeletePatientCommand { Id = 1 };
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.False(result.Succeeded);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task Handle_ExceptionThrown_ReturnsErrorResponse()
        {
            var Patient = new Patient
            {
                Id = 1,
                ApplicationUserId = "user-123"
            };

            _mockPatientService.Setup(s => s.GetPatientByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(Patient);

            _mockPatientService.Setup(s => s.SoftDeletePatient(It.IsAny<Patient>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            var command = new SoftDeletePatientCommand { Id = 1 };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Succeeded);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
    }
}
