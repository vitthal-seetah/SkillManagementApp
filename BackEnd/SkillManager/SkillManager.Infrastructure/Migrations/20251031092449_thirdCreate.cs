using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SkillManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class thirdCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "SkillId", "CategoryId", "Code", "CriticalityLevel", "FirstLevelDescription", "FourthLevelDescription", "Label", "ProjectRequiresSkill", "RequiredLevel", "SecondLevelDescription", "SubCategoryId", "ThirdLevelDescription" },
                values: new object[,]
                {
                    { 27, 4, "TEC : .NET", "P2", "Notions. Développe sur la base de Spec Technique détaillée fournies. A besoin d'un accompagnement rapproché", "Expertise. Développe en toute autonomie. Est reconnu comme référent sur ce langage. Maîtrise les outils de développement associés et leurs options avancées", "Langage de programmation .NET", false, 2, "Connaissances pratiques. Développe sur la base de Spec Technique détaillée fournies. A besoin d'un accompagnement ponctuel", 17, "Maitrise. Développe en toute autonomie" },
                    { 28, 4, "TEC : AFT", "P2", "Notions. Développe sur la base de Spec Technique détaillée fournies. A besoin d'un accompagnement rapproché", "Expertise. Développe en toute autonomie. Est reconnu comme référent sur ce langage. Maîtrise les outils de développement associés et leurs options avancées", "AFT", false, 2, "Connaissances pratiques. Développe sur la base de Spec Technique détaillée fournies. A besoin d'un accompagnement ponctuel", 18, "Maitrise. Développe en toute autonomie" },
                    { 29, 4, "TEC : Ansible", "P2", "Notions. Développe sur la base de Spec Technique détaillée fournies. A besoin d'un accompagnement rapproché", "Expertise. Développe en toute autonomie. Est reconnu comme référent sur ce langage. Maîtrise les outils de développement associés et leurs options avancées", "Ansible", false, 2, "Connaissances pratiques. Développe sur la base de Spec Technique détaillée fournies. A besoin d'un accompagnement ponctuel", 19, "Maitrise. Développe en toute autonomie" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: 29);
        }
    }
}
