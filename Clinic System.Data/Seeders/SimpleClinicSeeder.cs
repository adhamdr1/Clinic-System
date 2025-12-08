namespace Clinic_System.Data.Seed
{
    /// <summary>
    /// Simple Seeder - سهل وواضح
    /// </summary>
    public static class SimpleClinicSeeder
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var now = new DateTime(2025, 1, 1); // تاريخ ثابت

            // 1. Roles
            SeedRoles(modelBuilder);

            // 2. Users (Admin + Doctors + Patients)
            SeedUsers(modelBuilder);

            // 3. Doctors
            SeedDoctors(modelBuilder, now);

            // 4. Patients
            SeedPatients(modelBuilder, now);

            // 5. Appointments
            SeedAppointments(modelBuilder, now);

            // 6. Medical Records
            SeedMedicalRecords(modelBuilder, now);

            // 7. Prescriptions
            SeedPrescriptions(modelBuilder, now);

            // 8. Payments
            SeedPayments(modelBuilder, now);
        }

        // ============================================
        // 1. Roles
        // ============================================
        private static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "role-admin", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "role-doctor", Name = "Doctor", NormalizedName = "DOCTOR" },
                new IdentityRole { Id = "role-patient", Name = "Patient", NormalizedName = "PATIENT" }
            );
        }

        // ============================================
        // 2. Users
        // ============================================
        private static void SeedUsers(ModelBuilder modelBuilder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();

            // ===== Admin User =====
            var admin = new ApplicationUser
            {
                Id = "user-admin",
                UserName = "admin@clinic.com",
                NormalizedUserName = "ADMIN@CLINIC.COM",
                Email = "admin@clinic.com",
                NormalizedEmail = "ADMIN@CLINIC.COM",
                EmailConfirmed = true
            };
            admin.PasswordHash = hasher.HashPassword(admin, "Admin@123");
            modelBuilder.Entity<ApplicationUser>().HasData(admin);
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = "user-admin", RoleId = "role-admin" }
            );

            // ===== Doctor Users =====
            var doctors = new[]
            {
                new { Id = "user-doc1", Email = "dr.ahmed@clinic.com" },
                new { Id = "user-doc2", Email = "dr.sara@clinic.com" },
                new { Id = "user-doc3", Email = "dr.mohamed@clinic.com" },
                new { Id = "user-doc4", Email = "dr.layla@clinic.com" },
                new { Id = "user-doc5", Email = "dr.omar@clinic.com" }
            };

            foreach (var doc in doctors)
            {
                var user = new ApplicationUser
                {
                    Id = doc.Id,
                    UserName = doc.Email,
                    NormalizedUserName = doc.Email.ToUpper(),
                    Email = doc.Email,
                    NormalizedEmail = doc.Email.ToUpper(),
                    IsDeleted = false,
                    EmailConfirmed = true
                };
                user.PasswordHash = hasher.HashPassword(user, "Doctor@123");

                modelBuilder.Entity<ApplicationUser>().HasData(user);
                modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                    new IdentityUserRole<string> { UserId = doc.Id, RoleId = "role-doctor" }
                );
            }

            // ===== Patient Users =====
            var patients = new[]
            {
                new { Id = "user-pat1", Email = "mahmoud.ali@gmail.com" },
                new { Id = "user-pat2", Email = "fatima.hassan@gmail.com" },
                new { Id = "user-pat3", Email = "omar.khalid@gmail.com" },
                new { Id = "user-pat4", Email = "nour.mohamed@gmail.com" },
                new { Id = "user-pat5", Email = "karim.youssef@gmail.com" },
                new { Id = "user-pat6", Email = "mona.ahmed@gmail.com" },
                new { Id = "user-pat7", Email = "ali.ibrahim@gmail.com" }
            };

            foreach (var pat in patients)
            {
                var user = new ApplicationUser
                {
                    Id = pat.Id,
                    UserName = pat.Email,
                    NormalizedUserName = pat.Email.ToUpper(),
                    Email = pat.Email,
                    NormalizedEmail = pat.Email.ToUpper(),
                    IsDeleted = false,
                    EmailConfirmed = true
                };
                user.PasswordHash = hasher.HashPassword(user, "Patient@123");

                modelBuilder.Entity<ApplicationUser>().HasData(user);
                modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                    new IdentityUserRole<string> { UserId = pat.Id, RoleId = "role-patient" }
                );
            }
        }

        // ============================================
        // 3. Doctors
        // ============================================
        private static void SeedDoctors(ModelBuilder modelBuilder, DateTime now)
        {
            modelBuilder.Entity<Doctor>().HasData(
                // Doctor 1
                new Doctor
                {
                    Id = 1, // مختلف عن user-doc1
                    ApplicationUserId = "user-doc1", // مربوط بـ User
                    FullName = "Ahmed Mohamed",
                    Gender = Gender.Male,
                    DateOfBirth = new DateTime(1980, 5, 15),
                    Phone = "+201012345671",
                    Address = "15 Nile Street, Cairo",
                    Specialization = "Cardiology",
                    IsDeleted = false,
                    CreatedAt = now
                },
                // Doctor 2
                new Doctor
                {
                    Id = 2,
                    ApplicationUserId = "user-doc2",
                    FullName = "Sara Ali",
                    Gender = Gender.Female,
                    DateOfBirth = new DateTime(1985, 8, 22),
                    Phone = "+201012345672",
                    Address = "28 Mohandessin, Giza",
                    Specialization = "Pediatrics",
                    IsDeleted = false,
                    CreatedAt = now
                },
                // Doctor 3
                new Doctor
                {
                    Id = 3,
                    ApplicationUserId = "user-doc3",
                    FullName = "Mohamed Hassan",
                    Gender = Gender.Male,
                    DateOfBirth = new DateTime(1978, 3, 10),
                    Phone = "+201012345673",
                    Address = "42 Nasr City, Cairo",
                    Specialization = "Orthopedics",
                    IsDeleted = false,
                    CreatedAt = now
                },
                // Doctor 4
                new Doctor
                {
                    Id = 4,
                    ApplicationUserId = "user-doc4",
                    FullName = "Layla Ibrahim",
                    Gender = Gender.Female,
                    DateOfBirth = new DateTime(1982, 11, 5),
                    Phone = "+201012345674",
                    Address = "88 Zamalek, Cairo",
                    Specialization = "Dermatology",
                    IsDeleted = false,
                    CreatedAt = now
                },
                // Doctor 5
                new Doctor
                {
                    Id = 5,
                    ApplicationUserId = "user-doc5",
                    FullName = "Omar Khaled",
                    Gender = Gender.Male,
                    DateOfBirth = new DateTime(1975, 7, 18),
                    Phone = "+201012345675",
                    Address = "55 Heliopolis, Cairo",
                    Specialization = "Neurology",
                    IsDeleted = false,
                    CreatedAt = now
                }
            );
        }

        // ============================================
        // 4. Patients
        // ============================================
        private static void SeedPatients(ModelBuilder modelBuilder, DateTime now)
        {
            modelBuilder.Entity<Patient>().HasData(
                // Patient 1
                new Patient
                {
                    Id = 1,
                    ApplicationUserId = "user-pat1",
                    FullName = "Mahmoud Ali",
                    Gender = Gender.Male,
                    DateOfBirth = new DateTime(1990, 1, 15),
                    Phone = "+201098765431",
                    Address = "10 Garden City, Cairo",
                    IsDeleted = false,
                    CreatedAt = now
                },
                // Patient 2
                new Patient
                {
                    Id = 2,
                    ApplicationUserId = "user-pat2",
                    FullName = "Fatima Hassan",
                    Gender = Gender.Female,
                    DateOfBirth = new DateTime(1995, 6, 20),
                    Phone = "+201098765432",
                    Address = "22 Dokki, Giza",
                    IsDeleted = false,
                    CreatedAt = now
                },
                // Patient 3
                new Patient
                {
                    Id = 3,
                    ApplicationUserId = "user-pat3",
                    FullName = "Omar Khalid",
                    Gender = Gender.Male,
                    DateOfBirth = new DateTime(1988, 12, 5),
                    Phone = "+201098765433",
                    Address = "35 Zamalek, Cairo",
                    IsDeleted = false,
                    CreatedAt = now
                },
                // Patient 4
                new Patient
                {
                    Id = 4,
                    ApplicationUserId = "user-pat4",
                    FullName = "Nour Mohamed",
                    Gender = Gender.Female,
                    DateOfBirth = new DateTime(1992, 4, 18),
                    Phone = "+201098765434",
                    Address = "48 Agouza, Giza",
                    IsDeleted = false,
                    CreatedAt = now
                },
                // Patient 5
                new Patient
                {
                    Id = 5,
                    ApplicationUserId = "user-pat5",
                    FullName = "Karim Youssef",
                    Gender = Gender.Male,
                    DateOfBirth = new DateTime(1985, 9, 25),
                    Phone = "+201098765435",
                    Address = "60 Maadi, Cairo",
                    IsDeleted = false,
                    CreatedAt = now
                },
                // Patient 6
                new Patient
                {
                    Id = 6,
                    ApplicationUserId = "user-pat6",
                    FullName = "Mona Ahmed",
                    Gender = Gender.Female,
                    DateOfBirth = new DateTime(1993, 2, 14),
                    Phone = "+201098765436",
                    Address = "72 New Cairo, Cairo",
                    IsDeleted = false,
                    CreatedAt = now
                },
                // Patient 7
                new Patient
                {
                    Id = 7,
                    ApplicationUserId = "user-pat7",
                    FullName = "Ali Ibrahim",
                    Gender = Gender.Male,
                    DateOfBirth = new DateTime(1987, 10, 30),
                    Phone = "+201098765437",
                    Address = "85 6th October City, Giza",
                    IsDeleted = false,
                    CreatedAt = now
                }
            );
        }

        // ============================================
        // 5. Appointments
        // ============================================
        private static void SeedAppointments(ModelBuilder modelBuilder, DateTime now)
        {
            modelBuilder.Entity<Appointment>().HasData(
                // Completed Appointments (في الماضي)
                new Appointment { Id = 1, PatientId = 1, DoctorId = 1, AppointmentDate = now.AddDays(-10), Status = AppointmentStatus.Completed, IsDeleted = false, CreatedAt = now },
                new Appointment { Id = 2, PatientId = 2, DoctorId = 2, AppointmentDate = now.AddDays(-8), Status = AppointmentStatus.Completed, IsDeleted = false, CreatedAt = now },
                new Appointment { Id = 3, PatientId = 3, DoctorId = 3, AppointmentDate = now.AddDays(-5), Status = AppointmentStatus.Completed, IsDeleted = false, CreatedAt = now },
                new Appointment { Id = 4, PatientId = 4, DoctorId = 4, AppointmentDate = now.AddDays(-3), Status = AppointmentStatus.Completed, IsDeleted = false, CreatedAt = now },
                new Appointment { Id = 5, PatientId = 5, DoctorId = 5, AppointmentDate = now.AddDays(-2), Status = AppointmentStatus.Completed, IsDeleted = false, CreatedAt = now },
                new Appointment { Id = 6, PatientId = 6, DoctorId = 1, AppointmentDate = now.AddDays(-1), Status = AppointmentStatus.Completed, IsDeleted = false, CreatedAt = now },

                // Confirmed Appointments (قادمة)
                new Appointment { Id = 7, PatientId = 7, DoctorId = 2, AppointmentDate = now.AddDays(1), Status = AppointmentStatus.Confirmed, IsDeleted = false, CreatedAt = now },
                new Appointment { Id = 8, PatientId = 1, DoctorId = 3, AppointmentDate = now.AddDays(2), Status = AppointmentStatus.Confirmed, IsDeleted = false, CreatedAt = now },
                new Appointment { Id = 9, PatientId = 2, DoctorId = 4, AppointmentDate = now.AddDays(3), Status = AppointmentStatus.Confirmed, IsDeleted = false, CreatedAt = now },
                new Appointment { Id = 10, PatientId = 3, DoctorId = 5, AppointmentDate = now.AddDays(5), Status = AppointmentStatus.Confirmed, IsDeleted = false, CreatedAt = now },

                // Pending Appointments
                new Appointment { Id = 11, PatientId = 4, DoctorId = 1, AppointmentDate = now.AddDays(7), Status = AppointmentStatus.Pending, IsDeleted = false, CreatedAt = now },
                new Appointment { Id = 12, PatientId = 5, DoctorId = 2, AppointmentDate = now.AddDays(10), Status = AppointmentStatus.Pending, IsDeleted = false, CreatedAt = now },

                // Cancelled
                new Appointment { Id = 13, PatientId = 6, DoctorId = 3, AppointmentDate = now.AddDays(15), Status = AppointmentStatus.Cancelled, IsDeleted = false, CreatedAt = now }
            );
        }

        // ============================================
        // 6. Medical Records
        // ============================================
        private static void SeedMedicalRecords(ModelBuilder modelBuilder, DateTime now)
        {
            modelBuilder.Entity<MedicalRecord>().HasData(
                new MedicalRecord
                {
                    Id = 1,
                    AppointmentId = 1,
                    Diagnosis = "Hypertension",
                    DescriptionOfTheVisit = "المريض يعاني من ارتفاع في ضغط الدم، تم قياس الضغط وكان 150/95",
                    AdditionalNotes = "ينصح بتقليل الملح وممارسة الرياضة بانتظام",
                    IsDeleted = false,
                    CreatedAt = now
                },
                new MedicalRecord
                {
                    Id = 2,
                    AppointmentId = 2,
                    Diagnosis = "Acute Bronchitis",
                    DescriptionOfTheVisit = "طفل يعاني من كحة شديدة وحرارة، تم الفحص السريري",
                    AdditionalNotes = "الراحة والإكثار من السوائل الدافئة",
                    IsDeleted = false,
                    CreatedAt = now
                },
                new MedicalRecord
                {
                    Id = 3,
                    AppointmentId = 3,
                    Diagnosis = "Knee Osteoarthritis",
                    DescriptionOfTheVisit = "ألم في الركبة اليمنى، تم عمل أشعة وظهر احتكاك بسيط",
                    AdditionalNotes = "العلاج الطبيعي مرتين أسبوعياً وتجنب حمل الأثقال",
                    IsDeleted = false,
                    CreatedAt = now
                },
                new MedicalRecord
                {
                    Id = 4,
                    AppointmentId = 4,
                    Diagnosis = "Eczema",
                    DescriptionOfTheVisit = "طفح جلدي في اليدين والرقبة، حكة شديدة",
                    AdditionalNotes = "تجنب الصابون القوي واستخدام المرطبات",
                    IsDeleted = false,
                    CreatedAt = now
                },
                new MedicalRecord
                {
                    Id = 5,
                    AppointmentId = 5,
                    Diagnosis = "Migraine",
                    DescriptionOfTheVisit = "صداع نصفي متكرر مع حساسية للضوء والصوت",
                    AdditionalNotes = "تجنب المحفزات مثل القهوة والتوتر",
                    IsDeleted = false,
                    CreatedAt = now
                },
                new MedicalRecord
                {
                    Id = 6,
                    AppointmentId = 6,
                    Diagnosis = "Common Cold",
                    DescriptionOfTheVisit = "رشح وعطس واحتقان في الأنف منذ يومين",
                    AdditionalNotes = "الراحة وشرب السوائل الساخنة",
                    IsDeleted = false,
                    CreatedAt = now
                }
            );
        }

        // ============================================
        // 7. Prescriptions
        // ============================================
        private static void SeedPrescriptions(ModelBuilder modelBuilder, DateTime now)
        {
            var startDate = now;
            var endDate = now.AddDays(14);

            modelBuilder.Entity<Prescription>().HasData(
                // Medical Record 1 (Hypertension)
                new Prescription { Id = 1, MedicalRecordId = 1, MedicationName = "Lisinopril", Dosage = "10mg", Frequency = "مرة واحدة يومياً", SpecialInstructions = "يؤخذ صباحاً قبل الإفطار", StartDate = startDate, EndDate = endDate.AddDays(16), IsDeleted = false, CreatedAt = now },
                new Prescription { Id = 2, MedicalRecordId = 1, MedicationName = "Aspirin", Dosage = "81mg", Frequency = "مرة واحدة يومياً", SpecialInstructions = "يؤخذ بعد الطعام", StartDate = startDate, EndDate = endDate.AddDays(16), IsDeleted = false, CreatedAt = now },

                // Medical Record 2 (Bronchitis)
                new Prescription { Id = 3, MedicalRecordId = 2, MedicationName = "Amoxicillin", Dosage = "500mg", Frequency = "ثلاث مرات يومياً", SpecialInstructions = "يؤخذ كل 8 ساعات بعد الأكل", StartDate = startDate, EndDate = endDate, IsDeleted = false, CreatedAt = now },
                new Prescription { Id = 4, MedicalRecordId = 2, MedicationName = "Cough Syrup", Dosage = "10ml", Frequency = "ثلاث مرات يومياً", SpecialInstructions = "عند الكحة الشديدة", StartDate = startDate, EndDate = endDate, IsDeleted = false, CreatedAt = now },

                // Medical Record 3 (Knee Pain)
                new Prescription { Id = 5, MedicalRecordId = 3, MedicationName = "Ibuprofen", Dosage = "400mg", Frequency = "مرتين يومياً", SpecialInstructions = "يؤخذ بعد الأكل", StartDate = startDate, EndDate = endDate, IsDeleted = false, CreatedAt = now },
                new Prescription { Id = 6, MedicalRecordId = 3, MedicationName = "Glucosamine", Dosage = "1500mg", Frequency = "مرة واحدة يومياً", SpecialInstructions = "يؤخذ مع الوجبة", StartDate = startDate, EndDate = endDate.AddDays(46), IsDeleted = false, CreatedAt = now },

                // Medical Record 4 (Eczema)
                new Prescription { Id = 7, MedicalRecordId = 4, MedicationName = "Hydrocortisone Cream", Dosage = "1%", Frequency = "مرتين يومياً", SpecialInstructions = "يوضع على المنطقة المصابة", StartDate = startDate, EndDate = endDate, IsDeleted = false, CreatedAt = now },
                new Prescription { Id = 8, MedicalRecordId = 4, MedicationName = "Cetirizine", Dosage = "10mg", Frequency = "مرة واحدة يومياً", SpecialInstructions = "يؤخذ مساءً", StartDate = startDate, EndDate = endDate, IsDeleted = false, CreatedAt = now },

                // Medical Record 5 (Migraine)
                new Prescription { Id = 9, MedicalRecordId = 5, MedicationName = "Sumatriptan", Dosage = "50mg", Frequency = "عند بداية الصداع", SpecialInstructions = "لا يزيد عن جرعتين في اليوم", StartDate = startDate, EndDate = endDate.AddDays(16), IsDeleted = false, CreatedAt = now },
                new Prescription { Id = 10, MedicalRecordId = 5, MedicationName = "Propranolol", Dosage = "40mg", Frequency = "مرتين يومياً", SpecialInstructions = "للوقاية من نوبات الصداع", StartDate = startDate, EndDate = endDate.AddDays(46), IsDeleted = false, CreatedAt = now },

                // Medical Record 6 (Common Cold)
                new Prescription { Id = 11, MedicalRecordId = 6, MedicationName = "Paracetamol", Dosage = "500mg", Frequency = "كل 6 ساعات عند الحاجة", SpecialInstructions = "لخفض الحرارة وتسكين الألم", StartDate = startDate, EndDate = endDate, IsDeleted = false, CreatedAt = now },
                new Prescription { Id = 12, MedicalRecordId = 6, MedicationName = "Vitamin C", Dosage = "1000mg", Frequency = "مرة واحدة يومياً", SpecialInstructions = "يؤخذ مع الإفطار", StartDate = startDate, EndDate = endDate, IsDeleted = false, CreatedAt = now }
            );
        }

        // ============================================
        // 8. Payments
        // ============================================
        private static void SeedPayments(ModelBuilder modelBuilder, DateTime now)
        {
            modelBuilder.Entity<Payment>().HasData(
                new Payment { Id = 1, AppointmentId = 1, AmountPaid = 350.00m, PaymentMethod = PaymentMethod.Cash, PaymentDate = now.AddDays(-10), AdditionalNotes = "Paid in full", IsDeleted = false, CreatedAt = now },
                new Payment { Id = 2, AppointmentId = 2, AmountPaid = 250.00m, PaymentMethod = PaymentMethod.CreditCard, PaymentDate = now.AddDays(-8), AdditionalNotes = "Visa card payment", IsDeleted = false, CreatedAt = now },
                new Payment { Id = 3, AppointmentId = 3, AmountPaid = 400.00m, PaymentMethod = PaymentMethod.Insurance, PaymentDate = now.AddDays(-5), AdditionalNotes = "Insurance coverage", IsDeleted = false, CreatedAt = now },
                new Payment { Id = 4, AppointmentId = 4, AmountPaid = 200.00m, PaymentMethod = PaymentMethod.InstaPay, PaymentDate = now.AddDays(-3), AdditionalNotes = "InstaPay transfer", IsDeleted = false, CreatedAt = now },
                new Payment { Id = 5, AppointmentId = 5, AmountPaid = 300.00m, PaymentMethod = PaymentMethod.Cash, PaymentDate = now.AddDays(-2), AdditionalNotes = "Cash payment", IsDeleted = false, CreatedAt = now },
                new Payment { Id = 6, AppointmentId = 6, AmountPaid = 180.00m, PaymentMethod = PaymentMethod.CreditCard, PaymentDate = now.AddDays(-1), AdditionalNotes = "Mastercard payment", IsDeleted = false, CreatedAt = now }
            );
        }
    }
}