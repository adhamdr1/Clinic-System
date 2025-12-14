namespace Clinic_System.Application.Service.Implemention
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly TimeSpan DefaultStartTime = new TimeSpan(12, 0, 0); // 12:00 PM
        private readonly TimeSpan DefaultEndTime = new TimeSpan(22, 0, 0);   // 10:00 PM
        private const int SlotDurationInMinutes = 15;

        public AppointmentService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<List<Appointment>> GetBookedAppointmentsAsync(int doctorId, DateTime date, CancellationToken cancellationToken = default)
        {
            return (await unitOfWork.AppointmentsRepository
                .GetBookedAppointmentsAsync(doctorId, date, cancellationToken)).ToList();
        }

        public async Task<List<TimeSpan>> GetAvailableSlotsAsync(int doctorId, DateTime date, CancellationToken cancellationToken = default)
        {
            var bookedAppointments = await GetBookedAppointmentsAsync(doctorId, date, cancellationToken);

            var bookedTimes = bookedAppointments
                .Select(a => a.AppointmentDate.TimeOfDay)
                .ToHashSet();

            var allPossibleSlots = GenerateSlots(DefaultStartTime, DefaultEndTime, SlotDurationInMinutes);

            var availableSlots = allPossibleSlots
            .Where(slot => !bookedTimes.Contains(slot))
            .ToList();

            return availableSlots;
        }

        // في AppointmentService.cs
        public async Task<Appointment> BookAppointmentAsync(BookAppointmentCommand command, CancellationToken cancellationToken = default)
        {
            var appointmentDateTime = command.AppointmentDate.Date.Add(command.AppointmentTime);

            // 1. *** التحقق الأمني النهائي والمباشر من قاعدة البيانات (Anti-Concurrency) ***
            var bookedAppointmentsOnDay = await unitOfWork.AppointmentsRepository
                .GetBookedAppointmentsAsync(command.DoctorId, command.AppointmentDate, cancellationToken);

            var isSlotBooked = bookedAppointmentsOnDay
                .Any(a => a.AppointmentDate == appointmentDateTime);

            if (isSlotBooked)
            {
                throw new Exception("The selected time slot is no longer available. Please select another time.");
            }

            var appointment = new Appointment
            {
                DoctorId = command.DoctorId,
                PatientId = command.PatientId,
                AppointmentDate = appointmentDateTime, // نستخدم الـ DateTime المدمج
                Status = AppointmentStatus.Pending,
            };

            // 3. إضافة الكيان
            await unitOfWork.AppointmentsRepository.AddAsync(appointment, cancellationToken);

            // 4. *** الحفظ الفعلي والـ COMMIT (Transactional Safety) ***
            var result = await unitOfWork.SaveAsync();

            if (result == 0)
            {
                // إذا فشل الحفظ دون استثناء، يجب رفع استثناء هنا
                throw new Exception("Failed to save the new appointment to the database.");
            }

            // 5. إرجاع الكيان المحفوظ (الذي يحمل الـ ID الآن)
            return appointment;
        }

        /*
        //public async Task<List<Doctor>> GetDoctorsListAsync(CancellationToken cancellationToken = default)
        //{
        //    return (List<Doctor>)await unitOfWork.DoctorsRepository.GetAllAsync(cancellationToken: cancellationToken);
        //}

        //public async Task<PagedResult<Doctor>> GetDoctorsListPagingAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        //{
        //    var (items, totalCount) = await unitOfWork.DoctorsRepository.GetPaginatedAsync(pageNumber, pageSize, cancellationToken: cancellationToken);

        //    return new PagedResult<Doctor>(items, totalCount, pageNumber, pageSize);
        //}

        //public async Task<Doctor?> GetDoctorWithAppointmentsByIdAsync(int id, CancellationToken cancellationToken = default)
        //{
        //    return await unitOfWork.DoctorsRepository.GetDoctorWithAppointmentsByIdAsync(id, cancellationToken);
        //}
  
        //public async Task UpdateDoctor(Doctor doctor, CancellationToken cancellationToken = default)
        //{
        //    unitOfWork.DoctorsRepository.Update(doctor, cancellationToken);
        //}

        //public async Task<Doctor?> GetDoctorByIdAsync(int id, CancellationToken cancellationToken = default)
        //{
        //    return await unitOfWork.DoctorsRepository.GetByIdAsync(id, cancellationToken);
        //}

        //public async Task SoftDeleteDoctor(Doctor doctor, CancellationToken cancellationToken = default)
        //{
        //    unitOfWork.DoctorsRepository.SoftDelete(doctor, cancellationToken);
        //}

        //public async Task HardDeleteDoctor(Doctor doctor, CancellationToken cancellationToken = default)
        //{
        //    unitOfWork.DoctorsRepository.Delete(doctor, cancellationToken);
        //}

        //public async Task<List<Doctor>> GetDoctorsListBySpecializationAsync(string Specialization, CancellationToken cancellationToken = default)
        //{
        //    return (List<Doctor>)await unitOfWork.DoctorsRepository.GetDoctorsBySpecializationAsync(Specialization, cancellationToken);
        //}
        */


        /// <summary>
        /// دالة مساعدة لتوليد قائمة كل الأوقات الممكنة في يوم عمل الدكتور
        /// </summary>
        private List<TimeSpan> GenerateSlots(TimeSpan startTime, TimeSpan endTime, int durationMinutes)
        {
            var slots = new List<TimeSpan>();
            var currentSlot = startTime;

            // طالما أن الوقت الحالي يسبق نهاية العمل (مع التأكد من أن الموعد الأخير له مدة كافية)
            while (currentSlot.Add(TimeSpan.FromMinutes(durationMinutes)) <= endTime)
            {
                slots.Add(currentSlot);
                currentSlot = currentSlot.Add(TimeSpan.FromMinutes(durationMinutes));
            }

            return slots;
        }
    }
}
