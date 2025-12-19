namespace Clinic_System.Application.Tests.Features.AppointmentsTests.Queries.HandlersTests
{
    public class AvailableSlotQueryHandlerTests
    {
        private readonly Mock<IAppointmentService> _mockAppointmentService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<AvailableSlotQueryHandler>> _mockLogger;
        private readonly AvailableSlotQueryHandler _handler;
        public AvailableSlotQueryHandlerTests()
        {
            _mockAppointmentService = new Mock<IAppointmentService>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<AvailableSlotQueryHandler>>();

            _handler = new AvailableSlotQueryHandler(_mockAppointmentService.Object,
                _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsAvailableSlots()
        {
            // Arrange
            var request = new GetAvailableSlotQuery
            {
                DoctorId = 1,
                Date = DateTime.Today.AddDays(1)
            };

            var availableSlots = new List<TimeSpan>
            {
                new TimeSpan(9, 0, 0), // الساعة 9 صباحاً
                new TimeSpan(10, 0, 0) // الساعة 10 صباحاً
            };

            var availableSlotDTOs = new List<AvailableSlotDTO>
            {
                new AvailableSlotDTO { SlotTime = new TimeSpan(9, 0, 0) },
                new AvailableSlotDTO { SlotTime = new TimeSpan(10, 0, 0) }
            };

            _mockAppointmentService
                .Setup(s => s.GetAvailableSlotsAsync(request.DoctorId, request.Date, It.IsAny<CancellationToken>()))
                .ReturnsAsync(availableSlots);

            _mockMapper
                .Setup(m => m.Map<List<AvailableSlotDTO>>(availableSlots))
                .Returns(availableSlotDTOs);
            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(response.Succeeded);

            Assert.Equal(2, response.Data.Count);

            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Fetching available slots")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidDoctorId_ReturnsBadRequest()
        {
            // Arrange
            var request = new GetAvailableSlotQuery
            {
                DoctorId = -1,
                Date = DateTime.Today.AddDays(1)
            };
            // Act
            var response = await _handler.Handle(request, CancellationToken.None);
            // Assert
            Assert.False(response.Succeeded);
            Assert.Equal("Invalid doctor ID or date provided.", response.Message);
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Invalid request parameters")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ServiceThrowsException_ReturnsBadRequest()
        {
            // Arrange
            var request = new GetAvailableSlotQuery
            {
                DoctorId = 1,
                Date = DateTime.Today.AddDays(1)
            };
            _mockAppointmentService
                .Setup(s => s.GetAvailableSlotsAsync(request.DoctorId, request.Date, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Service error"));
            // Act
            var response = await _handler.Handle(request, CancellationToken.None);
            // Assert
            Assert.False(response.Succeeded);
            Assert.Contains("Error occurred while fetching available slots", response.Message);
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error fetching available slots")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_PastDate_ReturnsBadRequest()
        {
            // Arrange
            var request = new GetAvailableSlotQuery
            {
                DoctorId = 1,
                Date = DateTime.Today.AddDays(-1) // تاريخ في الماضي
            };
            // Act
            var response = await _handler.Handle(request, CancellationToken.None);
            // Assert
            Assert.False(response.Succeeded);
            Assert.Equal("Invalid doctor ID or date provided.", response.Message);
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Invalid request parameters")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
