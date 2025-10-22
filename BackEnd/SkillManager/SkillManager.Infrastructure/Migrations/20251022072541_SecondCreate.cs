using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SkillManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SecondCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CategoryTypes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 100);

            migrationBuilder.InsertData(
                table: "CategoryTypes",
                columns: new[] { "CategoryTypeId", "Name" },
                values: new object[,]
                {
                    { 1, "Technical" },
                    { 2, "Functional" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RefId",
                table: "Users",
                column: "RefId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_RefId",
                table: "Users");

            migrationBuilder.DeleteData(
                table: "CategoryTypes",
                keyColumn: "CategoryTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CategoryTypes",
                keyColumn: "CategoryTypeId",
                keyValue: 2);

            migrationBuilder.AlterColumn<int>(
                name: "Name",
                table: "CategoryTypes",
                type: "int",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
