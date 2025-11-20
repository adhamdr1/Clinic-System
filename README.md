# Clinic System

## 🗄️ Database Schema

## Entity-Relationship Diagram (ERD)

![ERD - Clinic System](https://github.com/user-attachments/assets/30518b4a-ca26-416f-b620-e8b4972d2ee1)

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



Tell me whether you want me to push the change to the `main` branch or create a new branch and open a PR, and I will prepare the commit.
