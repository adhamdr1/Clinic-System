namespace Clinic_System.Application.Events
{
    // 1. حدث حجز الموعد
    public record AppointmentBookedEvent
    {
        public int AppointmentId { get; init; }
        public required string PatientUserId { get; init; } // ده الـ ApplicationUserId عشان نجيب بيه الإيميل بسهولة
        public required string PatientName { get; init; }
        public required string DoctorName { get; init; }
        public required string DoctorSpecialization { get; init; }
        public required DateTime AppointmentDate { get; init; }
    }

    // 2. حدث إلغاء الموعد (من قبل المريض)
    public record AppointmentCancelledEvent
    {
        public int AppointmentId { get; init; }
        public required string PatientUserId { get; init; }
        public required string PatientName { get; init; }
        public required string DoctorName { get; init; }
        public required string DoctorSpecialization { get; init; }
        public required DateTime AppointmentDate { get; init; }
    }

    // 3. حدث إعادة جدولة الموعد
    public record AppointmentRescheduledEvent
    {
        public int AppointmentId { get; init; }
        public required string PatientUserId { get; init; }
        public required string PatientName { get; init; }
        public required string DoctorName { get; init; }
        public required string DoctorSpecialization { get; init; }
        public required DateTime OldDate { get; init; }
        public required DateTime NewDate { get; init; }
    }

    // 4. حدث عدم الحضور
    public record AppointmentNoShowEvent
    {
        public int AppointmentId { get; init; }
        public required string PatientUserId { get; init; }
        public required string PatientName { get; init; }
        public required string DoctorName { get; init; }
        public required string DoctorSpecialization { get; init; }
        public required DateTime AppointmentDate { get; init; }
    }

    // 5. حدث تأكيد الحجز (الدفع)
    public record AppointmentConfirmedEvent
    {
        public int AppointmentId { get; init; }
        public required string PatientUserId { get; init; }
        public required string PatientName { get; init; }
        public required string DoctorName { get; init; }
        public required string DoctorSpecialization { get; init; }
        public required DateTime AppointmentDate { get; init; }
        public required decimal AmountPaid { get; init; }
        public required string PaymentMethod { get; init; }
        public required int TransactionId { get; init; }
    }

    //  هنعمل Record صغير يمثل بيانات الدواء اللي هتتبعت في الرسالة
    public record MedicationInfo
    {
        public string MedicationName { get; init; }
        public string Dosage { get; init; }
        public string Frequency { get; init; }
        public string? SpecialInstructions { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
    }

    // 6. هنعدل الحدث عشان ياخد اللستة دي بدل الـ string
    public record MedicalReportGeneratedEvent
    {
        public int AppointmentId { get; init; }
        public string PatientUserId { get; init; }
        public string PatientName { get; init; }
        public string DoctorName { get; init; }
        public string DoctorSpecialization { get; init; }
        public string Diagnosis { get; init; }
        public string Description { get; init; }

        // التعديل هنا: اللستة بقت من نوع الأوبجكت الجديد
        public List<MedicationInfo> Medicines { get; init; }

        public string AdditionalNotes { get; init; }
    }

    // 7. حدث الإلغاء التلقائي (بواسطة Hangfire)
    public record AppointmentAutoCancelledEvent
    {
        public int AppointmentId { get; init; }
        public required string PatientUserId { get; init; }
        public required string PatientName { get; init; }
        public required string DoctorName { get; init; }
        public required string DoctorSpecialization { get; init; }
        public required DateTime AppointmentDate { get; init; }
    }
}