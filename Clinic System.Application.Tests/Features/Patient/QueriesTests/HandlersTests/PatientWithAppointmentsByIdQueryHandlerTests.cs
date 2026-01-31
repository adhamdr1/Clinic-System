namespace Clinic_System.Application.Tests.Features.Patients.QueriesTests.HandlersTests
{
    public class PatientWithAppointmentsByIdQueryHandlerTests
    {
        private readonly Mock<IPatientService> _mockPatientService;
        private readonly Mock<IIdentityService> _mockIdentityService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<PatientWithAppointmentsByIdQueryHandler>> _mockLogger;
        private readonly Mock<ICurrentUserService> _mockCurrentUserService;
        private readonly PatientWithAppointmentsByIdQueryHandler _handler;
        public PatientWithAppointmentsByIdQueryHandlerTests()
        {
            _mockPatientService = new Mock<IPatientService>();
            _mockIdentityService = new Mock<IIdentityService>();
            _mockMapper = new Mock<IMapper>();
            _mockCurrentUserService = new Mock<ICurrentUserService>();
            _mockLogger = new Mock<ILogger<PatientWithAppointmentsByIdQueryHandler>>();

            _handler = new PatientWithAppointmentsByIdQueryHandler(
                 _mockCurrentUserService.Object,
                _mockPatientService.Object,
                _mockMapper.Object,
                _mockIdentityService.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var query = new GetPatientWithAppointmentsByIdQuery { Id = 0 };
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Invalid ID")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_PatientNotFound_ReturnsNotFound()
        {
            // Arrange
            var query = new GetPatientWithAppointmentsByIdQuery { Id = 1 };

            _mockPatientService.Setup(s => s.GetPatientWithAppointmentsByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patient?)null);
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("not found")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ValidId_ReturnsSuccess()
        {
            // Arrange
            var query = new GetPatientWithAppointmentsByIdQuery { Id = 1 };
            var Patient = new Patient
            {
                Id = 1,
                FullName = "Dr. Smith",
                ApplicationUserId = "user-123"
            };
            var PatientDto = new GetPatientWhitAppointmentDTO
            {
                Id = 1,
                FullName = "Dr. Smith",
                Email = "adham@g.c",
                Appointments = new List<GetAppointmentForPatientDTO>()
            };

            _mockPatientService.Setup(s => s.GetPatientWithAppointmentsByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Patient);

            _mockMapper.
                Setup(m => m.Map<GetPatientWhitAppointmentDTO>(Patient)).
                Returns(PatientDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task Handle_EmailNotFound_LogsWarning()
        {
            // Arrange
            var query = new GetPatientWithAppointmentsByIdQuery { Id = 1 };

            var Patient = new Patient
            {
                Id = 1,
                FullName = "Dr. Smith",
                ApplicationUserId = "user-123"
            };

            var PatientDto = new GetPatientWhitAppointmentDTO
            {
                Id = 1,
                FullName = "Dr. Smith",
                Appointments = new List<GetAppointmentForPatientDTO>()
            };

            _mockPatientService.Setup(s => s.GetPatientWithAppointmentsByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Patient);

            _mockMapper
                .Setup(m => m.Map<GetPatientWhitAppointmentDTO>(Patient))
                .Returns(PatientDto);

            _mockIdentityService.Setup(s => s.GetUserEmailAsync("user-123", It.IsAny<CancellationToken>()))
                .ReturnsAsync((string?)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Email not found")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_UserNameNotFound_LogsWarning()
        {
            // Arrange
            var query = new GetPatientWithAppointmentsByIdQuery { Id = 1 };

            var Patient = new Patient
            {
                Id = 1,
                FullName = "Dr. Smith",
                ApplicationUserId = "user-123"
            };

            var PatientDto = new GetPatientWhitAppointmentDTO
            {
                Id = 1,
                FullName = "Dr. Smith",
                Appointments = new List<GetAppointmentForPatientDTO>()
            };

            _mockPatientService.Setup(s => s.GetPatientWithAppointmentsByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Patient);

            _mockMapper
                .Setup(m => m.Map<GetPatientWhitAppointmentDTO>(Patient))
                .Returns(PatientDto);

            _mockIdentityService.Setup(s => s.GetUserNameAsync("user-123", It.IsAny<CancellationToken>()))
                .ReturnsAsync((string?)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("UserName not found")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
}
