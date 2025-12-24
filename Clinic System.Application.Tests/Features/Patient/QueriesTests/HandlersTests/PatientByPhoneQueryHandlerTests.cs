namespace Clinic_System.Application.Tests.Features.Patients.QueriesTests.HandlersTests
{
    public class PatientByPhoneQueryHandlerTests
    {
        private readonly Mock<IPatientService> _mockPatientService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<PatientByPhoneQueryHandler>> _mockLogger;
        private readonly PatientByPhoneQueryHandler _handler;
        public PatientByPhoneQueryHandlerTests()
        {
            _mockPatientService = new Mock<IPatientService>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<PatientByPhoneQueryHandler>>();

            _handler = new PatientByPhoneQueryHandler(_mockPatientService.Object,
                _mockMapper.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_InvalidPhone_ReturnsBadRequest()
        {
            // Arrange
            var query = new GetPatientByPhoneQuery { Phone = "" };
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Invalid Phone")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_PatientNotFound_ReturnsNotFound()
        {
            // Arrange
            var query = new GetPatientByPhoneQuery { Phone = "01507489484" };
            _mockPatientService.Setup(s => s.GetPatientByPhoneAsync("01507489484", It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patient?)null);
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
        public async Task Handle_ValidPhone_ReturnsSuccess()
        {
            // Arrange
            var query = new GetPatientByPhoneQuery { Phone = "01507489484" };
            var patient = new Patient { Id = 1, FullName = "Dr. Smith" , Phone = "01507489484" };
            var patientDto = new GetPatientDTO { Id = 1, FullName = "Dr. Smith" , Phone = "01507489484" };

            _mockPatientService.Setup(s => s.GetPatientByPhoneAsync("01507489484", It.IsAny<CancellationToken>()))
                .ReturnsAsync(patient);

            _mockMapper.Setup(m => m.Map<GetPatientDTO>(patient))
                .Returns(patientDto);
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);
            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(patientDto, result.Data);
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Successfully retrieved patient")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
}
