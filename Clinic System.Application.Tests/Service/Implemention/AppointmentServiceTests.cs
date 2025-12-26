namespace Clinic_System.Application.Tests.Service.Implemention
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IPaymentService> _mockPaymentService;
        private readonly Mock<IMedicalRecordService> _mockMedicalRecordService;
        private readonly Mock<IAppointmentRepository> _mockAppointmentRepository;
        private readonly Mock<ILogger<AppointmentService>> _mockLogger;
        private readonly AppointmentService _appointmentService;


        public AppointmentServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockAppointmentRepository = new Mock<IAppointmentRepository>();
            _mockPaymentService = new Mock<IPaymentService>();
            _mockMedicalRecordService = new Mock<IMedicalRecordService>();
            _mockLogger = new Mock<ILogger<AppointmentService>>();
            _mockUnitOfWork.SetupGet(u => u.AppointmentsRepository).Returns(_mockAppointmentRepository.Object);
            _appointmentService = new AppointmentService(_mockUnitOfWork.Object, _mockPaymentService.Object,
               _mockMedicalRecordService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task BookAppointmentAsync_SlotAvailable_SavesAndReturnsAppointment()
        {
            // Arrange
            var command = new BookAppointmentCommand
            {
                DoctorId = 1,
                PatientId = 1,
                AppointmentDate = DateTime.Today,
                AppointmentTime = new TimeSpan(12, 0, 0) // 12:00 PM
            };

            _mockAppointmentRepository
                .Setup(r => r.GetBookedAppointmentsAsync(
                    command.DoctorId,
                    command.AppointmentDate,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Appointment>()); // No booked appointments
       
            _mockUnitOfWork
                .Setup(u => u.SaveAsync())
                .ReturnsAsync(1);

            // Act
            var result = await _appointmentService.BookAppointmentAsync(command);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(AppointmentStatus.Pending);

            _mockAppointmentRepository.Verify(r => r.AddAsync(
                 It.Is<Appointment>(a => a.DoctorId == command.DoctorId),
                 It.IsAny<CancellationToken>()), Times.Once); // التأكد من استدعاء AddAsync
           
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once); // التأكد من استدعاء SaveAsync
        }

        [Fact]
        public async Task BookAppointmentAsync_SaveFails_ThrowsException()
        {
            // Arrange
            var command = new BookAppointmentCommand
            {
                DoctorId = 1,
                PatientId = 1,
                AppointmentDate = DateTime.Today,
                AppointmentTime = new TimeSpan(12, 0, 0) // 12:00 PM
            };

            _mockAppointmentRepository
                .Setup(r => r.GetBookedAppointmentsAsync(
                    command.DoctorId,
                    command.AppointmentDate,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Appointment>()); // No booked appointments

            _mockUnitOfWork
                .Setup(u => u.SaveAsync())
                .ReturnsAsync(0);

            // Act

            // Assert
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await _appointmentService.BookAppointmentAsync(command, CancellationToken.None);
            });


            _mockAppointmentRepository.Verify(r => r.AddAsync(
                 It.IsAny<Appointment>(),
                 It.IsAny<CancellationToken>()), Times.Once); // التأكد من استدعاء AddAsync

            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once); // التأكد من استدعاء SaveAsync
        }

        [Fact]
        public async Task BookAppointmentAsync_SlotAlreadyBooked_ThrowsException()
        {
            // Arrange
            var command = new BookAppointmentCommand
            {
                DoctorId = 1,
                PatientId = 1,
                AppointmentDate = DateTime.Today,
                AppointmentTime = new TimeSpan(12, 0, 0) // 12:00 PM
            };

            var bookedAppointments = new List<Appointment>
            {
                new Appointment { 
                    AppointmentDate = command.AppointmentDate.Date.Add(command.AppointmentTime),
                    DoctorId = command.DoctorId
                } // Slot already booked
            };

            _mockAppointmentRepository
                .Setup(r => r.GetBookedAppointmentsAsync(
                    command.DoctorId,
                    command.AppointmentDate,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(bookedAppointments);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _appointmentService.BookAppointmentAsync(command));

            _mockAppointmentRepository.Verify(r => r.AddAsync(
                 It.IsAny<Appointment>(),
                 It.IsAny<CancellationToken>()), Times.Never); // التأكد من عدم استدعاء AddAsync

            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Never); // التأكد من عدم استدعاء SaveAsync
        }

        [Fact]
        public async Task GetAvailableSlotsAsync_OneSlotBooked_ReturnsCorrectAvailableSlots()
        {
            // Arrange
            int doctorId = 1;
            DateTime date = DateTime.Today;
            var bookedAppointments = new List<Appointment>
            {
                new Appointment { AppointmentDate = date.AddHours(12).AddMinutes(15) }, // 12:15 PM
            };


            _mockAppointmentRepository.Setup(u => u
                .GetBookedAppointmentsAsync(doctorId, date, It.IsAny<CancellationToken>()))
                .ReturnsAsync(bookedAppointments);

            // المواعيد المتوقعة (بداية من 12:15:00، لأن 12:00:00 محجوز)
            var expectedStartTime = new TimeSpan(12, 0, 0);
            var expectedEndTime = new TimeSpan(22, 0, 0);

            // Act
            var availableSlots = await _appointmentService.GetAvailableSlotsAsync(doctorId, date);

            // Assert
            // 1. التأكد من أن الموعد المحجوز قد أُزيل
            availableSlots.Should().NotContain(new TimeSpan(12, 15, 0));

            // 2. التأكد من أن القائمة تبدأ بالموعد التالي (12:15:00)
            availableSlots.First().Should().Be(expectedStartTime);

            // 3. التأكد من أن عدد الفترات صحيح: 
            // عدد الفترات الممكنة بين 12:00 و 22:00 (10 ساعات) هو 40 فترة (10 * 4).
            // إذا أزلنا فترة واحدة، يجب أن يكون العدد 39.
            availableSlots.Count.Should().Be(39);
        }
    }
}
