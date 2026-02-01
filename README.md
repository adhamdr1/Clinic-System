# 🏥 Elite Clinic Management System

<div align="center">

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-000000?style=for-the-badge&logo=JSON%20web%20tokens&logoColor=white)

**A comprehensive clinic management system built with ASP.NET Core 8.0, featuring advanced authentication, appointment scheduling, medical records, and payment processing.**

[Features](#-features) • [Architecture](#-architecture) • [Installation](#-installation) • [API Documentation](#-api-endpoints) • [Database](#-database-schema)

</div>

---

## 📋 Table of Contents

- [Overview](#-overview)
- [Key Features](#-key-features)
- [Architecture](#-architecture)
- [Technologies](#-technologies-used)
- [Database Schema](#-database-schema)
- [Authentication System](#-authentication--authorization)
- [API Endpoints](#-api-endpoints)
- [Installation](#-installation)
- [Configuration](#-configuration)
- [Default Users](#-default-seeded-users)
- [Screenshots](#-screenshots)
- [Future Enhancements](#-future-enhancements)
- [Contributing](#-contributing)
- [License](#-license)

---

## 🎯 Overview

**Elite Clinic Management System** is a full-featured healthcare management platform designed to streamline clinic operations. Built with modern software architecture patterns and best practices, it provides:

- **Role-based access control** (Admin, Doctor, Patient)
- **Complete appointment lifecycle** management
- **Electronic medical records** and prescriptions
- **Integrated payment processing**
- **Email notification system** with professional templates
- **Background job processing** for async tasks

The system follows **Clean Architecture** principles with clear separation of concerns across multiple layers.

---

## ✨ Key Features

### 🔐 Authentication & Authorization

![Login & JWT Authentication](./image/image1.png)

- **JWT-based authentication** with Access & Refresh Tokens
- **Secure password hashing** using ASP.NET Core Identity
- **Email confirmation** for new accounts
- **Password reset** functionality with secure tokens
- **Role-based authorization** (Admin, Doctor, Patient)
- **Refresh token rotation** for enhanced security
- Token expiration handling

**Key Components:**
- Access Token: Short-lived (configurable, e.g., 60 minutes)
- Refresh Token: Long-lived (e.g., 7 days) stored in database
- Email confirmation required for account activation
- Encrypted tokens for password reset

### 👥 Patient Management

- Patient registration with email confirmation
- Complete patient profiles (demographics, contact info)
- Medical history tracking
- Appointment history
- Search by name, phone, or ID
- Soft delete support (data retention)
- Pagination support for large datasets

### 👨‍⚕️ Doctor Management

- Doctor profiles with specializations
- Appointment scheduling
- Medical records access
- Revenue reports
- Search by specialization or name
- Doctor-specific appointment filtering

### 📅 Appointment System

![Book Appointment](./image/image2.png)

**Appointment Lifecycle:**
1. **Booking** - Patient requests appointment
2. **Pending** - Awaiting confirmation
3. **Confirmed** - Appointment confirmed with payment
4. **Completed** - Visit finished with medical record
5. **Cancelled** - Cancelled by patient/admin
6. **No-Show** - Patient didn't attend

![Confirm Appointment](./image/image3.png)

**Features:**
- Available time slot checking
- Double-booking prevention
- Appointment rescheduling
- Status-based filtering
- Past appointment history
- Doctor/Patient-specific views
- Automated email notifications

### 💳 Payment Processing

![Payment Confirmation](./image/image4.png)

**Payment Methods:**
- Cash
- Credit Card
- InstaPay

**Features:**
- Payment status tracking (Paid, Pending, Failed)
- Payment confirmation workflow
- Revenue reports (daily, by doctor)
- Payment history
- Automated payment confirmation emails

### 📋 Medical Records & Prescriptions

**Medical Records Include:**
- Diagnosis
- Visit description
- Treatment notes
- Associated prescriptions
- Doctor notes

**Prescription Management:**
- Medication name & dosage
- Frequency and duration
- Special instructions
- Start and end dates
- Linked to medical records

### 📧 Email Notification System

![Email Templates](./image/image5.png)

**Automated Emails:**
- Account confirmation
- Password reset
- Appointment booking confirmation
- Payment confirmation
- Appointment cancellation
- Appointment rescheduling
- No-show notifications
- Medical record/prescription delivery

**Email Infrastructure:**
- Professional HTML templates
- Background job processing (Hangfire)
- SMTP configuration
- Retry mechanism for failed emails

---

## 🏗️ Architecture

The system follows **Clean Architecture** with clear separation of concerns:

```
Elite-Clinic-System/
│
├── Clinic System.API/              # Presentation Layer
│   ├── Controllers/                # API Controllers
│   ├── Middlewares/                # Custom Middlewares
│   └── Program.cs                  # App Configuration
│
├── Clinic System.Application/      # Application Layer
│   ├── Features/                   # CQRS Commands & Queries
│   │   ├── Patients/
│   │   ├── Doctors/
│   │   ├── Appointments/
│   │   ├── Authentication/
│   │   ├── Payments/
│   │   └── MedicalRecords/
│   ├── DTOs/                       # Data Transfer Objects
│   ├── Mappings/                   # AutoMapper Profiles
│   └── Services/                   # Application Services
│
├── Clinic System.Core/             # Domain Layer
│   ├── Entities/                   # Domain Models
│   │   ├── Doctor.cs
│   │   ├── Patient.cs
│   │   ├── Appointment.cs
│   │   ├── MedicalRecord.cs
│   │   ├── Prescription.cs
│   │   └── Payment.cs
│   └── Enums/                      # Domain Enums
│
├── Clinic System.Data/             # Data Access Layer
│   ├── Context/                    # DbContext
│   ├── Configurations/             # Entity Configurations
│   ├── Migrations/                 # EF Migrations
│   ├── Repositories/               # Repository Pattern
│   └── Seeders/                    # Initial Data
│
└── Clinic System.Infrastructure/   # Infrastructure Layer
    ├── Authentication/             # JWT Services
    ├── Identity/                   # Identity Configuration
    ├── Email/                      # Email Services
    └── BackgroundJobs/             # Hangfire Jobs
```

### Design Patterns Used

- ✅ **Repository Pattern** - Data access abstraction
- ✅ **Unit of Work** - Transaction management
- ✅ **CQRS** (Command Query Responsibility Segregation)
- ✅ **Mediator Pattern** (MediatR)
- ✅ **Dependency Injection** - IoC Container
- ✅ **Factory Pattern** - Object creation
- ✅ **Strategy Pattern** - Payment methods

---

## 🛠️ Technologies Used

### Backend
- **ASP.NET Core 8.0** - Web API Framework
- **Entity Framework Core 8.0** - ORM
- **SQL Server** - Database
- **ASP.NET Core Identity** - User Management

### Libraries & Packages
- **MediatR** - CQRS implementation
- **AutoMapper** - Object mapping
- **FluentValidation** - Input validation
- **Hangfire** - Background job processing
- **Serilog** - Structured logging
- **Swashbuckle (Swagger)** - API documentation

### Authentication & Security
- **JWT Bearer Tokens** - Authentication
- **Refresh Tokens** - Token renewal
- **BCrypt** - Password hashing
- **Data Protection API** - Token encryption

### Email
- **SMTP Client** - Email delivery
- **HTML Email Templates** - Professional emails

---

## 🗄️ Database Schema

![Database Schema](./image/image6.png)

### Core Entities

#### 👤 **ApplicationUser** (ASP.NET Identity)
```
- Id (PK)
- UserName
- Email
- PasswordHash
- EmailConfirmed
- RefreshTokens (1:Many)
```

#### 🏥 **Doctor**
```
- Id (PK)
- ApplicationUserId (FK)
- FullName
- Gender
- DateOfBirth
- Phone
- Address
- Specialization
- Appointments (1:Many)
```

#### 🧑‍🦰 **Patient**
```
- Id (PK)
- ApplicationUserId (FK)
- FullName
- Gender
- DateOfBirth
- Phone
- Address
- Appointments (1:Many)
```

#### 📅 **Appointment**
```
- Id (PK)
- PatientId (FK)
- DoctorId (FK)
- AppointmentDate
- Status (Enum)
- MedicalRecord (1:1)
- Payment (1:1)
```

#### 📋 **MedicalRecord**
```
- Id (PK)
- AppointmentId (FK)
- Diagnosis
- DescriptionOfTheVisit
- AdditionalNotes
- Prescriptions (1:Many)
```

#### 💊 **Prescription**
```
- Id (PK)
- MedicalRecordId (FK)
- MedicationName
- Dosage
- Frequency
- SpecialInstructions
- StartDate
- EndDate
```

#### 💰 **Payment**
```
- Id (PK)
- AppointmentId (FK)
- AmountPaid
- PaymentMethod (Enum)
- PaymentStatus (Enum)
- PaymentDate
```

#### 🔄 **RefreshToken**
```
- Id (PK)
- Token
- ExpiresOn
- CreatedOn
- RevokedOn
- IsActive (computed)
- ApplicationUserId (FK)
```

### Relationships
- **One-to-One**: Appointment ↔ MedicalRecord, Appointment ↔ Payment
- **One-to-Many**: Doctor → Appointments, Patient → Appointments, MedicalRecord → Prescriptions
- **Many-to-One**: Appointments → Doctor, Appointments → Patient

---

## 🔐 Authentication & Authorization

### Login Flow

![Login Flow](./image/image1.png)

1. User sends credentials (email/username + password)
2. System validates credentials
3. If valid, generates JWT Access Token + Refresh Token
4. Returns tokens with user info and roles
5. Client stores tokens (localStorage/sessionStorage)
6. Client includes Access Token in Authorization header for API requests

**Example Login Request:**
```bash
POST /api/authentication/login
Content-Type: application/json

{
  "emailOrUserName": "adhamdr10@gmail.com",
  "password": "Doma-dr1"
}
```

**Example Response:**
```json
{
  "statusCode": 200,
  "succeeded": true,
  "message": "Login Successful",
  "data": {
    "id": 8,
    "userName": "adhamdr10",
    "email": "adhamdr10@gmail.com",
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "ey3hbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "expiresAt": "2026-02-01 22:32:54",
    "roles": ["Patient"]
  }
}
```

### Refresh Token Flow

![Refresh Token](./image/image8.png)

1. When Access Token expires, client sends Refresh Token
2. System validates Refresh Token
3. If valid, generates new Access Token + Refresh Token
4. Old Refresh Token is revoked
5. Returns new tokens

**Example Refresh Request:**
```bash
POST /api/authentication/refresh-token
Content-Type: application/json

{
  "accessToken": "expired_token_here",
  "refreshToken": "valid_refresh_token_here"
}
```

### Authorization

The system uses **Role-Based Access Control (RBAC)**:

| Role      | Permissions |
|-----------|-------------|
| **Admin** | Full system access, manage doctors, view all data |
| **Doctor**| Manage own appointments, create medical records, view patient info |
| **Patient**| Book appointments, view own records, manage profile |

**Example Protected Endpoint:**
```csharp
[Authorize(Roles = "Admin,Doctor")]
[HttpGet("pastfordoctor")]
public async Task<IActionResult> GetPastAppointmentsForDoctor(...)
```

---

## 🌐 API Endpoints

### 🔐 Authentication

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| POST | `/api/authentication/login` | User login | Public |
| POST | `/api/authentication/refresh-token` | Refresh access token | Public |
| GET  | `/api/authentication/confirm-email` | Confirm email address | Public |
| POST | `/api/authentication/resend-confirmation-email` | Resend confirmation email | Public |
| POST | `/api/authentication/send-reset-password` | Send password reset email | Public |
| POST | `/api/authentication/reset-password` | Reset password | Public |

### 👥 Patients

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/patients` | Get all patients | Admin |
| GET | `/api/patients/paging` | Get patients (paginated) | Admin |
| GET | `/api/patients/{id}` | Get patient by ID | Admin, Doctor, Patient |
| GET | `/api/patients/phone/{phone}` | Get patient by phone | Admin, Doctor |
| GET | `/api/patients/name/{name}` | Search patients by name | Admin |
| GET | `/api/patients/{id}/appointments` | Get patient appointments | Admin, Patient |
| POST | `/api/patients/create` | Register new patient | Public |
| PUT | `/api/patients/update` | Update patient info | Admin, Patient |
| DELETE | `/api/patients/soft-delete/{id}` | Soft delete patient | Admin |
| DELETE | `/api/patients/hard-delete/{id}` | Permanently delete patient | Admin |

### 👨‍⚕️ Doctors

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/doctors` | Get all doctors | Admin |
| GET | `/api/doctors/paging` | Get doctors (paginated) | Admin |
| GET | `/api/doctors/{id}` | Get doctor by ID | Admin, Doctor |
| GET | `/api/doctors/{id}/appointments` | Get doctor appointments | Admin, Doctor |
| GET | `/api/doctors/specializations/{specialization}` | Get doctors by specialization | Public |
| GET | `/api/doctors/name/{name}` | Search doctors by name | Public |
| POST | `/api/doctors/create` | Register new doctor | Admin |
| PUT | `/api/doctors/update` | Update doctor info | Admin, Doctor |
| DELETE | `/api/doctors/soft-delete/{id}` | Soft delete doctor | Admin |
| DELETE | `/api/doctors/hard-delete/{id}` | Permanently delete doctor | Admin |

### 📅 Appointments

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/appointments/stats` | Get appointment statistics | Admin |
| GET | `/api/appointments/AvailableSlots` | Check available time slots | Public |
| GET | `/api/appointments/doctor` | Get doctor's appointments | Admin, Doctor |
| GET | `/api/appointments/patient` | Get patient's appointments | Admin, Patient |
| GET | `/api/appointments/statusforadmin` | Get appointments by status | Admin |
| GET | `/api/appointments/statusfordoctor` | Get doctor appointments by status | Admin, Doctor |
| GET | `/api/appointments/pastforpatient` | Get patient's past appointments | Admin, Patient |
| GET | `/api/appointments/pastfordoctor` | Get doctor's past appointments | Admin, Doctor |
| POST | `/api/appointments/book` | Book new appointment | Admin, Patient |
| PUT | `/api/appointments/confirm` | Confirm appointment | Admin, Patient |
| PUT | `/api/appointments/complete` | Complete appointment | Admin, Doctor |
| PUT | `/api/appointments/reschedule` | Reschedule appointment | Admin, Patient |
| PUT | `/api/appointments/cancel` | Cancel appointment | Admin, Patient, Doctor |
| PUT | `/api/appointments/noshow` | Mark as no-show | Admin, Doctor |

### 📋 Medical Records

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/medicalrecords` | Get all medical records | Admin |
| GET | `/api/medicalrecords/{id}` | Get medical record by ID | Admin, Doctor, Patient |
| GET | `/api/medicalrecords/appointment/{appointmentId}` | Get record by appointment | Admin, Doctor, Patient |
| GET | `/api/medicalrecords/patient/{patientId}` | Get patient's medical records | Admin, Doctor, Patient |
| GET | `/api/medicalrecords/doctor/{doctorId}` | Get doctor's medical records | Admin, Doctor |

### 💳 Payments

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/payments/list` | Get all payments | Admin |
| GET | `/api/payments/{id}` | Get payment details | Admin, Doctor, Patient |
| GET | `/api/payments/daily-revenue` | Get daily revenue report | Admin |
| GET | `/api/payments/doctor-revenue` | Get doctor revenue report | Admin, Doctor |

### 👑 Roles

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| POST | `/api/role/promote-doctor` | Promote doctor to admin | Admin |

---

## 💻 Installation

### Prerequisites

- **.NET 8.0 SDK** or higher
- **SQL Server** (LocalDB, Express, or Full)
- **Visual Studio 2022** or **VS Code**
- **Git**

### Steps

1. **Clone the repository**
```bash
git clone https://github.com/adhamdr1/Clinic-System.git
cd Clinic-System
```

2. **Configure database connection**

Edit `appsettings.json` in `Clinic System.API` project:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\mssqllocaldb;Database=EliteClinicDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

3. **Configure JWT settings**

In `appsettings.json`:
```json
{
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyHere_MustBe32CharsOrMore",
    "IssuerIP": "https://localhost:7001",
    "AudienceIP": "https://localhost:7001",
    "TokenExpirationInMinutes": 60,
    "RefreshTokenExpirationInDays": 7
  }
}
```

4. **Configure email settings**

In `appsettings.json`:
```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SenderEmail": "your-email@gmail.com",
    "SenderName": "Elite Clinic",
    "Password": "your-app-password"
  }
}
```

5. **Restore NuGet packages**
```bash
dotnet restore
```

6. **Apply database migrations**
```bash
cd "Clinic System.API"
dotnet ef database update
```

7. **Run the application**
```bash
dotnet run
```

8. **Access Swagger UI**

Navigate to: `https://localhost:7001/swagger`

---

## ⚙️ Configuration

### Email Configuration

For **Gmail**, enable **2-Step Verification** and generate an **App Password**:
1. Go to Google Account → Security
2. Enable 2-Step Verification
3. Generate App Password
4. Use the 16-character password in `appsettings.json`

### Hangfire Dashboard

Access background jobs dashboard at: `/hangfire`

**Note:** By default, Hangfire is restricted to Admin role.

---

## 🔑 Default Seeded Users

The system comes with pre-seeded data for testing:

### Admin
- **Email:** admin@clinic.com
- **Password:** Admin@123
- **Role:** Admin

### Doctors
| Email | Password | Specialization |
|-------|----------|----------------|
| dr.ahmed@clinic.com | Doctor@123 | Cardiology |
| dr.sara@clinic.com | Doctor@123 | Pediatrics |
| dr.mohamed@clinic.com | Doctor@123 | Orthopedics |
| dr.layla@clinic.com | Doctor@123 | Dermatology |
| dr.omar@clinic.com | Doctor@123 | Neurology |

### Patients
| Email | Password |
|-------|----------|
| mahmoud.ali@gmail.com | Patient@123 |
| fatima.hassan@gmail.com | Patient@123 |
| omar.khalid@gmail.com | Patient@123 |
| nour.mohamed@gmail.com | Patient@123 |
| karim.youssef@gmail.com | Patient@123 |

---

## 📸 Screenshots

### Swagger API Documentation
![Swagger](./image/image7.png)

### Database Diagram
![Database](./image/image6.png)

---

## 🔮 Future Enhancements

### Planned Features

- ✅ **Unit Testing** - Comprehensive test coverage
- ✅ **Integration Testing** - API endpoint testing
- ✅ **SMS Notifications** - Appointment reminders via SMS
- ✅ **Docker Support** - Containerization for easy deployment
- 🔄 **CI/CD Pipeline** - Automated testing & deployment
- 🔄 **Real-time Updates** - SignalR for live notifications
- 🔄 **Mobile App** - React Native or Flutter app
- 🔄 **Reporting Dashboard** - Advanced analytics
- 🔄 **Multi-language Support** - i18n implementation
- 🔄 **Online Payments** - Stripe/PayPal integration
- 🔄 **Appointment Reminders** - Scheduled email/SMS
- 🔄 **Doctor Availability Calendar** - Visual schedule management
- 🔄 **Prescription QR Codes** - Digital prescriptions
- 🔄 **Telemedicine** - Video consultation support

---

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Coding Standards
- Follow C# coding conventions
- Use meaningful variable/method names
- Add XML documentation for public APIs
- Write unit tests for new features

---

## 📄 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

---

## 👨‍💻 Author

**Adham Adel**

- GitHub: [@adhamdr1](https://github.com/adhamdr1)
- Email: adhamdr10@gmail.com

---

## 🙏 Acknowledgments

- ASP.NET Core Team for the excellent framework
- MediatR for CQRS implementation
- All open-source contributors

---

<div align="center">

**⭐ Star this repository if you find it helpful!**

Made with ❤️ by [Adham Adel](https://github.com/adhamdr1)

</div>
