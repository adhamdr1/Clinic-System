namespace Clinic_System.Application.Service.Implemention
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IPaymentService paymentService;
        private readonly IMedicalRecordService medicalRecordService;
        private readonly ILogger<AppointmentService> logger;

        private readonly TimeSpan DefaultStartTime = new TimeSpan(12, 0, 0); // 12:00 PM
        private readonly TimeSpan DefaultEndTime = new TimeSpan(22, 0, 0);   // 10:00 PM
        private const int SlotDurationInMinutes = 15;

        public AppointmentService(IUnitOfWork unitOfWork, IPaymentService paymentService,
            IMedicalRecordService medicalRecordService, ILogger<AppointmentService> logger)
        {
            this.paymentService = paymentService;
            this.unitOfWork = unitOfWork;
            this.medicalRecordService = medicalRecordService;
            this.logger = logger;
        }

        public async Task<List<Appointment>> GetBookedAppointmentsAsync(int doctorId, DateTime date, CancellationToken cancellationToken = default)
        {
            return (await unitOfWork.AppointmentsRepository
                .GetBookedAppointmentsAsync(doctorId, date, cancellationToken)).ToList();
        }

        public async Task<List<TimeSpan>> GetAvailableSlotsAsync(int doctorId, DateTime date, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Fetching available slots for DoctorId: {DoctorId} on Date: {Date}", doctorId, date.ToShortDateString());

            var bookedAppointments = await GetBookedAppointmentsAsync(doctorId, date, cancellationToken);

            var bookedTimes = bookedAppointments
                .Select(a => a.AppointmentDate.TimeOfDay)
                .ToHashSet();

            var allPossibleSlots = GenerateSlots(DefaultStartTime, DefaultEndTime, SlotDurationInMinutes);

            var availableSlots = allPossibleSlots
            .Where(slot => !bookedTimes.Contains(slot))
            .ToList();

            logger.LogInformation("Available slots for DoctorId: {DoctorId} on Date: {Date}: {AvailableSlots}",
                doctorId, date.ToShortDateString(), string.Join(", ", availableSlots));

            return availableSlots;
        }

        // في AppointmentService.cs
        public async Task<Appointment> BookAppointmentAsync(BookAppointmentCommand command, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Attempting to book appointment for PatientId: {PatientId} with DoctorId: {DoctorId} on {AppointmentDate} at {AppointmentTime}",
                    command.PatientId, command.DoctorId, command.AppointmentDate.ToShortDateString(), command.AppointmentTime);

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var appointmentDateTime = command.AppointmentDate.Date.Add(command.AppointmentTime);

                    // 1. *** التحقق الأمني النهائي والمباشر من قاعدة البيانات (Anti-Concurrency) ***
                    var bookedAppointmentsOnDay = await unitOfWork.AppointmentsRepository
                        .GetBookedAppointmentsAsync(command.DoctorId, command.AppointmentDate, cancellationToken);

                    if (appointmentDateTime < DateTime.Now)
                        throw new ValidationException("Cannot book an appointment in the past.");

                    var isSlotBooked = bookedAppointmentsOnDay
                        .Any(a => a.AppointmentDate == appointmentDateTime);

                    if (isSlotBooked)
                    {
                        logger.LogError("Failed to book appointment: Slot already booked for DoctorId: {DoctorId} on {AppointmentDateTime}",
                            command.DoctorId, appointmentDateTime);
                        throw new SlotAlreadyBookedException("The selected time slot is no longer available. Please select another time.");
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

                    var result = await unitOfWork.SaveAsync();

                    if (result == 0)
                    {
                        logger.LogError("Failed to save the new appointment for PatientId: {PatientId} with DoctorId: {DoctorId} on {AppointmentDateTime}",
                            command.PatientId, command.DoctorId, appointmentDateTime);
                        // إذا فشل الحفظ دون استثناء، يجب رفع استثناء هنا
                        throw new DatabaseSaveException("Failed to save the new appointment to the database.");
                    }

                    await paymentService.CreatePaymentAsync(appointment.Id, cancellationToken);

                    // 4. *** الحفظ الفعلي والـ COMMIT (Transactional Safety) ***

                    result = await unitOfWork.SaveAsync();

                    if (result == 0)
                    {
                        logger.LogError("Failed to save the new appointment for PatientId: {PatientId} with DoctorId: {DoctorId} on {AppointmentDateTime}",
                            command.PatientId, command.DoctorId, appointmentDateTime);
                        // إذا فشل الحفظ دون استثناء، يجب رفع استثناء هنا
                        throw new DatabaseSaveException("Failed to save the new appointment to the database.");
                    }

                    logger.LogInformation("Successfully booked appointment with ID: {AppointmentId} for PatientId: {PatientId} with DoctorId: {DoctorId} on {AppointmentDateTime}",
                        appointment.Id, command.PatientId, command.DoctorId, appointmentDateTime);

                    transaction.Complete();

                    return appointment;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while booking appointment for PatientId: {PatientId} with DoctorId: {DoctorId} on {AppointmentDate} at {AppointmentTime}",
                        command.PatientId, command.DoctorId, command.AppointmentDate.ToShortDateString(), command.AppointmentTime);
                    throw; // إعادة رمي الاستثناء بعد تسجيله
                }
            }
        }

        public async Task<Appointment> RescheduleAppointmentAsync(RescheduleAppointmentCommand command, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Attempting to reschedule appointment ID: {AppointmentId} for PatientId: {PatientId} on {AppointmentDate} at {AppointmentTime}",
                command.AppointmentId, command.PatientId, command.AppointmentDate.ToShortDateString(), command.AppointmentTime);

            var appointment = await unitOfWork.AppointmentsRepository.GetByIdAsync(command.AppointmentId);

            if (appointment == null)
            {
                logger.LogError("Appointment with ID: {AppointmentId} not found for rescheduling", command.AppointmentId);
                throw new NotFoundException("Appointment not found.");
            }

            if (appointment.PatientId != command.PatientId)
                throw new UnauthorizedException("You are not authorized to reschedule this appointment.");

            var appointmentDateTime = command.AppointmentDate.Date.Add(command.AppointmentTime);

            var bookedAppointmentsOnDay = await unitOfWork.AppointmentsRepository
               .GetBookedAppointmentsAsync(appointment.DoctorId, command.AppointmentDate, cancellationToken);

            var isSlotBooked = bookedAppointmentsOnDay
                .Any(a => a.AppointmentDate == appointmentDateTime && a.Id != command.AppointmentId);

            if (isSlotBooked)
            {
                logger.LogError("Failed to book appointment: Slot already booked for DoctorId: {DoctorId} on {AppointmentDateTime}",
                    appointment.DoctorId, appointmentDateTime);
                throw new SlotAlreadyBookedException("The selected time slot is no longer available. Please select another time.");
            }

            appointment.Reschedule(appointmentDateTime);

            var result = await unitOfWork.SaveAsync();

            if (result == 0)
            {
                logger.LogError("Failed to save the Reschedule appointment for PatientId: {PatientId} with DoctorId: {DoctorId} on {AppointmentDateTime}",
                    command.PatientId, appointment.DoctorId, appointmentDateTime);
                // إذا فشل الحفظ دون استثناء، يجب رفع استثناء هنا
                throw new DatabaseSaveException("Failed to save the Reschedule appointment to the database.");
            }

            logger.LogInformation("Successfully rescheduled appointment with ID: {AppointmentId} for PatientId: {PatientId} with DoctorId: {DoctorId} on {AppointmentDateTime}",
                appointment.Id, command.PatientId, appointment.DoctorId, appointmentDateTime);
            // 5. إرجاع الكيان المحفوظ (الذي يحمل الـ ID الآن)
            return appointment;
        }
        
        public async Task<Appointment> CancelAppointmentAsync(CancelAppointmentCommand command, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Attempting to cancel appointment ID: {AppointmentId} for PatientId: {PatientId}",
                command.AppointmentId, command.PatientId);

            var appointment = await unitOfWork.AppointmentsRepository.GetByIdAsync(command.AppointmentId);

            if (appointment == null)
            {
                logger.LogError("Appointment with ID: {AppointmentId} not found for rescheduling", command.AppointmentId);
                throw new NotFoundException("Appointment not found.");
            }

            if (appointment.PatientId != command.PatientId)
                throw new UnauthorizedException("You are not authorized to reschedule this appointment.");

            appointment.Cancel();

            var result = await unitOfWork.SaveAsync();
            if (result == 0)
            {
                logger.LogError("Failed to save the Cancel appointment for PatientId: {PatientId} on AppointmentId: {AppointmentId}",
                    command.PatientId, command.AppointmentId);
                // إذا فشل الحفظ دون استثناء، يجب رفع استثناء هنا
                throw new DatabaseSaveException("Failed to save the Cancel appointment to the database.");
            }
            logger.LogInformation("Successfully Cancelled appointment with ID: {AppointmentId} for PatientId: {PatientId}",
                appointment.Id, command.PatientId);
            return appointment;
        }

        public async Task<Appointment> ConfirmAppointmentAsync(int AppointmentId, int PatientId, PaymentMethod? method = null
            , decimal? amount = null, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Attempting to confirm appointment ID: {AppointmentId} for PatientId: {PatientId}",
                AppointmentId, PatientId);

            var appointment = await unitOfWork.AppointmentsRepository.GetByIdAsync(AppointmentId);
            if (appointment == null)
            {
                logger.LogError("Appointment with ID: {AppointmentId} not found for confirmation", AppointmentId);
                throw new NotFoundException("Appointment not found.");
            }

            if (appointment.PatientId != PatientId)
                throw new UnauthorizedException("You are not authorized to confirm this appointment.");


            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    appointment.Confirm();

                    await paymentService.ConfirmPaymentAsync(appointment.Id, method, amount, cancellationToken);

                    var result = await unitOfWork.SaveAsync();

                    if (result == 0)
                    {
                        logger.LogError("Failed to save the Confirm appointment for PatientId: {PatientId} on AppointmentId: {AppointmentId}",
                         PatientId, AppointmentId);
                        throw new DatabaseSaveException("Failed to save the Confirm appointment to the database.");
                    }

                    logger.LogInformation("Successfully Confirmed appointment with ID: {AppointmentId} for PatientId: {PatientId}",
                        appointment.Id, PatientId);

                    transaction.Complete();
                    return appointment;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while confirming appointment ID: {AppointmentId} for PatientId: {PatientId}",
                        AppointmentId, PatientId);

                    throw; // إعادة رمي الاستثناء بعد تسجيله
                }
            }
        }

        public async Task<Appointment> NoShowAppointmentAsync(int AppointmentId, int DoctorId, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Attempting to mark no-show for appointment ID: {AppointmentId} for DoctorId: {DoctorId}",
                AppointmentId, DoctorId);

            var appointment = await unitOfWork.AppointmentsRepository.GetByIdAsync(AppointmentId);
            if (appointment == null)
            {
                logger.LogError("Appointment with ID: {AppointmentId} not found for no-show", AppointmentId);
                throw new NotFoundException("Appointment not found.");
            }

            if (appointment.DoctorId != DoctorId)
                throw new UnauthorizedException("You are not authorized to mark no-show for this appointment.");

            appointment.NoShow();
            var result = await unitOfWork.SaveAsync();
            if (result == 0)
            {
                logger.LogError("Failed to save the No-Show appointment for DoctorId: {DoctorId} on AppointmentId: {AppointmentId}",
                    DoctorId, AppointmentId);
                // إذا فشل الحفظ دون استثناء، يجب رفع استثناء هنا
                throw new DatabaseSaveException("Failed to save the No-Show appointment to the database.");
            }

            logger.LogInformation("Successfully marked No-Show for appointment with ID: {AppointmentId} for DoctorId: {DoctorId}",
                appointment.Id, DoctorId);

            return appointment;
        }

        public async Task<Appointment> CompleteAppointmentAsync(CompleteAppointmentCommand command, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Attempting to complete appointment ID: {AppointmentId} for DoctorId: {DoctorId}",
                command.AppointmentId, command.DoctorId);

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    var appointment = await unitOfWork.AppointmentsRepository.GetByIdAsync(command.AppointmentId);

                    if (appointment == null)
                    {
                        logger.LogError("Appointment with ID: {AppointmentId} not found for completion", command.AppointmentId);
                        throw new NotFoundException("Appointment not found.");
                    }

                    if (appointment.DoctorId != command.DoctorId)
                        throw new UnauthorizedException("You are not authorized to complete this appointment.");

                    appointment.Complete();

                    await medicalRecordService.CreateMedicalRecordAsync(appointment,
                        command.Diagnosis,command.Description,command.Medicines, cancellationToken);

                    var result = await unitOfWork.SaveAsync();
                    if (result == 0)
                    {
                        logger.LogError("Failed to save the Complete appointment for DoctorId: {DoctorId} on AppointmentId: {AppointmentId}",
                            command.DoctorId, command.AppointmentId);
                        // إذا فشل الحفظ دون استثناء، يجب رفع استثناء هنا
                        throw new DatabaseSaveException("Failed to save the Complete appointment to the database.");
                    }

                    logger.LogInformation("Successfully Completed appointment with ID: {AppointmentId} for DoctorId: {DoctorId}",
                        appointment.Id, command.DoctorId);

                    transaction.Complete();

                    return appointment;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while completing appointment ID: {AppointmentId} for DoctorId: {DoctorId}",
                        command.AppointmentId, command.DoctorId);
                    throw; // إعادة رمي الاستثناء بعد تسجيله
                }
            }
        }

        /// <summary>
        /// دالة مساعدة لتوليد قائمة كل الأوقات الممكنة في يوم عمل الدكتور
        /// </summary>
        private List<TimeSpan> GenerateSlots(TimeSpan startTime, TimeSpan endTime, int durationMinutes)
        {
            logger.LogInformation("Generating slots from {StartTime} to {EndTime} with duration {DurationMinutes} minutes",
                startTime, endTime, durationMinutes);

            var slots = new List<TimeSpan>();
            var currentSlot = startTime;

            // طالما أن الوقت الحالي يسبق نهاية العمل (مع التأكد من أن الموعد الأخير له مدة كافية)
            while (currentSlot.Add(TimeSpan.FromMinutes(durationMinutes)) <= endTime)
            {
                slots.Add(currentSlot);
                currentSlot = currentSlot.Add(TimeSpan.FromMinutes(durationMinutes));
            }

            logger.LogInformation("Generated {SlotCount} slots", slots.Count);

            return slots;
        }

        public async Task<PagedResult<Appointment>> GetDoctorAppointmentsAsync(GetDoctorAppointmentsQuery doctorAppointmentQuery, CancellationToken cancellationToken = default)
        {
            var (items, totalCount) = await unitOfWork.AppointmentsRepository.GetDoctorAppointmentsAsync
                (doctorAppointmentQuery.DoctorId, doctorAppointmentQuery.PageNumber,
                doctorAppointmentQuery.PageSize, doctorAppointmentQuery.DateTime, cancellationToken: cancellationToken);

            return new PagedResult<Appointment>(items, totalCount, doctorAppointmentQuery.PageNumber, doctorAppointmentQuery.PageSize);
        }

        public async Task<PagedResult<Appointment>> GetPatientAppointmentsAsync(GetPatientAppointmentsQuery patientAppointmentQuery, CancellationToken cancellationToken = default)
        {
            var (items, totalCount) = await unitOfWork.AppointmentsRepository.GetPatientAppointmentsAsync
                (patientAppointmentQuery.PatientId, patientAppointmentQuery.PageNumber,
                patientAppointmentQuery.PageSize, patientAppointmentQuery.DateTime, cancellationToken: cancellationToken);

            return new PagedResult<Appointment>(items, totalCount, patientAppointmentQuery.PageNumber, patientAppointmentQuery.PageSize);
        }

        public async Task<PagedResult<Appointment>> GetAppointmentsByStatusForAdminAsync(GetAppointmentsByStatusForAdminQuery appointmentsByStatusForAdminQuery, CancellationToken cancellationToken = default)
        {
            var (items, totalCount) = await unitOfWork.AppointmentsRepository.GetAppointmentsByStatusForAdminAsync
                (appointmentsByStatusForAdminQuery.Status, appointmentsByStatusForAdminQuery.PageNumber,
                appointmentsByStatusForAdminQuery.PageSize, appointmentsByStatusForAdminQuery.Start,
                appointmentsByStatusForAdminQuery.End, cancellationToken: cancellationToken);

            return new PagedResult<Appointment>(items, totalCount, appointmentsByStatusForAdminQuery.PageNumber, appointmentsByStatusForAdminQuery.PageSize);
        }

        public async Task<PagedResult<Appointment>> GetAppointmentsByStatusForDoctorAsync(GetAppointmentsByStatusForDoctorQuery appointmentsByStatusForDoctorQuery, CancellationToken cancellationToken = default)
        {
            var (items, totalCount) = await unitOfWork.AppointmentsRepository.GetAppointmentsByStatusForDoctorAsync
                (appointmentsByStatusForDoctorQuery.Status, appointmentsByStatusForDoctorQuery.DoctorId,
                appointmentsByStatusForDoctorQuery.PageNumber,
                appointmentsByStatusForDoctorQuery.PageSize, appointmentsByStatusForDoctorQuery.Start,
                appointmentsByStatusForDoctorQuery.End, cancellationToken: cancellationToken);

            return new PagedResult<Appointment>(items, totalCount, appointmentsByStatusForDoctorQuery.PageNumber, appointmentsByStatusForDoctorQuery.PageSize);
        }

        public async Task<PagedResult<Appointment>> GetPastAppointmentsForDoctorAsync(GetPastAppointmentsForDoctorQuery appointmentsForDoctorQuery, CancellationToken cancellationToken = default)
        {
            var (items, totalCount) = await unitOfWork.AppointmentsRepository.GetPastAppointmentsForDoctorAsync
                (appointmentsForDoctorQuery.DoctorId, appointmentsForDoctorQuery.PageNumber,
                appointmentsForDoctorQuery.PageSize, cancellationToken);


            return new PagedResult<Appointment>(items, totalCount, appointmentsForDoctorQuery.PageNumber, appointmentsForDoctorQuery.PageSize);
        }

        public async Task<PagedResult<Appointment>> GetPastAppointmentsForPatientAsync(GetPastAppointmentsForPatientQuery appointmentsForPatientQuery, CancellationToken cancellationToken = default)
        {
            var (items, totalCount) = await unitOfWork.AppointmentsRepository.GetPastAppointmentsForPatientAsync
                (appointmentsForPatientQuery.PatientId, appointmentsForPatientQuery.PageNumber,
                appointmentsForPatientQuery.PageSize, cancellationToken);


            return new PagedResult<Appointment>(items, totalCount, appointmentsForPatientQuery.PageNumber, appointmentsForPatientQuery.PageSize);

        }
    }
}
