namespace Clinic_System.Application.Tests.Features.Doctors.CommandsTests.HandlersTests
{
    public class UpdateIdentityDoctorCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IDoctorService> _mockDoctorService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IIdentityService> _mockIdentityService;
        private readonly Mock<ILogger<UpdateIdentityDoctorCommandHandler>> _mockLogger;
        private readonly UpdateIdentityDoctorCommandHandler _handler;
        public UpdateIdentityDoctorCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockDoctorService = new Mock<IDoctorService>();
            _mockIdentityService = new Mock<IIdentityService>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<UpdateIdentityDoctorCommandHandler>>();

            _handler = new UpdateIdentityDoctorCommandHandler(_mockDoctorService.Object,
                _mockMapper.Object,
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
            var command = new UpdateIdentityDoctorCommand { Id = 1 };
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.False(result.Succeeded);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task Handle_SuccessfulUpdate_ReturnsSuccessResponse()
        {
            var doctor = new Doctor
            {
                Id = 1,
                ApplicationUserId = "user-123"
            };

            _mockDoctorService.Setup(s => s.GetDoctorByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(doctor);

            _mockIdentityService.Setup(i => i.UpdateEmailUserAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _mockIdentityService.Setup(i => i.UpdateUserNameAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _mockIdentityService.Setup(i => i.UpdatePasswordUserAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _mockUnitOfWork.Setup(u => u.SaveAsync())
                .ReturnsAsync(1);

            var command = new UpdateIdentityDoctorCommand
            {
                Id = 1,
                Email = "adham@g.c",
                UserName = "adham",
                CurrentPassword = "OldPass123!",
                Password = "NewPass123!",
                ConfirmPassword = "NewPass123!"
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.Succeeded);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task Handle_EmailChangeWithoutUsername_ReturnsBadRequestResponse()
        {
            var doctor = new Doctor
            {
                Id = 1,
                ApplicationUserId = "user-123"
            };
            _mockDoctorService.Setup(s => s.GetDoctorByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(doctor);
            var command = new UpdateIdentityDoctorCommand
            {
                Id = 1,
                Email = "adham@g.c"
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Succeeded);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task Handle_UsernameChangeWithoutEmail_ReturnsBadRequestResponse()
        {
            var doctor = new Doctor
            {
                Id = 1,
                ApplicationUserId = "user-123"
            };
            _mockDoctorService.Setup(s => s.GetDoctorByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(doctor);
            var command = new UpdateIdentityDoctorCommand
            {
                Id = 1,
                UserName = "adham"
            };
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.False(result.Succeeded);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task Handle_PasswordChangeWithoutEmailOrUsername_ReturnsBadRequestResponse()
        {
            var doctor = new Doctor
            {
                Id = 1,
                ApplicationUserId = "user-123"
            };
            _mockDoctorService.Setup(s => s.GetDoctorByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(doctor);
            var command = new UpdateIdentityDoctorCommand
            {
                Id = 1,
                Password = "NewPass123!",
                ConfirmPassword = "NewPass123!",
                CurrentPassword = "OldPass123!"
            };
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.False(result.Succeeded);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task Handle_PasswordChangeWithoutCurrentPassword_ReturnsBadRequestResponse()
        {
            var doctor = new Doctor
            {
                Id = 1,
                ApplicationUserId = "user-123"
            };
            _mockDoctorService.Setup(s => s.GetDoctorByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(doctor);
            var command = new UpdateIdentityDoctorCommand
            {
                Id = 1,
                Email = "adham@g.c",
                Password = "NewPass123!",
                ConfirmPassword = "NewPass123!"
            };
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.False(result.Succeeded);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task Handle_IdentityServiceFails_ReturnsBadRequestResponse()
        {
            var doctor = new Doctor
            {
                Id = 1,
                ApplicationUserId = "user-123"
            };
            _mockDoctorService.Setup(s => s.GetDoctorByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(doctor);

            _mockIdentityService.Setup(i => i.UpdateEmailUserAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            var command = new UpdateIdentityDoctorCommand
            {
                Id = 1,
                Email = "adham@g.c",
                UserName = "adham"
            };

            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.False(result.Succeeded);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task Handle_NoChangesRequested_ReturnsSuccessResponse()
        {
            var doctor = new Doctor
            {
                Id = 1,
                ApplicationUserId = "user-123"
            };
            _mockDoctorService.Setup(s => s.GetDoctorByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(doctor);
            var command = new UpdateIdentityDoctorCommand
            {
                Id = 1
            };
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.True(result.Succeeded);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
    }
}
