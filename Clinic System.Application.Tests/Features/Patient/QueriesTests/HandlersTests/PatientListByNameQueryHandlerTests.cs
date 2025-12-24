namespace Clinic_System.Application.Tests.Features.Patients.QueriesTests.HandlersTests
{
    public class PatientListByNameQueryHandlerTests
    {
        private readonly Mock<IPatientService> _mockPatientService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<PatientListByNameQueryHandler>> _mockLogger;
        private readonly PatientListByNameQueryHandler _handler;
        public PatientListByNameQueryHandlerTests()
        {
            _mockPatientService = new Mock<IPatientService>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<PatientListByNameQueryHandler>>();

            _handler = new PatientListByNameQueryHandler(_mockPatientService.Object,
                _mockMapper.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_NoPatientsFound_ReturnsNotFound()
        {
            // Arrange
            var query = new GetPatientListByNameQuery { FullName = "Adham" };
            _mockPatientService.Setup(s => s.GetPatientListByNameAsync("Adham", It.IsAny<CancellationToken>()))
                .ReturnsAsync((List<Patient>?)null);
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);
            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("No patients found")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_PatientsFound_ReturnsSuccess()
        {
            // Arrange
            var query = new GetPatientListByNameQuery { FullName = "Adham" };
            var Patients = new List<Patient>
            {
                new Patient { Id = 1, FullName = "Dr. Smith Adham"}
            };

            _mockPatientService.Setup(s => s.GetPatientListByNameAsync("Adham", It.IsAny<CancellationToken>()))
                .ReturnsAsync(Patients);

            _mockMapper.Setup(m => m.Map<List<GetPatientListDTO>>(Patients))
                .Returns(new List<GetPatientListDTO>
                {
                    new GetPatientListDTO { Id = 1, FullName = "Dr. Smith Adham"}
                });
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);
            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Single(result.Data!);
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Found 1 patients")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
}
