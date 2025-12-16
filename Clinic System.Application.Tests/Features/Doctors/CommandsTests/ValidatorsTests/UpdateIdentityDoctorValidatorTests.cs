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

        [Fact]
        public async Task Email_NotEmpty_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new UpdateIdentityDoctorCommand
            {
                Id = 1,
                Email = "adhamdr10@g.c",
                UserName = "adhamdr10",
                Password = "Password123!",
                ConfirmPassword = "Password123!"
            };

            _mockUnitOfWork.Setup(u => u.DoctorsRepository.GetByIdAsync(1))
               .ReturnsAsync(new Doctor { Id = 1});

            // Act
            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.Email);
        }

        [Fact]
        public async Task Email_InvalidFormat_ShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateIdentityDoctorCommand
            {
                Id = 1,
                Email = "invalid-email-format",
                UserName = "adhamdr10",
                Password = "Password123!",
                ConfirmPassword = "Password123!"
            };

            _mockUnitOfWork.Setup(u => u.DoctorsRepository.GetByIdAsync(1))
              .ReturnsAsync(new Doctor { Id = 1 });

            // Act
            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Email);
        }

        [Fact]
        public async Task UserName_NotEmpty_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new UpdateIdentityDoctorCommand
            {
                Id = 1,
                Email = "adhamdr10@g.c",
                UserName = "adhamdr10",
                Password = "Password123!",
                ConfirmPassword = "Password123!"
            };
            _mockUnitOfWork.Setup(u => u.DoctorsRepository.GetByIdAsync(1))
              .ReturnsAsync(new Doctor { Id = 1 });
            // Act
            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.UserName);
        }

        [Theory]
        [InlineData("doctor", "Username must start with a letter and contain at least one number.")] // لا يوجد رقم
        [InlineData("1doctor", "Username must start with a letter and contain at least one number.")] // يبدأ برقم
        [InlineData("_doc1", "Username must start with a letter and contain at least one number.")]  // يبدأ برمز
        public async Task UserName_InvalidFormat_ShouldHaveValidationError(string userName, string expectedMessage)
        {
            // Arrange
            var command = new UpdateIdentityDoctorCommand
            {
                Id = 1,
                Email = "invalid-email-format",
                UserName = userName,
                Password = "Password123!",
                ConfirmPassword = "Password123!"
            };

            _mockUnitOfWork.Setup(u => u.DoctorsRepository.GetByIdAsync(1))
              .ReturnsAsync(new Doctor { Id = 1 });

            // Act
            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldHaveValidationErrorFor(c => c.UserName)
                .WithErrorMessage(expectedMessage);
        }

        [Fact]
        public async Task Password_NotEmpty_ShouldNotHaveValidationError()
        {
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

            _mockUnitOfWork.Setup(u => u.DoctorsRepository.GetByIdAsync(1))
              .ReturnsAsync(new Doctor { Id = 1 });

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
            // Arrange
            var command = new UpdateIdentityDoctorCommand
            {
                Id = 1,
                Email = "adhamdr10@g.c",
                UserName = "adhamdr10",
                CurrentPassword = "OldPassword123!",
                Password = Password,
                ConfirmPassword = "Password123!"
            };
            _mockUnitOfWork.Setup(u => u.DoctorsRepository.GetByIdAsync(1))
              .ReturnsAsync(new Doctor { Id = 1 });


            // Act
            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Password);
        }

        [Fact]
        public async Task ConfirmPassword_EqualPassword_ShouldNotHaveValidationError()
        {
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

            _mockUnitOfWork.Setup(u => u.DoctorsRepository.GetByIdAsync(1))
              .ReturnsAsync(new Doctor { Id = 1 });

            // Act
            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.ConfirmPassword);
        }

        [Fact]
        public async Task ConfirmPassword_EqualNotPassword_ShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateIdentityDoctorCommand
            {
                Id = 1,
                Email = "adhamdr10@g.c",
                UserName = "adhamdr10",
                CurrentPassword = "OldPassword123!",
                Password = "Password123!",
                ConfirmPassword = "Password124!"
            };

            _mockUnitOfWork.Setup(u => u.DoctorsRepository.GetByIdAsync(1))
              .ReturnsAsync(new Doctor { Id = 1 });

            // Act
            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldHaveValidationErrorFor(c => c.ConfirmPassword);
        }

        [Fact]
        public async Task Email_WhenAlreadyExistsInIdentity_ShouldHaveValidationError()
        {
            var command = new UpdateIdentityDoctorCommand {Id = 1, Email = "exists@test.com" };

            _mockUnitOfWork.Setup(u => u.DoctorsRepository.GetByIdAsync(1))
              .ReturnsAsync(new Doctor { Id = 1 });


            _mockIdentityService.Setup(s => s.ExistingEmail(command.Email))
               .ReturnsAsync(true);

            var result = await _validator.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(c => c.Email)
                .WithErrorMessage("Email is already exists");
        }

        [Fact]
        public async Task UserName_WhenAlreadyExistsInIdentity_ShouldHaveValidationError()
        {
            var command = new UpdateIdentityDoctorCommand {Id=1, UserName = "Adhamdr1" };

            _mockUnitOfWork.Setup(u => u.DoctorsRepository.GetByIdAsync(1))
              .ReturnsAsync(new Doctor { Id = 1 });


            _mockIdentityService.Setup(s => s.ExistingUserName(command.UserName))
               .ReturnsAsync(true);

            var result = await _validator.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(c => c.UserName)
                .WithErrorMessage("Username is already exists");
        }

        [Fact]
        public async Task Email_ProvidedWithoutUserName_ShouldHaveValidationError()
        {
            var command = new UpdateIdentityDoctorCommand
            {
                Id = 1,
                Email = "newemail@test.com",
                UserName = ""
            };

            _mockUnitOfWork.Setup(u => u.DoctorsRepository.GetByIdAsync(1))
                .ReturnsAsync(new Doctor { Id = 1 });

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Email)
                .WithErrorMessage("To change Email, you must provide Username");
        }

        [Fact]
        public async Task UserName_ProvidedWithoutEmail_ShouldHaveValidationError()
        {
            var command = new UpdateIdentityDoctorCommand
            {
                Id = 1,
                Email = "",
                UserName = "adhamdr1"
            };

            _mockUnitOfWork.Setup(u => u.DoctorsRepository.GetByIdAsync(1))
                .ReturnsAsync(new Doctor { Id = 1 });

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.UserName)
                .WithErrorMessage("To change Username, you must provide Email");
        }

        [Fact]
        public async Task Password_ProvidedWithoutIdentity_ShouldHaveValidationError()
        {
            var command = new UpdateIdentityDoctorCommand
            {
                Id = 1,
                Password = "NewPassword123!",
                Email = "",
                UserName = ""
            };

            _mockUnitOfWork.Setup(u => u.DoctorsRepository.GetByIdAsync(1))
                .ReturnsAsync(new Doctor { Id = 1 });

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Password)
                .WithErrorMessage("To change Password, you must provide Email or Username");
        }

        [Fact]
        public async Task CurrentPassword_EmptyWhileUpdatingPassword_ShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateIdentityDoctorCommand
            {
                Id = 1,
                Password = "NewPassword123!",
                ConfirmPassword = "NewPassword123!",
                CurrentPassword = "" // فارغ
            };

            _mockUnitOfWork.Setup(u => u.DoctorsRepository.GetByIdAsync(1))
                .ReturnsAsync(new Doctor { Id = 1 });

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.CurrentPassword)
                .WithErrorMessage("Current password is required when updating password.");
        }
    }
}
