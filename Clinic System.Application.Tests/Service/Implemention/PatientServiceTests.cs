namespace Clinic_System.Application.Tests.Service.Implemention
{
    public class PatientServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IPatientRepository> _mockPatientRepository;
        private readonly PatientService _patientService;


        public PatientServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockPatientRepository = new Mock<IPatientRepository>();
            _mockUnitOfWork.SetupGet(u => u.PatientsRepository).Returns(_mockPatientRepository.Object);
            _patientService = new PatientService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetPatientsListAsync_ReturnsListOfPatients()
        {
            // Arrange
            var patients = new List<Patient>
            {
                new Patient { Id = 1, FullName = "Dr. Smith" },
                new Patient { Id = 2, FullName = "Dr. Jones"}
            };
            _mockPatientRepository
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(patients);
            // Act
            var result = await _patientService.GetPatientsListAsync();
            // Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(2);
            result[0].FullName.Should().Be("Dr. Smith");
            result[1].FullName.Should().Be("Dr. Jones");
        }

        [Fact]
        public async Task GetPatientsListAsync_ReturnEmptyList()
        {
            // Arrange
            var Patients = new List<Patient>();

            _mockPatientRepository
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Patients);
            // Act
            var result = await _patientService.GetPatientsListAsync();
            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetPatientsListPagingAsync_ReturnsListOfPatientsPaging()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 10;
            var Patients = new List<Patient>
            {
                new Patient { Id = 1, FullName = "Dr. Smith" },
                new Patient { Id = 2, FullName = "Dr. Jones"}
            };

            _mockPatientRepository
                .Setup(r => r.GetPaginatedAsync(pageNumber, pageSize, null, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patients, 2)); // 👈 إرجاع (Items, TotalCount)

            // Act
            var result = await _patientService.GetPatientsListPagingAsync(pageNumber, pageSize);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2); // في الـ PagedResult نختبر الـ Items
            result.TotalCount.Should().Be(2);
            result.CurrentPage.Should().Be(pageNumber);
            result.Items.First().FullName.Should().Be("Dr. Smith");
        }

        [Fact]
        public async Task GetPatientsListPagingAsync_ReturnEmptyList()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 10;
            var Patients = new List<Patient>();

            _mockPatientRepository
                .Setup(r => r.GetPaginatedAsync(pageNumber, pageSize, null, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patients, 0)); // 👈 إرجاع (Items, TotalCount)

            // Act
            var result = await _patientService.GetPatientsListPagingAsync(pageNumber, pageSize);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(0); // في الـ PagedResult نختبر الـ Items
            result.TotalCount.Should().Be(0);
            result.CurrentPage.Should().Be(pageNumber);
        }

        [Fact]
        public async Task GetPatientByIdAsync_ExistingId_ReturnsPatient()
        {
            // Arrange
            int PatientId = 1;

            var patient = new Patient { Id = PatientId, FullName = "Dr. Smith"};

            _mockPatientRepository
                .Setup(r => r.GetByIdAsync(PatientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(patient);

            // Act
            var result = await _patientService.GetPatientByIdAsync(PatientId);
            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(PatientId);
            result.FullName.Should().Be("Dr. Smith");
        }

        [Fact]
        public async Task GetPatientByIdAsync_NotExistingId_ReturnNull()
        {
            // Arrange
            int PatientId = 99;

            _mockPatientRepository
                .Setup(r => r.GetByIdAsync(PatientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patient)null);

            // Act
            var result = await _patientService.GetPatientByIdAsync(PatientId);
            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetPatientByPhoneAsync_ExistingPhone_ReturnsPatient()
        {
            // Arrange
            int PatientId = 1;
            string phoneNumber = "1234567890";

            var patient = new Patient { Id = PatientId, FullName = "Dr. Smith",Phone = phoneNumber };

            _mockPatientRepository
                .Setup(r => r.GetPatientByPhoneAsync(phoneNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(patient);

            // Act
            var result = await _patientService.GetPatientByPhoneAsync(phoneNumber);
            // Assert
            result.Should().NotBeNull();
            result!.Phone.Should().Be(phoneNumber);
            result.FullName.Should().Be("Dr. Smith");
        }

        [Fact]
        public async Task GetPatientByPhoneAsync_NotExistingPhone_ReturnNull()
        {
            // Arrange
            int PatientId = 99;
            string phoneNumber = "0987654321";

            _mockPatientRepository
                .Setup(r => r.GetPatientByPhoneAsync(phoneNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patient)null);

            // Act
            var result = await _patientService.GetPatientByPhoneAsync(phoneNumber);
            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetPatientWithAppointmentsByIdAsync_ExistingId_ReturnsPatient()
        {
            // Arrange
            int PatientId = 1;

            var Patient = new Patient
            {
                Id = PatientId,
                FullName = "Dr. Smith"
                ,
                Appointments = new List<Appointment>
                {
                    new Appointment { Id=1, AppointmentDate= DateTime.Now.AddDays(1), DoctorId=1, PatientId=PatientId },
                    new Appointment { Id=2, AppointmentDate= DateTime.Now.AddDays(2), DoctorId=2, PatientId=PatientId}
                }
            };

            _mockPatientRepository
                .Setup(r => r.GetPatientWithAppointmentsByIdAsync(PatientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Patient);

            // Act
            var result = await _patientService.GetPatientWithAppointmentsByIdAsync(PatientId);
            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(PatientId);
            result.FullName.Should().Be("Dr. Smith");
            result.Appointments.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetPatientWithAppointmentsByIdAsync_NotExistingId_ReturnNull()
        {
            // Arrange
            int PatientId = 99;

            _mockPatientRepository
                .Setup(r => r.GetPatientWithAppointmentsByIdAsync(PatientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patient)null);

            // Act
            var result = await _patientService.GetPatientWithAppointmentsByIdAsync(PatientId);
            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreatePatientAsync_ValidPatient_AddsPatient()
        {
            var patient = new Patient { FullName = "Dr. New"};

            _mockPatientRepository
                .Setup(c => c.AddAsync(patient, It.IsAny<CancellationToken>()));

            // Act
            await _patientService.CreatePatientAsync(patient);

            // Assert
            _mockPatientRepository.Verify(c => c.AddAsync(patient, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdatePatientAsync_ValidPatient_UpdatesPatient()
        {
            var patient = new Patient { Id = 1, FullName = "Dr. New"};

            _mockPatientRepository
                .Setup(c => c.Update(patient, It.IsAny<CancellationToken>()));

            // Act
            await _patientService.UpdatePatient(patient);

            // Assert
            _mockPatientRepository.Verify(c => c.Update(patient, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeletePatientSoftAsync_ValidPatient_DeleteSoftPatient()
        {
            var patient = new Patient { Id = 1, FullName = "Dr. New"};

            _mockPatientRepository
                .Setup(c => c.SoftDelete(patient, It.IsAny<CancellationToken>()));

            // Act
            await _patientService.SoftDeletePatient(patient);

            // Assert
            _mockPatientRepository.Verify(c => c.SoftDelete(patient, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeletePatientHardAsync_ValidPatient_DeleteHardPatient()
        {
            var patient = new Patient { Id = 1, FullName = "Dr. New"};

            _mockPatientRepository
                .Setup(c => c.Delete(patient, It.IsAny<CancellationToken>()));

            // Act
            await _patientService.HardDeletePatient(patient);

            // Assert
            _mockPatientRepository.Verify(c => c.Delete(patient, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetPatientsListByNameAsync_NotExistingName_ReturnNull()
        {
            // Arrange
            string Name = "Ahdham";

            _mockPatientRepository
                .Setup(r => r.GetPatientsByNameAsync(Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Patient>());

            // Act
            var result = await _patientService.GetPatientListByNameAsync(Name);

            // Assert
            result.Should().NotBeNull(); // 👈 القائمة موجودة ككائن
            result.Should().BeEmpty();   // 👈 ولكنها لا تحتوي على عناصر
        }
    }
}
