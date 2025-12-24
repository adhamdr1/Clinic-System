namespace Clinic_System.Application.Tests.Features.Patients.CommandsTests.HandlersTests
{
    public class UpdateIdentityPatientCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IPatientService> _mockPatientService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IIdentityService> _mockIdentityService;
        private readonly Mock<ILogger<UpdateIdentityPatientCommandHandler>> _mockLogger;
        private readonly UpdateIdentityPatientCommandHandler _handler;
        public UpdateIdentityPatientCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockPatientService = new Mock<IPatientService>();
            _mockIdentityService = new Mock<IIdentityService>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<UpdateIdentityPatientCommandHandler>>();

            _handler = new UpdateIdentityPatientCommandHandler(_mockPatientService.Object,
                _mockMapper.Object,
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
            var command = new UpdateIdentityPatientCommand { Id = 1 };
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
                ApplicationUserId = "user-123"
            };

            _mockPatientService.Setup(s => s.GetPatientByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(Patient);

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

            var command = new UpdateIdentityPatientCommand
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
            var Patient = new Patient
            {
                Id = 1,
                ApplicationUserId = "user-123"
            };
            _mockPatientService.Setup(s => s.GetPatientByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(Patient);
            var command = new UpdateIdentityPatientCommand
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
            var Patient = new Patient
            {
                Id = 1,
                ApplicationUserId = "user-123"
            };
            _mockPatientService.Setup(s => s.GetPatientByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(Patient);
            var command = new UpdateIdentityPatientCommand
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
            var Patient = new Patient
            {
                Id = 1,
                ApplicationUserId = "user-123"
            };
            _mockPatientService.Setup(s => s.GetPatientByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(Patient);
            var command = new UpdateIdentityPatientCommand
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
            var Patient = new Patient
            {
                Id = 1,
                ApplicationUserId = "user-123"
            };
            _mockPatientService.Setup(s => s.GetPatientByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(Patient);
            var command = new UpdateIdentityPatientCommand
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
            var Patient = new Patient
            {
                Id = 1,
                ApplicationUserId = "user-123"
            };
            _mockPatientService.Setup(s => s.GetPatientByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(Patient);

            _mockIdentityService.Setup(i => i.UpdateEmailUserAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            var command = new UpdateIdentityPatientCommand
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
            var Patient = new Patient
            {
                Id = 1,
                ApplicationUserId = "user-123"
            };
            _mockPatientService.Setup(s => s.GetPatientByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(Patient);
            var command = new UpdateIdentityPatientCommand
            {
                Id = 1
            };
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.True(result.Succeeded);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
    }
}
