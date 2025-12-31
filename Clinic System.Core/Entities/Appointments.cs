using Clinic_System.Core.Exceptions;
using System.Threading.Channels;

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

        private void InvalidAppointmentState(string cancel, string complete,string noshow)
        {
            if (Status == AppointmentStatus.Completed)
                throw new InvalidAppointmentStateException(complete);
            if (Status == AppointmentStatus.Cancelled)
                throw new InvalidAppointmentStateException(cancel);
            if (Status == AppointmentStatus.NoShow)
                throw new InvalidAppointmentStateException(noshow);
        }

        public void Reschedule(DateTime newDate)
        {

            InvalidAppointmentState("Cannot reschedule a cancelled appointment.",
                "Cannot reschedule a completed appointment.",
                "Cannot reschedule a no-show appointment.");

            // قاعدة عمل: لا يمكن التعديل لموعد في الماضي
            if (newDate < DateTime.Now)
                throw new InvalidOperationException("Cannot reschedule to a past date.");

            this.AppointmentDate = newDate;
            this.Status = AppointmentStatus.Rescheduled;
            this.UpdatedAt = DateTime.Now;
        }

        public void Cancel()
        {
            InvalidAppointmentState("Cannot cancel a completed appointment.",
                "Appointment is already cancelled.", "Cannot cancel a no-show appointment.");

            if (AppointmentDate < DateTime.Now.AddHours(1))
                throw new InvalidAppointmentStateException("Cannot cancel appointment less than 1 hour before start.");

            this.Status = AppointmentStatus.Cancelled;
            this.UpdatedAt = DateTime.Now;
        }

        public void Complete()
        {
            InvalidAppointmentState("Cannot complete a cancelled appointment.",
                "Appointment is already completed.", "Cannot complete a no-show appointment.");

            this.Status = AppointmentStatus.Completed;
            this.UpdatedAt = DateTime.Now;
        }

        public void NoShow()
        {

            InvalidAppointmentState("Cannot mark a cancelled appointment as no-show.",
                "Cannot mark a completed appointment as no-show.",
                "Appointment is already marked as no-show.");

            this.Status = AppointmentStatus.NoShow;
            this.UpdatedAt = DateTime.Now;
        }

        public void Confirm()
        {
            InvalidAppointmentState("Cannot confirm a cancelled appointment.",
                "Cannot confirm a completed appointment.",
                "Cannot confirm a no-show appointment.");

            if (Status == AppointmentStatus.Confirmed)
                throw new InvalidAppointmentStateException("Appointment is already confirmed.");

            this.Status = AppointmentStatus.Confirmed;
            this.UpdatedAt = DateTime.Now;
        }
    }
}
