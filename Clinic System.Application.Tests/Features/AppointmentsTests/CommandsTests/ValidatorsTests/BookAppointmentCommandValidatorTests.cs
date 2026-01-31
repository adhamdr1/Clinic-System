namespace Clinic_System.Application.Tests.Features.AppointmentsTests.CommandsTests.ValidatorsTests
{
    public class BookAppointmentCommandValidatorTests
    {
        private readonly Mock<ICurrentUserService> _mockCurrentUserService;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IDoctorRepository> _mockDoctorRepo;
        private readonly Mock<IPatientRepository> _mockPatientRepo;
        private readonly BookAppointmentCommandValidator _validator;
        public BookAppointmentCommandValidatorTests()
        {
            // تهيئة Mocking لـ IUnitOfWork و Repositories
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockDoctorRepo = new Mock<IDoctorRepository>();
            _mockPatientRepo = new Mock<IPatientRepository>();
            _mockCurrentUserService = new Mock<ICurrentUserService>();
            // ربط الـ Repositories بالـ UnitOfWork
            _mockUnitOfWork.SetupGet(u => u.DoctorsRepository).Returns(_mockDoctorRepo.Object);
            _mockUnitOfWork.SetupGet(u => u.PatientsRepository).Returns(_mockPatientRepo.Object);

            // إنشاء الـ Validator الفعلي
            _validator = new BookAppointmentCommandValidator(_mockUnitOfWork.Object, _mockCurrentUserService.Object);
        }

        [Fact]
        public async Task DoctorId_WhenDoctorExists_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new BookAppointmentCommand
            {
                DoctorId = 1,
                PatientId = 1,
                AppointmentDate = DateTime.Today.AddDays(1),
                AppointmentTime = new TimeSpan(13, 0, 0) // 1:00 PM
            };

            _mockDoctorRepo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Doctor { Id = 1 });

            _mockPatientRepo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Patient { Id = 1 });

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.DoctorId);
        }

        [Fact]
        public async Task DoctorId_WhenDoctorNotExists_ShouldHaveValidationError()
        {
            // Arrange
            var command = new BookAppointmentCommand
            {
                DoctorId = 999,
                PatientId = 1,
                AppointmentDate = DateTime.Today.AddDays(1),
                AppointmentTime = new TimeSpan(13, 0, 0) // 1:00 PM
            };

            _mockDoctorRepo.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Doctor)null);

            _mockPatientRepo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Patient { Id = 1 });

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.DoctorId);
        }

        [Fact]
        public async Task PatientId_WhenPatientExists_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new BookAppointmentCommand
            {
                DoctorId = 1,
                PatientId = 1,
                AppointmentDate = DateTime.Today.AddDays(1),
                AppointmentTime = new TimeSpan(13, 0, 0) // 1:00 PM
            };

            _mockDoctorRepo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Doctor { Id = 1 });

            _mockPatientRepo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Patient { Id = 1 });

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.PatientId);
        }

        [Fact]
        public async Task PatientId_WhenPatientNotExists_ShouldHaveValidationError()
        {
            // Arrange
            var command = new BookAppointmentCommand
            {
                DoctorId = 1,
                PatientId = 1999,
                AppointmentDate = DateTime.Today.AddDays(1),
                AppointmentTime = new TimeSpan(13, 0, 0) // 1:00 PM
            };

            _mockDoctorRepo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Doctor { Id = 1 });

            _mockPatientRepo.Setup(r => r.GetByIdAsync(1999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patient) null);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.PatientId);
        }

        [Fact]
        public async Task AppointmentTime_WhenWithinServiceHours_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new BookAppointmentCommand
            {
                DoctorId = 1,
                PatientId = 1,
                AppointmentDate = DateTime.Today.AddDays(1),
                AppointmentTime = new TimeSpan(14, 0, 0) // 2:00 PM
            };
            _mockDoctorRepo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Doctor { Id = 1 });
            _mockPatientRepo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Patient { Id = 1 });
            // Act
            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.AppointmentTime);
        }

        [Fact]
        public async Task AppointmentTime_WhenOutsideServiceHours_ShouldHaveValidationError()
        {
            // Arrange
            var command = new BookAppointmentCommand
            {
                DoctorId = 1,
                PatientId = 1,
                AppointmentDate = DateTime.Today.AddDays(1),
                AppointmentTime = new TimeSpan(11, 0, 0) // 11:00 AM
            };
            _mockDoctorRepo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Doctor { Id = 1 });
            _mockPatientRepo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Patient { Id = 1 });
            // Act
            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldHaveValidationErrorFor(c => c.AppointmentTime);
        }

        [Fact]
        public async Task AppointmentDate_WhenInThePast_ShouldHaveValidationError()
        {
            // Arrange
            var command = new BookAppointmentCommand
            {
                DoctorId = 1,
                PatientId = 1,
                AppointmentDate = DateTime.Today.AddDays(-1), // Yesterday
                AppointmentTime = new TimeSpan(13, 0, 0) // 1:00 PM
            };
            _mockDoctorRepo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Doctor { Id = 1 });
            _mockPatientRepo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Patient { Id = 1 });
            // Act
            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldHaveValidationErrorFor(c => c.AppointmentDate);
        }

        [Fact]
        public async Task AppointmentDate_WhenTodayOrFuture_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new BookAppointmentCommand
            {
                DoctorId = 1,
                PatientId = 1,
                AppointmentDate = DateTime.Today, // Today
                AppointmentTime = new TimeSpan(13, 0, 0) // 1:00 PM
            };
            _mockDoctorRepo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Doctor { Id = 1 });
            _mockPatientRepo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Patient { Id = 1 });
            // Act
            var result = await _validator.TestValidateAsync(command);
            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.AppointmentDate);
        }
    }
}
