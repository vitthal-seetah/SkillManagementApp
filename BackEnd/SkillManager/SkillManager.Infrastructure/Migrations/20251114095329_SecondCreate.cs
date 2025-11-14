using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SecondCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "ProjectId", "ProjectDescription", "ProjectName" },
                values: new object[] { 1, "Doing Things", "SMAC" }
            );

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "RoleId",
                keyValue: 1,
                column: "Name",
                value: "SuperAdmin"
            );

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "RoleId",
                keyValue: 2,
                column: "Name",
                value: "Admin"
            );

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "RoleId",
                keyValue: 4,
                column: "Name",
                value: "TeamLead"
            );

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "Name" },
                values: new object[] { 5, "Employee" }
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 1, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8304)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 2, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8305)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 3, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8306)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 15, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8295)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 16, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8308)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 17, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8297)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 18, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8298)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 19, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8309)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 20, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8312)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 21, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8299)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 22, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8310)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 23, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8300)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 24, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8311)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 25, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8313)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 26, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8294)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 27, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8315)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 28, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8302)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 29, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8303)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 1, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8282)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 2, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8283)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 3, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8284)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 15, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8222)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 16, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8285)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 17, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8224)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 18, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8275)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 19, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8287)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 20, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8291)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 21, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8276)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 22, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8288)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 23, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8278)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 24, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8289)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 25, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8292)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 26, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8216)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 27, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8293)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 28, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8279)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 29, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 14, 9, 53, 28, 715, DateTimeKind.Utc).AddTicks(8280)
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "Projects", keyColumn: "ProjectId", keyValue: 1);

            migrationBuilder.DeleteData(table: "UserRoles", keyColumn: "RoleId", keyValue: 5);

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "RoleId",
                keyValue: 1,
                column: "Name",
                value: "Admin"
            );

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "RoleId",
                keyValue: 2,
                column: "Name",
                value: "TechLead"
            );

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "RoleId",
                keyValue: 4,
                column: "Name",
                value: "Employee"
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 1, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1637)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 2, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1638)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 3, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1639)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 15, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1631)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 16, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1640)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 17, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1632)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 18, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1633)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 19, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1641)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 20, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1644)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 21, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1634)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 22, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1642)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 23, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1635)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 24, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1643)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 25, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1645)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 26, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1630)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 27, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1646)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 28, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1636)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 29, 1 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1637)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 1, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1621)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 2, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1622)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 3, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1623)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 15, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1614)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 16, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1624)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 17, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1615)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 18, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1616)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 19, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1625)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 20, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1628)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 21, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1617)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 22, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1626)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 23, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1618)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 24, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1627)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 25, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1629)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 26, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1610)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 27, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1629)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 28, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1619)
            );

            migrationBuilder.UpdateData(
                table: "UserSkills",
                keyColumns: new[] { "SkillId", "UserId" },
                keyValues: new object[] { 29, 2 },
                column: "UpdatedTime",
                value: new DateTime(2025, 11, 6, 10, 29, 3, 620, DateTimeKind.Utc).AddTicks(1620)
            );
        }
    }
}
