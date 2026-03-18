using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Clinic_System.Data.Migrations
{
    /// <inheritdoc />
    public partial class SyncModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "role-admin", "7bf854c0-34e2-46a1-874c-0c78cb948540", "Admin", "ADMIN" },
                    { "role-doctor", "1b1ff7b2-4a8a-4b9c-9315-a2e5b3bd43c0", "Doctor", "DOCTOR" },
                    { "role-patient", "816b12e0-3695-4bfa-9268-1e62e728ef01", "Patient", "PATIENT" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DeletedAt", "Email", "EmailConfirmed", "IsDeleted", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "user-admin", 0, "8f64e14a-8750-4bf3-9f26-e9c2d34caac6", null, "admin@clinic.com", true, false, false, null, "ADMIN@CLINIC.COM", "ADMIN@CLINIC.COM", "AQAAAAIAAYagAAAAEAV9CxKRoG3cwG/tPX4KW2juZxA4TaTaplx0Io+VyuRfSC6oJITmHRhiXT36hsC/7A==", null, false, "f5a63e82-2739-45ed-92da-785cd1e346c1", false, "admin@clinic.com" },
                    { "user-doc1", 0, "e49c6ed5-a1c6-4502-b32a-dc825df5b6d2", null, "dr.ahmed@clinic.com", true, false, false, null, "DR.AHMED@CLINIC.COM", "DR.AHMED@CLINIC.COM", "AQAAAAIAAYagAAAAEEgDOJ6t82SOn2vdb0F2XSMfdzkqLQ62xh5UHtyWkDGdl43tGL4Cy/wG/4lKj8fUyA==", null, false, "4350c876-7e5e-4ae4-b43e-2f582482e38a", false, "dr.ahmed@clinic.com" },
                    { "user-doc2", 0, "952221fe-1090-4910-8206-563de5b565d0", null, "dr.sara@clinic.com", true, false, false, null, "DR.SARA@CLINIC.COM", "DR.SARA@CLINIC.COM", "AQAAAAIAAYagAAAAEM1DM+iK8dbLWSApR6I/DmV9DhJCIpPq2XwQJy5yZ1ygVugPaWLp3l7ANvZO3bcorQ==", null, false, "83c96e9a-eb95-483f-82cf-ead97e98d68e", false, "dr.sara@clinic.com" },
                    { "user-doc3", 0, "7456af5a-46e2-47be-857a-1f04ed94d151", null, "dr.mohamed@clinic.com", true, false, false, null, "DR.MOHAMED@CLINIC.COM", "DR.MOHAMED@CLINIC.COM", "AQAAAAIAAYagAAAAEPNfinCCpISt5/R9jfneG0njeRQCjP5qOOyRVvJEZfd2/oTrlSWvBw3vlRa4zYLEMg==", null, false, "f953c8ca-15d1-46d9-a13f-f6fe6e1def71", false, "dr.mohamed@clinic.com" },
                    { "user-doc4", 0, "a2f97c69-279b-4855-a935-89118cb35d65", null, "dr.layla@clinic.com", true, false, false, null, "DR.LAYLA@CLINIC.COM", "DR.LAYLA@CLINIC.COM", "AQAAAAIAAYagAAAAEFvjFMOqcaqgHcdUoteLdtCNNEDRhz3T0wgaEllwoBgXU2Q5l/zQEIMax210pdnO2w==", null, false, "339b5dd2-ee81-4bd8-9553-c9659c253f1f", false, "dr.layla@clinic.com" },
                    { "user-doc5", 0, "95ffed39-7feb-485b-a7da-b397562e16eb", null, "dr.omar@clinic.com", true, false, false, null, "DR.OMAR@CLINIC.COM", "DR.OMAR@CLINIC.COM", "AQAAAAIAAYagAAAAEC5qKaTitvSdw9zWWEL2/tU/FnWVLPFy6crrWSWfT9jWc6cHM9XTohFEK43h3Rps9g==", null, false, "7c361a01-c681-430e-ad8f-c7b7cfa98ca2", false, "dr.omar@clinic.com" },
                    { "user-pat1", 0, "8a1403db-eb94-48e4-8938-957ee3862451", null, "mahmoud.ali@gmail.com", true, false, false, null, "MAHMOUD.ALI@GMAIL.COM", "MAHMOUD.ALI@GMAIL.COM", "AQAAAAIAAYagAAAAEBQ4ugEMufSgN6laBfldK6ZwyPPD8VmFvMLozOr+XSSogUg+KCmzklhKF6jytvTUpg==", null, false, "8fbce8e0-e801-42e8-9b7d-f7965763ec88", false, "mahmoud.ali@gmail.com" },
                    { "user-pat2", 0, "db47de69-840c-40c5-9f4b-1d952952225b", null, "fatima.hassan@gmail.com", true, false, false, null, "FATIMA.HASSAN@GMAIL.COM", "FATIMA.HASSAN@GMAIL.COM", "AQAAAAIAAYagAAAAEBWgwemsufPVfir3cOaGYX2mk230smXcBWFwFhtwsx9vk1svsGoRFNiWKFj7n9Bz8A==", null, false, "b8a475fc-fab9-4cdc-ae55-568915f4a39a", false, "fatima.hassan@gmail.com" },
                    { "user-pat3", 0, "bbabe310-a287-40f7-9759-541ff11c41b0", null, "omar.khalid@gmail.com", true, false, false, null, "OMAR.KHALID@GMAIL.COM", "OMAR.KHALID@GMAIL.COM", "AQAAAAIAAYagAAAAEDYOls5rZOKym13xTVU1OKNDgT554naFyjc09ufYZyJnhpEDWMTmkv56S5yLa3N/nA==", null, false, "13fc6866-5cd7-4d82-adb0-22f22940b75a", false, "omar.khalid@gmail.com" },
                    { "user-pat4", 0, "607310f6-6da0-4923-9220-f7c74c1a251c", null, "nour.mohamed@gmail.com", true, false, false, null, "NOUR.MOHAMED@GMAIL.COM", "NOUR.MOHAMED@GMAIL.COM", "AQAAAAIAAYagAAAAECZnfQCI07+JibgQQ7s2M+CWt8gJxhIIY0GgFufD4i/FZ025LQ96BALAszHf4nvYQw==", null, false, "d97be14f-0ed8-47b6-9e4a-8c9e6d8141f6", false, "nour.mohamed@gmail.com" },
                    { "user-pat5", 0, "a069f1ec-2cba-4375-8245-dc8140f82d7c", null, "karim.youssef@gmail.com", true, false, false, null, "KARIM.YOUSSEF@GMAIL.COM", "KARIM.YOUSSEF@GMAIL.COM", "AQAAAAIAAYagAAAAEM24chkKmnDkVKcDPwl+Jg+fEqb0aQSvq0D3eyZsXy8R7Srf1+GQXm2xJWZhcvgjcA==", null, false, "c383d5aa-8801-4841-97d0-e70fef7ee037", false, "karim.youssef@gmail.com" },
                    { "user-pat6", 0, "31f3c5c1-9f0f-4979-ba0f-e4dec4d62ae5", null, "mona.ahmed@gmail.com", true, false, false, null, "MONA.AHMED@GMAIL.COM", "MONA.AHMED@GMAIL.COM", "AQAAAAIAAYagAAAAEGHr7pL/nz6LYnzUBtBx8tM1f9u9TBw/VxBTasOQYS+iMynnqo0XO43ahF0y/mnndw==", null, false, "e9def8d0-2e60-4243-82de-788e4ee8b3c2", false, "mona.ahmed@gmail.com" },
                    { "user-pat7", 0, "db58f95b-7811-48a0-8afb-9779d849b087", null, "ali.ibrahim@gmail.com", true, false, false, null, "ALI.IBRAHIM@GMAIL.COM", "ALI.IBRAHIM@GMAIL.COM", "AQAAAAIAAYagAAAAEOA8u6OCqODhy1dldb3aUeSCSAIR1hbZv9ONx2jKVYE1dxEAZgYUdP1bsKJ8hAZeHA==", null, false, "415ca316-611a-4a14-9d07-ff97cfdfc306", false, "ali.ibrahim@gmail.com" }
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
                columns: new[] { "Id", "AdditionalNotes", "AmountPaid", "AppointmentId", "CreatedAt", "DeletedAt", "PaymentDate", "PaymentMethod", "PaymentStatus", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Paid in full", 350.00m, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cash", "Paid", null },
                    { 2, "Visa card payment", 250.00m, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2024, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "CreditCard", "Paid", null },
                    { 3, "Insurance coverage", 400.00m, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2024, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "InstaPay", "Paid", null },
                    { 4, "Failed transfer", 200.00m, 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Failed", null },
                    { 5, "Cash payment", 300.00m, 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2024, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cash", "Paid", null },
                    { 6, "Pending payment", 180.00m, 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Pending", null }
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
    }
}
