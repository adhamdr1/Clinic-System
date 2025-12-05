namespace Clinic_System.Data.Configurations
{
    /// <summary>
    /// Configuration for Appointments Entity
    /// 
    /// هذا الـ Configuration مهم جداً لأنه:
    /// 1. يحدد العلاقات مع Patients و Doctors
    /// 2. يحدد Indexes على AppointmentDate للبحث السريع
    /// 3. يحدد Constraints على Status
    /// </summary>
    public class AppointmentsConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            // ============================================
            // Primary Key
            // ============================================
            builder.HasKey(a => a.Id);

            builder.ToTable("Appointments");

            // ============================================
            // AppointmentDate Property
            // ============================================
            builder.Property(a => a.AppointmentDate)
                .IsRequired()
                .HasColumnName("AppointmentDateTime");
            // AppointmentDate: تاريخ ووقت الموعد
            // HasColumnName: اسم العمود في Database يكون "AppointmentDateTime" (أكثر وضوحاً)

            // Index على AppointmentDate للبحث السريع بالمواعيد
            builder.HasIndex(a => a.AppointmentDate)
                .HasDatabaseName("IX_Appointments_AppointmentDate");
            // هذا الـ Index مهم جداً للبحث عن المواعيد في تاريخ معين

            // Composite Index على AppointmentDate و Status
            builder.HasIndex(a => new { a.AppointmentDate, a.Status })
                .HasDatabaseName("IX_Appointments_Date_Status");
            // Composite Index: Index على أكثر من عمود (مفيد للاستعلامات المعقدة)

            // ============================================
            // Status Property
            // ============================================
            builder.Property(a => a.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50)
                .HasColumnName("AppointmentStatus");
            // HasConversion<string>: يحول Enum إلى String في Database (بدلاً من int)
            // HasMaxLength(50): الحد الأقصى لطول النص
            // HasColumnName: اسم العمود في Database يكون "AppointmentStatus" (أكثر وضوحاً)
            // HasDefaultValue: القيمة الافتراضية هي "Pending" (كـ String)

            // Index على Status للبحث السريع
            builder.HasIndex(a => a.Status)
                .HasDatabaseName("IX_Appointments_Status");

            // ============================================
            // Patient Relationship (Many-to-One)
            // ============================================
            builder.HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
            // OnDelete(DeleteBehavior.Restrict): منع حذف Patient إذا كان له Appointments

            // تسمية Foreign Key Column
            builder.Property(a => a.PatientId)
                .HasColumnName("PatientId");

            // Index على PatientId للأداء
            builder.HasIndex(a => a.PatientId)
                .HasDatabaseName("IX_Appointments_PatientId");

            // ============================================
            // Doctor Relationship (Many-to-One)
            // ============================================
            builder.HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
            // منع حذف Doctor إذا كان له Appointments

            // تسمية Foreign Key Column
            builder.Property(a => a.DoctorId)
                .HasColumnName("DoctorId");

            // Index على DoctorId للأداء
            builder.HasIndex(a => a.DoctorId)
                .HasDatabaseName("IX_Appointments_DoctorId");

            // Composite Index على DoctorId و AppointmentDate
            builder.HasIndex(a => new { a.DoctorId, a.AppointmentDate })
                .HasDatabaseName("IX_Appointments_Doctor_Date");
            // مفيد للبحث عن مواعيد Doctor معين في تاريخ معين

            // ============================================
            // MedicalRecord Relationship (One-to-One)
            // ============================================
            // ملاحظة: العلاقة محددة بالكامل في MedicalRecordsConfiguration
            // هنا نحدد فقط Navigation Property بدون تكرار الـ Configuration
            builder.HasOne(a => a.MedicalRecord)
                .WithOne(m => m.Appointment);

            // ============================================
            // Payment Relationship (One-to-One)
            // ============================================
            // ملاحظة: العلاقة محددة بالكامل في PaymentsConfiguration
            // هنا نحدد فقط Navigation Property بدون تكرار الـ Configuration
            builder.HasOne(a => a.Payment)
                .WithOne(p => p.Appointment);

            // ============================================
            // Soft Delete
            // ============================================
            builder.Property(a => a.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("IsDeleted");

            builder.Property(a => a.DeletedAt)
                .IsRequired(false)
                .HasColumnName("DeletedAt");

            // ============================================
            // Audit Fields
            // ============================================
            builder.Property(a => a.CreatedAt)
                .IsRequired()
                .HasColumnName("CreatedAt");

            builder.Property(a => a.UpdatedAt)
                .IsRequired(false)
                .HasColumnName("UpdatedAt");

            builder.HasIndex(a => a.CreatedAt)
                .HasDatabaseName("IX_Appointments_CreatedAt");
        }
    }
}

