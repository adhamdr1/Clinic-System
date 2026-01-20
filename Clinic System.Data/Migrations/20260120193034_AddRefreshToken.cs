using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinic_System.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ExpiresOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    RevokedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "role-admin",
                column: "ConcurrencyStamp",
                value: "7bf854c0-34e2-46a1-874c-0c78cb948540");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "role-doctor",
                column: "ConcurrencyStamp",
                value: "1b1ff7b2-4a8a-4b9c-9315-a2e5b3bd43c0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "role-patient",
                column: "ConcurrencyStamp",
                value: "816b12e0-3695-4bfa-9268-1e62e728ef01");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8f64e14a-8750-4bf3-9f26-e9c2d34caac6", "AQAAAAIAAYagAAAAEAV9CxKRoG3cwG/tPX4KW2juZxA4TaTaplx0Io+VyuRfSC6oJITmHRhiXT36hsC/7A==", "f5a63e82-2739-45ed-92da-785cd1e346c1" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-doc1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e49c6ed5-a1c6-4502-b32a-dc825df5b6d2", "AQAAAAIAAYagAAAAEEgDOJ6t82SOn2vdb0F2XSMfdzkqLQ62xh5UHtyWkDGdl43tGL4Cy/wG/4lKj8fUyA==", "4350c876-7e5e-4ae4-b43e-2f582482e38a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-doc2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "952221fe-1090-4910-8206-563de5b565d0", "AQAAAAIAAYagAAAAEM1DM+iK8dbLWSApR6I/DmV9DhJCIpPq2XwQJy5yZ1ygVugPaWLp3l7ANvZO3bcorQ==", "83c96e9a-eb95-483f-82cf-ead97e98d68e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-doc3",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7456af5a-46e2-47be-857a-1f04ed94d151", "AQAAAAIAAYagAAAAEPNfinCCpISt5/R9jfneG0njeRQCjP5qOOyRVvJEZfd2/oTrlSWvBw3vlRa4zYLEMg==", "f953c8ca-15d1-46d9-a13f-f6fe6e1def71" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-doc4",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a2f97c69-279b-4855-a935-89118cb35d65", "AQAAAAIAAYagAAAAEFvjFMOqcaqgHcdUoteLdtCNNEDRhz3T0wgaEllwoBgXU2Q5l/zQEIMax210pdnO2w==", "339b5dd2-ee81-4bd8-9553-c9659c253f1f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-doc5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "95ffed39-7feb-485b-a7da-b397562e16eb", "AQAAAAIAAYagAAAAEC5qKaTitvSdw9zWWEL2/tU/FnWVLPFy6crrWSWfT9jWc6cHM9XTohFEK43h3Rps9g==", "7c361a01-c681-430e-ad8f-c7b7cfa98ca2" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-pat1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8a1403db-eb94-48e4-8938-957ee3862451", "AQAAAAIAAYagAAAAEBQ4ugEMufSgN6laBfldK6ZwyPPD8VmFvMLozOr+XSSogUg+KCmzklhKF6jytvTUpg==", "8fbce8e0-e801-42e8-9b7d-f7965763ec88" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-pat2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "db47de69-840c-40c5-9f4b-1d952952225b", "AQAAAAIAAYagAAAAEBWgwemsufPVfir3cOaGYX2mk230smXcBWFwFhtwsx9vk1svsGoRFNiWKFj7n9Bz8A==", "b8a475fc-fab9-4cdc-ae55-568915f4a39a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-pat3",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bbabe310-a287-40f7-9759-541ff11c41b0", "AQAAAAIAAYagAAAAEDYOls5rZOKym13xTVU1OKNDgT554naFyjc09ufYZyJnhpEDWMTmkv56S5yLa3N/nA==", "13fc6866-5cd7-4d82-adb0-22f22940b75a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-pat4",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "607310f6-6da0-4923-9220-f7c74c1a251c", "AQAAAAIAAYagAAAAECZnfQCI07+JibgQQ7s2M+CWt8gJxhIIY0GgFufD4i/FZ025LQ96BALAszHf4nvYQw==", "d97be14f-0ed8-47b6-9e4a-8c9e6d8141f6" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-pat5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a069f1ec-2cba-4375-8245-dc8140f82d7c", "AQAAAAIAAYagAAAAEM24chkKmnDkVKcDPwl+Jg+fEqb0aQSvq0D3eyZsXy8R7Srf1+GQXm2xJWZhcvgjcA==", "c383d5aa-8801-4841-97d0-e70fef7ee037" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-pat6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "31f3c5c1-9f0f-4979-ba0f-e4dec4d62ae5", "AQAAAAIAAYagAAAAEGHr7pL/nz6LYnzUBtBx8tM1f9u9TBw/VxBTasOQYS+iMynnqo0XO43ahF0y/mnndw==", "e9def8d0-2e60-4243-82de-788e4ee8b3c2" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-pat7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "db58f95b-7811-48a0-8afb-9779d849b087", "AQAAAAIAAYagAAAAEOA8u6OCqODhy1dldb3aUeSCSAIR1hbZv9ONx2jKVYE1dxEAZgYUdP1bsKJ8hAZeHA==", "415ca316-611a-4a14-9d07-ff97cfdfc306" });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Token",
                table: "RefreshTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "ApplicationUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "role-admin",
                column: "ConcurrencyStamp",
                value: "22444086-99db-403f-b007-0d41c3b9c577");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "role-doctor",
                column: "ConcurrencyStamp",
                value: "2b753f14-46eb-4036-9ff7-33a638c36866");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "role-patient",
                column: "ConcurrencyStamp",
                value: "0add5e7d-4dcb-40f1-b4b8-da22e8a90846");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "57b0ec93-b945-4600-b3a6-c3eda50325ff", "AQAAAAIAAYagAAAAEDaGkrwgMUaewFoCLkctXqWGEMhik9rw4QxJ6S5uVXlLw7r0lyHORpCBkFLMT4bFdw==", "26d3cfac-7bad-4234-9fb9-675cc0503e70" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-doc1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0c8bf8cd-d1b3-4511-992e-82fcb44f1a89", "AQAAAAIAAYagAAAAEHEOOjdczSQm66fPvTnsh1YUqlOJykZc3wRAGeCIlSd0eVR2pZ/cSMo6miiq/2L51A==", "8ffab16d-03c5-40aa-82e3-c0c46f64137d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-doc2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "08b89fa7-9e91-4ea8-b514-7b1fce95d19e", "AQAAAAIAAYagAAAAENDQImACiTTDrg4p2LBcJmN04IWeP54pMtc6jTNztricuTDZ20FEO0r+SzG0buksKg==", "f3edaca3-e5d7-4610-853a-430115ba0e5c" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-doc3",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b29c7229-72ff-46bb-a314-e87641ea6caa", "AQAAAAIAAYagAAAAEN4wwQlL/3uYjFQ1BsKzObUCOHZKwE4QblORwjFa4jJe2dEqTIj8WcKZd4DCGaev7A==", "e1f7dd68-d1f5-421e-81c5-f0d0e46d6d32" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-doc4",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2b35febc-1306-4e1a-a555-4f42835fe0d0", "AQAAAAIAAYagAAAAEDTIgD5/Ot/j6MwYyzkFX/ww0YuHc2Q4/IaaHlASPk3rL9iSnLa0pf6638StIHRzlQ==", "c90a1e2e-e8a9-47b9-8f5d-1e4c066a05e9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-doc5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9db808e1-df22-4337-927f-512e87aa4379", "AQAAAAIAAYagAAAAEClv+V7cDZnPUwwfHlJxkEu+njwU7Jr2xKYJMxkhFwp2otBFOapiasTNaca81ZHkbQ==", "c862f258-8a56-4602-939f-fca51ad61040" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-pat1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7365bd8e-a05f-4c46-bc53-32d42c5c0f00", "AQAAAAIAAYagAAAAEOE608b07nU1cS8IcxrGUtX70K0hxVIBBl9eysxffmSuKh/ivCjJ8KE4nWAy9TR9Zg==", "16fbbc58-00a4-4ea3-9626-199b0dcaa425" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-pat2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "472b92bf-8cf6-47a4-8556-927fb67f36a8", "AQAAAAIAAYagAAAAEON8Jt6xEwpbtIcWElFzOzKELBZ/6K7xjMmOTqx42MsN2zUGbJzEQJoTKem0KkC2Ng==", "02619588-9b4f-426c-b6a8-fa68b2954dd6" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-pat3",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4d1c5d36-606a-462b-bdda-795c82adea92", "AQAAAAIAAYagAAAAEEqR78CF6T3nXob1oWq9VF1x9UlEa4adfmtGkVMBx6Wh0mYveO9RkIG7Xyc2nHDJOw==", "73b63957-f453-47e5-9e41-61bc2803d0f9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-pat4",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5f425f9d-ef02-4d93-a1f4-d7b1299c2009", "AQAAAAIAAYagAAAAEKzVz92Bt229TodGcgtBc0XySktU4UbfU2c+4ZRca7v2GGPuiodxLUmdRrqd1TwFHA==", "1974114a-2bc5-47ca-8dbc-3a58136dc86f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-pat5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bd311e45-a81e-46d7-86d0-71f9d6cd3e29", "AQAAAAIAAYagAAAAEClmBuu/ul0EvTJWvTdQwCR+f0USJdIT/eBINdGELfWIrD73GRCtOqJA1lrCXDAtSA==", "860a041a-91d6-4997-96e2-b9794b9362ae" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-pat6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "85202e4c-98d5-4f58-8f29-da1b32803be5", "AQAAAAIAAYagAAAAEKFb7SBDCEkNKXCba19Pi2WzN6VjyPbk2xkWygAg8aH3H53zMg4WX1Z2wmUf2e1e0g==", "6aacf5a7-fd98-4cf8-a30b-22075483c896" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-pat7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c40dbdae-988b-40f3-a2b6-88914defd943", "AQAAAAIAAYagAAAAEHLPTUjW5/TfOH8LbDoJSwSdMwSYJnSHAh6AqqIOijwoqAe5wF2Z0bBiuzOV0C42HQ==", "5abf1b6d-eb38-4d4e-b86a-18de023cbb02" });
        }
    }
}
