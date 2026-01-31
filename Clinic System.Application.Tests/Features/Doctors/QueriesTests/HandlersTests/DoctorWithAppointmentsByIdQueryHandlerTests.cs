namespace Clinic_System.Application.Tests.Features.Doctors.QueriesTests.HandlersTests
{
    public class DoctorWithAppointmentsByIdQueryHandlerTests
    {
        private readonly Mock<ICurrentUserService> _mockCurrentUserService;
        private readonly Mock<IDoctorService> _mockDoctorService;
        private readonly Mock<IIdentityService> _mockIdentityService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<DoctorWithAppointmentsByIdQueryHandler>> _mockLogger;
        private readonly DoctorWithAppointmentsByIdQueryHandler _handler;
        public DoctorWithAppointmentsByIdQueryHandlerTests()
        {
            _mockDoctorService = new Mock<IDoctorService>();
            _mockIdentityService = new Mock<IIdentityService>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<DoctorWithAppointmentsByIdQueryHandler>>();
            _mockCurrentUserService = new Mock<ICurrentUserService>();
            _handler = new DoctorWithAppointmentsByIdQueryHandler(
                _mockCurrentUserService.Object,
                _mockDoctorService.Object,
                _mockMapper.Object,
                _mockIdentityService.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var query = new GetDoctorWithAppointmentsByIdQuery { Id = 0 };
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
        public async Task Handle_DoctorNotFound_ReturnsNotFound()
        {
            // Arrange
            var query = new GetDoctorWithAppointmentsByIdQuery { Id = 1 };

            _mockDoctorService.Setup(s => s.GetDoctorWithAppointmentsByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Doctor?)null);
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
            var query = new GetDoctorWithAppointmentsByIdQuery { Id = 1 };
            var doctor = new Doctor
            {
                Id = 1,
                FullName = "Dr. Smith",
                ApplicationUserId = "user-123"
            };
            var doctorDto = new GetDoctorWhitAppointmentDTO
            {
                Id = 1,
                FullName = "Dr. Smith",
                Email = "adham@g.c",
                Appointments = new List<GetAppointmentForDoctorDTO>()
            };

            _mockDoctorService.Setup(s => s.GetDoctorWithAppointmentsByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _mockMapper.
                Setup(m => m.Map<GetDoctorWhitAppointmentDTO>(doctor)).
                Returns(doctorDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task Handle_EmailNotFound_LogsWarning()
        {
            // Arrange
            var query = new GetDoctorWithAppointmentsByIdQuery { Id = 1 };

            var doctor = new Doctor
            {
                Id = 1,
                FullName = "Dr. Smith",
                ApplicationUserId = "user-123"
            };

            var doctorDto = new GetDoctorWhitAppointmentDTO
            {
                Id = 1,
                FullName = "Dr. Smith",
                Appointments = new List<GetAppointmentForDoctorDTO>()
            };

            _mockDoctorService.Setup(s => s.GetDoctorWithAppointmentsByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _mockMapper
                .Setup(m => m.Map<GetDoctorWhitAppointmentDTO>(doctor))
                .Returns(doctorDto);

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
            var query = new GetDoctorWithAppointmentsByIdQuery { Id = 1 };

            var doctor = new Doctor
            {
                Id = 1,
                FullName = "Dr. Smith",
                ApplicationUserId = "user-123"
            };

            var doctorDto = new GetDoctorWhitAppointmentDTO
            {
                Id = 1,
                FullName = "Dr. Smith",
                Appointments = new List<GetAppointmentForDoctorDTO>()
            };

            _mockDoctorService.Setup(s => s.GetDoctorWithAppointmentsByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _mockMapper
                .Setup(m => m.Map<GetDoctorWhitAppointmentDTO>(doctor))
                .Returns(doctorDto);

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
