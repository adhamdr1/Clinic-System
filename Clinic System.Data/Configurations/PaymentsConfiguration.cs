namespace Clinic_System.Data.Configurations
{
    /// <summary>
    /// Configuration for Payments Entity
    /// 
    /// هذا الـ Configuration يحدد:
    /// 1. العلاقة مع Appointments (Many-to-One)
    /// 2. Constraints على AmountPaid (يجب أن يكون > 0)
    /// 3. PaymentMethod Enum Configuration
    /// </summary>
    public class PaymentsConfiguration : IEntityTypeConfiguration<Payments>
    {
        public void Configure(EntityTypeBuilder<Payments> builder)
        {
            // ============================================
            // Primary Key
            // ============================================
            builder.HasKey(p => p.Id);

            builder.ToTable("Payments", table =>
            {
                // Check Constraint: AmountPaid يجب أن يكون أكبر من 0
                table.HasCheckConstraint("CK_Payments_AmountPaid_Positive", 
                    "[AmountPaid] > 0");
            });

            // ============================================
            // AmountPaid Property
            // ============================================
            builder.Property(p => p.AmountPaid)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasColumnName("AmountPaid");
            // HasColumnType: يحدد نوع البيانات في Database
            // decimal(18,2): 18 رقم إجمالي، 2 بعد الفاصلة (مثل: 1500.50)
            // HasColumnName: اسم العمود في Database
            // Check Constraint محدد في ToTable أعلاه

            // ============================================
            // PaymentDate Property
            // ============================================
            builder.Property(p => p.PaymentDate)
                .IsRequired()
                .HasColumnName("PaymentDate");

            // Index على PaymentDate للبحث السريع
            builder.HasIndex(p => p.PaymentDate)
                .HasDatabaseName("IX_Payments_PaymentDate");

            // ============================================
            // PaymentMethod Property
            // ============================================
            builder.Property(p => p.PaymentMethod)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50)
                .HasColumnName("PaymentMethod");
            // HasConversion<string>: يحول Enum إلى String في Database (بدلاً من int)
            // HasMaxLength(50): الحد الأقصى لطول النص
            // HasColumnName: اسم العمود في Database

            // Index على PaymentMethod للبحث السريع
            builder.HasIndex(p => p.PaymentMethod)
                .HasDatabaseName("IX_Payments_PaymentMethod");

            // ============================================
            // AdditionalNotes Property (Optional)
            // ============================================
            builder.Property(p => p.AdditionalNotes)
                .IsRequired(false)
                .HasMaxLength(500)
                .HasColumnName("AdditionalNotes");
            // AdditionalNotes: ملاحظات إضافية (مثل: رقم الفاتورة)

            // ============================================
            // Appointment Relationship (One-to-One)
            // ============================================
            builder.HasOne(p => p.Appointment)
                .WithOne(a => a.Payment)
                .HasForeignKey<Payments>(p => p.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);
            // OnDelete(DeleteBehavior.Cascade): عند حذف Appointment، يتم حذف Payment تلقائياً
            // هذا متسق مع AppointmentsConfiguration حيث Payment لا معنى له بدون Appointment

            // تسمية Foreign Key Column
            builder.Property(p => p.AppointmentId)
                .HasColumnName("AppointmentId");

            // Index على AppointmentId (لكنه Unique بالفعل بسبب One-to-One)
            builder.HasIndex(p => p.AppointmentId)
                .IsUnique()
                .HasDatabaseName("IX_Payments_AppointmentId");
            // IsUnique: يضمن أن كل Appointment له Payment واحد فقط

            // Composite Index على PaymentDate و PaymentMethod
            builder.HasIndex(p => new { p.PaymentDate, p.PaymentMethod })
                .HasDatabaseName("IX_Payments_Date_Method");
            // مفيد للبحث عن المدفوعات في تاريخ معين بطريقة دفع معينة

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
                .HasDatabaseName("IX_Payments_CreatedAt");
        }
    }
}

