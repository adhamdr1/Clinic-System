namespace Clinic_System.Application.Tests.Features.Patients.QueriesTests.HandlersTests
{
    public class PatientListPagingQueryHandlerTests
    {
        private readonly Mock<IPatientService> _mockPatientService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<PatientListPagingQueryHandler>> _mockLogger;
        private readonly PatientListPagingQueryHandler _handler;
        public PatientListPagingQueryHandlerTests()
        {
            _mockPatientService = new Mock<IPatientService>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<PatientListPagingQueryHandler>>();

            _handler = new PatientListPagingQueryHandler(_mockPatientService.Object,
                _mockMapper.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsPagedResult()
        {
            // Arrange
            var request = new GetPatientListPagingQuery { PageNumber = 1, PageSize = 10 };
            var Patients = new PagedResult<Patient>
            (
                new List<Patient>
                {
                    new Patient { Id = 1, FullName = "Dr. Smith" },
                    new Patient { Id = 2, FullName = "Dr. Jones" }
                },
                count: 2,
                pageIndex: 1,
                pageSize: 10
            );
            _mockPatientService.Setup(s => s.GetPatientsListPagingAsync(request.PageNumber, request.PageSize, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Patients);
            _mockMapper.Setup(m => m.Map<List<GetPatientListDTO>>(It.IsAny<List<Patient>>()))
                .Returns(new List<GetPatientListDTO>
                {
                    new GetPatientListDTO { Id = 1, FullName = "Dr. Smith" },
                    new GetPatientListDTO { Id = 2, FullName = "Dr. Jones" }
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
        public async Task Handle_NoPatientsFound_ReturnsNotFound()
        {
            // Arrange
            var request = new GetPatientListPagingQuery { PageNumber = 1, PageSize = 10 };
            var Patients = new PagedResult<Patient>(new List<Patient>(), count: 0, pageIndex: 1, pageSize: 10);

            _mockPatientService.Setup(s => s.GetPatientsListPagingAsync(request.PageNumber, request.PageSize, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Patients);
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
            var request = new GetPatientListPagingQuery { PageNumber = 0, PageSize = 10 };
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
            var request = new GetPatientListPagingQuery { PageNumber = 1, PageSize = 0 };
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
            var request = new GetPatientListPagingQuery { PageNumber = 1, PageSize = 101 };
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
            var request = new GetPatientListPagingQuery { PageNumber = 1, PageSize = 10 };
            var Patients = new PagedResult<Patient>
            (
                new List<Patient>
                {
                    new Patient { Id = 1, FullName = "Dr. Smith" }
                },
                count: 1,
                pageIndex: 1,
                pageSize: 10
            );
            _mockPatientService.Setup(s => s.GetPatientsListPagingAsync(request.PageNumber, request.PageSize, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Patients);
            _mockMapper.Setup(m => m.Map<List<GetPatientListDTO>>(It.IsAny<List<Patient>>()))
                .Returns(new List<GetPatientListDTO>
                {
                    new GetPatientListDTO { Id = 1, FullName = "Dr. Smith" }
                });
            // Act
            var response = await _handler.Handle(request, CancellationToken.None);
            // Assert
            _mockMapper.Verify(m => m.Map<List<GetPatientListDTO>>(It.IsAny<List<Patient>>()), Times.Once);
            Assert.True(response.Succeeded);
            Assert.Single(response.Data.Items);
        }
        }
}
