using Clinic_System.Core.Exceptions;

namespace Clinic_System.Core.Entities
{
    public class Appointment : ISoftDelete, IAuditable
    {
        public virtual int Id { get; set; }
        public virtual DateTime AppointmentDate { get; set; }
        public virtual AppointmentStatus Status { get; set; }

        public virtual int PatientId { get; set; }
        public virtual Patient Patient { get; set; } = null!;

        public virtual int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; } = null!;

        public virtual MedicalRecord? MedicalRecord { get; set; }
        public virtual Payment? Payment { get; set; }

        // Soft Delete
        public virtual bool IsDeleted { get; set; } = false;
        public virtual DateTime? DeletedAt { get; set; }

        // Audit Fields (automatically set by SaveChanges)
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime? UpdatedAt { get; set; }

        public void Reschedule(DateTime newDate)
        {
            if (Status != AppointmentStatus.Confirmed && Status != AppointmentStatus.Pending)
                throw new InvalidAppointmentStateException("Cannot reschedule an inactive appointment.");

            // قاعدة عمل: لا يمكن التعديل لموعد في الماضي
            if (newDate < DateTime.Now)
                throw new InvalidOperationException("Cannot reschedule to a past date.");

            this.AppointmentDate = newDate;
            this.Status = AppointmentStatus.Rescheduled;
            this.UpdatedAt = DateTime.Now;
        }

        public void Cancel()
        {
            // قاعدة عمل: لا يمكن إلغاء موعد تم الانتهاء منه
            if (Status == AppointmentStatus.Completed)
                throw new InvalidAppointmentStateException("Cannot cancel a completed appointment.");
            if (Status == AppointmentStatus.Cancelled)
                throw new InvalidAppointmentStateException("Appointment is already cancelled.");
            if (AppointmentDate < DateTime.Now.AddHours(1))
                throw new InvalidAppointmentStateException("Cannot cancel appointment less than 1 hour before start.");

            this.Status = AppointmentStatus.Cancelled;
            this.UpdatedAt = DateTime.Now;
        }

        public void Complete()
        {
            // قاعدة عمل: لا يمكن إكمال موعد تم إلغاؤه
            if (Status == AppointmentStatus.Cancelled)
                throw new InvalidAppointmentStateException("Cannot complete a cancelled appointment.");
            if (Status == AppointmentStatus.Completed)
                throw new InvalidAppointmentStateException("Appointment is already completed.");

            this.Status = AppointmentStatus.Completed;
            this.UpdatedAt = DateTime.Now;
        }

        public void NoShow()
        {
            // قاعدة عمل: لا يمكن وضع حالة عدم الحضور لموعد تم إلغاؤه أو إكماله
            if (Status == AppointmentStatus.Cancelled)
                throw new InvalidAppointmentStateException("Cannot mark a cancelled appointment as no-show.");
            if (Status == AppointmentStatus.Completed)
                throw new InvalidAppointmentStateException("Cannot mark a completed appointment as no-show.");
            this.Status = AppointmentStatus.NoShow;
            this.UpdatedAt = DateTime.Now;
        }

        public void Confirm()
        {
            // قاعدة عمل: لا يمكن تأكيد موعد تم إلغاؤه أو إكماله
            if (Status == AppointmentStatus.Cancelled)
                throw new InvalidAppointmentStateException("Cannot confirm a cancelled appointment.");
            if (Status == AppointmentStatus.Completed)
                throw new InvalidAppointmentStateException("Cannot confirm a completed appointment.");

            this.Status = AppointmentStatus.Confirmed;
            this.UpdatedAt = DateTime.Now;
        }
    }
}
