namespace Clinic_System.Application.Tests.Features.Doctors.CommandsTests.HandlersTests
{
    public class CreateDoctorCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IDoctorService> _mockDoctorService;
        private readonly Mock<IIdentityService> _mockIdentityService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<CreateDoctorCommandHandler>> _mockLogger;
        private readonly CreateDoctorCommandHandler _handler;
        public CreateDoctorCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockDoctorService = new Mock<IDoctorService>();
            _mockIdentityService = new Mock<IIdentityService>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<CreateDoctorCommandHandler>>();

            _handler = new CreateDoctorCommandHandler(_mockDoctorService.Object,
                _mockMapper.Object,
                _mockIdentityService.Object,
                _mockUnitOfWork.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateDoctorSuccessfully()
        {
            // Arrange
            var command = new CreateDoctorCommand
            {
                FullName = "Dr. John Doe",
                UserName = "johndoe",
                Email = "john1@g.c",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Gender = Gender.Male,
                DateOfBirth = new DateTime(1980, 1, 1),
                Specialization = "Cardiology",
                Phone = "01507489484",
                Address = "123 Main"
            };

            var userId = "user-123";
            var doctorEntity = new Doctor { Id = 1, FullName = command.FullName, ApplicationUserId = userId };
            var expectedDto = new CreateDoctorDTO { Id = 1, FullName = command.FullName, Email = command.Email };

            // 1. Setup IdentityService: إنشاء المستخدم وإرجاع الـ ID
            _mockIdentityService
                .Setup(s => s.CreateUserAsync(command.UserName, command.Email, command.Password, "Doctor", It.IsAny<CancellationToken>()))
                .ReturnsAsync(userId);

            // 2. Setup IdentityService: جلب البريد الإلكتروني (يُستدعى في نهاية الـ Handler)
            _mockIdentityService
                .Setup(s => s.GetUserEmailAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(command.Email);


            _mockUnitOfWork
                .Setup(u => u.DoctorsRepository.AddAsync(It.IsAny<Doctor>(),
                It.IsAny<CancellationToken>()));

            _mockDoctorService
                .Setup(c => c.CreateDoctorAsync(It.IsAny<Doctor>(),
                It.IsAny<CancellationToken>()));

            // 3. Setup Mapper: تحويل الـ Command لـ Entity والـ Entity لـ DTO
            _mockMapper.Setup(m => m.Map<Doctor>(command)).Returns(doctorEntity);
            _mockMapper.Setup(m => m.Map<CreateDoctorDTO>(doctorEntity)).Returns(expectedDto);

            _mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockIdentityService.Verify(s => s.CreateUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), "Doctor", It.IsAny<CancellationToken>()), Times.Once);
            _mockDoctorService.Verify(s => s.CreateDoctorAsync(doctorEntity, It.IsAny<CancellationToken>()), Times.Once);
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
            var command = new CreateDoctorCommand
            {
                FullName = "Dr. John Doe",
                UserName = "johndoe",
                Email = "john1@g.c",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Gender = Gender.Male,
                DateOfBirth = new DateTime(1980, 1, 1),
                Specialization = "Cardiology",
                Phone = "01507489484",
                Address = "123 Main"
            };

            var userId = "user-123";
            var doctorEntity = new Doctor { Id = 1, FullName = command.FullName, ApplicationUserId = userId };

            // 1. Setup IdentityService: إنشاء المستخدم وإرجاع الـ ID
            _mockIdentityService
                .Setup(s => s.CreateUserAsync(command.UserName, command.Email, command.Password, "Doctor", It.IsAny<CancellationToken>()))
                .ReturnsAsync(userId);


            // 3. Setup Mapper: تحويل الـ Command لـ Entity والـ Entity لـ DTO
            _mockMapper.Setup(m => m.Map<Doctor>(command)).Returns(doctorEntity);

            _mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(0);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert

            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);

            // التحقق من النتيجة
            Assert.False(result.Succeeded);
            Assert.Equal("Failed to create doctor", result.Message);

        }

        [Theory]
        [InlineData("User creation failed: Email already exists", "Email already exists")]
        [InlineData("User creation failed: Invalid password", "Invalid password")]
        [InlineData("User creation failed: UserName already exists", "UserName already exists")]
        public async Task Handle_IdentityServiceThrowsException_ShouldReturnBadRequest(string exceptionMessage, string expectedInResponse)
        {
            // Arrange
            var command = new CreateDoctorCommand
            {
                FullName = "Dr. John Doe",
                UserName = "johndoe",
                Email = "john1@g.c",
                Password = "Password123!",
                ConfirmPassword = "Password123!",

            };

            var userId = "user-123";
            var doctorEntity = new Doctor { Id = 1, FullName = command.FullName, ApplicationUserId = userId };

            // 1. Setup IdentityService: إنشاء المستخدم وإرجاع الـ ID
            _mockIdentityService
                .Setup(s => s.CreateUserAsync(command.UserName, command.Email, command.Password, "Doctor", It.IsAny<CancellationToken>()))
                .ReturnsAsync(userId);


            // 3. Setup Mapper: تحويل الـ Command لـ Entity والـ Entity لـ DTO
            _mockMapper.Setup(m => m.Map<Doctor>(command)).Returns(doctorEntity);

            _mockDoctorService
                .Setup(c => c.CreateDoctorAsync(It.IsAny<Doctor>(),
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
        public async Task Handle_CreateDoctorServiceThrowsException_ShouldReturnBadRequest(string exceptionMessage, string expectedInResponse)
        {
            // Arrange
            var command = new CreateDoctorCommand
            {
                FullName = "Dr. John Doe",
                UserName = "johndoe",
                Email = "john1@g.c",
                Password = "Password123!",
                ConfirmPassword = "Password123!",

            };

            var userId = "user-123";
            var doctorEntity = new Doctor { Id = 1, FullName = command.FullName, ApplicationUserId = userId };

            // 1. Setup IdentityService: إنشاء المستخدم وإرجاع الـ ID
            _mockIdentityService
                .Setup(s => s.CreateUserAsync(command.UserName, command.Email, command.Password, "Doctor", It.IsAny<CancellationToken>()))
                .ReturnsAsync(userId);


            // 3. Setup Mapper: تحويل الـ Command لـ Entity والـ Entity لـ DTO
            _mockMapper.Setup(m => m.Map<Doctor>(command)).Returns(doctorEntity);

            _mockDoctorService
                .Setup(c => c.CreateDoctorAsync(It.IsAny<Doctor>(),
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