namespace Clinic_System.Application.Tests.Features.Doctors.CommandsTests.ValidatorsTests
{
    public class UpdateIdentityDoctorValidatorTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IIdentityService> _mockIdentityService;
        private readonly UpdateIdentityDoctorValidator _validator;
        public UpdateIdentityDoctorValidatorTests()
        {
            // تهيئة Mocking لـ IUnitOfWork و Repositories
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockIdentityService = new Mock<IIdentityService>();

            // إنشاء الـ Validator الفعلي
            _validator = new UpdateIdentityDoctorValidator(_mockIdentityService.Object, _mockUnitOfWork.Object);
        }

        private void SetupMocks(int id, string appUserId, string oldEmail, string oldUserName)
        {
            var doctor = new Doctor { Id = id, ApplicationUserId = appUserId };
            var patient = new Patient { Id = id, ApplicationUserId = appUserId };

            // عمل Mock للاثنين لأن الفاليديشن ينادي عليهما بالتبادل
            _mockUnitOfWork.Setup(u => u.DoctorsRepository.GetByIdAsync(id)).ReturnsAsync(doctor);
            _mockUnitOfWork.Setup(u => u.PatientsRepository.GetByIdAsync(id)).ReturnsAsync(patient);

            _mockIdentityService.Setup(s => s.GetUserEmailAsync(appUserId, It.IsAny<CancellationToken>())).ReturnsAsync(oldEmail);
            _mockIdentityService.Setup(s => s.GetUserNameAsync(appUserId, It.IsAny<CancellationToken>())).ReturnsAsync(oldUserName);
        }

        [Fact]
        public async Task Email_WhenNewAndNoUserName_ShouldHaveValidationError()
        {
            // Arrange
            SetupMocks(1, "user123", "old@test.com", "oldUser1");
            var command = new UpdateIdentityDoctorCommand
            {
                Id = 1,
                Email = "new@test.com", // إيميل جديد
                UserName = "" // يوزر نيم فارغ
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            // تم تعديل الرسالة لتطابق الفاليديتور الخاص بك بالضبط
            result.ShouldHaveValidationErrorFor(c => c.Email)
                .WithErrorMessage("To change Email, you must provide Username AND Email must be unique.");
        }

        [Fact]
        public async Task Email_InvalidFormat_ShouldHaveValidationError()
        {
            SetupMocks(1, "user123", "old@test.com", "oldUser1");
            // Arrange
            var command = new UpdateIdentityDoctorCommand
            {
                Id = 1,
                Email = "invalid-email-format",
                UserName = "adhamdr10",
                Password = "Password123!",
                ConfirmPassword = "Password123!"
            };

            // Act
            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Email);
        }

        [Fact]
        public async Task UserName_WhenNewAndNoEmail_ShouldHaveValidationError()
        {
            // Arrange
            SetupMocks(1, "user123", "old@test.com", "oldUser1");
            var command = new UpdateIdentityDoctorCommand
            {
                Id = 1,
                Email = "",
                UserName = "newUser2" // يوزر نيم جديد
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            // تم تعديل الرسالة لتطابق الفاليديتور الخاص بك بالضبط
            result.ShouldHaveValidationErrorFor(c => c.UserName)
                .WithErrorMessage("To change Username, you must provide Email AND Username must be unique.");
        }

        [Theory]
        [InlineData("doctor", "Username must start with a letter and contain at least one number.")] // لا يوجد رقم
        [InlineData("1doctor", "Username must start with a letter and contain at least one number.")] // يبدأ برقم
        [InlineData("_doc1", "Username must start with a letter and contain at least one number.")]  // يبدأ برمز
        public async Task UserName_InvalidFormat_ShouldHaveValidationError(string userName, string expectedMessage)
        {
            SetupMocks(1, "user123", "old@test.com", userName);

            // Arrange
            var command = new UpdateIdentityDoctorCommand
            {
                Id = 1,
                Email = "invalid-email-format",
                UserName = userName,
                Password = "Password123!",
                ConfirmPassword = "Password123!"
            };

            // Act
            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldHaveValidationErrorFor(c => c.UserName)
                .WithErrorMessage(expectedMessage);
        }

        [Fact]
        public async Task Password_NotEmpty_ShouldNotHaveValidationError()
        {
            SetupMocks(1, "user123", "old@test.com", "oldUser1");

            // Arrange
            var command = new UpdateIdentityDoctorCommand
            {
                Id = 1,
                Email = "adhamdr10@g.c",
                UserName = "adhamdr10",
                CurrentPassword = "OldPassword123!",
                Password = "Password123!",
                ConfirmPassword = "Password123!"
            };

            // Act
            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.Password);
        }

        
        [Theory]
        [InlineData("Short1!")]
        [InlineData("nouppercase1!")]
        [InlineData("NOLOWERCASE1!")]
        [InlineData("NoNumber!")]
        [InlineData("NoSpecialChar1")]
        public async Task Password_InvalidFormat_ShouldHaveValidationError(string Password)
        {
            SetupMocks(1, "user123", "old@test.com", "oldUser1");
            // Arrange
            var command = new UpdateIdentityDoctorCommand
            {
                Id = 1,
                CurrentPassword = "OldPassword123!",
                Password = Password,
                ConfirmPassword = "Password123!"
            };
           
            // Act
            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Password);
        }

        [Fact]
        public async Task ConfirmPassword_EqualPassword_ShouldNotHaveValidationError()
        {
            SetupMocks(1, "user123", "old@test.com", "oldUser1");
            // Arrange
            var command = new UpdateIdentityDoctorCommand
            {
                Id = 1,
                CurrentPassword = "OldPassword123!",
                Password = "Password123!",
                ConfirmPassword = "Password123!"
            };

            // Act
            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.ConfirmPassword);
        }

        [Fact]
        public async Task ConfirmPassword_EqualNotPassword_ShouldHaveValidationError()
        {
            SetupMocks(1, "user123", "old@test.com", "oldUser1");
            // Arrange
            var command = new UpdateIdentityDoctorCommand
            {
                Id = 1,
                CurrentPassword = "OldPassword123!",
                Password = "Password123!",
                ConfirmPassword = "Password124!"
            };

            // Act
            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldHaveValidationErrorFor(c => c.ConfirmPassword);
        }

        [Fact]
        public async Task Email_WhenAlreadyExistsInIdentity_ShouldHaveValidationError()
        {
            SetupMocks(1, "user123", "old@test.com", "oldUser1");
            var command = new UpdateIdentityDoctorCommand { Id = 1, Email = "exists@test.com", UserName = "any" };

            _mockIdentityService.Setup(s => s.ExistingEmail(command.Email)).ReturnsAsync(true);

            var result = await _validator.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(c => c.Email)
                .WithErrorMessage("Email is already exists");
        }

        [Fact]
        public async Task UserName_WhenAlreadyExistsInIdentity_ShouldHaveValidationError()
        {
            SetupMocks(1, "user123", "old@test.com", "oldUser1");
            var command = new UpdateIdentityDoctorCommand { Id = 1, Email = "exists@test.com", UserName = "any" };

            _mockIdentityService.Setup(s => s.ExistingUserName(command.UserName))
               .ReturnsAsync(true);

            var result = await _validator.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(c => c.UserName)
                .WithErrorMessage("Username is already exists");
        }

        [Fact]
        public async Task Password_ProvidedWithoutIdentity_ShouldHaveValidationError()
        {
            SetupMocks(1, "user123", "old@test.com", "oldUser1");

            var command = new UpdateIdentityDoctorCommand
            {
                Id = 1,
                Password = "NewPassword123!",
                Email = "",
                UserName = ""
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Password)
                .WithErrorMessage("To change Password, you must provide Email or Username for identity verification.");
        }

        [Fact]
        public async Task CurrentPassword_EmptyWhileUpdatingPassword_ShouldHaveValidationError()
        {
            // Arrange
            SetupMocks(1, "user123", "old@test.com", "oldUser1");
            var command = new UpdateIdentityDoctorCommand
            {
                Id = 1,
                Password = "NewPassword123!",
                ConfirmPassword = "NewPassword123!",
                CurrentPassword = ""
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            // لاحظ هنا أن الفاليديتور الخاص بك يملك رسالتين مختلفتين لنفس الحقل، التيست سيأخذ الرسالة من ApplyCrossFieldRules
            result.ShouldHaveValidationErrorFor(c => c.CurrentPassword)
                .WithErrorMessage("Current password is required to authorize password change.");
        }
    }
}
