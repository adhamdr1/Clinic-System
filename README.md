# Clinic System - ASP.NET Core API

## 🗄️ Database Schema

## Entity-Relationship Diagram (ERD)

<img width="1920" height="1080" alt="image" src="https://github.com/user-attachments/assets/45399b6a-20f0-4907-b6a1-49b79027f4c1" />


The database schema includes the following main entities and their relationships:

### Main Entities:
- **Patients** - Stores patient personal details and contact information
- **Doctors** - Stores doctor information and their medical specializations
- **Appointments** - Manages scheduling between patients and doctors with status tracking
- **MedicalRecords** - Stores clinical details, diagnosis, and notes for attended appointments
- **Prescriptions** - Manages medication details, dosage, and instructions linked to medical records
- **Payments** - Tracks financial transactions and payment methods for appointments

### Relationships:
- A patient can have multiple appointments (One-to-many: Patient → Appointments).
- A doctor can have multiple appointments (One-to-many: Doctor → Appointments).
- An appointment may have one medical record (One-to-one: Appointment → Medical Records).
- A medical record can have one prescriptions (One-to-one: Medical Records → Prescriptions).
- An appointment can be associated with one payments (One-to-one: Appointment → Payments).
