namespace Clinic_System.Application.Tests.Features.Doctors.CommandsTests.ValidatorsTests
{
    public class UpdateDoctorValidatorTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IDoctorRepository> _mockDoctorRepo;
        private readonly Mock<IPatientRepository> _mockPatientRepo;
        private readonly UpdateDoctorValidator _validator;
        public UpdateDoctorValidatorTests()
        {
            // تهيئة Mocking لـ IUnitOfWork و Repositories
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockDoctorRepo = new Mock<IDoctorRepository>();
            _mockPatientRepo = new Mock<IPatientRepository>();

            // ربط الـ Repositories بالـ UnitOfWork
            _mockUnitOfWork.SetupGet(u => u.DoctorsRepository).Returns(_mockDoctorRepo.Object);
            _mockUnitOfWork.SetupGet(u => u.PatientsRepository).Returns(_mockPatientRepo.Object);

            // إنشاء الـ Validator الفعلي
            _validator = new UpdateDoctorValidator(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task DoctorName_NotEmpty_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new UpdateDoctorCommand
            {
                FullName = "Dr. John Doe",
                Address = "123 Main St",
                Specialization = "Cardiology",
                Phone = "+12345678901"
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.FullName);

        }

        [Fact]
        public async Task DoctorName_Empty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateDoctorCommand
            {
                FullName = "",
                Address = "123 Main St",
                Specialization = "Cardiology",
                Phone = "+12345678901"
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.FullName);
        }

        [Fact]
        public async Task DoctorName_LengthMoreThan100_ShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateDoctorCommand
            {
                FullName = "fasdhkfhjkdashgjkdhkgjvdfjksbnvkhjsdfbjkbsdfjvbjsd" +
                "fdsjivjkdsfnbjkvndfjkbnvksndfkjbvnkfgjsnbkfgnkjbnjknskjbgnksfnbknfsgnbjsnbkjngsnfjoadijgjioe",
                Address = "123 Main St",
                Specialization = "Cardiology",
                Phone = "+12345678901"
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
            var command = new UpdateDoctorCommand
            {
                FullName = "Dr. John Doe",
                Address = "123 Main St",
                Specialization = "Cardiology",
                Phone = "+12345678901"
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
            var command = new UpdateDoctorCommand
            {
                FullName = "Dr. John Doe",
                Address = "",
                Specialization = "Cardiology",
                Phone = "+12345678901"
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Address);
        }

        [Fact]
        public async Task Specialization_NotEmpty_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new UpdateDoctorCommand
            {
                FullName = "Dr. John Doe",
                Address = "123 Main St",
                Specialization = "Cardiology",
                Phone = "+12345678901"
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.Specialization);
        }

        [Fact]
        public async Task Specialization_Empty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateDoctorCommand
            {
                FullName = "Dr. John Doe",
                Address = "123 Main St",
                Specialization = "",
                Phone = "+12345678901"
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Specialization);
        }

        [Fact]
        public async Task Phone_NotEmpty_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new UpdateDoctorCommand
            {
                FullName = "Dr. John Doe",
                Address = "123 Main St",
                Specialization = "Cardiology",
                Phone = "+12345678901"
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
            var command = new UpdateDoctorCommand
            {
                FullName = "Dr. John Doe",
                Address = "123 Main St",
                Specialization = "Cardiology",
                Phone = ""
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
            var command = new UpdateDoctorCommand
            {
                FullName = "Dr. John Doe",
                Address = "123 Main St",
                Specialization = "Cardiology",
                Phone = "InvalidPhoneNumber"
            };

            // Act
            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Phone);
        }

        [Fact]
        public async Task All_ValidFields_ShouldNotHaveAnyValidationErrors()
        {
            // Arrange
            var command = new UpdateDoctorCommand
            {
                Id = 1,
                FullName = "Dr. John Doe",
                Address = "123 Main St",
                Specialization = "Cardiology",
                Phone = "+12345678901"
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
            var command = new UpdateDoctorCommand
            {
                FullName = "",
                Address = "",
                Specialization = "",
                Phone = ""
            };
            // Act
            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldHaveValidationErrorFor(c => c.FullName);
            result.ShouldHaveValidationErrorFor(c => c.Address);
            result.ShouldHaveValidationErrorFor(c => c.Specialization);
            result.ShouldHaveValidationErrorFor(c => c.Phone);
        }

        [Fact]
        public async Task Phone_WhenAlreadyExistsInPatient_ShouldHaveValidationError()
        {
            var command = new UpdateDoctorCommand { Phone = "01507489484" };

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
            var command = new UpdateDoctorCommand {Id = 1 , Phone = "01507489484" };

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
            var command = new UpdateDoctorCommand { Phone = "01507489484" };

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