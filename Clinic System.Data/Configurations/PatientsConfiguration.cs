namespace Clinic_System.Data.Configurations
{
    /// <summary>
    /// Configuration for Patients Entity
    /// 
    /// لماذا نستخدم Entity Configuration؟
    /// 1. فصل الـ Database Schema Configuration عن الـ Entity Class
    /// 2. تحديد Constraints و Rules للـ Database (مثل MaxLength, Required, Indexes)
    /// 3. تحديد العلاقات (Relationships) بين الـ Entities
    /// 4. تحديد Cascade Delete Rules
    /// 5. تحسين الأداء باستخدام Indexes
    /// 
    /// كيف يعمل؟
    /// - EF Core يقرأ هذه الـ Configurations تلقائياً من خلال modelBuilder.ApplyConfigurationsFromAssembly()
    /// - يتم تطبيقها عند إنشاء الـ Database أو Migration
    /// </summary>
    public class PatientsConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            // ============================================
            // Primary Key Configuration
            // ============================================
            builder.HasKey(p => p.Id);
            // يحدد أن Id هو Primary Key

            // ============================================
            // Table Name Configuration
            // ============================================
            builder.ToTable("Patients");
            // يحدد اسم الجدول في Database (اختياري - EF Core يستخدم اسم الـ Class تلقائياً)

            // ============================================
            // Properties Configuration (من Person Base Class)
            // ============================================
            
            // FullName: Required, MaxLength 100
            builder.Property(p => p.FullName)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("PatientName");
            // IsRequired: لا يمكن أن يكون null
            // HasMaxLength: يحدد الحد الأقصى لطول النص في Database
            // HasColumnName: اسم العمود في Database يكون "PatientName" (أكثر وضوحاً)

            // Gender: Required, Enum
            builder.Property(d => d.Gender)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(10)
                .HasColumnName("Gender");
            // HasConversion<int>: يحول Enum إلى int في Database

            // DateOfBirth: Required
            builder.Property(p => p.DateOfBirth)
                .IsRequired()
                .HasColumnName("DateOfBirth");

            // Address: Required, MaxLength 200
            builder.Property(p => p.Address)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("Address");

            // Phone: Required, MaxLength 20
            builder.Property(p => p.Phone)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("PhoneNumber");
            // HasColumnName: اسم العمود في Database يكون "PhoneNumber" (أكثر وضوحاً)

            // ============================================
            // ApplicationUser Relationship (One-to-One)
            // ============================================
            // Note: Navigation property موجودة فقط في Data layer (لـ EF Core)
            // Core entity يحتوي فقط على ApplicationUserId (Foreign Key)
            // هذا يحافظ على Clean Architecture
            
            // تكوين العلاقة باستخدام Fluent API
            builder.HasOne<ApplicationUser>()
                .WithOne(u => u.Patient)
                .HasForeignKey<Patient>(p => p.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);
            // HasOne<ApplicationUser>(): Patient له ApplicationUser واحد
            // WithOne(u => u.Patient): ApplicationUser له Patient واحد
            // HasForeignKey: يحدد Foreign Key في Patients table
            // OnDelete(DeleteBehavior.Cascade): عند حذف ApplicationUser، يتم حذف Patient

            // تسمية Foreign Key Column
            builder.Property(p => p.ApplicationUserId)
                .IsRequired()
                .HasColumnName("ApplicationUserId");

            // Index على ApplicationUserId للأداء والـ Unique constraint
            builder.HasIndex(p => p.ApplicationUserId)
                .IsUnique()
                .HasDatabaseName("IX_Patients_ApplicationUserId");
            // IsUnique: يضمن أن كل ApplicationUser مرتبط بـ Patient واحد فقط

            // ============================================
            // Appointments Relationship (One-to-Many)
            // ============================================
            builder.HasMany(p => p.Appointments)
                .WithOne(a => a.Patient)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
            // HasMany: Patient له Appointments متعددة
            // WithOne: كل Appointment له Patient واحد
            // OnDelete(DeleteBehavior.Restrict): منع حذف Patient إذا كان له Appointments
            // ملاحظة: Index على PatientId موجود في AppointmentsConfiguration

            // ============================================
            // Soft Delete Configuration
            // ============================================
            builder.Property(p => p.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("IsDeleted");
            // HasDefaultValue: قيمة افتراضية في Database

            builder.Property(p => p.DeletedAt)
                .IsRequired(false)
                .HasColumnName("DeletedAt");
            // IsRequired(false): يمكن أن يكون null

            // ============================================
            // Audit Fields Configuration
            // ============================================
            builder.Property(p => p.CreatedAt)
                .IsRequired()
                .HasColumnName("CreatedAt");

            builder.Property(p => p.UpdatedAt)
                .IsRequired(false)
                .HasColumnName("UpdatedAt");

            // Index على CreatedAt للاستعلامات السريعة
            builder.HasIndex(p => p.CreatedAt)
                .HasDatabaseName("IX_Patients_CreatedAt");
        }
    }
}

