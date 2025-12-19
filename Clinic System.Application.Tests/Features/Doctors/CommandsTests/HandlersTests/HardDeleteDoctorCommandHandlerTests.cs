namespace Clinic_System.Application.Tests.Features.Doctors.CommandsTests.HandlersTests
{
    public class HardDeleteDoctorCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IDoctorService> _mockDoctorService;
        private readonly Mock<IIdentityService> _mockIdentityService;
        private readonly Mock<ILogger<HardDeleteDoctorCommandHandler>> _mockLogger;
        private readonly HardDeleteDoctorCommandHandler _handler;
        public HardDeleteDoctorCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockDoctorService = new Mock<IDoctorService>();
            _mockIdentityService = new Mock<IIdentityService>();
            _mockLogger = new Mock<ILogger<HardDeleteDoctorCommandHandler>>();

            _handler = new HardDeleteDoctorCommandHandler(_mockDoctorService.Object,
                _mockIdentityService.Object,
                _mockUnitOfWork.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_DoctorNotFound_ReturnsNotFoundResponse()
        {
            var doctor = (Doctor?)null;

            _mockDoctorService.Setup(s => s.GetDoctorByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(doctor);

            var command = new HardDeleteDoctorCommand { Id = 1 };
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Succeeded);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task Handle_SuccessfulDeletion_ReturnsSuccessResponse()
        {
            var doctor = new Doctor
            {
                Id = 1,
                ApplicationUserId = "user-123"
            };

            _mockDoctorService.Setup(s => s.GetDoctorByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(doctor);

            _mockDoctorService.Setup(s => s.HardDeleteDoctor(It.IsAny<Doctor>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.SaveAsync())
                .ReturnsAsync(1);

            _mockIdentityService.Setup(i => i.HardDeleteUserAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var command = new HardDeleteDoctorCommand { Id = 1 };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.Succeeded);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task Handle_IdentityServiceDeletionFails_ReturnsBadRequestResponse()
        {
            var doctor = new Doctor
            {
                Id = 1,
                ApplicationUserId = "user-123"
            };

            _mockDoctorService.Setup(s => s.GetDoctorByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(doctor);

            _mockDoctorService.Setup(s => s.HardDeleteDoctor(It.IsAny<Doctor>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.SaveAsync())
                .ReturnsAsync(1);

            _mockIdentityService.Setup(i => i.HardDeleteUserAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var command = new HardDeleteDoctorCommand { Id = 1 };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Succeeded);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task Handle_UnitOfWorkSaveFails_ReturnsBadRequestResponse()
        {
            var doctor = new Doctor
            {
                Id = 1,
                ApplicationUserId = "user-123"
            };
            _mockDoctorService.Setup(s => s.GetDoctorByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(doctor);

            _mockDoctorService.Setup(s => s.HardDeleteDoctor(It.IsAny<Doctor>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.SaveAsync())
                .ReturnsAsync(0); // Simulate failure

            var command = new HardDeleteDoctorCommand { Id = 1 };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Succeeded);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task Handle_ExceptionDuringDeletion_ReturnsBadRequestResponse()
        {
            var doctor = new Doctor
            {
                Id = 1,
                ApplicationUserId = "user-123"
            };
            _mockDoctorService.Setup(s => s.GetDoctorByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(doctor);

            _mockDoctorService.Setup(s => s.HardDeleteDoctor(It.IsAny<Doctor>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            var command = new HardDeleteDoctorCommand { Id = 1 };
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.False(result.Succeeded);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
    }
}
