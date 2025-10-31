using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SkillManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class secondCreates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "SkillId", "CategoryId", "Code", "CriticalityLevel", "FirstLevelDescription", "FourthLevelDescription", "Label", "ProjectRequiresSkill", "RequiredLevel", "SecondLevelDescription", "SubCategoryId", "ThirdLevelDescription" },
                values: new object[,]
                {
                    { 15, 3, "ARCADE", "P2", "Connait les informations clés sur l'application (lecture du starter kit, du user guide…)", "Développe / gère l'amélioration continue de l'application. Est capable de challenger le métier sur les évolutions demandées", "ARCADE", true, 2, "Traite en autonomie partielle : le support sur l'application les évolutions mineures", 10, "Traite en autonomie complète : le support L1/L2/L3 sur l'application les évolutions majeures" },
                    { 16, 3, "AVI", "P2", "Connait les informations clés sur l'application (lecture du starter kit, du user guide…)", "Développe / gère l'amélioration continue de l'application. Est capable de challenger le métier sur les évolutions demandées", "AVI", true, 2, "Traite en autonomie partielle : le support sur l'application les évolutions mineures", 10, "Traite en autonomie complète : le support L1/L2/L3 sur l'application les évolutions majeures" },
                    { 17, 3, "BASE EFFECTIF", "P2", "Connait les informations clés sur l'application (lecture du starter kit, du user guide…)", "Développe / gère l'amélioration continue de l'application. Est capable de challenger le métier sur les évolutions demandées", "BASE EFFECTIF", true, 2, "Traite en autonomie partielle : le support sur l'application les évolutions mineures", 11, "Traite en autonomie complète : le support L1/L2/L3 sur l'application les évolutions majeures" },
                    { 18, 6, "DIV : Anglais", "P3", "Sait rédiger un document en anglais.", "Sait animer un comité en anglais.", "Maitrise de l'Anglais", false, 2, "Comprend tout échange téléphonique en anglais.", 33, "Prend la parole en anglais lors d'un comité." },
                    { 19, 6, "DIV : Communication écrite", "P3", "Sait rechercher, analyser et traiter l'information / Sait retranscrire de l'information orale (prise de notes).", "Sait rédiger un argumentaire.", "Communication à l'écrit", false, 2, "Sait structurer et rédiger un document d'analyse.", 33, "Sait rédiger un document de synthèse / Sait adapter le niveau de détails aux équipes et à la complexité du sujet." },
                    { 20, 6, "DIV : Communication orale", "P3", "Sait se faire comprendre dans des situations quotidiennes de communication orales.", "Sait faire un exposé oral devant un auditoire / Sait communiquer spontanément avec le sponsor, le top management GIT.", "Communication à l'oral", false, 2, "Sait mener un entretien individuel.", 33, "Sait prendre la parole au cours d'une réunion / Sait communiquer spontanément avec sa hiérarchie, les contributeurs projets (IT et métiers)." },
                    { 21, 5, "PJT : Méthode de gestion de projet", "P2", "Connaissance théorique sans pratique", "Justifie de n expériences de conduite de projet avec la méthodologie", "Connaissance d'une méthodologie de gestion de projet (CMMI…)", false, 2, "Formation théorique à la méthodologie, y a contribué mais n'a pas encore conduit de projet avec cette méthodologie", 33, "Justifie d'une expérience de conduite de projet avec la méthodologie" },
                    { 22, 5, "PJT : Méthodo AGILE", "P2", "Notions", "Expertise", "Maîtrise de la gestion de projet en AGILE", false, 2, "Connaissances pratiques", 33, "Maitrise" },
                    { 23, 5, "PJT : Méthodologie CACIB TAO", "P2", "Supervise un projet à la fois / N'a pas encore obtenu de certification dans le cadre de la Project Academy.", "Supervise plusieurs projets à la fois / A été certifié PROGRAM DIRECTOR dans le cadre de la Project Academy.", "Méthodologie de Gestion de Projets ARPEGIO / TAO", true, 2, "Supervise un projet à la fois / A été certifié Chef de projet SILVER dans le cadre de la Project Academy.", 33, "Supervise un ou plusieurs projet / A été certifié Chef de projet GOLD dans le cadre de la Project Academy." },
                    { 24, 2, "SI : Chiffrage", "P2", "Connaissance des fondamentaux de chiffrage", "Découper des projets (taille dépendante du profil de la ressource: experte fonctionnelle ou CP) en chantiers et chiffrer ces derniers", "Chiffrage", false, 2, "Chiffrer des tâches unitaires (<2j/h)", 3, "Découper des chantiers (<10j/h) en tâches unitaires et chiffrer ces dernières" },
                    { 25, 2, "SI : Analyse d'impact", "P2", "Identifier les impacts de modification sur une tâche allouée", "Identifier les impacts de modification d'un ensemble de fonctionnalités de toute complexité", "Etude d'impact", false, 2, "Identifier les impacts de modification d'une fonctionnalité simple ou moyennement complexe", 3, "Identifier les impacts de modification d'une fonctionnalité complexe ou d'un ensemble de fonctionnalités simples ou moyennement complexes" },
                    { 26, 2, "SI : Conception générale", "P2", "Notions", "Expertise", "Etude préalable / Conception générale (Architecture, …)", false, 2, "Connaissances pratiques", 3, "Maitrise" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "Eid",
                value: "vitthal.seetah");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: 26);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "Eid",
                value: "vithal.seetah");
        }
    }
}
