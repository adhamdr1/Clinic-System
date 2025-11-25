using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Clinic_System.Migrations
{
    /// <inheritdoc />
    public partial class firstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Specialization = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DoctorName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Doctors_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PatientName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patients_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AppointmentStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointments_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MedicalRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Diagnosis = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AdditionalNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    VisitDescription = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    AppointmentId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalRecords_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AdditionalNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AppointmentId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.CheckConstraint("CK_Payments_AmountPaid_Positive", "[AmountPaid] > 0");
                    table.ForeignKey(
                        name: "FK_Payments_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prescriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Dosage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MedicationName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SpecialInstructions = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Frequency = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MedicalRecordId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescriptions", x => x.Id);
                    table.CheckConstraint("CK_Prescriptions_EndDate_After_StartDate", "[EndDate] > [StartDate]");
                    table.ForeignKey(
                        name: "FK_Prescriptions_MedicalRecords_MedicalRecordId",
                        column: x => x.MedicalRecordId,
                        principalTable: "MedicalRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "role-admin", null, "Admin", "ADMIN" },
                    { "role-doctor", null, "Doctor", "DOCTOR" },
                    { "role-patient", null, "Patient", "PATIENT" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "user-admin", 0, "27ce55d2-9bd4-4292-89aa-2ff20cc0318b", "admin@clinic.com", true, false, null, "ADMIN@CLINIC.COM", "ADMIN@CLINIC.COM", "AQAAAAIAAYagAAAAELvCwoVWZpSOpiM0PBhIuCb05AcDUMARgcH3M7VIfFZ1R7IJ98jE6JuBPALje5+Kow==", null, false, "16e0ce06-63b3-4b73-a228-04d1d6d13d6a", false, "admin@clinic.com" },
                    { "user-doc1", 0, "24a99b5e-6e3e-4ab1-a674-2e98c15ac576", "dr.ahmed@clinic.com", true, false, null, "DR.AHMED@CLINIC.COM", "DR.AHMED@CLINIC.COM", "AQAAAAIAAYagAAAAEFTBKTLlO8Evbjck+EMsxXtwe+lhnXM2Eyj7K+2FxAw7aGc06slGZQzCehc/e5rn/g==", null, false, "c2e156a1-c264-45dd-9ff3-b458e7c3399c", false, "dr.ahmed@clinic.com" },
                    { "user-doc2", 0, "2c5bc0f9-ecd2-43aa-810c-17e79e9b1148", "dr.sara@clinic.com", true, false, null, "DR.SARA@CLINIC.COM", "DR.SARA@CLINIC.COM", "AQAAAAIAAYagAAAAENKl0LZSW54BJaXHlPcjnbKOnwxnC3x2N0LeM9iTZNx4sAsTDuG9Yn2XEhntUDrWvA==", null, false, "b5e0889f-314f-43b8-b777-0e2b02e66d21", false, "dr.sara@clinic.com" },
                    { "user-doc3", 0, "7755a86b-bf57-4d42-942a-1ebfdbd4741f", "dr.mohamed@clinic.com", true, false, null, "DR.MOHAMED@CLINIC.COM", "DR.MOHAMED@CLINIC.COM", "AQAAAAIAAYagAAAAEPprMITE4FRmyetQD9nAL7BSR9FyPZwE017YwcrilT17aN70e0FGii8UKeLJyYBoSA==", null, false, "31742e2b-5d30-4a4a-b96f-0d337c43410f", false, "dr.mohamed@clinic.com" },
                    { "user-doc4", 0, "edbdd483-c5a0-46ec-a0c4-d567c4e4e6bc", "dr.layla@clinic.com", true, false, null, "DR.LAYLA@CLINIC.COM", "DR.LAYLA@CLINIC.COM", "AQAAAAIAAYagAAAAEH0FVk93L13TSzoUDlte0UR6pP821DYosA4p37n8W8qc5tRyXeyBq3s32JD0rfKjgQ==", null, false, "fd8e05f3-8537-4227-99de-0439a0ce04f9", false, "dr.layla@clinic.com" },
                    { "user-doc5", 0, "2d63b76a-2c87-478d-8959-2aeab599bbd2", "dr.omar@clinic.com", true, false, null, "DR.OMAR@CLINIC.COM", "DR.OMAR@CLINIC.COM", "AQAAAAIAAYagAAAAEAgrwOZLiE2jBftJT0IWyiUyzRLfXUuqNQazsvelJ484KFN/1qp9qeN8IJCeKcL3TA==", null, false, "db6adf20-58ba-44f3-872a-3cd37ab3fc11", false, "dr.omar@clinic.com" },
                    { "user-pat1", 0, "76e88d86-8eec-4cff-8ef3-40ca817c7923", "mahmoud.ali@gmail.com", true, false, null, "MAHMOUD.ALI@GMAIL.COM", "MAHMOUD.ALI@GMAIL.COM", "AQAAAAIAAYagAAAAEOzRtnzegIxSDrQN+5Akh9A/nWsgAffiaD6iIhvZqMbWvcivG2BYaplM0BnWTvvgKg==", null, false, "523cd1f8-fff9-475d-91c0-de3e43e12e07", false, "mahmoud.ali@gmail.com" },
                    { "user-pat2", 0, "ffc9c485-6775-40aa-aae4-253ec6391d3b", "fatima.hassan@gmail.com", true, false, null, "FATIMA.HASSAN@GMAIL.COM", "FATIMA.HASSAN@GMAIL.COM", "AQAAAAIAAYagAAAAEDQ6h48Tejay9g0UO++UxwYpAsabaT2+0qUOfr0WgJUlBh7Y31a7uUv6HirIvttyOw==", null, false, "2820a7f9-d6fe-4b3d-9392-2eea767fa756", false, "fatima.hassan@gmail.com" },
                    { "user-pat3", 0, "84e13bfc-c071-437a-95f6-01897a3bc89a", "omar.khalid@gmail.com", true, false, null, "OMAR.KHALID@GMAIL.COM", "OMAR.KHALID@GMAIL.COM", "AQAAAAIAAYagAAAAEEA4LlISmYjj8owIAQYbmCtJTefTHzpl3AEYbsEkR5ytt4+TqB5MpgnaCgqYsCHyag==", null, false, "b0880d1d-fd88-4251-a591-ba19cecc1380", false, "omar.khalid@gmail.com" },
                    { "user-pat4", 0, "da0eccfa-271e-418f-8249-b2f7e02c4af2", "nour.mohamed@gmail.com", true, false, null, "NOUR.MOHAMED@GMAIL.COM", "NOUR.MOHAMED@GMAIL.COM", "AQAAAAIAAYagAAAAEMBC6oy3uQwvrJDpUoBDETCDL9CoJhfAgWbMoPBKnQUNQ0qU+/4RbRQm4j7KLPW2EA==", null, false, "3ffebb7c-a023-4352-9a39-f4563b42a7c7", false, "nour.mohamed@gmail.com" },
                    { "user-pat5", 0, "6ca6440a-4594-4e89-b422-b98a1523e10b", "karim.youssef@gmail.com", true, false, null, "KARIM.YOUSSEF@GMAIL.COM", "KARIM.YOUSSEF@GMAIL.COM", "AQAAAAIAAYagAAAAEEvsDW0AdzJ2IaGqUORZuvuMC30/mUuf+JLTTZq2IAnzrQAElm1HqAhd2QlUFQ155w==", null, false, "7e4613b4-bcd5-460d-b9e8-7ba54509b8be", false, "karim.youssef@gmail.com" },
                    { "user-pat6", 0, "cb4e0d81-aa82-45a1-8fd8-f4fd612edd9e", "mona.ahmed@gmail.com", true, false, null, "MONA.AHMED@GMAIL.COM", "MONA.AHMED@GMAIL.COM", "AQAAAAIAAYagAAAAEE1SCvyK/y5q/4GNdu+Bsmfw56gCLWlX30v3gThaF9RSe67nvZc5icC0pPIY4UXfCg==", null, false, "d4de0fba-4282-4053-848a-1c3d58b066e7", false, "mona.ahmed@gmail.com" },
                    { "user-pat7", 0, "d37c037e-5130-466f-ba13-3bd02524a7b1", "ali.ibrahim@gmail.com", true, false, null, "ALI.IBRAHIM@GMAIL.COM", "ALI.IBRAHIM@GMAIL.COM", "AQAAAAIAAYagAAAAEPCxJ8bEBrWQR/+h+JCGLKiwJXEBPQaD8aIw16htKpqZf5s/zJLVVHX7hKFC1ZDelQ==", null, false, "919a380f-8d2c-4aba-b923-01c53ab85231", false, "ali.ibrahim@gmail.com" }
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
                    { 1, "15 Nile Street, Cairo", "user-doc1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1980, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Ahmed Mohamed", 1, "+201012345671", "Cardiology", null },
                    { 2, "28 Mohandessin, Giza", "user-doc2", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1985, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Sara Ali", 2, "+201012345672", "Pediatrics", null },
                    { 3, "42 Nasr City, Cairo", "user-doc3", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1978, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Mohamed Hassan", 1, "+201012345673", "Orthopedics", null },
                    { 4, "88 Zamalek, Cairo", "user-doc4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1982, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Layla Ibrahim", 2, "+201012345674", "Dermatology", null },
                    { 5, "55 Heliopolis, Cairo", "user-doc5", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1975, 7, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Omar Khaled", 1, "+201012345675", "Neurology", null }
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

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_AppointmentDate",
                table: "Appointments",
                column: "AppointmentDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_CreatedAt",
                table: "Appointments",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_Date_Status",
                table: "Appointments",
                columns: new[] { "AppointmentDateTime", "AppointmentStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_Doctor_Date",
                table: "Appointments",
                columns: new[] { "DoctorId", "AppointmentDateTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointments",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientId",
                table: "Appointments",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_Status",
                table: "Appointments",
                column: "AppointmentStatus");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_ApplicationUserId",
                table: "Doctors",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_CreatedAt",
                table: "Doctors",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_Specialization",
                table: "Doctors",
                column: "Specialization");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRecords_AppointmentId",
                table: "MedicalRecords",
                column: "AppointmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRecords_CreatedAt",
                table: "MedicalRecords",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_ApplicationUserId",
                table: "Patients",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_CreatedAt",
                table: "Patients",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_AppointmentId",
                table: "Payments",
                column: "AppointmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CreatedAt",
                table: "Payments",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_Date_Method",
                table: "Payments",
                columns: new[] { "PaymentDate", "PaymentMethod" });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentDate",
                table: "Payments",
                column: "PaymentDate");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentMethod",
                table: "Payments",
                column: "PaymentMethod");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_CreatedAt",
                table: "Prescriptions",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_MedicalRecordId",
                table: "Prescriptions",
                column: "MedicalRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_MedicationName",
                table: "Prescriptions",
                column: "MedicationName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Prescriptions");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "MedicalRecords");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
