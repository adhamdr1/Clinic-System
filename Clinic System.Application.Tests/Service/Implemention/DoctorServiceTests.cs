namespace Clinic_System.Application.Tests.Service.Implemention
{
    public class DoctorServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IDoctorRepository> _mockDoctorRepository;
        private readonly DoctorService _doctorService;


        public DoctorServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockDoctorRepository = new Mock<IDoctorRepository>();
            _mockUnitOfWork.SetupGet(u => u.DoctorsRepository).Returns(_mockDoctorRepository.Object);
            _doctorService = new DoctorService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetDoctorsListAsync_ReturnsListOfDoctors()
        {
            // Arrange
            var doctors = new List<Doctor>
            {
                new Doctor { Id = 1, FullName = "Dr. Smith", Specialization = "Cardiology" },
                new Doctor { Id = 2, FullName = "Dr. Jones", Specialization = "Neurology" }
            };
            _mockDoctorRepository
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctors);
            // Act
            var result = await _doctorService.GetDoctorsListAsync();
            // Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(2);
            result[0].FullName.Should().Be("Dr. Smith");
            result[1].FullName.Should().Be("Dr. Jones");
        }

        [Fact]
        public async Task GetDoctorsListAsync_ReturnEmptyList()
        {
            // Arrange
            var doctors = new List<Doctor>();

            _mockDoctorRepository
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctors);
            // Act
            var result = await _doctorService.GetDoctorsListAsync();
            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetDoctorsListPagingAsync_ReturnsListOfDoctorsPaging()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 10;
            var doctors = new List<Doctor>
            {
                new Doctor { Id = 1, FullName = "Dr. Smith", Specialization = "Cardiology" },
                new Doctor { Id = 2, FullName = "Dr. Jones", Specialization = "Neurology" }
            };

            _mockDoctorRepository
                .Setup(r => r.GetPaginatedAsync(pageNumber, pageSize, null, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync((doctors, 2)); // 👈 إرجاع (Items, TotalCount)

            // Act
            var result = await _doctorService.GetDoctorsListPagingAsync(pageNumber, pageSize);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2); // في الـ PagedResult نختبر الـ Items
            result.TotalCount.Should().Be(2);
            result.CurrentPage.Should().Be(pageNumber);
            result.Items.First().FullName.Should().Be("Dr. Smith");
        }

        [Fact]
        public async Task GetDoctorsListPagingAsync_ReturnEmptyList()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 10;
            var doctors = new List<Doctor>();

            _mockDoctorRepository
                .Setup(r => r.GetPaginatedAsync(pageNumber, pageSize, null, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync((doctors, 0)); // 👈 إرجاع (Items, TotalCount)

            // Act
            var result = await _doctorService.GetDoctorsListPagingAsync(pageNumber, pageSize);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(0); // في الـ PagedResult نختبر الـ Items
            result.TotalCount.Should().Be(0);
            result.CurrentPage.Should().Be(pageNumber);
        }

        [Fact]
        public async Task GetDoctorByIdAsync_ExistingId_ReturnsDoctor()
        {
            // Arrange
            int doctorId = 1;

            var doctor = new Doctor { Id = doctorId, FullName = "Dr. Smith", Specialization = "Cardiology" };

            _mockDoctorRepository
                .Setup(r => r.GetByIdAsync(doctorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            // Act
            var result = await _doctorService.GetDoctorByIdAsync(doctorId);
            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(doctorId);
            result.FullName.Should().Be("Dr. Smith");
        }

        [Fact]
        public async Task GetDoctorByIdAsync_NotExistingId_ReturnNull()
        {
            // Arrange
            int doctorId = 99;

            _mockDoctorRepository
                .Setup(r => r.GetByIdAsync(doctorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Doctor)null);

            // Act
            var result = await _doctorService.GetDoctorByIdAsync(doctorId);
            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetDoctorWithAppointmentsByIdAsync_ExistingId_ReturnsDoctor()
        {
            // Arrange
            int doctorId = 1;

            var doctor = new Doctor
            {
                Id = doctorId,
                FullName = "Dr. Smith"
                ,
                Specialization = "Cardiology"
                ,
                Appointments = new List<Appointment>
                {
                    new Appointment { Id=1, AppointmentDate= DateTime.Now.AddDays(1), DoctorId=doctorId, PatientId=1 },
                    new Appointment { Id=2, AppointmentDate= DateTime.Now.AddDays(2), DoctorId=doctorId, PatientId=2 }
                }
            };

            _mockDoctorRepository
                .Setup(r => r.GetDoctorWithAppointmentsByIdAsync(doctorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            // Act
            var result = await _doctorService.GetDoctorWithAppointmentsByIdAsync(doctorId);
            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(doctorId);
            result.FullName.Should().Be("Dr. Smith");
            result.Appointments.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetDoctorWithAppointmentsByIdAsync_NotExistingId_ReturnNull()
        {
            // Arrange
            int doctorId = 99;

            _mockDoctorRepository
                .Setup(r => r.GetDoctorWithAppointmentsByIdAsync(doctorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Doctor)null);

            // Act
            var result = await _doctorService.GetDoctorWithAppointmentsByIdAsync(doctorId);
            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateDoctorAsync_ValidDoctor_AddsDoctor()
        {
            var doctor = new Doctor { FullName = "Dr. New", Specialization = "Dermatology" };

            _mockDoctorRepository
                .Setup(c => c.AddAsync(doctor, It.IsAny<CancellationToken>()));

            // Act
            await _doctorService.CreateDoctorAsync(doctor);

            // Assert
            _mockDoctorRepository.Verify(c => c.AddAsync(doctor, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateDoctorAsync_ValidDoctor_UpdatesDoctor()
        {
            var doctor = new Doctor { Id = 1, FullName = "Dr. New", Specialization = "Dermatology" };

            _mockDoctorRepository
                .Setup(c => c.Update(doctor, It.IsAny<CancellationToken>()));

            // Act
            await _doctorService.UpdateDoctor(doctor);

            // Assert
            _mockDoctorRepository.Verify(c => c.Update(doctor, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteDoctorSoftAsync_ValidDoctor_DeleteSoftDoctor()
        {
            var doctor = new Doctor { Id = 1, FullName = "Dr. New", Specialization = "Dermatology" };

            _mockDoctorRepository
                .Setup(c => c.SoftDelete(doctor, It.IsAny<CancellationToken>()));

            // Act
            await _doctorService.SoftDeleteDoctor(doctor);

            // Assert
            _mockDoctorRepository.Verify(c => c.SoftDelete(doctor, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteDoctorHardAsync_ValidDoctor_DeleteHardDoctor()
        {
            var doctor = new Doctor { Id = 1, FullName = "Dr. New", Specialization = "Dermatology" };

            _mockDoctorRepository
                .Setup(c => c.Delete(doctor, It.IsAny<CancellationToken>()));

            // Act
            await _doctorService.HardDeleteDoctor(doctor);

            // Assert
            _mockDoctorRepository.Verify(c => c.Delete(doctor, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetDoctorsListBySpecializationAsync_NotExistingSpecialization_ReturnNull()
        {
            // Arrange
            string specialization = "Cardiology";
            
            _mockDoctorRepository
                .Setup(r => r.GetDoctorsBySpecializationAsync(specialization, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Doctor>());

            // Act
            var result = await _doctorService.GetDoctorsListBySpecializationAsync(specialization);

            // Assert
            result.Should().NotBeNull(); // 👈 القائمة موجودة ككائن
            result.Should().BeEmpty();   // 👈 ولكنها لا تحتوي على عناصر
        }

        [Fact]
        public async Task GetDoctorsListByNameAsync_NotExistingName_ReturnNull()
        {
            // Arrange
            string Name = "Ahdham";

            _mockDoctorRepository
                .Setup(r => r.GetDoctorsByNameAsync(Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Doctor>());

            // Act
            var result = await _doctorService.GetDoctorsListByNameAsync(Name);

            // Assert
            result.Should().NotBeNull(); // 👈 القائمة موجودة ككائن
            result.Should().BeEmpty();   // 👈 ولكنها لا تحتوي على عناصر
        }
    }
}
