namespace Clinic_System.Application.Service.Implemention
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentNotificationService notification;
        private readonly IUnitOfWork unitOfWork;
        private readonly IPaymentService paymentService;
        private readonly IMedicalRecordService medicalRecordService;
        private readonly ILogger<AppointmentService> logger;
        private readonly ClinicSettings clinicSettings;
        public AppointmentService(
            IAppointmentNotificationService notification,
            IUnitOfWork unitOfWork,
            IPaymentService paymentService,
            IMedicalRecordService medicalRecordService,
            ILogger<AppointmentService> logger,
            IOptions<ClinicSettings> clinicSettings)
        {
            this.notification = notification;
            this.unitOfWork = unitOfWork;
            this.paymentService = paymentService;
            this.medicalRecordService = medicalRecordService;
            this.logger = logger;
            this.clinicSettings = clinicSettings.Value;
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

            var allPossibleSlots = GenerateSlots(clinicSettings.DayStartTime, clinicSettings.DayEndTime, clinicSettings.SlotDurationInMinutes);

            var availableSlots = allPossibleSlots
            .Where(slot => !bookedTimes.Contains(slot))
            .ToList();

            logger.LogInformation("Available slots for DoctorId: {DoctorId} on Date: {Date}: {AvailableSlots}",
                doctorId, date.ToShortDateString(), string.Join(", ", availableSlots));

            return availableSlots;
        }

        public async Task CancelOverdueAppointmentsAsync()
        {
            // 1. تحديد المواعيد اللي فات ميعادها بـ 60 دقيقة ولسه Pending
            var threshold = DateTime.Now.AddHours(-1);

            var overdueAppointments = await unitOfWork.AppointmentsRepository
                .GetPendingOverdueAppointmentsAsync(threshold);

            if (!overdueAppointments.Any()) return;

            foreach (var appointment in overdueAppointments)
            {
                logger.LogInformation("Auto-cancelling overdue appointment ID: {Id}", appointment.Id);
                appointment.SystemExpire(); // استخدام ميثود الـ Domain اللي إنت عاملها

                if (appointment.Payment != null && appointment.Payment.PaymentStatus == PaymentStatus.Pending)
                {
                    appointment.Payment.MarkAsCancelling("Appointment cancelled");
                }

                await notification.SendAutoCancellationAsync(appointment);
            }

            // 2. حفظ كل التغييرات مرة واحدة
            await unitOfWork.SaveAsync();
        }

        public async Task<Appointment> BookAppointmentAsync(int patientId, int doctorId, DateTime appointmentDate, TimeSpan appointmentTime, CancellationToken cancellationToken = default)
        {
            var appointmentDateTime = appointmentDate.Date.Add(appointmentTime);

            logger.LogInformation("Attempting to book appointment for PatientId: {PatientId} with DoctorId: {DoctorId} on {AppointmentDateTime}",
                    patientId, doctorId, appointmentDateTime);

            string PatientEmail = string.Empty;
            Appointment appointment2 = null;


            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    // 1. *** التحقق الأمني النهائي والمباشر من قاعدة البيانات (Anti-Concurrency) ***
                    var bookedAppointmentsOnDay = await unitOfWork.AppointmentsRepository
                         .GetBookedAppointmentsAsync(doctorId, appointmentDate, cancellationToken);

                    if (appointmentDateTime < DateTime.Now)
                        throw new ValidationException("Cannot book an appointment in the past.");

                    var isSlotBooked = bookedAppointmentsOnDay
                        .Any(a => a.AppointmentDate == appointmentDateTime);

                    if (isSlotBooked)
                    {
                        logger.LogError("Failed to book appointment: Slot already booked for DoctorId: {DoctorId} on {AppointmentDateTime}",
                            doctorId, appointmentDateTime);
                        throw new SlotAlreadyBookedException("The selected time slot is no longer available. Please select another time.");
                    }

                    var appointment = new Appointment
                    {
                        DoctorId = doctorId,
                        PatientId = patientId,
                        AppointmentDate = appointmentDateTime, // نستخدم الـ DateTime المدمج
                        Status = AppointmentStatus.Pending,
                    };

                    // 3. إضافة الكيان
                    await unitOfWork.AppointmentsRepository.AddAsync(appointment, cancellationToken);

                    var result = await unitOfWork.SaveAsync();

                    if (result == 0)
                    {
                        logger.LogError("Failed to save the new appointment for PatientId: {PatientId} with DoctorId: {DoctorId} on {AppointmentDateTime}",
                           patientId, doctorId, appointmentDateTime);
                        // إذا فشل الحفظ دون استثناء، يجب رفع استثناء هنا
                        throw new DatabaseSaveException("Failed to save the new appointment to the database.");
                    }

                    await paymentService.CreatePaymentAsync(appointment.Id, cancellationToken);

                    // 4. *** الحفظ الفعلي والـ COMMIT (Transactional Safety) ***

                    result = await unitOfWork.SaveAsync();

                    if (result == 0)
                    {
                        logger.LogError("Failed to save the new appointment for PatientId: {PatientId} with DoctorId: {DoctorId} on {AppointmentDateTime}",
                            patientId, doctorId, appointmentDateTime);
                        // إذا فشل الحفظ دون استثناء، يجب رفع استثناء هنا
                        throw new DatabaseSaveException("Failed to save the new appointment to the database.");
                    }

                    logger.LogInformation("Successfully booked appointment with ID: {AppointmentId} for PatientId: {PatientId} with DoctorId: {DoctorId} on {AppointmentDateTime}",
                        appointment.Id, patientId, doctorId, appointmentDateTime);


                    appointment2 = await unitOfWork.AppointmentsRepository.GetAppointmentWithDetailsAsync(appointment.Id, cancellationToken);

                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while booking appointment for PatientId: {PatientId} with DoctorId: {DoctorId} on {AppointmentDate} at {AppointmentTime}",
                       patientId, doctorId, appointmentDate.ToShortDateString(), appointmentTime);
                    throw; // إعادة رمي الاستثناء بعد تسجيله
                }
            }

            await notification.SendBookingConfirmationAsync(appointment2);

            return appointment2;
        }

        public async Task<Appointment> RescheduleAppointmentAsync(RescheduleAppointmentCommand command, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Attempting to reschedule appointment ID: {AppointmentId} for PatientId: {PatientId} on {AppointmentDate} at {AppointmentTime}",
                command.AppointmentId, command.PatientId, command.AppointmentDate.ToShortDateString(), command.AppointmentTime);

            var appointment = await unitOfWork.AppointmentsRepository.GetAppointmentWithDetailsAsync(command.AppointmentId, cancellationToken);

            if (appointment == null)
            {
                logger.LogError("Appointment with ID: {AppointmentId} not found for rescheduling", command.AppointmentId);
                throw new NotFoundException("Appointment not found.");
            }

            if (appointment.PatientId != command.PatientId)
                throw new UnauthorizedException("You are not authorized to reschedule this appointment.");

            var oldAppointmentDate = appointment.AppointmentDate;

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

            await notification.SendRescheduleAsync(appointment, oldAppointmentDate);
            
            return appointment;
        }
        
        public async Task<Appointment> CancelAppointmentAsync(CancelAppointmentCommand command, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Attempting to cancel appointment ID: {AppointmentId} for PatientId: {PatientId}",
                command.AppointmentId, command.PatientId);

            var appointment = await unitOfWork.AppointmentsRepository.GetAppointmentWithDetailsAsync(command.AppointmentId, cancellationToken);


            if (appointment == null)
            {
                logger.LogError("Appointment with ID: {AppointmentId} not found for rescheduling", command.AppointmentId);
                throw new NotFoundException("Appointment not found.");
            }

            if (appointment.PatientId != command.PatientId)
                throw new UnauthorizedException("You are not authorized to reschedule this appointment.");

            appointment.Cancel();

            StatePaymentOnAppointmentCancellation(appointment);

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

            await notification.SendCancellationAsync(appointment);

            return appointment;
        }

        public async Task<Appointment> ConfirmAppointmentAsync(int AppointmentId, int PatientId, PaymentMethod method,
            string? notes = null, decimal? amount = null, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Attempting to confirm appointment ID: {AppointmentId} for PatientId: {PatientId}",
                AppointmentId, PatientId);

            var appointment = await unitOfWork.AppointmentsRepository.GetAppointmentWithDetailsAsync(AppointmentId, cancellationToken);

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
                    await paymentService.ConfirmPaymentAsync(appointment.Id, method, notes, amount, cancellationToken);

                    appointment.Confirm();

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
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while confirming appointment ID: {AppointmentId} for PatientId: {PatientId}",
                        AppointmentId, PatientId);

                    throw; // إعادة رمي الاستثناء بعد تسجيله
                }
            }

            await notification.SendPaymentConfirmationAsync(appointment);

            return appointment;
        }

        public async Task<Appointment> NoShowAppointmentAsync(NoShowAppointmentCommand command, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Attempting to mark no-show for appointment ID: {AppointmentId} for DoctorId: {DoctorId}",
                command.AppointmentId, command.DoctorId);

            var appointment = await unitOfWork.AppointmentsRepository.GetAppointmentWithDetailsAsync(command.AppointmentId , cancellationToken);
            if (appointment == null)
            {
                logger.LogError("Appointment with ID: {AppointmentId} not found for no-show", command.AppointmentId);
                throw new NotFoundException("Appointment not found.");
            }

            if (appointment.DoctorId != command.DoctorId)
                throw new UnauthorizedException("You are not authorized to mark no-show for this appointment.");

            appointment.NoShow();
            var result = await unitOfWork.SaveAsync();
            if (result == 0)
            {
                logger.LogError("Failed to save the No-Show appointment for DoctorId: {DoctorId} on AppointmentId: {AppointmentId}",
                    command.DoctorId, command.AppointmentId);
                // إذا فشل الحفظ دون استثناء، يجب رفع استثناء هنا
                throw new DatabaseSaveException("Failed to save the No-Show appointment to the database.");
            }

            logger.LogInformation("Successfully marked No-Show for appointment with ID: {AppointmentId} for DoctorId: {DoctorId}",
                appointment.Id, command.DoctorId);

            await notification.SendNoShowAsync(appointment);

            return appointment;
        }

        public async Task<Appointment> CompleteAppointmentAsync(CompleteAppointmentCommand command, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Attempting to complete appointment ID: {AppointmentId} for DoctorId: {DoctorId}",
                command.AppointmentId, command.DoctorId);

            Appointment appointment = null;

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    appointment = await unitOfWork.AppointmentsRepository.GetAppointmentWithDetailsAsync(command.AppointmentId , cancellationToken);

                    if (appointment == null)
                    {
                        logger.LogError("Appointment with ID: {AppointmentId} not found for completion", command.AppointmentId);
                        throw new NotFoundException("Appointment not found.");
                    }

                    if (appointment.DoctorId != command.DoctorId)
                        throw new UnauthorizedException("You are not authorized to complete this appointment.");

                    appointment.Complete();

                    await medicalRecordService.CreateMedicalRecordAsync(appointment,
                        command.Diagnosis,command.Description,  command.Medicines, command.AdditionalNotes, cancellationToken);

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

                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while completing appointment ID: {AppointmentId} for DoctorId: {DoctorId}",
                        command.AppointmentId, command.DoctorId);
                    throw; // إعادة رمي الاستثناء بعد تسجيله
                }
            }

            await notification.SendMedicalReportAsync(appointment);

            return appointment;
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

        private void StatePaymentOnAppointmentCancellation(Appointment appointment)
        {
            if (appointment.Payment != null && appointment.Payment.PaymentStatus == PaymentStatus.Pending)
            {
                appointment.Payment.MarkAsCancelling("Appointment cancelled");
            }
            else if (appointment.Payment != null && appointment.Payment.PaymentStatus == PaymentStatus.Paid)
            {
                appointment.Payment.MarkAsRefunded("Appointment cancelled by patient.");
            }
        }

        public async Task<PagedResult<Appointment>> GetDoctorAppointmentsAsync(GetDoctorAppointmentsQuery doctorAppointmentQuery, CancellationToken cancellationToken = default)
        {
            var (items, totalCount) = await unitOfWork.AppointmentsRepository.GetDoctorAppointmentsAsync
                (doctorAppointmentQuery.DoctorId.Value, doctorAppointmentQuery.PageNumber,
                doctorAppointmentQuery.PageSize, doctorAppointmentQuery.DateTime, cancellationToken: cancellationToken);

            return new PagedResult<Appointment>(items, totalCount, doctorAppointmentQuery.PageNumber, doctorAppointmentQuery.PageSize);
        }

        public async Task<PagedResult<Appointment>> GetPatientAppointmentsAsync(GetPatientAppointmentsQuery patientAppointmentQuery, CancellationToken cancellationToken = default)
        {
            var (items, totalCount) = await unitOfWork.AppointmentsRepository.GetPatientAppointmentsAsync
                (patientAppointmentQuery.PatientId.Value, patientAppointmentQuery.PageNumber,
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
                (appointmentsByStatusForDoctorQuery.Status, appointmentsByStatusForDoctorQuery.DoctorId.Value,
                appointmentsByStatusForDoctorQuery.PageNumber,
                appointmentsByStatusForDoctorQuery.PageSize, appointmentsByStatusForDoctorQuery.Start,
                appointmentsByStatusForDoctorQuery.End, cancellationToken: cancellationToken);

            return new PagedResult<Appointment>(items, totalCount, appointmentsByStatusForDoctorQuery.PageNumber, appointmentsByStatusForDoctorQuery.PageSize);
        }

        public async Task<PagedResult<Appointment>> GetPastAppointmentsForDoctorAsync(GetPastAppointmentsForDoctorQuery appointmentsForDoctorQuery, CancellationToken cancellationToken = default)
        {
            var (items, totalCount) = await unitOfWork.AppointmentsRepository.GetPastAppointmentsForDoctorAsync
                (appointmentsForDoctorQuery.DoctorId.Value, appointmentsForDoctorQuery.PageNumber,
                appointmentsForDoctorQuery.PageSize, cancellationToken);


            return new PagedResult<Appointment>(items, totalCount, appointmentsForDoctorQuery.PageNumber, appointmentsForDoctorQuery.PageSize);
        }

        public async Task<PagedResult<Appointment>> GetPastAppointmentsForPatientAsync(GetPastAppointmentsForPatientQuery appointmentsForPatientQuery, CancellationToken cancellationToken = default)
        {
            var (items, totalCount) = await unitOfWork.AppointmentsRepository.GetPastAppointmentsForPatientAsync
                (appointmentsForPatientQuery.PatientId.Value, appointmentsForPatientQuery.PageNumber,
                appointmentsForPatientQuery.PageSize, cancellationToken);


            return new PagedResult<Appointment>(items, totalCount, appointmentsForPatientQuery.PageNumber, appointmentsForPatientQuery.PageSize);

        }

        public async Task<AppointmentStatsDto> GetAdminAppointmentsStatsAsync(GetAdminAppointmentsStatsQuery query, CancellationToken cancellationToken = default)
        {
            var start = query.StartDate ?? DateTime.Today;
            var end = query.EndDate ?? DateTime.Today.AddDays(1).AddSeconds(-1);

            var counts = await unitOfWork.AppointmentsRepository
                .GetAppointmentsCountByStatusAsync(start, end, cancellationToken);

            return new AppointmentStatsDto
            {
                TotalAppointments = counts.Values.Sum(),
                Pending = counts.ContainsKey(AppointmentStatus.Pending) ? counts[AppointmentStatus.Pending] : 0,
                Confirmed = counts.ContainsKey(AppointmentStatus.Confirmed) ? counts[AppointmentStatus.Confirmed] : 0,
                Cancelled = counts.ContainsKey(AppointmentStatus.Cancelled) ? counts[AppointmentStatus.Cancelled] : 0,
                NoShow = counts.ContainsKey(AppointmentStatus.NoShow) ? counts[AppointmentStatus.NoShow] : 0,
                Rescheduled = counts.ContainsKey(AppointmentStatus.Rescheduled) ? counts[AppointmentStatus.Rescheduled] : 0,
                Completed = counts.ContainsKey(AppointmentStatus.Completed) ? counts[AppointmentStatus.Completed] : 0
            };
        }
    }
}
