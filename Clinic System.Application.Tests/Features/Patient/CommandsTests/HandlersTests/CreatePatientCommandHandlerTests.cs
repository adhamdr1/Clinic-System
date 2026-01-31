namespace Clinic_System.Application.Tests.Features.Patients.CommandsTests.HandlersTests
{
    public class CreatePatientCommandHandlerTests
    {
        private readonly Mock<IEmailService> mockEmailService;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IPatientService> _mockPatientService;
        private readonly Mock<IIdentityService> _mockIdentityService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<CreatePatientCommandHandler>> _mockLogger;
        private readonly CreatePatientCommandHandler _handler;
        public CreatePatientCommandHandlerTests()
        {
            mockEmailService = new Mock<IEmailService>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockPatientService = new Mock<IPatientService>();
            _mockIdentityService = new Mock<IIdentityService>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<CreatePatientCommandHandler>>();

            _handler = new CreatePatientCommandHandler(
                _mockPatientService.Object,
                _mockMapper.Object,
                _mockIdentityService.Object,
                _mockUnitOfWork.Object,
                mockEmailService.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreatePatientSuccessfully()
        {
            // Arrange
            var command = new CreatePatientCommand
            {
                FullName = "Dr. John Doe",
                UserName = "johndoe",
                Email = "john1@g.c",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Gender = Gender.Male,
                DateOfBirth = new DateTime(1980, 1, 1),
                Phone = "01507489484",
                Address = "123 Main"
            };

            var userId = "user-123";
            var PatientEntity = new Patient { Id = 1, FullName = command.FullName, ApplicationUserId = userId };
            var expectedDto = new CreatePatientDTO { Id = 1, FullName = command.FullName, Email = command.Email };

            // 1. Setup IdentityService: إنشاء المستخدم وإرجاع الـ ID
            _mockIdentityService
                .Setup(s => s.CreateUserAsync(command.UserName, command.Email, command.Password, "Patient", It.IsAny<CancellationToken>()))
                .ReturnsAsync(userId);

            // 2. Setup IdentityService: جلب البريد الإلكتروني (يُستدعى في نهاية الـ Handler)
            _mockIdentityService
                .Setup(s => s.GetUserEmailAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(command.Email);


            _mockUnitOfWork
                .Setup(u => u.PatientsRepository.AddAsync(It.IsAny<Patient>(),
                It.IsAny<CancellationToken>()));

            _mockPatientService
                .Setup(c => c.CreatePatientAsync(It.IsAny<Patient>(),
                It.IsAny<CancellationToken>()));

            // 3. Setup Mapper: تحويل الـ Command لـ Entity والـ Entity لـ DTO
            _mockMapper.Setup(m => m.Map<Patient>(command)).Returns(PatientEntity);
            _mockMapper.Setup(m => m.Map<CreatePatientDTO>(PatientEntity)).Returns(expectedDto);

            _mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockIdentityService.Verify(s => s.CreateUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), "Patient", It.IsAny<CancellationToken>()), Times.Once);
            _mockPatientService.Verify(s => s.CreatePatientAsync(PatientEntity, It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);

            // التحقق من النتيجة
            result.Succeeded.Should().BeTrue();
            result.Data.Email.Should().Be(command.Email);
            result.Message.Should().Be("Created");
        }

        [Fact]
        public async Task Handle_SaveAsyncReturnsZero_ShouldReturnBadRequest()
        {
            // Arrange
            var command = new CreatePatientCommand
            {
                FullName = "Dr. John Doe",
                UserName = "johndoe",
                Email = "john1@g.c",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Gender = Gender.Male,
                DateOfBirth = new DateTime(1980, 1, 1),
                Phone = "01507489484",
                Address = "123 Main"
            };

            var userId = "user-123";
            var PatientEntity = new Patient { Id = 1, FullName = command.FullName, ApplicationUserId = userId };

            // 1. Setup IdentityService: إنشاء المستخدم وإرجاع الـ ID
            _mockIdentityService
                .Setup(s => s.CreateUserAsync(command.UserName, command.Email, command.Password, "Patient", It.IsAny<CancellationToken>()))
                .ReturnsAsync(userId);


            // 3. Setup Mapper: تحويل الـ Command لـ Entity والـ Entity لـ DTO
            _mockMapper.Setup(m => m.Map<Patient>(command)).Returns(PatientEntity);

            _mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(0);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert

            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);

            // التحقق من النتيجة
            Assert.False(result.Succeeded);
            Assert.Equal("Failed to create patient", result.Message);

        }

        [Theory]
        [InlineData("User creation failed: Email already exists", "Email already exists")]
        [InlineData("User creation failed: Invalid password", "Invalid password")]
        [InlineData("User creation failed: UserName already exists", "UserName already exists")]
        public async Task Handle_IdentityServiceThrowsException_ShouldReturnBadRequest(string exceptionMessage, string expectedInResponse)
        {
            // Arrange
            var command = new CreatePatientCommand
            {
                FullName = "Dr. John Doe",
                UserName = "johndoe",
                Email = "john1@g.c",
                Password = "Password123!",
                ConfirmPassword = "Password123!",

            };

            var userId = "user-123";
            var PatientEntity = new Patient { Id = 1, FullName = command.FullName, ApplicationUserId = userId };

            // 1. Setup IdentityService: إنشاء المستخدم وإرجاع الـ ID
            _mockIdentityService
                .Setup(s => s.CreateUserAsync(command.UserName, command.Email, command.Password, "Patient", It.IsAny<CancellationToken>()))
                .ReturnsAsync(userId);


            // 3. Setup Mapper: تحويل الـ Command لـ Entity والـ Entity لـ DTO
            _mockMapper.Setup(m => m.Map<Patient>(command)).Returns(PatientEntity);

            _mockPatientService
                .Setup(c => c.CreatePatientAsync(It.IsAny<Patient>(),
                It.IsAny<CancellationToken>()))
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

        [Theory]
        [InlineData("Profile creation failed: FullName Invalid", "FullName Invalid")]
        [InlineData("Database connection timeout", "Database connection")]
        [InlineData("Selected specialization is no longer active", "specialization")]
        [InlineData("Internal server error occurred", "Internal server error")]
        public async Task Handle_CreatePatientServiceThrowsException_ShouldReturnBadRequest(string exceptionMessage, string expectedInResponse)
        {
            // Arrange
            var command = new CreatePatientCommand
            {
                FullName = "Dr. John Doe",
                UserName = "johndoe",
                Email = "john1@g.c",
                Password = "Password123!",
                ConfirmPassword = "Password123!",

            };

            var userId = "user-123";
            var PatientEntity = new Patient { Id = 1, FullName = command.FullName, ApplicationUserId = userId };

            // 1. Setup IdentityService: إنشاء المستخدم وإرجاع الـ ID
            _mockIdentityService
                .Setup(s => s.CreateUserAsync(command.UserName, command.Email, command.Password, "Patient", It.IsAny<CancellationToken>()))
                .ReturnsAsync(userId);


            // 3. Setup Mapper: تحويل الـ Command لـ Entity والـ Entity لـ DTO
            _mockMapper.Setup(m => m.Map<Patient>(command)).Returns(PatientEntity);

            _mockPatientService
                .Setup(c => c.CreatePatientAsync(It.IsAny<Patient>(),
                It.IsAny<CancellationToken>()))
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
    }
}