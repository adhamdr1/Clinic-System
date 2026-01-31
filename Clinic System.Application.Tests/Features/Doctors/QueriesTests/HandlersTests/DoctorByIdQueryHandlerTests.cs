namespace Clinic_System.Application.Tests.Features.Doctors.QueriesTests.HandlersTests
{
    public class DoctorByIdQueryHandlerTests
    {
        private readonly Mock<ICurrentUserService> _mockCurrentUserService;

        private readonly Mock<IDoctorService> _mockDoctorService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<DoctorByIdQueryHandler>> _mockLogger;
        private readonly DoctorByIdQueryHandler _handler;
        public DoctorByIdQueryHandlerTests()
        {
            _mockDoctorService = new Mock<IDoctorService>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<DoctorByIdQueryHandler>>();
            _mockCurrentUserService = new Mock<ICurrentUserService>();
            _handler = new DoctorByIdQueryHandler(
                _mockCurrentUserService.Object, _mockDoctorService.Object,
                _mockMapper.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var query = new GetDoctorByIdQuery { Id = 0 };
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
            var query = new GetDoctorByIdQuery { Id = 1 };
            _mockDoctorService.Setup(s => s.GetDoctorByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Doctor?)null);
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);
            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
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
            var query = new GetDoctorByIdQuery { Id = 1 };
            var doctor = new Doctor { Id = 1, FullName = "Dr. Smith" };
            var doctorDto = new GetDoctorDTO { Id = 1, FullName = "Dr. Smith" };
            _mockDoctorService.Setup(s => s.GetDoctorByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);
            _mockMapper.Setup(m => m.Map<GetDoctorDTO>(doctor))
                .Returns(doctorDto);
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);
            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(doctorDto, result.Data);
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Successfully retrieved doctor")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
}
