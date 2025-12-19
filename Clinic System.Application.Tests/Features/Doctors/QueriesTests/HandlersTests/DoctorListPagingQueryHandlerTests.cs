namespace Clinic_System.Application.Tests.Features.Doctors.QueriesTests.HandlersTests
{
    public class DoctorListPagingQueryHandlerTests
    {
        private readonly Mock<IDoctorService> _mockDoctorService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<DoctorListPagingQueryHandler>> _mockLogger;
        private readonly DoctorListPagingQueryHandler _handler;
        public DoctorListPagingQueryHandlerTests()
        {
            _mockDoctorService = new Mock<IDoctorService>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<DoctorListPagingQueryHandler>>();

            _handler = new DoctorListPagingQueryHandler(_mockDoctorService.Object,
                _mockMapper.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsPagedResult()
        {
            // Arrange
            var request = new GetDoctorListPagingQuery { PageNumber = 1, PageSize = 10 };
            var doctors = new PagedResult<Doctor>
            (
                new List<Doctor>
                {
                    new Doctor { Id = 1, FullName = "Dr. Smith" },
                    new Doctor { Id = 2, FullName = "Dr. Jones" }
                },
                count: 2,
                pageIndex: 1,
                pageSize: 10
            );
            _mockDoctorService.Setup(s => s.GetDoctorsListPagingAsync(request.PageNumber, request.PageSize, It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctors);
            _mockMapper.Setup(m => m.Map<List<GetDoctorListDTO>>(It.IsAny<List<Doctor>>()))
                .Returns(new List<GetDoctorListDTO>
                {
                    new GetDoctorListDTO { Id = 1, FullName = "Dr. Smith" },
                    new GetDoctorListDTO { Id = 2, FullName = "Dr. Jones" }
                });
            // Act
            var response = await _handler.Handle(request, CancellationToken.None);
            // Assert
            Assert.True(response.Succeeded);
            Assert.NotNull(response.Data);
            Assert.Equal(2, response.Data.TotalCount);
            Assert.Equal(1, response.Data.CurrentPage);
            Assert.Equal(10, response.Data.PageSize);
            Assert.Equal(2, response.Data.Items.Count());
        }

        [Fact]
        public async Task Handle_NoDoctorsFound_ReturnsNotFound()
        {
            // Arrange
            var request = new GetDoctorListPagingQuery { PageNumber = 1, PageSize = 10 };
            var doctors = new PagedResult<Doctor>(new List<Doctor>(), count: 0, pageIndex: 1, pageSize: 10);

            _mockDoctorService.Setup(s => s.GetDoctorsListPagingAsync(request.PageNumber, request.PageSize, It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctors);
            // Act
            var response = await _handler.Handle(request, CancellationToken.None);
            // Assert
            Assert.False(response.Succeeded);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Handle_InvalidPageNumber_ReturnsBadRequest()
        {
            // Arrange
            var request = new GetDoctorListPagingQuery { PageNumber = 0, PageSize = 10 };
            // Act
            var response = await _handler.Handle(request, CancellationToken.None);
            // Assert
            Assert.False(response.Succeeded);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("Page number must be greater than 0", response.Message);
        }

        [Fact]
        public async Task Handle_InvalidPageSize_ReturnsBadRequest()
        {
            // Arrange
            var request = new GetDoctorListPagingQuery { PageNumber = 1, PageSize = 0 };
            // Act
            var response = await _handler.Handle(request, CancellationToken.None);
            // Assert
            Assert.False(response.Succeeded);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("Page size must be between 1 and 100", response.Message);
        }

        [Fact]
        public async Task Handle_PageSizeExceedsLimit_ReturnsBadRequest()
        {
            // Arrange
            var request = new GetDoctorListPagingQuery { PageNumber = 1, PageSize = 101 };
            // Act
            var response = await _handler.Handle(request, CancellationToken.None);
            // Assert
            Assert.False(response.Succeeded);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("Page size must be between 1 and 100", response.Message);
        }

        [Fact]
        public async Task Handle_MapperCalled_CorrectlyMapsEntities()
        {
            // Arrange
            var request = new GetDoctorListPagingQuery { PageNumber = 1, PageSize = 10 };
            var doctors = new PagedResult<Doctor>
            (
                new List<Doctor>
                {
                    new Doctor { Id = 1, FullName = "Dr. Smith" }
                },
                count: 1,
                pageIndex: 1,
                pageSize: 10
            );
            _mockDoctorService.Setup(s => s.GetDoctorsListPagingAsync(request.PageNumber, request.PageSize, It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctors);
            _mockMapper.Setup(m => m.Map<List<GetDoctorListDTO>>(It.IsAny<List<Doctor>>()))
                .Returns(new List<GetDoctorListDTO>
                {
                    new GetDoctorListDTO { Id = 1, FullName = "Dr. Smith" }
                });
            // Act
            var response = await _handler.Handle(request, CancellationToken.None);
            // Assert
            _mockMapper.Verify(m => m.Map<List<GetDoctorListDTO>>(It.IsAny<List<Doctor>>()), Times.Once);
            Assert.True(response.Succeeded);
            Assert.Single(response.Data.Items);
        }
        }
}
