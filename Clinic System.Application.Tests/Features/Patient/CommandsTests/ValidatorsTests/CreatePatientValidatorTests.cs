namespace Clinic_System.Application.Tests.Features.Patients.CommandsTests.ValidatorsTests
{
    public class CreatePatientValidatorTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IDoctorRepository> _mockDoctorRepo;
        private readonly Mock<IPatientRepository> _mockPatientRepo;
        private readonly Mock<IIdentityService> _mockIdentityService;
        private readonly CreatePatientValidator _validator;
        public CreatePatientValidatorTests()
        {
            // تهيئة Mocking لـ IUnitOfWork و Repositories
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockDoctorRepo = new Mock<IDoctorRepository>();
            _mockPatientRepo = new Mock<IPatientRepository>();
            _mockIdentityService = new Mock<IIdentityService>();

            // ربط الـ Repositories بالـ UnitOfWork
            _mockUnitOfWork.SetupGet(u => u.DoctorsRepository).Returns(_mockDoctorRepo.Object);
            _mockUnitOfWork.SetupGet(u => u.PatientsRepository).Returns(_mockPatientRepo.Object);

            // إنشاء الـ Validator الفعلي
            _validator = new CreatePatientValidator(_mockIdentityService.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task PatientName_NotEmpty_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new CreatePatientCommand
            {
                FullName = "Dr. John Doe",
                Address = "123 Main St",
                Phone = "+12345678901",
                Email = "adhamdr10@g.c",
                UserName = "adhamdr10",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                DateOfBirth = DateTime.Now.AddYears(-30),
                Gender = Gender.Male
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.FullName);

        }

        [Fact]
        public async Task PatientName_Empty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new CreatePatientCommand
            {
                FullName = "",
                Address = "123 Main St",
                Phone = "+12345678901",
                Email = "adhamdr10@g.c",
                UserName = "adhamdr10",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                DateOfBirth = DateTime.Now.AddYears(-30),
                Gender = Gender.Male
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.FullName);
        }

        [Fact]
        public async Task PatientName_LengthMoreThan100_ShouldHaveValidationError()
        {
            // Arrange
            var command = new CreatePatientCommand
            {
                FullName = "fasdhkfhjkdashgjkdhkgjvdfjksbnvkhjsdfbjkbsdfjvbjsd" +
                "fdsjivjkdsfnbjkvndfjkbnvksndfkjbvnkfgjsnbkfgnkjbnjknskjbgnksfnbknfsgnbjsnbkjngsnfjoadijgjioe",
                Address = "123 Main St",
                Phone = "+12345678901",
                Email = "adhamdr10@g.c",
                UserName = "adhamdr10",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                DateOfBirth = DateTime.Now.AddYears(-30),
                Gender = Gender.Male
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.FullName);
        }

        [Fact]
        public async Task Addrees_NotEmpty_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new CreatePatientCommand
            {
                FullName = "Dr. John Doe",
                Address = "123 Main St",
                Phone = "+12345678901",
                Email = "adhamdr10@g.c",
                UserName = "adhamdr10",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                DateOfBirth = DateTime.Now.AddYears(-30),
                Gender = Gender.Male
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.Address);
        }

        [Fact]
        public async Task Addrees_Empty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new CreatePatientCommand
            {
                FullName = "Dr. John Doe",
                Address = "",
                Phone = "+12345678901",
                Email = "adhamdr10@g.c",
                UserName = "adhamdr10",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                DateOfBirth = DateTime.Now.AddYears(-30),
                Gender = Gender.Male
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Address);
        }

        [Fact]
        public async Task Phone_NotEmpty_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new CreatePatientCommand
            {
                FullName = "Dr. John Doe",
                Address = "123 Main St",
                Phone = "+12345678901",
                Email = "adhamdr10@g.c",
                UserName = "adhamdr10",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                DateOfBirth = DateTime.Now.AddYears(-30),
                Gender = Gender.Male
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.Phone);
        }

        [Fact]
        public async Task Phone_Empty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new CreatePatientCommand
            {
                FullName = "Dr. John Doe",
                Address = "123 Main St",
                Phone = "",
                Email = "adhamdr10@g.c",
                UserName = "adhamdr10",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                DateOfBirth = DateTime.Now.AddYears(-30),
                Gender = Gender.Male
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Phone);
        }

        [Fact]
        public async Task Phone_InvalidFormat_ShouldHaveValidationError()
        {
            // Arrange
            var command = new CreatePatientCommand
            {
                FullName = "Dr. John Doe",
                Address = "123 Main St",
                Phone = "InvalidPhoneNumber",
                Email = "adhamdr10@g.c",
                UserName = "adhamdr10",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                DateOfBirth = DateTime.Now.AddYears(-30),
                Gender = Gender.Male
            };

            // Act
            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Phone);
        }

        [Fact]
        public async Task Email_NotEmpty_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new CreatePatientCommand
            {
                FullName = "Dr. John Doe",
                Address = "123 Main St",
                Phone = "+12345678901",
                Email = "adhamdr10@g.c",
                UserName = "adhamdr10",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                DateOfBirth = DateTime.Now.AddYears(-30),
                Gender = Gender.Male
            };
            // Act
            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.Email);
        }

        [Fact]
        public async Task Email_Empty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new CreatePatientCommand
            {
                FullName = "Dr. John Doe",
                Address = "123 Main St",
                Phone = "+12345678901",
                Email = "",
                UserName = "adhamdr10",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                DateOfBirth = DateTime.Now.AddYears(-30),
                Gender = Gender.Male
            };

            // Act

            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Email);
        }

        [Fact]
        public async Task Email_InvalidFormat_ShouldHaveValidationError()
        {
            // Arrange
            var command = new CreatePatientCommand
            {
                FullName = "Dr. John Doe",
                Address = "123 Main St",
                Phone = "+12345678901",
                Email = "invalid-email-format",
                UserName = "adhamdr10",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                DateOfBirth = DateTime.Now.AddYears(-30),
                Gender = Gender.Male
            };
            // Act
            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Email);
        }

        [Fact]
        public async Task UserName_NotEmpty_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new CreatePatientCommand
            {
                FullName = "Dr. John Doe",
                Address = "123 Main St",
                Phone = "+12345678901",
                Email = "adhamdr10@g.c",
                UserName = "adhamdr10",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                DateOfBirth = DateTime.Now.AddYears(-30),
                Gender = Gender.Male
            };
            // Act
            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.UserName);
        }

        [Fact]
        public async Task UserName_Empty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new CreatePatientCommand
            {
                FullName = "Dr. John Doe",
                Address = "123 Main St",
                Phone = "+12345678901",
                Email = "",
                UserName = "",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                DateOfBirth = DateTime.Now.AddYears(-30),
                Gender = Gender.Male
            };

            // Act

            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldHaveValidationErrorFor(c => c.UserName);
        }

        [Theory]
        [InlineData("patient", "Username must start with a letter and contain at least one number.")] // لا يوجد رقم
        [InlineData("1patient", "Username must start with a letter and contain at least one number.")] // يبدأ برقم
        [InlineData("_pat1", "Username must start with a letter and contain at least one number.")]  // يبدأ برمز
        public async Task UserName_InvalidFormat_ShouldHaveValidationError(string userName , string expectedMessage)
        {
            // Arrange
            var command = new CreatePatientCommand
            {
                FullName = "Dr. John Doe",
                Address = "123 Main St",
                Phone = "+12345678901",
                Email = "invalid-email-format",
                UserName = userName,
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                DateOfBirth = DateTime.Now.AddYears(-30),
                Gender = Gender.Male
            };
            // Act
            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldHaveValidationErrorFor(c => c.UserName)
                .WithErrorMessage(expectedMessage);
        }

        [Fact]
        public async Task DateOfBirth_NotEmpty_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new CreatePatientCommand
            {
                FullName = "Dr. John Doe",
                Address = "123 Main St",
                Phone = "+12345678901",
                Email = "adhamdr10@g.c",
                UserName = "adhamdr10",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                DateOfBirth = DateTime.Now.AddYears(-30),
                Gender = Gender.Male
            };
            // Act
            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.DateOfBirth);
        }

        [Fact]
        public async Task DateOfBirth_Empty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new CreatePatientCommand
            {
                FullName = "Dr. John Doe",
                Address = "123 Main St",
                Phone = "+12345678901",
                Email = "",
                UserName = "",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Gender = Gender.Male
            };

            // Act

            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldHaveValidationErrorFor(c => c.DateOfBirth);
        }

        [Fact]
        public async Task DateOfBirth_InvalidFormat_ShouldHaveValidationError()
        {
            // Arrange
            var command = new CreatePatientCommand
            {
                FullName = "Dr. John Doe",
                Address = "123 Main St",
                Phone = "+12345678901",
                Email = "invalid-email-format",
                UserName = "1invalidUsername",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                DateOfBirth = DateTime.Now.AddYears(1),
                Gender = Gender.Male
            };
            // Act
            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldHaveValidationErrorFor(c => c.DateOfBirth);
        }

        [Fact]
        public async Task Password_NotEmpty_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new CreatePatientCommand
            {
                FullName = "Dr. John Doe",
                Address = "123 Main St",
                Phone = "+12345678901",
                Email = "adhamdr10@g.c",
                UserName = "adhamdr10",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                DateOfBirth = DateTime.Now.AddYears(-30),
                Gender = Gender.Male
            };
            // Act
            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.Password);
        }

        [Fact]
        public async Task Password_Empty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new CreatePatientCommand
            {
                FullName = "Dr. John Doe",
                Address = "123 Main St",
                Phone = "+12345678901",
                Email = "adhamdr10@g.c",
                UserName = "adhamdr10",
                Password = "",
                ConfirmPassword = "Password123!",
                DateOfBirth = DateTime.Now.AddYears(-30),
                Gender = Gender.Male
            };

            // Act

            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Password);
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
            var command = new CreatePatientCommand
            {
                FullName = "Dr. John Doe",
                Address = "123 Main St",
                Phone = "+12345678901",
                Email = "adhamdr10@g.c",
                UserName = "adhamdr10",
                Password = Password,
                ConfirmPassword = "Password123!",
                DateOfBirth = DateTime.Now.AddYears(-30),
                Gender = Gender.Male
            };
            // Act
            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Password);
        }

        [Fact]
        public async Task ConfirmPassword_EqualPassword_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new CreatePatientCommand
            {
                FullName = "Dr. John Doe",
                Address = "123 Main St",
                Phone = "+12345678901",
                Email = "adhamdr10@g.c",
                UserName = "adhamdr10",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                DateOfBirth = DateTime.Now.AddYears(-30),
                Gender = Gender.Male
            };
            // Act
            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.ConfirmPassword);
        }

        [Fact]
        public async Task ConfirmPassword_EqualNotPassword_ShouldHaveValidationError()
        {
            // Arrange
            var command = new CreatePatientCommand
            {
                FullName = "Dr. John Doe",
                Address = "123 Main St",
                Phone = "+12345678901",
                Email = "adhamdr10@g.c",
                UserName = "adhamdr10",
                Password = "Password123!",
                ConfirmPassword = "Password124!",
                DateOfBirth = DateTime.Now.AddYears(-30),
                Gender = Gender.Male
            };
            // Act
            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldHaveValidationErrorFor(c => c.ConfirmPassword);
        }

        [Fact]
        public async Task All_ValidFields_ShouldNotHaveAnyValidationErrors()
        {
            // Arrange
            var command = new CreatePatientCommand
            {
                FullName = "Dr. John Doe",
                Address = "123 Main St",
                Phone = "+12345678901",
                Email = "adhamdr10@g.c",
                UserName = "adhamdr10",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                DateOfBirth = DateTime.Now.AddYears(-30),
                Gender = Gender.Male
            };
            // Act
            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task All_EmptyFields_ShouldHaveValidationErrors()
        {
            // Arrange
            var command = new CreatePatientCommand
            {
                FullName = "",
                Address = "",
                Phone = "",
                Email = "",
                UserName = "",
                Password = "",
                ConfirmPassword = "",
            };
            // Act
            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldHaveValidationErrorFor(c => c.FullName);
            result.ShouldHaveValidationErrorFor(c => c.Address);
            result.ShouldHaveValidationErrorFor(c => c.Phone);
            result.ShouldHaveValidationErrorFor(c => c.Email);
            result.ShouldHaveValidationErrorFor(c => c.UserName);
            result.ShouldHaveValidationErrorFor(c => c.Password);
            result.ShouldHaveValidationErrorFor(c => c.DateOfBirth);
        }

        [Fact]
        public async Task Email_WhenAlreadyExistsInIdentity_ShouldHaveValidationError()
        {
            var command = new CreatePatientCommand { Email = "exists@test.com" };

            _mockIdentityService.Setup(s => s.ExistingEmail(command.Email))
               .ReturnsAsync(true);

            var result = await _validator.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(c => c.Email)
                .WithErrorMessage("Email is already exists");
        }

        [Fact]
        public async Task Email_WhenNotExistsInIdentity_ShouldNotHaveValidationError()
        {
            var command = new CreatePatientCommand { Email = "Notexists@test.com" };

            _mockIdentityService.Setup(s => s.ExistingEmail(command.Email))
               .ReturnsAsync(false);

            var result = await _validator.TestValidateAsync(command);

            result.ShouldNotHaveValidationErrorFor(c => c.Email);
        }

        [Fact]
        public async Task UserName_WhenAlreadyExistsInIdentity_ShouldHaveValidationError()
        {
            var command = new CreatePatientCommand { UserName = "Adhamdr1" };

            _mockIdentityService.Setup(s => s.ExistingUserName(command.UserName))
               .ReturnsAsync(true);

            var result = await _validator.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(c => c.UserName)
                .WithErrorMessage("Username is already exists");
        }

        [Fact]
        public async Task UserName_WhenNotExistsInIdentity_ShouldNotHaveValidationError()
        {
            var command = new CreatePatientCommand { UserName = "Adhamdr1" };

            _mockIdentityService.Setup(s => s.ExistingUserName(command.UserName))
               .ReturnsAsync(false);

            var result = await _validator.TestValidateAsync(command);

            result.ShouldNotHaveValidationErrorFor(c => c.UserName);
        }

        [Fact]
        public async Task Phone_WhenAlreadyExistsInPatient_ShouldHaveValidationError()
        {
            var command = new CreatePatientCommand { Phone = "01507489484" };

            // محاكاة أنه غير موجود في الدكاترة ولكن موجود في المرضى
            _mockDoctorRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Doctor, bool>>>()))
                .ReturnsAsync(new List<Doctor>());

            _mockPatientRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Patient, bool>>>()))
                .ReturnsAsync(new List<Patient> { new Patient() }); // موجود هنا

            var result = await _validator.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(c => c.Phone)
                .WithErrorMessage("Phone number is already exists");
        }

        [Fact]
        public async Task Phone_WhenAlreadyExistsInDoctor_ShouldHaveValidationError()
        {
            var command = new CreatePatientCommand { Phone = "01507489484" };

            // محاكاة أنه غير موجود في المرضى ولكن موجود في الدكاترة
            _mockDoctorRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Doctor, bool>>>()))
                .ReturnsAsync(new List<Doctor> { new Doctor() });// موجود هنا

            _mockPatientRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Patient, bool>>>()))
                .ReturnsAsync(new List<Patient>()); 

            var result = await _validator.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(c => c.Phone)
                .WithErrorMessage("Phone number is already exists");
        }

        [Fact]
        public async Task Phone_WhenNotExistsInSystem_ShouldNotHaveValidationError()
        {
            var command = new CreatePatientCommand { Phone = "01507489484" };

            // محاكاة أنه غير موجود في المرضى وأنه غير موجود في الدكاترة
            _mockDoctorRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Doctor, bool>>>()))
                .ReturnsAsync(new List<Doctor>());

            _mockPatientRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Patient, bool>>>()))
                .ReturnsAsync(new List<Patient>());

            var result = await _validator.TestValidateAsync(command);

            result.ShouldNotHaveValidationErrorFor(c => c.Phone);
        }
    }
}