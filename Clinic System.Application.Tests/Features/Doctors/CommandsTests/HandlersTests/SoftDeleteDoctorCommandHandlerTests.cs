namespace Clinic_System.Application.Tests.Features.Doctors.CommandsTests.HandlersTests
{
    public class SoftDeleteDoctorCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IDoctorService> _mockDoctorService;
        private readonly Mock<IIdentityService> _mockIdentityService;
        private readonly Mock<ILogger<SoftDeleteDoctorCommandHandler>> _mockLogger;
        private readonly SoftDeleteDoctorCommandHandler _handler;
        public SoftDeleteDoctorCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockDoctorService = new Mock<IDoctorService>();
            _mockIdentityService = new Mock<IIdentityService>();
            _mockLogger = new Mock<ILogger<SoftDeleteDoctorCommandHandler>>();

            _handler = new SoftDeleteDoctorCommandHandler(_mockDoctorService.Object,
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
            var command = new SoftDeleteDoctorCommand { Id = 1 };
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.False(result.Succeeded);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task Handle_SuccessfulSoftDeletion_ReturnsSuccessResponse()
        {
            var doctor = new Doctor
            {
                Id = 1,
                ApplicationUserId = "user-123"
            };
            _mockDoctorService.Setup(s => s.GetDoctorByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(doctor);

            _mockDoctorService.Setup(s => s.SoftDeleteDoctor(It.IsAny<Doctor>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.SaveAsync())
                .ReturnsAsync(1);

            _mockIdentityService.Setup(i => i.SoftDeleteUserAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var command = new SoftDeleteDoctorCommand { Id = 1 };
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.True(result.Succeeded);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task Handle_IdentityServiceFails_ReturnsErrorResponse()
        {
            var doctor = new Doctor
            {
                Id = 1,
                ApplicationUserId = "user-123"
            };
            _mockDoctorService.Setup(s => s.GetDoctorByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(doctor);

            _mockDoctorService.Setup(s => s.SoftDeleteDoctor(It.IsAny<Doctor>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.SaveAsync())
                .ReturnsAsync(1);

            _mockIdentityService.Setup(i => i.SoftDeleteUserAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var command = new SoftDeleteDoctorCommand { Id = 1 };
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Succeeded);

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task Handle_UnitOfWorkSaveFails_ReturnsErrorResponse()
        {
            var doctor = new Doctor
            {
                Id = 1,
                ApplicationUserId = "user-123"
            };
            _mockDoctorService.Setup(s => s.GetDoctorByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(doctor);
            _mockDoctorService.Setup(s => s.SoftDeleteDoctor(It.IsAny<Doctor>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveAsync())
                .ReturnsAsync(0);
            var command = new SoftDeleteDoctorCommand { Id = 1 };
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.False(result.Succeeded);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task Handle_ExceptionThrown_ReturnsErrorResponse()
        {
            var doctor = new Doctor
            {
                Id = 1,
                ApplicationUserId = "user-123"
            };

            _mockDoctorService.Setup(s => s.GetDoctorByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(doctor);

            _mockDoctorService.Setup(s => s.SoftDeleteDoctor(It.IsAny<Doctor>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            var command = new SoftDeleteDoctorCommand { Id = 1 };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Succeeded);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
    }
}
