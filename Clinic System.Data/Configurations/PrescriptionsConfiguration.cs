namespace Clinic_System.Data.Configurations
{
    /// <summary>
    /// Configuration for Prescriptions Entity
    /// 
    /// هذا الـ Configuration يحدد:
    /// 1. العلاقة مع MedicalRecords (Many-to-One)
    /// 2. Constraints على الحقول (Dosage, MedicationName, etc.)
    /// 3. Check Constraints على StartDate و EndDate
    /// </summary>
    public class PrescriptionsConfiguration : IEntityTypeConfiguration<Prescriptions>
    {
        public void Configure(EntityTypeBuilder<Prescriptions> builder)
        {
            // ============================================
            // Primary Key
            // ============================================
            builder.HasKey(p => p.Id);

            builder.ToTable("Prescriptions", table =>
            {
                // Check Constraint: EndDate يجب أن يكون بعد StartDate
                table.HasCheckConstraint("CK_Prescriptions_EndDate_After_StartDate", 
                    "[EndDate] > [StartDate]");
            });

            // ============================================
            // MedicationName Property
            // ============================================
            builder.Property(p => p.MedicationName)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("MedicationName");
            // MedicationName: اسم الدواء

            // Index على MedicationName للبحث السريع
            builder.HasIndex(p => p.MedicationName)
                .HasDatabaseName("IX_Prescriptions_MedicationName");

            // ============================================
            // Dosage Property
            // ============================================
            builder.Property(p => p.Dosage)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("Dosage");
            // Dosage: الجرعة (مثل: 500mg, 2 tablets)

            // ============================================
            // Frequency Property
            // ============================================
            builder.Property(p => p.Frequency)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("Frequency");
            // Frequency: التكرار (مثل: مرتين يومياً، كل 8 ساعات)

            // ============================================
            // StartDate Property
            // ============================================
            builder.Property(p => p.StartDate)
                .IsRequired()
                .HasColumnName("StartDate");

            // ============================================
            // EndDate Property
            // ============================================
            builder.Property(p => p.EndDate)
                .IsRequired()
                .HasColumnName("EndDate");

            // ============================================
            // SpecialInstructions Property (Optional)
            // ============================================
            builder.Property(p => p.SpecialInstructions)
                .IsRequired(false)
                .HasMaxLength(500)
                .HasColumnName("SpecialInstructions");
            // SpecialInstructions: تعليمات خاصة (مثل: قبل الأكل، بعد الأكل)

            // ============================================
            // MedicalRecord Relationship (Many-to-One)
            // ============================================
            builder.HasOne(p => p.MedicalRecord)
                .WithMany(m => m.Prescriptions)
                .HasForeignKey(p => p.MedicalRecordId)
                .OnDelete(DeleteBehavior.Cascade);
            // OnDelete(DeleteBehavior.Cascade): عند حذف MedicalRecord، يتم حذف جميع Prescriptions تلقائياً
            // هذا متسق مع MedicalRecordsConfiguration حيث Prescriptions لا معنى لها بدون MedicalRecord

            // تسمية Foreign Key Column
            builder.Property(p => p.MedicalRecordId)
                .HasColumnName("MedicalRecordId");

            // Index على MedicalRecordId للأداء
            builder.HasIndex(p => p.MedicalRecordId)
                .HasDatabaseName("IX_Prescriptions_MedicalRecordId");

            // ============================================
            // Soft Delete
            // ============================================
            builder.Property(p => p.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("IsDeleted");

            builder.Property(p => p.DeletedAt)
                .IsRequired(false)
                .HasColumnName("DeletedAt");

            // ============================================
            // Audit Fields
            // ============================================
            builder.Property(p => p.CreatedAt)
                .IsRequired()
                .HasColumnName("CreatedAt");

            builder.Property(p => p.UpdatedAt)
                .IsRequired(false)
                .HasColumnName("UpdatedAt");

            builder.HasIndex(p => p.CreatedAt)
                .HasDatabaseName("IX_Prescriptions_CreatedAt");
        }
    }
}

