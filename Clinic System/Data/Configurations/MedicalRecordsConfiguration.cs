namespace Clinic_System.Data.Configurations
{
    /// <summary>
    /// Configuration for MedicalRecords Entity
    /// 
    /// هذا الـ Configuration يحدد:
    /// 1. العلاقة مع Appointments (One-to-One)
    /// 2. العلاقة مع Prescriptions (One-to-Many)
    /// 3. Constraints على الحقول النصية
    /// </summary>
    public class MedicalRecordsConfiguration : IEntityTypeConfiguration<MedicalRecords>
    {
        public void Configure(EntityTypeBuilder<MedicalRecords> builder)
        {
            // ============================================
            // Primary Key
            // ============================================
            builder.HasKey(m => m.Id);

            builder.ToTable("MedicalRecords");

            // ============================================
            // Diagnosis Property
            // ============================================
            builder.Property(m => m.Diagnosis)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnName("Diagnosis");
            // Diagnosis: التشخيص الطبي

            // ============================================
            // DescriptionOfTheVisit Property
            // ============================================
            builder.Property(m => m.DescriptionOfTheVisit)
                .IsRequired()
                .HasMaxLength(2000)
                .HasColumnName("VisitDescription");
            // DescriptionOfTheVisit: وصف الزيارة
            // HasColumnName: اسم العمود في Database يكون "VisitDescription" (أكثر وضوحاً)

            // ============================================
            // AdditionalNotes Property (Optional)
            // ============================================
            builder.Property(m => m.AdditionalNotes)
                .IsRequired(false)
                .HasMaxLength(1000)
                .HasColumnName("AdditionalNotes");
            // AdditionalNotes: ملاحظات إضافية (اختياري)

            // ============================================
            // Appointment Relationship (Many-to-One)
            // ============================================
            builder.HasOne(m => m.Appointment)
                .WithOne(a => a.MedicalRecord)
                .HasForeignKey<MedicalRecords>(m => m.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);
            // OnDelete(DeleteBehavior.Cascade): عند حذف Appointment، يتم حذف MedicalRecord
            // لأن MedicalRecord مرتبط بـ Appointment واحد فقط

            // تسمية Foreign Key Column
            builder.Property(m => m.AppointmentId)
                .HasColumnName("AppointmentId");

            // Index على AppointmentId (لكنه Unique بالفعل بسبب One-to-One)
            builder.HasIndex(m => m.AppointmentId)
                .IsUnique()
                .HasDatabaseName("IX_MedicalRecords_AppointmentId");
            // IsUnique: يضمن أن كل Appointment له MedicalRecord واحد فقط

            // ============================================
            // Prescriptions Relationship (One-to-Many)
            // ============================================
            builder.HasMany(m => m.Prescriptions)
                .WithOne(p => p.MedicalRecord)
                .HasForeignKey(p => p.MedicalRecordId)
                .OnDelete(DeleteBehavior.Cascade);
            // OnDelete(DeleteBehavior.Cascade): عند حذف MedicalRecord، يتم حذف جميع Prescriptions
            // لأن Prescriptions لا معنى لها بدون MedicalRecord

            // ============================================
            // Soft Delete
            // ============================================
            builder.Property(m => m.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("IsDeleted");

            builder.Property(m => m.DeletedAt)
                .IsRequired(false)
                .HasColumnName("DeletedAt");

            // ============================================
            // Audit Fields
            // ============================================
            builder.Property(m => m.CreatedAt)
                .IsRequired()
                .HasColumnName("CreatedAt");

            builder.Property(m => m.UpdatedAt)
                .IsRequired(false)
                .HasColumnName("UpdatedAt");

            builder.HasIndex(m => m.CreatedAt)
                .HasDatabaseName("IX_MedicalRecords_CreatedAt");
        }
    }
}

