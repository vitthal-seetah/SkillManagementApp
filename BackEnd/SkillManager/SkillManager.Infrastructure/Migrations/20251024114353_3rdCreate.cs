using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SkillManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _3rdCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Skills_Code",
                table: "Skills");

            migrationBuilder.AlterColumn<string>(
                name: "ThirdLevelDescription",
                table: "Skills",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SecondLevelDescription",
                table: "Skills",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Label",
                table: "Skills",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "FourthLevelDescription",
                table: "Skills",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstLevelDescription",
                table: "Skills",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CriticalityLevel",
                table: "Skills",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "CategoryTypeId",
                value: 1);

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "SkillId", "CategoryId", "Code", "CriticalityLevel", "FirstLevelDescription", "FourthLevelDescription", "Label", "ProjectRequiresSkill", "RequiredLevel", "SecondLevelDescription", "SubCategoryId", "ThirdLevelDescription" },
                values: new object[,]
                {
                    { 1, 1, "FCT-COMPTA-OPS", "P1", "Connaissance des grands principes de la comptabilité des produits financiers, capacité à lire un bilan comptable, connaissance des grandes lignes d'un plan de compte d'une banque d'investissement.", "Dépasse le niveau expérimenté. Connaissance des nouvelles réglementations comptables en lien avec les opérations de marché.", "Comptabilité des instruments financiers : Comptabilité des opérations de marché", false, 2, "Connaissance des grands principes de la comptabilité des opérations de marché, Capacité à comptabiliser des opérations de marché selon les normes IAS39/IFRS9.", 1, "Expérimentée pour créer et /ou mettre à jour les spécifications fonctionnelles en lien avec les shémas comptables des produits financiers selon les normes IAS39/IFRS9." },
                    { 2, 1, "FCT-COMPTA-INTL", "P2", "Dispose d'une connaissance théorique du Système d'Information comptable d'une banque internationale.", "Sait faire le lien entre les fonctionnalités du SI Finance International et les processus de la Direction Financière de CACIB.", "Connaissance du système de Finance International de CACIB et de son architecture (fonctionnelle et applicative)", true, 2, "Connait l'architecture applicative du système d'information Finance International de CACIB.", 1, "Connait les fonctionnalités assurées par les différents composants applicatifs du SI Finance International de CACIB." },
                    { 3, 1, "FCT-RISQUES-ALM", "P2", "Dispose d'une connaissance théorique du domaine des risques bancaires / de l'ALM.", "Dispose d'une connaissance approfondie du monde des risques bancaire et une bonne connaissance du contexte CA-CIB (alimentation systèmes risques, organisation fonction risque…).", "Connaissance Risques / Ratios / ALM", false, 2, "Dispose d'une connaissance approfondie du monde des risques bancaire et une connaissance générale de la gestion des risques par produit (opérations de marché et de financement en général).", 1, "Dispose d'une connaissance approfondie du monde des risques bancaire et une bonne connaissance des impacts Risque sur les opérations de marché et de financement en général." },
                    { 4, 1, "FCT-CONSOLIDATION", "P2", "Dispose d'une connaissance théorique du domaine de la Consolidation.", "Connaissance avec précision de la Consolidation et peut évoluer en toute autonomie sur le sujet. Peut être considéré expert et mener une réflexion structurante chez le client.", "Connaissances conso", false, 2, "Dispose d'une bonne connaissance de la Consolidation.", 1, "Dispose d'une connaissance précise de la Consolidation." },
                    { 5, 1, "FCT-FISCALITE", "P2", "Dispose d'une connaissance théorique du domaine de la fiscalité.", "Connaissance avec précision de la fiscalité et peut évoluer en toute autonomie sur le sujet. Peut être considéré expert et mener une réflexion structurante chez le client.", "Connaissances fiscalité : Base Fiscale, gestion TVA, Coeffficients de fiscalité, FEC, DES", false, 2, "Dispose d'une bonne connaissance de la fiscalité.", 1, "Dispose d'une connaissance précise de la fiscalité." },
                    { 6, 1, "FCT-PROD-FRENCH", "P2", "Dispose d'une connaissance théorique du domaine des Productions French.", "Connaissance avec précision des Productions French et peut évoluer en toute autonomie sur le sujet. Peut être considéré expert et mener une réflexion structurante chez le client.", "Connaissances Productions French (ex : SURFI, PROTIDE, CRT/RPC)", false, 2, "Dispose d'une bonne connaissance des Productions French.", 1, "Dispose d'une connaissance précise des Productions French." },
                    { 7, 1, "FCT-CONTROLE-GESTION", "P1", "Dispose d'une connaissance théorique des campagnes budgétaires. Possède des notions théoriques sur les principaux concepts métiers et leur vocabulaire.", "Être reconnu par ses pairs comme référent du domaine dans les groupes de travail.", "Connaissance du contrôle de gestion", false, 2, "Savoir théorique approfondi mais peu mis en pratique ou mise en pratique sur un périmètre restreint.", 1, "Savoir théorique confronté à une mise en pratique concrète sur un périmètre large." },
                    { 8, 1, "FCT-MIGRATION-COMPTABLE", "P2", "Comprendre les problématiques liés à une migration comptable.", "Organiser, gérer et participer à une migration comptable avec des intervenants internes et externes à MAC.", "Organiser et gérer une migration comptable", false, 2, "Participer à une migration comptable (en recette ou en production).", 1, "Organiser une migration comptable avec des intervenants internes à MAC." },
                    { 9, 1, "FCT-MODELE-DONNEES", "P2", "Dispose d'une connaissance théorique du Modèle de données comptable et réglementaire d'une Banque d'Investissement.", "Sait faire le lien entre le Modèle de données et les différents reporting issus du système d'information Finance.", "Connaissance du modèle de données de la Finance", true, 2, "Connait les principales typologies de données utilisées par la Direction Financière de CACIB.", 1, "Connait le Modèle de Données Finance ainsi que les règles de calcul de ces données." },
                    { 10, 1, "FCT-PROCESSUS-DF", "P2", "Dispose d'une connaissance théorique du mode d'organisation et des processus d'une fonction Finance dans un environnement bancaire.", "Connait les \"pain points\" associés aux différents processus de la fonction Finance et liés à des dysfonctionnements du SI existant.", "Connaissance de l'organisation et des processus du métier Finance", true, 2, "Connait l'organisation générale de la Fonction Finance de CACIB, ainsi que ses principaux processus.", 1, "Connait de manière détaillée les processus du métier Finance de CACIB." },
                    { 11, 1, "FCT-PRODUITS-FINANCIERS", "P1", "Dispose d'une connaissance théorique du domaine bancaire. Possède des notions théoriques sur les principaux concepts métiers et leur vocabulaire.", "Dispose d'une connaissance approfondie du monde bancaire et une connaissance détaillée des opérations de marché et de financement.", "Connaissance des produits financiers, des produits de marchés, de l'organisation d'une BFI", false, 2, "Dispose d'une connaissance approfondie du monde bancaire et une connaissance générale des opérations de marché et de financement en général.", 1, "Dispose d'une connaissance approfondie du monde bancaire et une bonne connaissance des opérations de marché et de financement en général." },
                    { 12, 1, "FCT-PRODUITS-FIN", "P1", "Dispose d'une connaissance théorique du domaine bancaire. Possède des notions théoriques sur les principaux concepts métiers et leur vocabulaire.", "Dispose d'une connaissance approfondie du monde bancaire et une connaissance détaillée des opérations de marché et de financement.", "Connaissance des produits financiers, des produits de marchés, de l'organisation d'une BFI", false, 2, "Dispose d'une connaissance approfondie du monde bancaire et une connaissance générale des opérations de marché et de financement en général.", 1, "Dispose d'une connaissance approfondie du monde bancaire et une bonne connaissance des opérations de marché et de financement en général." },
                    { 13, 1, "FCT-RAPPROCHEMENTS", "P2", "Dispose d'une connaissance théorique des rapprochements comptables dans une banque.", "Sait faire le lien entre les fonctionnalités des outils applicatifs et les processus de rapprochement de la Direction Financière de CACIB CN1 & CN2.", "Connaissance des Rapprochements Finance de CACIB et de son architecture (fonctionnelle et applicative)", true, 2, "Connait l'architecture applicative des systèmes de rapprochement de CACIB.", 1, "Connait les fonctionnalités assurées par les différents composants applicatifs (Intellimatch et EasyMatch) des systèmes de rapprochements." },
                    { 14, 1, "FCT-REGLEMENTAIRE", "P1", "Connaissance de l'environnement réglementaire & prudentiel propre aux établissements financiers. Possède des notions théoriques sur les principaux concepts métiers et leur vocabulaire.", "Connaissance avec précision des publications et de l'environnement de contrôle réglementaire prudentiel et peut évoluer en toute autonomie sur le sujet. Peut être considéré expert et mener une réflexion structurante chez le client.", "Connaissance Réglementaire et Reportings Financiers : Attributs réglementaires", false, 2, "Connaissance de l'environnement/contexte réglementaire & prudentiel et a une expérience en lien avec cet environnement.", 1, "Connaissance de manière très précise l'environnement/contexte réglementaire & prudentiel et a une expérience en lien avec cet environnement." }
                });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 2,
                column: "Name",
                value: "Cadrage");

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 3,
                column: "Name",
                value: "Design");

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 4,
                column: "Name",
                value: "Design fonctionnel");

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 5,
                column: "Name",
                value: "Design technique");

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 6,
                column: "Name",
                value: "Data");

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 7,
                column: "Name",
                value: "Analyse");

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 8,
                column: "Name",
                value: "Testing");

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 9,
                column: "Name",
                value: "Sécurité");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: 14);

            migrationBuilder.AlterColumn<string>(
                name: "ThirdLevelDescription",
                table: "Skills",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "SecondLevelDescription",
                table: "Skills",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "Label",
                table: "Skills",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "FourthLevelDescription",
                table: "Skills",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "FirstLevelDescription",
                table: "Skills",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "CriticalityLevel",
                table: "Skills",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "CategoryTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 2,
                column: "Name",
                value: "01-Cadrage");

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 3,
                column: "Name",
                value: "02-Design");

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 4,
                column: "Name",
                value: "02-Design fonctionnel");

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 5,
                column: "Name",
                value: "02-Design technique");

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 6,
                column: "Name",
                value: "03-Data");

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 7,
                column: "Name",
                value: "04-Analyse");

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 8,
                column: "Name",
                value: "05-Testing");

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "SubCategoryId",
                keyValue: 9,
                column: "Name",
                value: "06-Sécurité");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_Code",
                table: "Skills",
                column: "Code",
                unique: true);
        }
    }
}
