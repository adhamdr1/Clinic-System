using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Clinic_System.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteToApplicationUser2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "role-admin", "58a534d0-5356-4729-8192-62020a8b778d", "Admin", "ADMIN" },
                    { "role-doctor", "6cbcdb6f-fc3c-46c9-9ecd-01d3eb95c6de", "Doctor", "DOCTOR" },
                    { "role-patient", "e808eb8c-35f7-4e34-bbb4-f4fa86d0451d", "Patient", "PATIENT" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DeletedAt", "Email", "EmailConfirmed", "IsDeleted", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "user-admin", 0, "fcc6a631-824c-4b3a-af69-0287f75110c2", null, "admin@clinic.com", true, false, false, null, "ADMIN@CLINIC.COM", "ADMIN@CLINIC.COM", "AQAAAAIAAYagAAAAECkXpa6slaJvl/Y0sJTG/swK3iPQujBuyrwdz7/8nPk5KRcvfVf06edR9bcMmSg9jA==", null, false, "774a6d7d-2a29-461b-9c4b-4c9842b58d86", false, "admin@clinic.com" },
                    { "user-doc1", 0, "09a1e6f7-82aa-44a4-b969-a66eb556129d", null, "dr.ahmed@clinic.com", true, false, false, null, "DR.AHMED@CLINIC.COM", "DR.AHMED@CLINIC.COM", "AQAAAAIAAYagAAAAEBCvE5cy20gdJr08VeMqiX+g0A4kaXB7ss5M43RMnRQ1lJjlHqw82EP0f0BMGYrbSA==", null, false, "1f373ab9-c2a8-4049-8da3-f32d3a582533", false, "dr.ahmed@clinic.com" },
                    { "user-doc2", 0, "30883ae2-d250-4c7b-9ca1-02a18a326459", null, "dr.sara@clinic.com", true, false, false, null, "DR.SARA@CLINIC.COM", "DR.SARA@CLINIC.COM", "AQAAAAIAAYagAAAAENCKcgnPJKwLg9+OwXy9NJ2Pf+Kk2CIe9/KNdOYHiG980M+9h+5/dD3jahcNtzp3BA==", null, false, "5218a735-6375-44d9-9faa-a45efa75bc93", false, "dr.sara@clinic.com" },
                    { "user-doc3", 0, "6b3c4a00-5db1-4c47-990d-862e9d15375d", null, "dr.mohamed@clinic.com", true, false, false, null, "DR.MOHAMED@CLINIC.COM", "DR.MOHAMED@CLINIC.COM", "AQAAAAIAAYagAAAAENITSoz6Y8xO0nwC3vR7jOFCZDhZ0G3QJ+SIjjMClRNA75Ou1RUpgqroY+c4BH4fMg==", null, false, "9dcdb360-0917-487a-be15-918aad8de7da", false, "dr.mohamed@clinic.com" },
                    { "user-doc4", 0, "48134770-b3db-4d67-92ac-b48102b68e76", null, "dr.layla@clinic.com", true, false, false, null, "DR.LAYLA@CLINIC.COM", "DR.LAYLA@CLINIC.COM", "AQAAAAIAAYagAAAAEJX35+cm/Csa0GOp7tMk27KvyILAifQdExesSakBmBK74fNG3+LaDtSJRJwJckwHQA==", null, false, "07e9f936-6047-45b0-855a-bdcbe5abf92a", false, "dr.layla@clinic.com" },
                    { "user-doc5", 0, "82446926-73b4-4d66-82b5-77c1f715af95", null, "dr.omar@clinic.com", true, false, false, null, "DR.OMAR@CLINIC.COM", "DR.OMAR@CLINIC.COM", "AQAAAAIAAYagAAAAEEhBqHUnGVS3RTDGwEpFZaXWu2LDmcSgx66mStX/j3o3KmC9A7yZ2fx98rkmpfO6pw==", null, false, "8d24bdaf-5f41-4c8e-8ea9-9e8c75352074", false, "dr.omar@clinic.com" },
                    { "user-pat1", 0, "9c62d8d2-46e0-4b72-a904-49638324dfbe", null, "mahmoud.ali@gmail.com", true, false, false, null, "MAHMOUD.ALI@GMAIL.COM", "MAHMOUD.ALI@GMAIL.COM", "AQAAAAIAAYagAAAAEBuxEy1q3Z1RrvYD0zCdb/Yk2+pYj9JYh1wheUiCK/mUqz3lFsLY4IvOeEC8CLRfhQ==", null, false, "2a667f9d-dbfa-4846-ac3d-54ac08dd0ea1", false, "mahmoud.ali@gmail.com" },
                    { "user-pat2", 0, "af71b16d-ad24-4f4b-9d6b-96bce097c986", null, "fatima.hassan@gmail.com", true, false, false, null, "FATIMA.HASSAN@GMAIL.COM", "FATIMA.HASSAN@GMAIL.COM", "AQAAAAIAAYagAAAAEAadIAWwcQAcagXjzQ+SC5SbSyjAgKqzduTJhrMCDCN+1Pp05y/M2md3U7zd1rRv7Q==", null, false, "846d5a62-33d1-4579-8036-0f5ba4bae68d", false, "fatima.hassan@gmail.com" },
                    { "user-pat3", 0, "b2af3a72-4a99-4d09-b2f9-0d07863998a2", null, "omar.khalid@gmail.com", true, false, false, null, "OMAR.KHALID@GMAIL.COM", "OMAR.KHALID@GMAIL.COM", "AQAAAAIAAYagAAAAEICXV6KOPVEbCV4DF8MMUaRsAbif98IkhDCNItV22bWqXkkVM27wHm5Sy2l8BE9j9Q==", null, false, "4f6b8ff0-8f0f-4564-9f73-e2a7b645dce6", false, "omar.khalid@gmail.com" },
                    { "user-pat4", 0, "67fb9938-7771-4394-a55f-ad8b8feed9d5", null, "nour.mohamed@gmail.com", true, false, false, null, "NOUR.MOHAMED@GMAIL.COM", "NOUR.MOHAMED@GMAIL.COM", "AQAAAAIAAYagAAAAEOZx2wzjkl0cxTXe3UIAy+6sX+jfNIIwRVmdmzvBy+ZqDad6UFS+OPhEC+1upPPQ6Q==", null, false, "fe9922a4-6fc4-4c96-8987-68ec91ef8a3d", false, "nour.mohamed@gmail.com" },
                    { "user-pat5", 0, "85634411-070c-4425-9475-e883059a3d47", null, "karim.youssef@gmail.com", true, false, false, null, "KARIM.YOUSSEF@GMAIL.COM", "KARIM.YOUSSEF@GMAIL.COM", "AQAAAAIAAYagAAAAEBHXG+rw3FOtQIBRYygcmbloxKfJ7Nc3xUn+l4Q6vQ7WWTPTuybUIPk75grInggDQA==", null, false, "8c919ca6-08b1-4047-b8fd-91801ee84bef", false, "karim.youssef@gmail.com" },
                    { "user-pat6", 0, "eef291f1-6c43-4451-82d0-d580f637deca", null, "mona.ahmed@gmail.com", true, false, false, null, "MONA.AHMED@GMAIL.COM", "MONA.AHMED@GMAIL.COM", "AQAAAAIAAYagAAAAECS9kujRgEAwrBuMKCDjQgOhrnD92W+Dk66Oa1u99bcko1AFoA3+TtJYJ+27pHnceA==", null, false, "8fd9300f-f5db-4866-a66d-4f0c12bec16f", false, "mona.ahmed@gmail.com" },
                    { "user-pat7", 0, "dcb8f153-9a08-4da3-868d-34526a83f35f", null, "ali.ibrahim@gmail.com", true, false, false, null, "ALI.IBRAHIM@GMAIL.COM", "ALI.IBRAHIM@GMAIL.COM", "AQAAAAIAAYagAAAAEOmj83kgyJ8bdqA5a71LAU2zW+sZGLBrAw09jBXLY7YdsZ4WkO4/hVq/ApJHo9uC2A==", null, false, "e135c8df-0979-4d00-8a78-6d5a44ecc326", false, "ali.ibrahim@gmail.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "role-admin", "user-admin" },
                    { "role-doctor", "user-doc1" },
                    { "role-doctor", "user-doc2" },
                    { "role-doctor", "user-doc3" },
                    { "role-doctor", "user-doc4" },
                    { "role-doctor", "user-doc5" },
                    { "role-patient", "user-pat1" },
                    { "role-patient", "user-pat2" },
                    { "role-patient", "user-pat3" },
                    { "role-patient", "user-pat4" },
                    { "role-patient", "user-pat5" },
                    { "role-patient", "user-pat6" },
                    { "role-patient", "user-pat7" }
                });

            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "Id", "Address", "ApplicationUserId", "CreatedAt", "DateOfBirth", "DeletedAt", "DoctorName", "Gender", "PhoneNumber", "Specialization", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "15 Nile Street, Cairo", "user-doc1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1980, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Ahmed Mohamed", "Male", "+201012345671", "Cardiology", null },
                    { 2, "28 Mohandessin, Giza", "user-doc2", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1985, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Sara Ali", "Female", "+201012345672", "Pediatrics", null },
                    { 3, "42 Nasr City, Cairo", "user-doc3", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1978, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Mohamed Hassan", "Male", "+201012345673", "Orthopedics", null },
                    { 4, "88 Zamalek, Cairo", "user-doc4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1982, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Layla Ibrahim", "Female", "+201012345674", "Dermatology", null },
                    { 5, "55 Heliopolis, Cairo", "user-doc5", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1975, 7, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Omar Khaled", "Male", "+201012345675", "Neurology", null }
                });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "Id", "Address", "ApplicationUserId", "CreatedAt", "DateOfBirth", "DeletedAt", "PatientName", "Gender", "PhoneNumber", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "10 Garden City, Cairo", "user-pat1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1990, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Mahmoud Ali", "Male", "+201098765431", null },
                    { 2, "22 Dokki, Giza", "user-pat2", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1995, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Fatima Hassan", "Female", "+201098765432", null },
                    { 3, "35 Zamalek, Cairo", "user-pat3", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1988, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Omar Khalid", "Male", "+201098765433", null },
                    { 4, "48 Agouza, Giza", "user-pat4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1992, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Nour Mohamed", "Female", "+201098765434", null },
                    { 5, "60 Maadi, Cairo", "user-pat5", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1985, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Karim Youssef", "Male", "+201098765435", null },
                    { 6, "72 New Cairo, Cairo", "user-pat6", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1993, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Mona Ahmed", "Female", "+201098765436", null },
                    { 7, "85 6th October City, Giza", "user-pat7", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1987, 10, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Ali Ibrahim", "Male", "+201098765437", null }
                });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "Id", "AppointmentDateTime", "CreatedAt", "DeletedAt", "DoctorId", "PatientId", "AppointmentStatus", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, 1, "Completed", null },
                    { 2, new DateTime(2024, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 2, "Completed", null },
                    { 3, new DateTime(2024, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3, "Completed", null },
                    { 4, new DateTime(2024, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 4, "Completed", null },
                    { 5, new DateTime(2024, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5, 5, "Completed", null },
                    { 6, new DateTime(2024, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, 6, "Completed", null },
                    { 7, new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 7, "Confirmed", null },
                    { 8, new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 1, "Confirmed", null },
                    { 9, new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 2, "Confirmed", null },
                    { 10, new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5, 3, "Confirmed", null },
                    { 11, new DateTime(2025, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, 4, "Pending", null },
                    { 12, new DateTime(2025, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 5, "Pending", null },
                    { 13, new DateTime(2025, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 6, "Cancelled", null }
                });

            migrationBuilder.InsertData(
                table: "MedicalRecords",
                columns: new[] { "Id", "AdditionalNotes", "AppointmentId", "CreatedAt", "DeletedAt", "VisitDescription", "Diagnosis", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "ينصح بتقليل الملح وممارسة الرياضة بانتظام", 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "المريض يعاني من ارتفاع في ضغط الدم، تم قياس الضغط وكان 150/95", "Hypertension", null },
                    { 2, "الراحة والإكثار من السوائل الدافئة", 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "طفل يعاني من كحة شديدة وحرارة، تم الفحص السريري", "Acute Bronchitis", null },
                    { 3, "العلاج الطبيعي مرتين أسبوعياً وتجنب حمل الأثقال", 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "ألم في الركبة اليمنى، تم عمل أشعة وظهر احتكاك بسيط", "Knee Osteoarthritis", null },
                    { 4, "تجنب الصابون القوي واستخدام المرطبات", 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "طفح جلدي في اليدين والرقبة، حكة شديدة", "Eczema", null },
                    { 5, "تجنب المحفزات مثل القهوة والتوتر", 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "صداع نصفي متكرر مع حساسية للضوء والصوت", "Migraine", null },
                    { 6, "الراحة وشرب السوائل الساخنة", 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "رشح وعطس واحتقان في الأنف منذ يومين", "Common Cold", null }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "Id", "AdditionalNotes", "AmountPaid", "AppointmentId", "CreatedAt", "DeletedAt", "PaymentDate", "PaymentMethod", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Paid in full", 350.00m, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cash", null },
                    { 2, "Visa card payment", 250.00m, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2024, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "CreditCard", null },
                    { 3, "Insurance coverage", 400.00m, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2024, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Insurance", null },
                    { 4, "InstaPay transfer", 200.00m, 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2024, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "InstaPay", null },
                    { 5, "Cash payment", 300.00m, 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2024, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cash", null },
                    { 6, "Mastercard payment", 180.00m, 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2024, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "CreditCard", null }
                });

            migrationBuilder.InsertData(
                table: "Prescriptions",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Dosage", "EndDate", "Frequency", "MedicalRecordId", "MedicationName", "SpecialInstructions", "StartDate", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "10mg", new DateTime(2025, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "مرة واحدة يومياً", 1, "Lisinopril", "يؤخذ صباحاً قبل الإفطار", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "81mg", new DateTime(2025, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "مرة واحدة يومياً", 1, "Aspirin", "يؤخذ بعد الطعام", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "500mg", new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "ثلاث مرات يومياً", 2, "Amoxicillin", "يؤخذ كل 8 ساعات بعد الأكل", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "10ml", new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "ثلاث مرات يومياً", 2, "Cough Syrup", "عند الكحة الشديدة", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "400mg", new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "مرتين يومياً", 3, "Ibuprofen", "يؤخذ بعد الأكل", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "1500mg", new DateTime(2025, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "مرة واحدة يومياً", 3, "Glucosamine", "يؤخذ مع الوجبة", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "1%", new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "مرتين يومياً", 4, "Hydrocortisone Cream", "يوضع على المنطقة المصابة", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "10mg", new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "مرة واحدة يومياً", 4, "Cetirizine", "يؤخذ مساءً", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 9, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "50mg", new DateTime(2025, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "عند بداية الصداع", 5, "Sumatriptan", "لا يزيد عن جرعتين في اليوم", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "40mg", new DateTime(2025, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "مرتين يومياً", 5, "Propranolol", "للوقاية من نوبات الصداع", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 11, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "500mg", new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "كل 6 ساعات عند الحاجة", 6, "Paracetamol", "لخفض الحرارة وتسكين الألم", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 12, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "1000mg", new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "مرة واحدة يومياً", 6, "Vitamin C", "يؤخذ مع الإفطار", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "role-admin", "user-admin" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "role-doctor", "user-doc1" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "role-doctor", "user-doc2" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "role-doctor", "user-doc3" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "role-doctor", "user-doc4" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "role-doctor", "user-doc5" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "role-patient", "user-pat1" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "role-patient", "user-pat2" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "role-patient", "user-pat3" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "role-patient", "user-pat4" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "role-patient", "user-pat5" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "role-patient", "user-pat6" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "role-patient", "user-pat7" });

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Prescriptions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Prescriptions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Prescriptions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Prescriptions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Prescriptions",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Prescriptions",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Prescriptions",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Prescriptions",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Prescriptions",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Prescriptions",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Prescriptions",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Prescriptions",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "role-admin");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "role-doctor");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "role-patient");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-admin");

            migrationBuilder.DeleteData(
                table: "MedicalRecords",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MedicalRecords",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MedicalRecords",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MedicalRecords",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MedicalRecords",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "MedicalRecords",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-pat7");

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-doc1");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-doc2");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-doc3");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-doc4");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-doc5");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-pat1");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-pat2");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-pat3");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-pat4");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-pat5");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-pat6");
        }
    }
}
