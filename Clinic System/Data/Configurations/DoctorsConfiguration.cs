namespace Clinic_System.Data.Configurations
{
    /// <summary>
    /// Configuration for Doctors Entity
    /// 
    /// نفس المبادئ المستخدمة في PatientsConfiguration
    /// مع إضافة Specialization Field
    /// </summary>
    public class DoctorsConfiguration : IEntityTypeConfiguration<Doctors>
    {
        public void Configure(EntityTypeBuilder<Doctors> builder)
        {
            // ============================================
            // Primary Key
            // ============================================
            builder.HasKey(d => d.Id);

            builder.ToTable("Doctors");

            // ============================================
            // Properties from Person Base Class
            // ============================================
            builder.Property(d => d.FullName)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("DoctorName");

            builder.Property(d => d.Gender)
                .IsRequired()
                .HasConversion<int>()
                .HasColumnName("Gender");

            builder.Property(d => d.DateOfBirth)
                .IsRequired()
                .HasColumnName("DateOfBirth");

            builder.Property(d => d.Address)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("Address");

            builder.Property(d => d.Phone)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("PhoneNumber");

            // ============================================
            // Specialization Property (خاص بـ Doctors)
            // ============================================
            builder.Property(d => d.Specialization)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("Specialization");
            // Specialization: التخصص الطبي (مثل: قلب، عظام، أطفال)

            // Index على Specialization للبحث السريع
            builder.HasIndex(d => d.Specialization)
                .HasDatabaseName("IX_Doctors_Specialization");

            // ============================================
            // ApplicationUser Relationship
            // ============================================
            builder.HasOne(d => d.ApplicationUser)
                .WithOne(u => u.Doctor)
                .HasForeignKey<Doctors>(d => d.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);

            // تسمية Foreign Key Column
            builder.Property(d => d.ApplicationUserId)
                .HasColumnName("ApplicationUserId");

            builder.HasIndex(d => d.ApplicationUserId)
                .IsUnique();

            // ============================================
            // Appointments Relationship
            // ============================================
            builder.HasMany(d => d.Appointments)
                .WithOne(a => a.Doctor)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
            // منع حذف Doctor إذا كان له Appointments

            // ============================================
            // Soft Delete
            // ============================================
            builder.Property(d => d.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("IsDeleted");

            builder.Property(d => d.DeletedAt)
                .IsRequired(false)
                .HasColumnName("DeletedAt");

            // ============================================
            // Audit Fields
            // ============================================
            builder.Property(d => d.CreatedAt)
                .IsRequired()
                .HasColumnName("CreatedAt");

            builder.Property(d => d.UpdatedAt)
                .IsRequired(false)
                .HasColumnName("UpdatedAt");

            builder.HasIndex(d => d.CreatedAt)
                .HasDatabaseName("IX_Doctors_CreatedAt");
        }
    }
}

