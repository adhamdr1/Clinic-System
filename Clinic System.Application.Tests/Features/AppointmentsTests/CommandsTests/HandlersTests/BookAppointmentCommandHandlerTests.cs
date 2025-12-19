namespace Clinic_System.Application.Tests.Features.AppointmentsTests.CommandsTests.HandlersTests
{
    public class BookAppointmentCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IAppointmentService> _mockAppointmentService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<BookAppointmentCommandHandler>> _mockLogger;
        private readonly BookAppointmentCommandHandler _handler;
        public BookAppointmentCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockAppointmentService = new Mock<IAppointmentService>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<BookAppointmentCommandHandler>>();

            _handler = new BookAppointmentCommandHandler(_mockAppointmentService.Object,
                _mockMapper.Object, _mockUnitOfWork.Object, _mockLogger.Object);
        }

        [Fact]
        public void Handle_ValidCommand_ShouldBookAppointmentSucceeded()
        {
            // Arrange
            var doctorId = 1;
            var patientId = 1;
            var command = new BookAppointmentCommand { DoctorId = doctorId, PatientId = patientId, AppointmentDate = DateTime.Now.AddDays(1) };
            // تأكد أن الـ Entity والـ DTO متطابقان في السيناريو
            var appointmentEntity = new Appointment { Id = 100, DoctorId = doctorId, PatientId = patientId };
            var expectedDto = new CreateAppointmentDTO { Id = 100 };

            // 1. إعداد الـ Service ليعيد الـ Entity (بدلاً من الافتراضي Null)
            _mockAppointmentService
                .Setup(s => s.BookAppointmentAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointmentEntity);

            // 2. إعداد الـ Repositories لتعيد بيانات الطبيب والمريض
            _mockUnitOfWork.Setup(u => u.DoctorsRepository.GetByIdAsync(doctorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Doctor { Id = doctorId, FullName = "Dr. Smith" });

            _mockUnitOfWork.Setup(u => u.PatientsRepository.GetByIdAsync(patientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Patient { Id = patientId, FullName = "John Doe" });

            // 3. إعداد الـ Mapper
            _mockMapper.Setup(m => m.Map<CreateAppointmentDTO>(appointmentEntity)).Returns(expectedDto);

            // 4. إعداد الحفظ
            _mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(1);

            // Act
            var result = _handler.Handle(command, CancellationToken.None).Result;

            // Assert
            _mockAppointmentService.Verify(s => s.BookAppointmentAsync(command, It.IsAny<CancellationToken>()), Times.Once);


            Assert.True(result.Succeeded);
            Assert.Equal("Appointment booked successfully.", result.Message);

        }

        [Theory]
        [InlineData("Doctor is not available at this time", "not available")]
        [InlineData("Patient already has an appointment", "already has")]
        [InlineData("Doctor is on vacation", "vacation")]
        public async Task Handle_InValidCommand_ShouldReturnBadRequest(string exceptionMessage, string expectedInResponse)
        {
            // Arrange
            var command = new BookAppointmentCommand
            {
                DoctorId = 1,
                PatientId = 1,
                AppointmentDate = DateTime.Now.AddDays(1)
            };

            _mockUnitOfWork.Setup(u => u.PatientsRepository.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new Patient { Id = 1 });

            // محاكاة رمي استثناء برسائل مختلفة بناءً على الـ InlineData
            _mockAppointmentService
                .Setup(s => s.BookAppointmentAsync(It.IsAny<BookAppointmentCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            // 1. التأكد أن العملية فشلت
            Assert.False(result.Succeeded);

            // 2. التأكد أن رسالة الخطأ تحتوي على الجزء المطلوب (Case-Insensitive check)
            Assert.Contains(expectedInResponse.ToLower(), result.Message.ToLower());

            // 3. التأكد من حماية قاعدة البيانات: عدم استدعاء الحفظ عند حدوث خطأ في الخدمة
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_PatientNotFound_ShouldReturnBadRequest()
        {
            // Arrange
            var command = new BookAppointmentCommand { PatientId = 999 };

            // محاكاة إرجاع null عند البحث عن المريض
            _mockUnitOfWork.Setup(u => u.PatientsRepository.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patient)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("Patient account not found.", result.Message);

            // التأكد أن الـ Service لم يتم استدعاؤه أبداً توفيراً للأداء وحماية للمنطق
            _mockAppointmentService.Verify(s => s.BookAppointmentAsync(It.IsAny<BookAppointmentCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
