namespace Clinic_System.Application.Tests.Features.Doctors.CommandsTests.HandlersTests
{
    public class UpdateDoctorCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IDoctorService> _mockDoctorService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<UpdateDoctorCommandHandler>> _mockLogger;
        private readonly UpdateDoctorCommandHandler _handler;
        private readonly Mock<ICurrentUserService> _mockCurrentUserService;
        public UpdateDoctorCommandHandlerTests()
        {
            _mockCurrentUserService = new Mock<ICurrentUserService>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockDoctorService = new Mock<IDoctorService>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<UpdateDoctorCommandHandler>>();
            _handler = new UpdateDoctorCommandHandler(_mockDoctorService.Object,
                _mockCurrentUserService.Object,
                _mockMapper.Object,
                _mockUnitOfWork.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_DoctorNotFound_ReturnsNotFoundResponse()
        {
            var doctor = (Doctor?)null;

            _mockDoctorService.Setup(s => s.GetDoctorByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(doctor);

            var command = new UpdateDoctorCommand { Id = 1 };

            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.False(result.Succeeded);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task Handle_SuccessfulUpdate_ReturnsSuccessResponse()
        {
            var doctor = new Doctor
            {
                Id = 1,
                FullName = "Dr. Smith",
                Specialization = "Cardiology"
            };

            _mockDoctorService.Setup(s => s.GetDoctorByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(doctor);

            _mockMapper.Setup(m => m.Map(It.IsAny<UpdateDoctorCommand>(), It.IsAny<Doctor>()))
               .Callback<UpdateDoctorCommand, Doctor>((cmd, doc) =>
               {
                   doc.FullName = cmd.FullName;
                   doc.Specialization = cmd.Specialization;
               });

            _mockUnitOfWork.Setup(u => u.SaveAsync())
                .ReturnsAsync(1);

           

            _mockMapper.Setup(m => m.Map<UpdateDoctorDTO>(It.IsAny<Doctor>()))
                .Returns((Doctor doc) => new UpdateDoctorDTO
                {
                    FullName = doc.FullName,
                    Specialization = doc.Specialization
                });

            var command = new UpdateDoctorCommand
            {
                Id = 1,
                FullName = "Dr. John Smith",
                Specialization = "Neurology"
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.Succeeded);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Dr. John Smith", result.Data.FullName);
            Assert.Equal("Neurology", result.Data.Specialization);
        }

        [Fact]
        public async Task Handle_UpdateFails_ReturnsBadRequestResponse()
        {
            var doctor = new Doctor
            {
                Id = 1,
                FullName = "Dr. Smith",
                Specialization = "Cardiology"
            };
            _mockDoctorService.Setup(s => s.GetDoctorByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(doctor);

            _mockMapper.Setup(m => m.Map(It.IsAny<UpdateDoctorCommand>(), It.IsAny<Doctor>()))
               .Callback<UpdateDoctorCommand, Doctor>((cmd, doc) =>
               {
                   doc.FullName = cmd.FullName;
                   doc.Specialization = cmd.Specialization;
               });

            _mockUnitOfWork.Setup(u => u.SaveAsync())
                .ReturnsAsync(0); // Simulate failure

            var command = new UpdateDoctorCommand
            {
                Id = 1,
                FullName = "Dr. John Smith",
                Specialization = "Neurology"
            };

            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.False(result.Succeeded);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
    }
}
