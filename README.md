# 🏥 Elite Clinic Management System

<div align="center">

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-000000?style=for-the-badge&logo=JSON%20web%20tokens&logoColor=white)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-512BD4?style=for-the-badge&logo=.net&logoColor=white)

**A comprehensive, enterprise-grade clinic management system built with ASP.NET Core 10.0, featuring advanced authentication, real-time appointment scheduling, medical records management, and automated payment processing.**

[✨ Features](#-key-features) • [🏗️ Architecture](#️-architecture) • [💻 Installation](#-installation) • [🌐 API Docs](#-api-endpoints) • [🗄️ Database](#️-database-schema)

</div>

---

## 📋 Table of Contents

- [🎯 Overview](#-overview)
- [✨ Key Features](#-key-features)
- [🏗️ Architecture](#️-architecture)
- [🛠️ Technologies Used](#-technologies-used)
- [🗄️ Database Schema](#-database-schema)
- [🔐 Authentication & Authorization](#-authentication--authorization)
- [🌐 API Endpoints](#-api-endpoints)
- [💻 Installation](#-installation)
- [⚙️ Configuration](#️-configuration)
- [🔑 Default Seeded Users](#-default-seeded-users)
- [📸 Screenshots](#-screenshots)
- [🔮 Future Enhancements](#-future-enhancements)
- [🤝 Contributing](#-contributing)
- [📄 License](#-license)
- [👨‍💻 Author](#-author)

---

## 🎯 Overview

**Elite Clinic Management System** is a modern, full-featured healthcare management platform designed to streamline clinic operations. Built with **Clean Architecture** principles and implementing **CQRS pattern with MediatR**, this system provides a robust solution for managing patients, doctors, appointments, medical records, prescriptions, and payments.

### Why This System?

- ✅ **Production-Ready**: Enterprise-level code quality with proper error handling and logging
- ✅ **Scalable Architecture**: Clean Architecture ensures maintainability and testability
- ✅ **Security First**: JWT authentication, role-based authorization, and email confirmation
- ✅ **Real-Time Operations**: Background job processing with Hangfire
- ✅ **Comprehensive API**: RESTful API with Swagger documentation
- ✅ **Email Automation**: Professional HTML email templates for all notifications

---

## ✨ Key Features

### 🔐 Authentication & Authorization

![Login & JWT Authentication](./image/Login.png)

**Advanced Security Features:**
- 🔑 **JWT-based authentication** with Access & Refresh Tokens
- 🔒 **Secure password hashing** using ASP.NET Core Identity
- ✉️ **Email confirmation** for new account activation
- 🔄 **Password reset** functionality with secure token encoding
- 👥 **Role-based authorization** (Admin, Doctor, Patient)
- 🔁 **Refresh token rotation** with automatic revocation
- ⏰ **Token expiration handling** with configurable timeouts
- 🔐 **Account lockout** after failed login attempts (5 attempts)

**Key Components:**
- Access Token: Short-lived (configurable, default: 60 minutes)
- Refresh Token: Long-lived (default: 30 days) with database storage
- Email confirmation required before account activation
- Encrypted tokens for password reset with URL-safe encoding

![Authorization Roles](./image/Authorization.png)

---

### 👥 Patient Management

**Complete Patient Lifecycle:**
- 📝 Patient registration with automatic email confirmation
- 👤 Comprehensive patient profiles (demographics, contact info, medical history)
- 📊 Medical history tracking across all appointments
- 📅 Complete appointment history with status tracking
- 🔍 Advanced search by name, phone, or patient ID
- 🗑️ Soft delete support for data retention and compliance
- 📄 Pagination support for handling large patient databases
- 📧 Automated welcome emails with account details

---

### 👨‍⚕️ Doctor Management

**Doctor Administration:**
- ✏️ Create, update, and manage doctor profiles
- 🏥 Specialization-based categorization and filtering
- 📅 Doctor availability and schedule management
- 📊 Track doctor appointments and patient load
- 📈 Doctor revenue tracking and statistics
- 🔍 Search by specialization, name, or ID
- 👑 Promote doctors to Admin role
- 🗑️ Soft and hard delete options

---

### 📅 Appointment System

![Appointment Booking](./image/booked.png)

**Smart Scheduling Features:**
- 🗓️ **Real-time availability checking** for doctor slots
- ⏰ **Configurable time slots** (default: 15-minute intervals)
- 📋 **Multiple appointment statuses**:
  - Pending (initial booking)
  - Confirmed (after payment)
  - Completed (visit done)
  - Cancelled (by patient or admin)
  - Rescheduled (new time assigned)
  - NoShow (patient didn't arrive)
  
![Appointment Confirmation](./image/Confirm.png)

- 🔄 **Appointment rescheduling** with automatic notifications
- ❌ **Cancellation system** with email alerts
- 📊 **Statistics dashboard** for admins
- 📧 **Automated email notifications** for all status changes
- 🕒 **Auto-cancellation** of unpaid appointments after 30 minutes (Hangfire job)
- 📈 **Past appointment tracking** for doctors and patients

**Email Notifications:**

![Booking Confirmation Email](./image/book%20Email.jpeg)

![Appointment Confirmation Email](./image/Confirm%20Appointment%20email.jpeg)

---

### 💳 Payment Processing

**Comprehensive Payment System:**
- 💰 Multiple payment methods (Cash, Credit Card, Insurance)
- 📊 Payment status tracking (Pending, Completed, Failed, Refunded)
- 💵 Automatic amount calculation based on appointment
- 📧 Payment confirmation emails with transaction details
- 📈 Daily revenue reports for administrators
- 💼 Doctor revenue tracking and analytics
- 🔍 Payment history for patients and appointments
- 🧾 Transaction ID generation and tracking

---

### 📋 Medical Records & Prescriptions

**Clinical Documentation:**
- 📝 Create detailed medical records for each appointment
- 🩺 Record diagnosis, visit descriptions, and clinical notes
- 💊 Multi-prescription support per medical record
- 📋 Prescription details:
  - Medication name and dosage
  - Frequency and administration instructions
  - Start and end dates
  - Special instructions
- 🔒 Secure access control (only doctors and admins can create)
- 📖 Patient access to their own medical history
- 📊 Complete medical record tracking

---

### 📧 Email Notification System

**Professional Email Templates:**

![Email Confirmation](./image/Confirm%20Email.jpeg)

![Password Reset Email](./image/Reset%20Password.jpeg)

**Automated Emails:**
- ✅ **Account confirmation** with branded templates
- 🔑 **Password reset** with secure token links
- 📅 **Appointment booking confirmation**
- 💳 **Payment confirmation** with receipt details
- ❌ **Appointment cancellation** notifications
- 🔄 **Appointment rescheduling** alerts
- 🚫 **No-show notifications** for missed appointments
- 📋 **Medical record/prescription** delivery

**Email Infrastructure:**
- 🎨 Professional HTML templates with responsive design
- ⚡ Background job processing using Hangfire
- 📧 SMTP configuration (Gmail integration ready)
- 🔁 Retry mechanism for failed email delivery
- 📊 Email tracking and logging

---

### 📊 Advanced Features

**Pagination & Filtering:**

![Pagination Support](./image/Paging.png)

- 📄 **Server-side pagination** for all list endpoints
- 🔍 **Advanced filtering** by status, date, doctor, patient
- 📈 **Sorting capabilities** for all data tables
- ⚡ **Performance optimized** queries with EF Core

**Background Jobs (Hangfire):**
- ⏰ **Scheduled tasks** for appointment management
- 🔄 **Recurring jobs** for data cleanup
- 📧 **Email queue processing** with retry logic
- 🗑️ **Auto-cancellation** of unpaid appointments
- 📊 **Dashboard monitoring** for job execution

**Logging & Monitoring:**
- 📝 **Serilog integration** with file and console logging
- 🔍 **Request/Response logging** for debugging
- ⚠️ **Error tracking** with detailed stack traces
- 📊 **Performance monitoring** of database queries

**Validation & Error Handling:**
- ✅ **FluentValidation** for request validation
- 🛡️ **Global exception handling** middleware
- 📋 **Consistent API responses** with status codes
- 🔒 **Business rule enforcement** at application layer

---

## 🏗️ Architecture

The system follows **Clean Architecture** with clear separation of concerns and **CQRS pattern** implementation:

```
Elite-Clinic-System/
│
├── Clinic System.API/              # 🌐 Presentation Layer
│   ├── Controllers/                # API Controllers (Doctors, Patients, Appointments, etc.)
│   ├── Middlewares/                # Global Error Handler, Logging
│   ├── Bases/                      # Base Controller with MediatR
│   └── Program.cs                  # App Configuration & Dependency Injection
│
├── Clinic System.Application/      # 📋 Application Layer (Business Logic)
│   ├── Features/                   # ⚡ CQRS Implementation
│   │   ├── Patients/              # Patient Commands & Queries
│   │   │   ├── Commands/          # Create, Update, Delete Patient
│   │   │   └── Queries/           # Get Patient, List Patients, Search
│   │   ├── Doctors/               # Doctor Commands & Queries
│   │   ├── Appointments/          # Appointment Management
│   │   ├── Authentication/        # Login, Register, Password Reset
│   │   ├── Authorization/         # Role Management
│   │   ├── Payments/              # Payment Processing
│   │   ├── MedicalRecords/        # Medical Records Management
│   │   └── Prescriptions/         # Prescription Management
│   ├── DTOs/                       # 📦 Data Transfer Objects
│   ├── Mappings/                   # 🔄 AutoMapper Profiles
│   ├── Validators/                 # ✅ FluentValidation Rules
│   ├── Behaviors/                  # 🔧 MediatR Pipeline Behaviors
│   └── Services/                   # 🛠️ Application Services & Interfaces
│
├── Clinic System.Core/             # 🎯 Domain Layer (Entities & Business Rules)
│   ├── Entities/                   # Domain Models
│   │   ├── Doctor.cs
│   │   ├── Patient.cs
│   │   ├── Appointment.cs
│   │   ├── MedicalRecord.cs
│   │   ├── Prescription.cs
│   │   └── Payment.cs
│   ├── Enums/                      # Domain Enums
│   │   ├── AppointmentStatus.cs
│   │   ├── PaymentStatus.cs
│   │   ├── PaymentMethod.cs
│   │   └── Gender.cs
│   ├── Interfaces/                 # Repository & Service Contracts
│   │   ├── IRepository.cs
│   │   └── IUnitOfWork.cs
│   └── Exceptions/                 # Custom Exception Classes
│
├── Clinic System.Data/             # 💾 Data Access Layer (Persistence)
│   ├── Context/                    # DbContext Configuration
│   ├── Configurations/             # EF Core Entity Configurations
│   ├── Migrations/                 # Database Migrations
│   ├── Repository/                 # Repository Pattern Implementation
│   │   ├── GenericRepository.cs
│   │   ├── RepositoriesForEntities/
│   │   └── UnitOfWork/
│   └── Seeders/                    # Initial Data Seeding
│
└── Clinic System.Infrastructure/   # 🔧 Infrastructure Layer (External Services)
    ├── Authentication/             # JWT Token Generation & Validation
    ├── Authorization/              # Role & Permission Management
    ├── Identity/                   # ASP.NET Core Identity Setup
    ├── Services/                   # External Service Implementations
    │   ├── Email/                  # Email Service (SMTP)
    │   │   ├── EmailService.cs
    │   │   ├── AppointmentEmailNotificationService.cs
    │   │   └── EmailTemplates.cs
    │   ├── AppointmentService.cs
    │   ├── DoctorService.cs
    │   ├── PatientService.cs
    │   └── CurrentUserService.cs
    └── Helpers/                    # Helper Classes & Extensions
```

### 🎨 Design Patterns Used

1. **Clean Architecture**: Separation of concerns with independent layers
2. **CQRS Pattern**: Command Query Responsibility Segregation with MediatR
3. **Repository Pattern**: Data access abstraction
4. **Unit of Work Pattern**: Transaction management
5. **Dependency Injection**: Loose coupling and testability
6. **Strategy Pattern**: Payment method handling
7. **Factory Pattern**: Entity creation and initialization
8. **Mediator Pattern**: Decoupled request/response handling

---

## 🛠️ Technologies Used

### Backend Framework
- **ASP.NET Core 10.0** - Modern web framework
- **C# 12** - Latest C# features

### Database & ORM
- **Microsoft SQL Server** - Enterprise database
- **Entity Framework Core 10.0** - ORM with Code-First approach
- **LINQ** - Type-safe queries

### Authentication & Security
- **ASP.NET Core Identity** - User management
- **JWT (JSON Web Tokens)** - Stateless authentication
- **BCrypt** - Password hashing

### Libraries & Packages
- **MediatR** - CQRS implementation
- **AutoMapper** - Object-to-object mapping
- **FluentValidation** - Request validation
- **Serilog** - Structured logging
- **Hangfire** - Background job processing
- **Swashbuckle (Swagger)** - API documentation

### Design & Architecture
- **Clean Architecture** - Maintainable code structure
- **CQRS Pattern** - Scalable operations
- **Repository Pattern** - Data access abstraction
- **Unit of Work** - Transaction management

---

## 🗄️ Database Schema

![Database Schema](./image/DB.jpeg)

### Core Entities

#### 👤 **ApplicationUser** (ASP.NET Identity)
```
- Id (PK) - string
- UserName - string (unique, required)
- Email - string (unique, required)
- PasswordHash - string
- EmailConfirmed - bool
- PhoneNumber - string (optional)
- TwoFactorEnabled - bool
- LockoutEnabled - bool
- AccessFailedCount - int
- RefreshTokens - Collection<RefreshToken> (1:Many)
- Doctor - Doctor (1:1, optional)
- Patient - Patient (1:1, optional)
```

#### 🏥 **Doctor**
```
- Id (PK) - int (Auto-increment)
- ApplicationUserId (FK) - string (Unique)
- FullName - string (100 chars, required, indexed)
- Gender - enum (Male/Female)
- DateOfBirth - DateTime
- Phone - string (20 chars, required, unique)
- Address - string (250 chars)
- Specialization - string (100 chars, required, indexed)
- CreatedAt - DateTime (Auto-generated)
- IsDeleted - bool (Soft delete)
- Appointments - Collection<Appointment> (1:Many)
```

#### 🧑‍🦰 **Patient**
```
- Id (PK) - int (Auto-increment)
- ApplicationUserId (FK) - string (Unique)
- FullName - string (100 chars, required, indexed)
- Gender - enum (Male/Female)
- DateOfBirth - DateTime
- Phone - string (20 chars, required, unique)
- Address - string (250 chars)
- CreatedAt - DateTime (Auto-generated)
- IsDeleted - bool (Soft delete)
- Appointments - Collection<Appointment> (1:Many)
```

#### 📅 **Appointment**
```
- Id (PK) - int (Auto-increment)
- PatientId (FK) - int (Required, Indexed)
- DoctorId (FK) - int (Required, Indexed)
- AppointmentDate - DateTime (Required, Indexed)
- Status - enum (Pending/Confirmed/Completed/Cancelled/Rescheduled/NoShow)
- CreatedAt - DateTime (Auto-generated)
- UpdatedAt - DateTime (Auto-updated)
- IsDeleted - bool (Soft delete)
- Patient - Patient (Many:1)
- Doctor - Doctor (Many:1)
- MedicalRecord - MedicalRecord (1:1, optional)
- Payment - Payment (1:1, optional)
```

#### 📋 **MedicalRecord**
```
- Id (PK) - int (Auto-increment)
- AppointmentId (FK) - int (Unique, Required)
- Diagnosis - string (500 chars, required)
- DescriptionOfTheVisit - string (1000 chars)
- AdditionalNotes - string (500 chars)
- CreatedAt - DateTime (Auto-generated)
- IsDeleted - bool (Soft delete)
- Appointment - Appointment (1:1)
- Prescriptions - Collection<Prescription> (1:Many)
```

#### 💊 **Prescription**
```
- Id (PK) - int (Auto-increment)
- MedicalRecordId (FK) - int (Required, Indexed)
- MedicationName - string (100 chars, required)
- Dosage - string (50 chars, required)
- Frequency - string (100 chars, required)
- SpecialInstructions - string (300 chars)
- StartDate - DateTime (Required)
- EndDate - DateTime (Required)
- CreatedAt - DateTime (Auto-generated)
- IsDeleted - bool (Soft delete)
- MedicalRecord - MedicalRecord (Many:1)
```

#### 💰 **Payment**
```
- Id (PK) - int (Auto-increment)
- AppointmentId (FK) - int (Unique, Required)
- AmountPaid - decimal(18,2) (Required)
- PaymentMethod - enum (Cash/CreditCard/Insurance)
- PaymentStatus - enum (Pending/Completed/Failed/Refunded)
- PaymentDate - DateTime (Auto-generated)
- IsDeleted - bool (Soft delete)
- Appointment - Appointment (1:1)
```

#### 🔄 **RefreshToken**
```
- Id (PK) - int (Auto-increment)
- Token - string (500 chars, unique, indexed)
- ExpiresOn - DateTime (Required)
- CreatedOn - DateTime (Auto-generated)
- RevokedOn - DateTime (Nullable)
- IsActive - bool (Computed: !RevokedOn && ExpiresOn > Now)
- ApplicationUserId (FK) - string (Indexed)
- ApplicationUser - ApplicationUser (Many:1)
```

### Relationships Summary

```
ApplicationUser 1:1 Doctor
ApplicationUser 1:1 Patient
ApplicationUser 1:N RefreshToken

Doctor 1:N Appointment
Patient 1:N Appointment

Appointment 1:1 MedicalRecord
Appointment 1:1 Payment

MedicalRecord 1:N Prescription
```

### Database Features

- ✅ **Soft Delete Pattern**: All entities support soft delete (IsDeleted flag)
- 📊 **Indexes**: Strategic indexes on frequently queried columns
- 🔒 **Foreign Keys**: Referential integrity with appropriate cascade rules
- 📅 **Timestamps**: Automatic CreatedAt/UpdatedAt tracking
- 🔍 **Global Query Filters**: Automatic filtering of soft-deleted records
- 💾 **Lazy Loading**: Virtual navigation properties with proxies
- 🗂️ **Fluent Configuration**: Separate entity configurations for clean code

---

## 🔐 Authentication & Authorization

### Login Flow

![Login Flow](./image/Login.png)

**Step-by-Step Process:**

1. 👤 User sends credentials (email/username + password) to `/api/authentication/login`
2. 🔍 System validates credentials against database using ASP.NET Identity
3. ✅ If valid and email confirmed, generates JWT Access Token + Refresh Token
4. 💾 Refresh Token saved to database with expiration date
5. 📤 Returns tokens, user info, and roles in response
6. 🔐 Client stores tokens (localStorage/sessionStorage)
7. 🎫 Client includes Access Token in Authorization header for subsequent API requests

**Example Login Request:**
```bash
POST /api/authentication/login
Content-Type: application/json

{
  "emailOrUserName": "patient1@gmail.com",
  "password": "Patient@123"
}
```

**Example Success Response:**
```json
{
  "statusCode": 200,
  "succeeded": true,
  "message": "Login Successful",
  "data": {
    "id": 1,
    "userName": "patient1",
    "email": "patient1@gmail.com",
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ1c2VyLXBhdDEiLCJ1bmlxdWVfbmFtZSI6InBhdGllbnQxIiwiZW1haWwiOiJwYXRpZW50MUBnbWFpbC5jb20iLCJqdGkiOiI4YzRhM2YxZS02ZjdhLTRkOGItYjJlYy03ZjVhOWIyYzRkMWEiLCJyb2xlIjoiUGF0aWVudCIsImV4cCI6MTczODQ0MDA3MH0.R3v9x1qQ9kXN7sT8yZ2wL4pF5mN6hJ1bG8tD3yC4xK0",
    "refreshToken": "hG8Lj2RpQm3Tk9Wn6Yz4Cv1Xf7Sb5Da0Ke8Jm4Pq2Vr6",
    "expiresAt": "2026-02-01 22:32:54",
    "roles": ["Patient"]
  }
}
```

---

### Refresh Token Flow

When Access Token expires:

1. 🔄 Client sends expired Access Token + Refresh Token to `/api/authentication/refresh-token`
2. 🔍 System validates Refresh Token from database
3. ✅ If valid and not revoked, generates new Access Token + new Refresh Token
4. 🗑️ Old Refresh Token is revoked in database
5. 💾 New Refresh Token saved to database
6. 📤 Returns new tokens to client

**Token Security Features:**
- ⏰ Short-lived Access Tokens (60 minutes)
- 🔁 Automatic token refresh before expiration
- 🔒 Refresh Token rotation (single-use tokens)
- 💾 Database storage for Refresh Tokens with revocation support
- 🚫 Immediate revocation on logout
- 🕐 Configurable expiration times

---

### Authorization

The system uses **Role-Based Access Control (RBAC)** with three distinct roles:

| Role      | Permissions | Access Level |
|-----------|-------------|--------------|
| **Admin** | • Full system access<br>• Manage all doctors and patients<br>• View all appointments and payments<br>• Access system statistics<br>• Promote doctors to admin<br>• Delete records (soft/hard) | 🔴 Highest |
| **Doctor**| • Manage own appointments<br>• Create medical records and prescriptions<br>• View patient information<br>• Access past appointments<br>• Track revenue<br>• Update own profile | 🟡 Medium |
| **Patient**| • Book and manage own appointments<br>• View own medical records<br>• Access prescription history<br>• Make payments<br>• Update own profile<br>• Cancel appointments | 🟢 Limited |

**Example Protected Endpoints:**

```csharp
// Admin Only
[Authorize(Roles = "Admin")]
[HttpGet("stats")]
public async Task<IActionResult> GetAppointmentsStats()

// Admin and Doctor
[Authorize(Roles = "Admin,Doctor")]
[HttpGet("pastfordoctor")]
public async Task<IActionResult> GetPastAppointmentsForDoctor()

// Admin and Patient
[Authorize(Roles = "Admin,Patient")]
[HttpPost("book")]
public async Task<IActionResult> BookAppointment()

// All Authenticated Users
[Authorize]
[HttpGet("{id:int}")]
public async Task<IActionResult> GetPatientById(int id)
```

**Authorization Features:**
- 🎯 Fine-grained role-based access control
- 🔒 Attribute-based authorization on controllers and actions
- 👤 Current user context injection in handlers
- 🚫 Automatic 401/403 responses for unauthorized access
- 📊 Role claims embedded in JWT tokens

---

## 🌐 API Endpoints

### 🔐 Authentication & Authorization

#### Login
```http
POST /api/authentication/login
Content-Type: application/json

{
  "emailOrUserName": "patient1@gmail.com",
  "password": "Patient@123"
}
```

#### Refresh Token
```http
POST /api/authentication/refresh-token
Content-Type: application/json

{
  "accessToken": "expired_token_here",
  "refreshToken": "refresh_token_here"
}
```

#### Confirm Email
```http
GET /api/authentication/confirm-email?userId={userId}&code={encodedCode}
```

#### Resend Confirmation Email
```http
POST /api/authentication/resend-confirmation-email
Content-Type: application/json

{
  "email": "user@example.com",
  "baseUrl": "https://yourdomain.com"
}
```

#### Send Password Reset
```http
POST /api/authentication/send-reset-password
Content-Type: application/json

{
  "email": "user@example.com",
  "baseUrl": "https://yourdomain.com"
}
```

#### Reset Password
```http
POST /api/authentication/reset-password
Content-Type: application/json

{
  "email": "user@example.com",
  "code": "encoded_reset_token",
  "newPassword": "NewPassword@123"
}
```

---

### 👥 Patient Endpoints

#### Create Patient
```http
POST /api/patients
Authorization: Bearer {token}
Content-Type: application/json

{
  "fullName": "Ahmed Hassan",
  "userName": "ahmed123",
  "email": "ahmed@example.com",
  "password": "Ahmed@123",
  "phone": "01234567890",
  "address": "Cairo, Egypt",
  "gender": "Male",
  "dateOfBirth": "1990-05-15",
  "baseUrl": "https://yourdomain.com"
}
```

#### Get All Patients
```http
GET /api/patients
Authorization: Bearer {admin_token}
```

#### Get Patients with Pagination
```http
GET /api/patients/paging?pageNumber=1&pageSize=10
Authorization: Bearer {admin_token}
```

#### Get Patient by ID
```http
GET /api/patients/{id}
Authorization: Bearer {token}
```

#### Get Patient by Phone
```http
GET /api/patients/phone/{phone}
Authorization: Bearer {admin_or_doctor_token}
```

#### Search Patients by Name
```http
GET /api/patients/name/{name}
Authorization: Bearer {admin_token}
```

#### Update Patient
```http
PUT /api/patients/{id}
Authorization: Bearer {admin_or_patient_token}
Content-Type: application/json

{
  "fullName": "Ahmed Hassan Updated",
  "phone": "01234567890",
  "address": "Giza, Egypt",
  "dateOfBirth": "1990-05-15"
}
```

#### Soft Delete Patient
```http
DELETE /api/patients/{id}
Authorization: Bearer {admin_token}
```

#### Hard Delete Patient
```http
DELETE /api/patients/{id}/hard
Authorization: Bearer {admin_token}
```

---

### 👨‍⚕️ Doctor Endpoints

#### Create Doctor
```http
POST /api/doctors
Authorization: Bearer {admin_token}
Content-Type: application/json

{
  "fullName": "Dr. Mohamed Ali",
  "userName": "dr_mohamed",
  "email": "dr.mohamed@clinic.com",
  "password": "Doctor@123",
  "phone": "01098765432",
  "address": "Alexandria, Egypt",
  "gender": "Male",
  "dateOfBirth": "1980-03-20",
  "specialization": "Cardiology",
  "baseUrl": "https://yourdomain.com"
}
```

#### Get All Doctors
```http
GET /api/doctors
Authorization: Bearer {admin_token}
```

#### Get Doctors with Pagination
```http
GET /api/doctors/paging?pageNumber=1&pageSize=10
Authorization: Bearer {admin_token}
```

#### Get Doctor by ID
```http
GET /api/doctors/{id}
Authorization: Bearer {token}
```

#### Get Doctor with Appointments
```http
GET /api/doctors/{id}/appointments
Authorization: Bearer {admin_or_doctor_token}
```

#### Search Doctors by Specialization
```http
GET /api/doctors/specializations/{specialization}
Authorization: Bearer {token}
```

#### Search Doctors by Name
```http
GET /api/doctors/name/{name}
Authorization: Bearer {token}
```

#### Update Doctor
```http
PUT /api/doctors/{id}
Authorization: Bearer {admin_or_doctor_token}
Content-Type: application/json

{
  "fullName": "Dr. Mohamed Ali Updated",
  "phone": "01098765432",
  "address": "Cairo, Egypt",
  "specialization": "Cardiology",
  "dateOfBirth": "1980-03-20"
}
```

#### Soft Delete Doctor
```http
DELETE /api/doctors/{id}
Authorization: Bearer {admin_token}
```

#### Hard Delete Doctor
```http
DELETE /api/doctors/{id}/hard
Authorization: Bearer {admin_token}
```

---

### 📅 Appointment Endpoints

#### Get Available Slots
```http
GET /api/appointments/AvailableSlots?DoctorId=1&Date=2026-02-05
Authorization: Bearer {token}
```

#### Book Appointment
```http
POST /api/appointments/book
Authorization: Bearer {patient_token}
Content-Type: application/json

{
  "doctorId": 1,
  "patientId": 1,
  "appointmentDate": "2026-02-05T14:30:00"
}
```

#### Get Doctor Appointments
```http
GET /api/appointments/doctor?pageNumber=1&pageSize=10
Authorization: Bearer {doctor_token}
```

#### Get Patient Appointments
```http
GET /api/appointments/patient?pageNumber=1&pageSize=10
Authorization: Bearer {patient_token}
```

#### Get Past Appointments for Doctor
```http
GET /api/appointments/pastfordoctor?pageNumber=1&pageSize=10
Authorization: Bearer {doctor_token}
```

#### Get Past Appointments for Patient
```http
GET /api/appointments/pastforpatient?pageNumber=1&pageSize=10
Authorization: Bearer {patient_token}
```

#### Get Appointments by Status (Admin)
```http
GET /api/appointments/statusforadmin?status=Pending&pageNumber=1&pageSize=10
Authorization: Bearer {admin_token}
```

#### Get Appointments by Status (Doctor)
```http
GET /api/appointments/statusfordoctor?status=Confirmed&pageNumber=1&pageSize=10
Authorization: Bearer {doctor_token}
```

#### Get Appointment Statistics (Admin)
```http
GET /api/appointments/stats?StartDate=2026-02-01&EndDate=2026-02-28
Authorization: Bearer {admin_token}
```

#### Confirm Appointment
```http
PUT /api/appointments/confirm
Authorization: Bearer {admin_or_doctor_token}
Content-Type: application/json

{
  "appointmentId": 5
}
```

#### Reschedule Appointment
```http
PUT /api/appointments/reschedule
Authorization: Bearer {admin_or_patient_token}
Content-Type: application/json

{
  "appointmentId": 5,
  "newAppointmentDate": "2026-02-06T15:00:00"
}
```

#### Complete Appointment
```http
PUT /api/appointments/complete
Authorization: Bearer {doctor_token}
Content-Type: application/json

{
  "appointmentId": 5
}
```

#### Mark as No-Show
```http
PUT /api/appointments/noshow
Authorization: Bearer {admin_or_doctor_token}
Content-Type: application/json

{
  "appointmentId": 5
}
```

#### Cancel Appointment
```http
PUT /api/appointments/cancel
Authorization: Bearer {admin_or_patient_token}
Content-Type: application/json

{
  "appointmentId": 5,
  "cancellationReason": "Patient unavailable"
}
```

---

### 📋 Medical Record Endpoints

#### Create Medical Record
```http
POST /api/medicalrecords
Authorization: Bearer {doctor_token}
Content-Type: application/json

{
  "appointmentId": 5,
  "diagnosis": "Hypertension",
  "descriptionOfTheVisit": "Patient complained of high blood pressure...",
  "additionalNotes": "Follow up in 2 weeks"
}
```

#### Get Medical Record by ID
```http
GET /api/medicalrecords/{id}
Authorization: Bearer {token}
```

#### Get Medical Record by Appointment ID
```http
GET /api/medicalrecords/appointment/{appointmentId}
Authorization: Bearer {token}
```

---

### 💊 Prescription Endpoints

#### Add Prescription to Medical Record
```http
POST /api/prescriptions
Authorization: Bearer {doctor_token}
Content-Type: application/json

{
  "medicalRecordId": 3,
  "medicationName": "Lisinopril",
  "dosage": "10mg",
  "frequency": "Once daily",
  "specialInstructions": "Take in the morning with food",
  "startDate": "2026-02-05",
  "endDate": "2026-03-05"
}
```

---

### 💳 Payment Endpoints

#### Get All Payments (Admin)
```http
GET /api/payment/list?pageNumber=1&pageSize=20
Authorization: Bearer {admin_token}
```

#### Get Payment by ID
```http
GET /api/payment/{id}
Authorization: Bearer {token}
```

#### Get Daily Revenue
```http
GET /api/payment/daily-revenue?Date=2026-02-01
Authorization: Bearer {admin_token}
```

#### Get Doctor Revenue
```http
GET /api/payment/doctor-revenue?DoctorId=1&StartDate=2026-02-01&EndDate=2026-02-28
Authorization: Bearer {admin_or_doctor_token}
```

#### Create Payment (automatically creates after appointment confirmation)
```http
POST /api/payment
Authorization: Bearer {patient_token}
Content-Type: application/json

{
  "appointmentId": 5,
  "amountPaid": 500.00,
  "paymentMethod": "CreditCard"
}
```

---

### 👑 Role Management Endpoints

#### Promote Doctor to Admin
```http
POST /api/role/promote-doctor
Authorization: Bearer {admin_token}
Content-Type: application/json

{
  "doctorId": 2
}
```

---

## 💻 Installation

### Prerequisites

- **.NET SDK 10.0** or higher ([Download](https://dotnet.microsoft.com/download))
- **SQL Server** 2019 or higher ([Download](https://www.microsoft.com/sql-server/sql-server-downloads))
- **Visual Studio 2022** or **VS Code** ([Download](https://visualstudio.microsoft.com/))
- **SQL Server Management Studio (SSMS)** (optional, for database management)

### Step-by-Step Setup

#### 1. Clone the Repository
```bash
git clone https://github.com/adhamdr1/Clinic-System.git
cd Clinic-System
```

#### 2. Restore NuGet Packages
```bash
dotnet restore
```

#### 3. Update Database Connection String

Open `appsettings.json` in the `Clinic System.API` project and update the connection string:

```json
{
  "constr": "Server=YOUR_SERVER_NAME;Database=ClinicSystem;Integrated Security=SSPI;TrustServerCertificate=True;MultipleActiveResultSets=True"
}
```

Replace `YOUR_SERVER_NAME` with your SQL Server instance name (e.g., `localhost` or `DESKTOP-XYZ123`).

#### 4. Configure Email Settings (Optional but Recommended)

Update email settings in `appsettings.json`:

```json
{
  "EmailSettings": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "FromEmail": "your-email@gmail.com",
    "Password": "your-app-password",
    "SenderName": "Elite Clinic"
  }
}
```

**Note**: For Gmail, you need to use an **App Password** instead of your regular password. See [Google's guide](https://support.google.com/accounts/answer/185833).

#### 5. Apply Database Migrations

```bash
cd "Clinic System.API"
dotnet ef database update
```

This will create the database and seed initial data (admin, doctors, patients).

#### 6. Run the Application

```bash
dotnet run
```

The API will start at:
- **HTTPS**: `https://localhost:7179`
- **HTTP**: `http://localhost:5129`

#### 7. Access Swagger Documentation

Open your browser and navigate to:
```
https://localhost:7179/swagger
```

---

## ⚙️ Configuration

### JWT Settings

Configure JWT authentication in `appsettings.json`:

```json
{
  "JWT": {
    "SecritKey": "your-super-secret-key-minimum-32-characters-long",
    "AudienceIP": "https://localhost:4200",
    "IssuerIP": "https://localhost:7179",
    "TokenExpirationInMinutes": 60,
    "RefreshTokenExpirationInDays": 30
  }
}
```

**Important Security Notes:**
- 🔑 **SecretKey**: Must be at least 32 characters long. Use a random, strong key in production.
- 🌐 **AudienceIP**: Your frontend application URL
- 🏢 **IssuerIP**: Your backend API URL
- ⏰ **TokenExpiration**: Adjust based on your security requirements

### Email Configuration

```json
{
  "EmailSettings": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "FromEmail": "your-email@gmail.com",
    "Password": "your-app-password",
    "SenderName": "Elite Clinic"
  }
}
```

**Supported Email Providers:**
- Gmail (smtp.gmail.com:587)
- Outlook (smtp-mail.outlook.com:587)
- Office365 (smtp.office365.com:587)
- Custom SMTP servers

### Clinic Settings

Configure clinic working hours and appointment slots:

```json
{
  "ClinicSettings": {
    "DayStartTime": "09:00:00",
    "DayEndTime": "21:00:00",
    "SlotDurationInMinutes": 15
  }
}
```

### Logging Configuration

Serilog logging settings:

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "Microsoft.EntityFrameworkCore.Database.Command": "Information",
        "System": "Warning"
      }
    },
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "File",
        "Args": {
          "path": "Logs/log-.txt",
          "rollingInterval": "Day",
          "retainedFileCountLimit": 7
        }
      }
    ]
  }
}
```

---

## 🔑 Default Seeded Users

The system comes pre-loaded with test users for immediate testing:

### 👑 Admin Account
```
Email: admin@clinic.com
Username: admin
Password: Admin@123
Role: Admin
```

### 👨‍⚕️ Doctor Accounts

**Doctor 1 (Cardiology)**
```
Email: dr.ahmed@clinic.com
Username: dr_ahmed
Password: Doctor@123
Role: Doctor
Specialization: Cardiology
```

**Doctor 2 (Dermatology)**
```
Email: dr.sara@clinic.com
Username: dr_sara
Password: Doctor@123
Role: Doctor
Specialization: Dermatology
```

**Doctor 3 (Pediatrics)**
```
Email: dr.mohamed@clinic.com
Username: dr_mohamed
Password: Doctor@123
Role: Doctor
Specialization: Pediatrics
```

### 🧑‍🦰 Patient Accounts

**Patient 1**
```
Email: patient1@gmail.com
Username: patient1
Password: Patient@123
Role: Patient
```

**Patient 2**
```
Email: patient2@gmail.com
Username: patient2
Password: Patient@123
Role: Patient
```

**Patient 3**
```
Email: patient3@gmail.com
Username: patient3
Password: Patient@123
Role: Patient
```

**⚠️ Important**: Change these default passwords in production!

---

## 📸 Screenshots

### Swagger API Documentation
![Swagger API](./image/Authorization.png)

### Authentication Flow
![Login](./image/Login.png)

### Appointment Booking
![Booking](./image/booked.png)

### Appointment Confirmation
![Confirmation](./image/Confirm.png)

### Email Templates
![Email Confirmation](./image/Confirm%20Email.jpeg)
![Booking Email](./image/book%20Email.jpeg)
![Password Reset](./image/Reset%20Password.jpeg)

### Database Diagram
![Database](./image/DB.jpeg)

### Pagination Feature
![Pagination](./image/Paging.png)

---

## 🔮 Future Enhancements

The following features are planned for future releases:

### 🧪 Testing
- [ ] **Unit Tests** - Comprehensive unit test coverage for business logic
- [ ] **Integration Tests** - API endpoint testing with test database
- [ ] **Test Coverage Reports** - Automated coverage analysis

### 📱 Communication
- [ ] **SMS Notifications** - Appointment reminders via SMS (Twilio integration)
- [ ] **Push Notifications** - Real-time mobile notifications
- [ ] **WhatsApp Integration** - Automated messages via WhatsApp Business API

### 🐳 DevOps & Deployment
- [ ] **Docker Support** - Containerization with Docker Compose
  - API container
  - SQL Server container
  - Hangfire dashboard container
- [ ] **CI/CD Pipeline** - Automated builds and deployments (GitHub Actions)
- [ ] **Kubernetes** - Orchestration for scalability

### 📊 Analytics & Reporting
- [ ] **Analytics Dashboard** - Visual charts and statistics
- [ ] **Revenue Reports** - Detailed financial reporting
- [ ] **Patient Analytics** - Demographics and visit patterns
- [ ] **Doctor Performance Metrics** - Appointment statistics and ratings

### 🎨 Frontend
- [ ] **Admin Dashboard** - React/Angular admin panel
- [ ] **Patient Portal** - Self-service patient website
- [ ] **Mobile App** - Cross-platform mobile application (Flutter/React Native)

### 🔧 Technical Improvements
- [ ] **Caching** - Redis integration for performance
- [ ] **Rate Limiting** - API throttling and abuse prevention
- [ ] **GraphQL Support** - Alternative to REST API
- [ ] **SignalR** - Real-time updates for appointments
- [ ] **Multi-tenancy** - Support multiple clinic branches
- [ ] **Audit Logging** - Complete action history tracking

### 🏥 Medical Features
- [ ] **Lab Results** - Integration with lab systems
- [ ] **Medical Imaging** - DICOM image support
- [ ] **Telemedicine** - Video consultation capability
- [ ] **E-Prescriptions** - Electronic prescription submission to pharmacies
- [ ] **Inventory Management** - Medicine and equipment tracking
- [ ] **Insurance Claims** - Automated insurance processing

---

## 🤝 Contributing

Contributions are welcome! Here's how you can help:

### How to Contribute

1. **Fork the Repository**
2. **Create a Feature Branch**
   ```bash
   git checkout -b feature/AmazingFeature
   ```
3. **Commit Your Changes**
   ```bash
   git commit -m 'Add some AmazingFeature'
   ```
4. **Push to the Branch**
   ```bash
   git push origin feature/AmazingFeature
   ```
5. **Open a Pull Request**

### Contribution Guidelines

- ✅ Follow C# coding conventions and best practices
- ✅ Write clear, concise commit messages
- ✅ Add unit tests for new features
- ✅ Update documentation for API changes
- ✅ Ensure all tests pass before submitting PR
- ✅ Keep PRs focused on a single feature or fix

### Code of Conduct

- Be respectful and inclusive
- Provide constructive feedback
- Focus on the code, not the person
- Help others learn and grow

---

## 📄 License

This project is licensed under the **MIT License**.

```
MIT License

Copyright (c) 2026 Adham Adel

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```

---

## 👨‍💻 Author

**Adham Adel**

- 💼 Backend Developer specializing in ASP.NET Core
- 🎓 Computer Science Student
- 📧 Email: adhamdr32@gmail.com
- 🌐 GitHub: [@adhamdr1](https://github.com/adhamdr1)
- 💼 LinkedIn: [Adham Adel](https://linkedin.com/in/adhamdr1)

---

## 🙏 Acknowledgments

Special thanks to:

- **Microsoft** - For the excellent ASP.NET Core framework
- **MediatR** - For simplifying CQRS implementation
- **Hangfire** - For robust background job processing
- **The Clean Architecture Community** - For architectural guidance
- **Open Source Contributors** - For the amazing libraries used in this project

---

## 📞 Support

If you have any questions, issues, or suggestions:

- 📧 **Email**: adhamdr32@gmail.com
- 🐛 **Issues**: [GitHub Issues](https://github.com/adhamdr1/Clinic-System/issues)
- 💬 **Discussions**: [GitHub Discussions](https://github.com/adhamdr1/Clinic-System/discussions)

---

<div align="center">

### ⭐ Star this repository if you find it helpful!

**Built with ❤️ using ASP.NET Core**

[Back to Top ↑](#-elite-clinic-management-system)

</div>