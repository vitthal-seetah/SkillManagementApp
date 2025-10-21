using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AppManagement.Infrastructure.Identity.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "e4a52831-c19e-469e-a3d7-f30ebe5badec", "526033fb-d870-4f16-bdc9-74dbed29a281" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "9e560806-f61f-4031-ae9b-a9faad6824f9", "e67a49ce-0022-477e-9b16-69c705a0d99a" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9e560806-f61f-4031-ae9b-a9faad6824f9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e4a52831-c19e-469e-a3d7-f30ebe5badec");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "526033fb-d870-4f16-bdc9-74dbed29a281");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e67a49ce-0022-477e-9b16-69c705a0d99a");

            migrationBuilder.AddColumn<int>(
                name: "DeliveryType",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RefId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UTCode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SkillSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillSections_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SkillSectionId = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skills_SkillSections_SkillSectionId",
                        column: x => x.SkillSectionId,
                        principalTable: "SkillSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSkills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SkillId = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSkills_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "310e3593-d06a-4617-887b-2f9153edea09", null, "Employee", "EMPLOYEE" },
                    { "4a0e59b4-9c1e-4536-b43f-119d13556b8e", null, "Admin", "ADMIN" },
                    { "4c31900d-90f7-43a2-beec-c2bf0af83dea", null, "Tech Lead", "TECH LEAD" },
                    { "5bdb9b9d-f62f-4ec2-9451-fbcf9ed13752", null, "Manager", "MANAGER" },
                    { "98a4b2e6-9833-4b7c-9d2b-fba562f7a4ef", null, "SME", "SME" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DeliveryType", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefId", "RoleId", "SecurityStamp", "Status", "TwoFactorEnabled", "UTCode", "UserName" },
                values: new object[,]
                {
                    { "8310a350-45e3-4b03-82d6-3120d3edad80", 0, "5d2b0495-7215-4c9a-a515-af517929e480", 1, "leader@localhost.com", true, "System", "Leader", false, null, "LEADER@LOCALHOST.COM", "LEADER@LOCALHOST.COM", "AQAAAAIAAYagAAAAEAWLa5NIu3Yq/gaXjiu7QRBknFxI5sSu3iLZEXLwgxo7usnl5Z6duOyDSTAb97k0LQ==", null, false, "HR003", 2, "c71b8216-6f77-48b4-8556-b977cca9939d", 1, false, "UT003", "leader@localhost.com" },
                    { "a4950d3d-ca05-40ab-b8ff-7791c173ba98", 0, "aeb9b6e1-d492-48c1-bbf4-5c5079f0d564", 1, "admin@localhost.com", true, "System", "Admin", false, null, "ADMIN@LOCALHOST.COM", "ADMIN@LOCALHOST.COM", "AQAAAAIAAYagAAAAEGJ5wcxQ3UGcK7pq9iQ1LAkDBW+sW98nP0Z2I36NPLP+VzkpW38ebpTxezV7LcHBiw==", null, false, "HR001", 1, "16c0130f-b502-47f8-b70d-ebb846e11514", 1, false, "UT001", "admin@localhost.com" },
                    { "a6146e7c-febf-4fbb-83ab-97fccabb044c", 0, "41ca50cb-0973-475e-85ba-55819c3fdc97", 1, "user1@localhost.com", true, "System", "User", false, null, "USER1@LOCALHOST.COM", "USER1@LOCALHOST.COM", "AQAAAAIAAYagAAAAEFeFIU48h2wYzUqeUsBlFSdnOD2Kz5DQhhBVvsm/hil3U106cFlH9BjHf3XfFQ6mcA==", null, false, "HR002", 5, "1896f609-d1c6-4091-aae2-04114409640c", 1, false, "UT002", "user1@localhost.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "4c31900d-90f7-43a2-beec-c2bf0af83dea", "8310a350-45e3-4b03-82d6-3120d3edad80" },
                    { "4a0e59b4-9c1e-4536-b43f-119d13556b8e", "a4950d3d-ca05-40ab-b8ff-7791c173ba98" },
                    { "310e3593-d06a-4617-887b-2f9153edea09", "a6146e7c-febf-4fbb-83ab-97fccabb044c" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Skills_SkillSectionId",
                table: "Skills",
                column: "SkillSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillSections_CategoryId",
                table: "SkillSections",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkills_SkillId",
                table: "UserSkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkills_UserId",
                table: "UserSkills",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSkills");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "SkillSections");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5bdb9b9d-f62f-4ec2-9451-fbcf9ed13752");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "98a4b2e6-9833-4b7c-9d2b-fba562f7a4ef");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "4c31900d-90f7-43a2-beec-c2bf0af83dea", "8310a350-45e3-4b03-82d6-3120d3edad80" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "4a0e59b4-9c1e-4536-b43f-119d13556b8e", "a4950d3d-ca05-40ab-b8ff-7791c173ba98" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "310e3593-d06a-4617-887b-2f9153edea09", "a6146e7c-febf-4fbb-83ab-97fccabb044c" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "310e3593-d06a-4617-887b-2f9153edea09");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4a0e59b4-9c1e-4536-b43f-119d13556b8e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4c31900d-90f7-43a2-beec-c2bf0af83dea");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8310a350-45e3-4b03-82d6-3120d3edad80");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a4950d3d-ca05-40ab-b8ff-7791c173ba98");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a6146e7c-febf-4fbb-83ab-97fccabb044c");

            migrationBuilder.DropColumn(
                name: "DeliveryType",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RefId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UTCode",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9e560806-f61f-4031-ae9b-a9faad6824f9", null, "Customer", "CUSTOMER" },
                    { "e4a52831-c19e-469e-a3d7-f30ebe5badec", null, "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "526033fb-d870-4f16-bdc9-74dbed29a281", 0, "124278c3-1a7e-4a74-8cfe-193f7288b630", "admin@localhost.com", true, "System", "Admin", false, null, "ADMIN@LOCALHOST.COM", "ADMIN@LOCALHOST.COM", "AQAAAAIAAYagAAAAEG5zSC0HE5gV4F+vfsu/MZA8luZUR8tWZ4qBaRxuhXNBnD5jj/eogJ0PRk15ebJm6w==", null, false, "92edaf1d-243e-4e2c-adc6-f7c7abfba3fc", false, "admin@localhost.com" },
                    { "e67a49ce-0022-477e-9b16-69c705a0d99a", 0, "2fc78d0a-22c2-4b7b-a96e-dc02d940e4d1", "customer01@localhost.com", true, "System", "User", false, null, "CUSTOMER01@LOCALHOST.COM", "CUSTOMER01@LOCALHOST.COM", "AQAAAAIAAYagAAAAELy/JSfz27vOxYaSFq75AF7QlXLSv2ribXzyg0czto42F8Y4bib7tEIEvieSn/vVJw==", null, false, "c7e7963e-58ed-4a05-965a-a6d734d7a6d7", false, "customer01@localhost.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "e4a52831-c19e-469e-a3d7-f30ebe5badec", "526033fb-d870-4f16-bdc9-74dbed29a281" },
                    { "9e560806-f61f-4031-ae9b-a9faad6824f9", "e67a49ce-0022-477e-9b16-69c705a0d99a" }
                });
        }
    }
}
