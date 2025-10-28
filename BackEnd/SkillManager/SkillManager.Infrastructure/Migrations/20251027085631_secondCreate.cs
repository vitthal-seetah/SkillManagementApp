using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SkillManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class secondCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationSuites",
                columns: table => new
                {
                    SuiteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Perimeter = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationSuites", x => x.SuiteId);
                });

            migrationBuilder.CreateTable(
                name: "CategoryTypes",
                columns: table => new
                {
                    CategoryTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTypes", x => x.CategoryTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Levels",
                columns: table => new
                {
                    LevelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Levels", x => x.LevelId);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CategoryTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                    table.ForeignKey(
                        name: "FK_Categories_CategoryTypes_CategoryTypeId",
                        column: x => x.CategoryTypeId,
                        principalTable: "CategoryTypes",
                        principalColumn: "CategoryTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UtCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RefId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    Domain = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Eid = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_UserRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "UserRoles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    ApplicationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SuiteId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.ApplicationId);
                    table.ForeignKey(
                        name: "FK_Applications_ApplicationSuites_SuiteId",
                        column: x => x.SuiteId,
                        principalTable: "ApplicationSuites",
                        principalColumn: "SuiteId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applications_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubCategories",
                columns: table => new
                {
                    SubCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.SubCategoryId);
                    table.ForeignKey(
                        name: "FK_SubCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    SkillId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    SubCategoryId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Label = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CriticalityLevel = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ProjectRequiresSkill = table.Column<bool>(type: "bit", nullable: false),
                    RequiredLevel = table.Column<int>(type: "int", nullable: false),
                    FirstLevelDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    SecondLevelDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ThirdLevelDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    FourthLevelDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.SkillId);
                    table.ForeignKey(
                        name: "FK_Skills_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Skills_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "SubCategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationSkills",
                columns: table => new
                {
                    ApplicationId = table.Column<int>(type: "int", nullable: false),
                    SkillId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationSkills", x => new { x.ApplicationId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_ApplicationSkills_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "ApplicationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationSkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "SkillId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSkills",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SkillId = table.Column<int>(type: "int", nullable: false),
                    LevelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSkills", x => new { x.UserId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_UserSkills_Levels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Levels",
                        principalColumn: "LevelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserSkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "SkillId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSkills_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSMEs",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SkillId = table.Column<int>(type: "int", nullable: false),
                    CategoryTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSMEs", x => new { x.UserId, x.SkillId, x.CategoryTypeId });
                    table.ForeignKey(
                        name: "FK_UserSMEs_CategoryTypes_CategoryTypeId",
                        column: x => x.CategoryTypeId,
                        principalTable: "CategoryTypes",
                        principalColumn: "CategoryTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserSMEs_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "SkillId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSMEs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CategoryTypes",
                columns: new[] { "CategoryTypeId", "Name" },
                values: new object[,]
                {
                    { 1, "Technical" },
                    { 2, "Functional" }
                });

            migrationBuilder.InsertData(
                table: "Levels",
                columns: new[] { "LevelId", "Name", "Points" },
                values: new object[,]
                {
                    { 1, "Notion", 1 },
                    { 2, "Pratique", 2 },
                    { 3, "Maitrise", 3 },
                    { 4, "Expert", 4 }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "Name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "TechLead" },
                    { 3, "Manager" },
                    { 4, "Employee" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryTypeId", "Name" },
                values: new object[,]
                {
                    { 1, 2, "CONNAISSANCES METIER" },
                    { 2, 1, "CONCEPTION SOLUTION SI" },
                    { 3, 1, "CONNAISSANCE APPLI AMC" },
                    { 4, 1, "TECHNIQUES DE PROGRAMMATION" },
                    { 5, 2, "GESTION DE PROJET" },
                    { 6, 2, "AUTRES COMPETENCES" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "DeliveryType", "Domain", "Eid", "FirstName", "LastName", "RefId", "RoleId", "Status", "UtCode" },
                values: new object[,]
                {
                    { 1, "Onshore", "DIR", "girish.s.jagroop", "Girish", "Jagroop", "Rf002", 1, "Active", "UT003" },
                    { 2, "Onshore", "DIR", "vitthal.seetah", "Vitthal", "Seetah", "Rf001", 1, "Active", "UT002" }
                });

            migrationBuilder.InsertData(
                table: "SubCategories",
                columns: new[] { "SubCategoryId", "CategoryId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Finance" },
                    { 2, 2, "Cadrage" },
                    { 3, 2, "Design" },
                    { 4, 2, "Design fonctionnel" },
                    { 5, 2, "Design technique" },
                    { 6, 2, "Data" },
                    { 7, 2, "Analyse" },
                    { 8, 2, "Testing" },
                    { 9, 2, "Sécurité" },
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
                    { 33, 6, "Communication" }
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Applications_CategoryId",
                table: "Applications",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_SuiteId",
                table: "Applications",
                column: "SuiteId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationSkills_ApplicationId",
                table: "ApplicationSkills",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationSkills_SkillId",
                table: "ApplicationSkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CategoryTypeId",
                table: "Categories",
                column: "CategoryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_CategoryId",
                table: "Skills",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_SubCategoryId",
                table: "Skills",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_CategoryId",
                table: "SubCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_Name",
                table: "UserRoles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Eid",
                table: "Users",
                column: "Eid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RefId",
                table: "Users",
                column: "RefId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UtCode",
                table: "Users",
                column: "UtCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSkills_LevelId",
                table: "UserSkills",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkills_SkillId",
                table: "UserSkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkills_UserId",
                table: "UserSkills",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSMEs_CategoryTypeId",
                table: "UserSMEs",
                column: "CategoryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSMEs_SkillId",
                table: "UserSMEs",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSMEs_UserId",
                table: "UserSMEs",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationSkills");

            migrationBuilder.DropTable(
                name: "UserSkills");

            migrationBuilder.DropTable(
                name: "UserSMEs");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "Levels");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ApplicationSuites");

            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "CategoryTypes");
        }
    }
}
