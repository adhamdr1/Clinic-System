namespace Clinic_System.Application.Tests.Features.Doctors.QueriesTests.HandlersTests
{
    public class DoctorListBySpecializationQueryHandlerTests
    {
        private readonly Mock<IDoctorService> _mockDoctorService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<DoctorListBySpecializationQueryHandler>> _mockLogger;
        private readonly DoctorListBySpecializationQueryHandler _handler;
        public DoctorListBySpecializationQueryHandlerTests()
        {
            _mockDoctorService = new Mock<IDoctorService>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<DoctorListBySpecializationQueryHandler>>();

            _handler = new DoctorListBySpecializationQueryHandler(_mockDoctorService.Object,
                _mockMapper.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_NoDoctorsFound_ReturnsNotFound()
        {
            // Arrange
            var query = new GetDoctorListBySpecializationQuery { Specialization = "Cardiology" };
            _mockDoctorService.Setup(s => s.GetDoctorsListBySpecializationAsync("Cardiology", It.IsAny<CancellationToken>()))
                .ReturnsAsync((List<Doctor>?)null);
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);
            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("No doctors found")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_DoctorsFound_ReturnsSuccess()
        {
            // Arrange
            var query = new GetDoctorListBySpecializationQuery { Specialization = "Cardiology" };
            var doctors = new List<Doctor>
            {
                new Doctor { Id = 1, FullName = "Dr. Smith", Specialization = "Cardiology" }
            };
            _mockDoctorService.Setup(s => s.GetDoctorsListBySpecializationAsync("Cardiology", It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctors);
            _mockMapper.Setup(m => m.Map<List<GetDoctorListDTO>>(doctors))
                .Returns(new List<GetDoctorListDTO>
                {
                    new GetDoctorListDTO { Id = 1, FullName = "Dr. Smith", Specialization = "Cardiology" }
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
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Found 1 doctors")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
}
