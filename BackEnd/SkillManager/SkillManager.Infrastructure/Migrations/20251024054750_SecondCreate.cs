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
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_SubCategories_SubCategoryId",
                table: "Skills"
            );

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryTypeId", "Name" },
                values: new object[,]
                {
                    { 1, 2, "CONNAISSANCES METIER" },
                    { 2, 2, "CONCEPTION SOLUTION SI" },
                    { 3, 1, "CONNAISSANCE APPLI AMC" },
                    { 4, 1, "TECHNIQUES DE PROGRAMMATION" },
                    { 5, 2, "GESTION DE PROJET" },
                    { 6, 2, "AUTRES COMPETENCES" },
                }
            );

            migrationBuilder.InsertData(
                table: "Levels",
                columns: new[] { "LevelId", "Name", "Points" },
                values: new object[,]
                {
                    { 1, "Notion", 1 },
                    { 2, "Pratique", 2 },
                    { 3, "Maitrise", 3 },
                    { 4, "Expert", 4 },
                }
            );

            migrationBuilder.InsertData(
                table: "SubCategories",
                columns: new[] { "SubCategoryId", "CategoryId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Finance" },
                    { 2, 2, "01-Cadrage" },
                    { 3, 2, "02-Design" },
                    { 4, 2, "02-Design fonctionnel" },
                    { 5, 2, "02-Design technique" },
                    { 6, 2, "03-Data" },
                    { 7, 2, "04-Analyse" },
                    { 8, 2, "05-Testing" },
                    { 9, 2, "06-Sécurité" },
                    { 10, 3, "Outil Compta" },
                    { 11, 3, "Outil CdG" },
                    { 12, 3, "Outil Compta reglementaire" },
                    { 13, 3, "Outil Conso" },
                    { 14, 3, "Interprétation comptable" },
                    { 15, 3, "Rapprochements" },
                    { 16, 3, "Technique" },
                    { 17, 4, "Langage de programmation" },
                    { 18, 4, "Transfert de fichiers" },
                    { 19, 4, "CI/CD" },
                    { 20, 4, "Frameworks .NET" },
                    { 21, 4, "Conso" },
                    { 22, 4, "Scripting" },
                    { 23, 4, "Front-End" },
                    { 24, 4, "Middleware" },
                    { 25, 4, "Gestion du code" },
                    { 26, 4, "Base de données" },
                    { 27, 4, "Testing" },
                    { 28, 4, "Sécurité" },
                    { 29, 4, "OS" },
                    { 30, 4, "ETL" },
                    { 31, 4, "Supervision / Monitoring" },
                    { 32, 4, "Paramétrage progiciel" },
                    { 33, 6, "Communication" },
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_SubCategories_SubCategoryId",
                table: "Skills",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "SubCategoryId",
                onDelete: ReferentialAction.Cascade
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_SubCategories_SubCategoryId",
                table: "Skills"
            );

            migrationBuilder.DropIndex(name: "IX_Categories_Name", table: "Categories");

            migrationBuilder.DeleteData(table: "Categories", keyColumn: "CategoryId", keyValue: 5);

            migrationBuilder.DeleteData(table: "Levels", keyColumn: "LevelId", keyValue: 1);

            migrationBuilder.DeleteData(table: "Levels", keyColumn: "LevelId", keyValue: 2);

            migrationBuilder.DeleteData(table: "Levels", keyColumn: "LevelId", keyValue: 3);

            migrationBuilder.DeleteData(table: "Levels", keyColumn: "LevelId", keyValue: 4);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 1
            );

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 2
            );

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 3
            );

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 4
            );

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 5
            );

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 6
            );

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 7
            );

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 8
            );

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 9
            );

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 10
            );

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 11
            );

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 12
            );

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 13
            );

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 14
            );

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 15
            );

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 16
            );

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 17
            );

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 18
            );

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 19
            );

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 20
            );

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 21
            );

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 22
            );

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 23
            );

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 24
            );

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 25
            );

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 26
            );

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 27
            );

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 28
            );

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 29
            );

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 30
            );

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 31
            );

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 32
            );

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 33
            );

            migrationBuilder.DeleteData(table: "Categories", keyColumn: "CategoryId", keyValue: 1);

            migrationBuilder.DeleteData(table: "Categories", keyColumn: "CategoryId", keyValue: 2);

            migrationBuilder.DeleteData(table: "Categories", keyColumn: "CategoryId", keyValue: 3);

            migrationBuilder.DeleteData(table: "Categories", keyColumn: "CategoryId", keyValue: 4);

            migrationBuilder.DeleteData(table: "Categories", keyColumn: "CategoryId", keyValue: 6);

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_SubCategories_SubCategoryId",
                table: "Skills",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "SubCategoryId",
                onDelete: ReferentialAction.Restrict
            );
        }
    }
}
