using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillManager.Domain.Entities;

namespace SkillManager.Infrastructure.Identity.AppDbContext.Configurations
{
    public class SkillConfiguration : IEntityTypeConfiguration<Skill>
    {
        public void Configure(EntityTypeBuilder<Skill> builder)
        {
            builder.ToTable("Skills");

            builder.HasKey(s => s.SkillId);

            builder.Property(s => s.Code).HasMaxLength(50).IsRequired();
            builder.Property(s => s.Label).HasMaxLength(500).IsRequired();
            builder.Property(s => s.CriticalityLevel).HasMaxLength(10);
            builder.Property(s => s.FirstLevelDescription).HasMaxLength(1000);
            builder.Property(s => s.SecondLevelDescription).HasMaxLength(1000);
            builder.Property(s => s.ThirdLevelDescription).HasMaxLength(1000);
            builder.Property(s => s.FourthLevelDescription).HasMaxLength(1000);

            builder.HasData(
                new Skill
                {
                    SkillId = 1,
                    CategoryId = 1, // CONNAISSANCES METIER
                    SubCategoryId = 1, // Finance
                    Code = "FCT-COMPTA-OPS",
                    Label =
                        "Comptabilité des instruments financiers : Comptabilité des opérations de marché",
                    CriticalityLevel = "P1",
                    ProjectRequiresSkill = false,
                    RequiredLevel = 2,
                    FirstLevelDescription =
                        "Connaissance des grands principes de la comptabilité des produits financiers, capacité à lire un bilan comptable, connaissance des grandes lignes d'un plan de compte d'une banque d'investissement.",
                    SecondLevelDescription =
                        "Connaissance des grands principes de la comptabilité des opérations de marché, Capacité à comptabiliser des opérations de marché selon les normes IAS39/IFRS9.",
                    ThirdLevelDescription =
                        "Expérimentée pour créer et /ou mettre à jour les spécifications fonctionnelles en lien avec les shémas comptables des produits financiers selon les normes IAS39/IFRS9.",
                    FourthLevelDescription =
                        "Dépasse le niveau expérimenté. Connaissance des nouvelles réglementations comptables en lien avec les opérations de marché.",
                },
                new Skill
                {
                    SkillId = 2,
                    CategoryId = 1, // CONNAISSANCES METIER
                    SubCategoryId = 1, // Finance
                    Code = "FCT-COMPTA-INTL",
                    Label =
                        "Connaissance du système de Finance International de CACIB et de son architecture (fonctionnelle et applicative)",
                    CriticalityLevel = "P2",
                    ProjectRequiresSkill = true,
                    RequiredLevel = 2,
                    FirstLevelDescription =
                        "Dispose d'une connaissance théorique du Système d'Information comptable d'une banque internationale.",
                    SecondLevelDescription =
                        "Connait l'architecture applicative du système d'information Finance International de CACIB.",
                    ThirdLevelDescription =
                        "Connait les fonctionnalités assurées par les différents composants applicatifs du SI Finance International de CACIB.",
                    FourthLevelDescription =
                        "Sait faire le lien entre les fonctionnalités du SI Finance International et les processus de la Direction Financière de CACIB.",
                },
                new Skill
                {
                    SkillId = 3,
                    CategoryId = 1,
                    SubCategoryId = 1,
                    Code = "FCT-RISQUES-ALM",
                    Label = "Connaissance Risques / Ratios / ALM",
                    CriticalityLevel = "P2",
                    ProjectRequiresSkill = false,
                    RequiredLevel = 2,
                    FirstLevelDescription =
                        "Dispose d'une connaissance théorique du domaine des risques bancaires / de l'ALM.",
                    SecondLevelDescription =
                        "Dispose d'une connaissance approfondie du monde des risques bancaire et une connaissance générale de la gestion des risques par produit (opérations de marché et de financement en général).",
                    ThirdLevelDescription =
                        "Dispose d'une connaissance approfondie du monde des risques bancaire et une bonne connaissance des impacts Risque sur les opérations de marché et de financement en général.",
                    FourthLevelDescription =
                        "Dispose d'une connaissance approfondie du monde des risques bancaire et une bonne connaissance du contexte CA-CIB (alimentation systèmes risques, organisation fonction risque…).",
                },
                new Skill
                {
                    SkillId = 4,
                    CategoryId = 1,
                    SubCategoryId = 1,
                    Code = "FCT-CONSOLIDATION",
                    Label = "Connaissances conso",
                    CriticalityLevel = "P2",
                    ProjectRequiresSkill = false,
                    RequiredLevel = 2,
                    FirstLevelDescription =
                        "Dispose d'une connaissance théorique du domaine de la Consolidation.",
                    SecondLevelDescription =
                        "Dispose d'une bonne connaissance de la Consolidation.",
                    ThirdLevelDescription =
                        "Dispose d'une connaissance précise de la Consolidation.",
                    FourthLevelDescription =
                        "Connaissance avec précision de la Consolidation et peut évoluer en toute autonomie sur le sujet. Peut être considéré expert et mener une réflexion structurante chez le client.",
                },
                new Skill
                {
                    SkillId = 5,
                    CategoryId = 1,
                    SubCategoryId = 1,
                    Code = "FCT-FISCALITE",
                    Label =
                        "Connaissances fiscalité : Base Fiscale, gestion TVA, Coeffficients de fiscalité, FEC, DES",
                    CriticalityLevel = "P2",
                    ProjectRequiresSkill = false,
                    RequiredLevel = 2,
                    FirstLevelDescription =
                        "Dispose d'une connaissance théorique du domaine de la fiscalité.",
                    SecondLevelDescription = "Dispose d'une bonne connaissance de la fiscalité.",
                    ThirdLevelDescription = "Dispose d'une connaissance précise de la fiscalité.",
                    FourthLevelDescription =
                        "Connaissance avec précision de la fiscalité et peut évoluer en toute autonomie sur le sujet. Peut être considéré expert et mener une réflexion structurante chez le client.",
                },
                new Skill
                {
                    SkillId = 6,
                    CategoryId = 1,
                    SubCategoryId = 1,
                    Code = "FCT-PROD-FRENCH",
                    Label = "Connaissances Productions French (ex : SURFI, PROTIDE, CRT/RPC)",
                    CriticalityLevel = "P2",
                    ProjectRequiresSkill = false,
                    RequiredLevel = 2,
                    FirstLevelDescription =
                        "Dispose d'une connaissance théorique du domaine des Productions French.",
                    SecondLevelDescription =
                        "Dispose d'une bonne connaissance des Productions French.",
                    ThirdLevelDescription =
                        "Dispose d'une connaissance précise des Productions French.",
                    FourthLevelDescription =
                        "Connaissance avec précision des Productions French et peut évoluer en toute autonomie sur le sujet. Peut être considéré expert et mener une réflexion structurante chez le client.",
                },
                new Skill
                {
                    SkillId = 7,
                    CategoryId = 1,
                    SubCategoryId = 1,
                    Code = "FCT-CONTROLE-GESTION",
                    Label = "Connaissance du contrôle de gestion",
                    CriticalityLevel = "P1",
                    ProjectRequiresSkill = false,
                    RequiredLevel = 2,
                    FirstLevelDescription =
                        "Dispose d'une connaissance théorique des campagnes budgétaires. Possède des notions théoriques sur les principaux concepts métiers et leur vocabulaire.",
                    SecondLevelDescription =
                        "Savoir théorique approfondi mais peu mis en pratique ou mise en pratique sur un périmètre restreint.",
                    ThirdLevelDescription =
                        "Savoir théorique confronté à une mise en pratique concrète sur un périmètre large.",
                    FourthLevelDescription =
                        "Être reconnu par ses pairs comme référent du domaine dans les groupes de travail.",
                },
                new Skill
                {
                    SkillId = 8,
                    CategoryId = 1,
                    SubCategoryId = 1,
                    Code = "FCT-MIGRATION-COMPTABLE",
                    Label = "Organiser et gérer une migration comptable",
                    CriticalityLevel = "P2",
                    ProjectRequiresSkill = false,
                    RequiredLevel = 2,
                    FirstLevelDescription =
                        "Comprendre les problématiques liés à une migration comptable.",
                    SecondLevelDescription =
                        "Participer à une migration comptable (en recette ou en production).",
                    ThirdLevelDescription =
                        "Organiser une migration comptable avec des intervenants internes à MAC.",
                    FourthLevelDescription =
                        "Organiser, gérer et participer à une migration comptable avec des intervenants internes et externes à MAC.",
                },
                new Skill
                {
                    SkillId = 9,
                    CategoryId = 1,
                    SubCategoryId = 1,
                    Code = "FCT-MODELE-DONNEES",
                    Label = "Connaissance du modèle de données de la Finance",
                    CriticalityLevel = "P2",
                    ProjectRequiresSkill = true,
                    RequiredLevel = 2,
                    FirstLevelDescription =
                        "Dispose d'une connaissance théorique du Modèle de données comptable et réglementaire d'une Banque d'Investissement.",
                    SecondLevelDescription =
                        "Connait les principales typologies de données utilisées par la Direction Financière de CACIB.",
                    ThirdLevelDescription =
                        "Connait le Modèle de Données Finance ainsi que les règles de calcul de ces données.",
                    FourthLevelDescription =
                        "Sait faire le lien entre le Modèle de données et les différents reporting issus du système d'information Finance.",
                },
                new Skill
                {
                    SkillId = 10,
                    CategoryId = 1,
                    SubCategoryId = 1,
                    Code = "FCT-PROCESSUS-DF",
                    Label = "Connaissance de l'organisation et des processus du métier Finance",
                    CriticalityLevel = "P2",
                    ProjectRequiresSkill = true,
                    RequiredLevel = 2,
                    FirstLevelDescription =
                        "Dispose d'une connaissance théorique du mode d'organisation et des processus d'une fonction Finance dans un environnement bancaire.",
                    SecondLevelDescription =
                        "Connait l'organisation générale de la Fonction Finance de CACIB, ainsi que ses principaux processus.",
                    ThirdLevelDescription =
                        "Connait de manière détaillée les processus du métier Finance de CACIB.",
                    FourthLevelDescription =
                        "Connait les \"pain points\" associés aux différents processus de la fonction Finance et liés à des dysfonctionnements du SI existant.",
                },
                new Skill
                {
                    SkillId = 11,
                    CategoryId = 1,
                    SubCategoryId = 1,
                    Code = "FCT-PRODUITS-FINANCIERS",
                    Label =
                        "Connaissance des produits financiers, des produits de marchés, de l'organisation d'une BFI",
                    CriticalityLevel = "P1",
                    ProjectRequiresSkill = false,
                    RequiredLevel = 2,
                    FirstLevelDescription =
                        "Dispose d'une connaissance théorique du domaine bancaire. Possède des notions théoriques sur les principaux concepts métiers et leur vocabulaire.",
                    SecondLevelDescription =
                        "Dispose d'une connaissance approfondie du monde bancaire et une connaissance générale des opérations de marché et de financement en général.",
                    ThirdLevelDescription =
                        "Dispose d'une connaissance approfondie du monde bancaire et une bonne connaissance des opérations de marché et de financement en général.",
                    FourthLevelDescription =
                        "Dispose d'une connaissance approfondie du monde bancaire et une connaissance détaillée des opérations de marché et de financement.",
                },
                new Skill
                {
                    SkillId = 12,
                    CategoryId = 1, // CONNAISSANCES METIER
                    SubCategoryId = 1, // Finance
                    Code = "FCT-PRODUITS-FIN",
                    Label =
                        "Connaissance des produits financiers, des produits de marchés, de l'organisation d'une BFI",
                    CriticalityLevel = "P1",
                    ProjectRequiresSkill = false,
                    RequiredLevel = 2,
                    FirstLevelDescription =
                        "Dispose d'une connaissance théorique du domaine bancaire. Possède des notions théoriques sur les principaux concepts métiers et leur vocabulaire.",
                    SecondLevelDescription =
                        "Dispose d'une connaissance approfondie du monde bancaire et une connaissance générale des opérations de marché et de financement en général.",
                    ThirdLevelDescription =
                        "Dispose d'une connaissance approfondie du monde bancaire et une bonne connaissance des opérations de marché et de financement en général.",
                    FourthLevelDescription =
                        "Dispose d'une connaissance approfondie du monde bancaire et une connaissance détaillée des opérations de marché et de financement.",
                },
                new Skill
                {
                    SkillId = 13,
                    CategoryId = 1,
                    SubCategoryId = 1,
                    Code = "FCT-RAPPROCHEMENTS",
                    Label =
                        "Connaissance des Rapprochements Finance de CACIB et de son architecture (fonctionnelle et applicative)",
                    CriticalityLevel = "P2",
                    ProjectRequiresSkill = true,
                    RequiredLevel = 2,
                    FirstLevelDescription =
                        "Dispose d'une connaissance théorique des rapprochements comptables dans une banque.",
                    SecondLevelDescription =
                        "Connait l'architecture applicative des systèmes de rapprochement de CACIB.",
                    ThirdLevelDescription =
                        "Connait les fonctionnalités assurées par les différents composants applicatifs (Intellimatch et EasyMatch) des systèmes de rapprochements.",
                    FourthLevelDescription =
                        "Sait faire le lien entre les fonctionnalités des outils applicatifs et les processus de rapprochement de la Direction Financière de CACIB CN1 & CN2.",
                },
                new Skill
                {
                    SkillId = 14,
                    CategoryId = 1,
                    SubCategoryId = 1,
                    Code = "FCT-REGLEMENTAIRE",
                    Label =
                        "Connaissance Réglementaire et Reportings Financiers : Attributs réglementaires",
                    CriticalityLevel = "P1",
                    ProjectRequiresSkill = false,
                    RequiredLevel = 2,
                    FirstLevelDescription =
                        "Connaissance de l'environnement réglementaire & prudentiel propre aux établissements financiers. Possède des notions théoriques sur les principaux concepts métiers et leur vocabulaire.",
                    SecondLevelDescription =
                        "Connaissance de l'environnement/contexte réglementaire & prudentiel et a une expérience en lien avec cet environnement.",
                    ThirdLevelDescription =
                        "Connaissance de manière très précise l'environnement/contexte réglementaire & prudentiel et a une expérience en lien avec cet environnement.",
                    FourthLevelDescription =
                        "Connaissance avec précision des publications et de l'environnement de contrôle réglementaire prudentiel et peut évoluer en toute autonomie sur le sujet. Peut être considéré expert et mener une réflexion structurante chez le client.",
                },
                new Skill
                {
                    SkillId = 15,
                    CategoryId = 3,
                    SubCategoryId = 10,
                    Code = "ARCADE",
                    Label = "ARCADE",
                    CriticalityLevel = "P2",
                    ProjectRequiresSkill = true,
                    RequiredLevel = 2,
                    FirstLevelDescription =
                        "Connait les informations clés sur l'application (lecture du starter kit, du user guide…)",
                    SecondLevelDescription =
                        "Traite en autonomie partielle : le support sur l'application les évolutions mineures",
                    ThirdLevelDescription =
                        "Traite en autonomie complète : le support L1/L2/L3 sur l'application les évolutions majeures",
                    FourthLevelDescription =
                        "Développe / gère l'amélioration continue de l'application. Est capable de challenger le métier sur les évolutions demandées",
                },
                new Skill
                {
                    SkillId = 16,
                    CategoryId = 3,
                    SubCategoryId = 10,
                    Code = "AVI",
                    Label = "AVI",
                    CriticalityLevel = "P2",
                    ProjectRequiresSkill = true,
                    RequiredLevel = 2,
                    FirstLevelDescription =
                        "Connait les informations clés sur l'application (lecture du starter kit, du user guide…)",
                    SecondLevelDescription =
                        "Traite en autonomie partielle : le support sur l'application les évolutions mineures",
                    ThirdLevelDescription =
                        "Traite en autonomie complète : le support L1/L2/L3 sur l'application les évolutions majeures",
                    FourthLevelDescription =
                        "Développe / gère l'amélioration continue de l'application. Est capable de challenger le métier sur les évolutions demandées",
                },
                new Skill
                {
                    SkillId = 17,
                    CategoryId = 3,
                    SubCategoryId = 11,
                    Code = "BASE EFFECTIF",
                    Label = "BASE EFFECTIF",
                    CriticalityLevel = "P2",
                    ProjectRequiresSkill = true,
                    RequiredLevel = 2,
                    FirstLevelDescription =
                        "Connait les informations clés sur l'application (lecture du starter kit, du user guide…)",
                    SecondLevelDescription =
                        "Traite en autonomie partielle : le support sur l'application les évolutions mineures",
                    ThirdLevelDescription =
                        "Traite en autonomie complète : le support L1/L2/L3 sur l'application les évolutions majeures",
                    FourthLevelDescription =
                        "Développe / gère l'amélioration continue de l'application. Est capable de challenger le métier sur les évolutions demandées",
                },
                new Skill
                {
                    SkillId = 18,
                    CategoryId = 6,
                    SubCategoryId = 33,
                    Code = "DIV : Anglais",
                    Label = "Maitrise de l'Anglais",
                    CriticalityLevel = "P3",
                    ProjectRequiresSkill = false,
                    RequiredLevel = 2,
                    FirstLevelDescription = "Sait rédiger un document en anglais.",
                    SecondLevelDescription = "Comprend tout échange téléphonique en anglais.",
                    ThirdLevelDescription = "Prend la parole en anglais lors d'un comité.",
                    FourthLevelDescription = "Sait animer un comité en anglais.",
                },
                new Skill
                {
                    SkillId = 19,
                    CategoryId = 6,
                    SubCategoryId = 33,
                    Code = "DIV : Communication écrite",
                    Label = "Communication à l'écrit",
                    CriticalityLevel = "P3",
                    ProjectRequiresSkill = false,
                    RequiredLevel = 2,
                    FirstLevelDescription =
                        "Sait rechercher, analyser et traiter l'information / Sait retranscrire de l'information orale (prise de notes).",
                    SecondLevelDescription = "Sait structurer et rédiger un document d'analyse.",
                    ThirdLevelDescription =
                        "Sait rédiger un document de synthèse / Sait adapter le niveau de détails aux équipes et à la complexité du sujet.",
                    FourthLevelDescription = "Sait rédiger un argumentaire.",
                },
                new Skill
                {
                    SkillId = 20,
                    CategoryId = 6,
                    SubCategoryId = 33,
                    Code = "DIV : Communication orale",
                    Label = "Communication à l'oral",
                    CriticalityLevel = "P3",
                    ProjectRequiresSkill = false,
                    RequiredLevel = 2,
                    FirstLevelDescription =
                        "Sait se faire comprendre dans des situations quotidiennes de communication orales.",
                    SecondLevelDescription = "Sait mener un entretien individuel.",
                    ThirdLevelDescription =
                        "Sait prendre la parole au cours d'une réunion / Sait communiquer spontanément avec sa hiérarchie, les contributeurs projets (IT et métiers).",
                    FourthLevelDescription =
                        "Sait faire un exposé oral devant un auditoire / Sait communiquer spontanément avec le sponsor, le top management GIT.",
                },
                new Skill
                {
                    SkillId = 21,
                    CategoryId = 5,
                    SubCategoryId = 33,
                    Code = "PJT : Méthode de gestion de projet",
                    Label = "Connaissance d'une méthodologie de gestion de projet (CMMI…)",
                    CriticalityLevel = "P2",
                    ProjectRequiresSkill = false,
                    RequiredLevel = 2,
                    FirstLevelDescription = "Connaissance théorique sans pratique",
                    SecondLevelDescription =
                        "Formation théorique à la méthodologie, y a contribué mais n'a pas encore conduit de projet avec cette méthodologie",
                    ThirdLevelDescription =
                        "Justifie d'une expérience de conduite de projet avec la méthodologie",
                    FourthLevelDescription =
                        "Justifie de n expériences de conduite de projet avec la méthodologie",
                },
                new Skill
                {
                    SkillId = 22,
                    CategoryId = 5,
                    SubCategoryId = 33,
                    Code = "PJT : Méthodo AGILE",
                    Label = "Maîtrise de la gestion de projet en AGILE",
                    CriticalityLevel = "P2",
                    ProjectRequiresSkill = false,
                    RequiredLevel = 2,
                    FirstLevelDescription = "Notions",
                    SecondLevelDescription = "Connaissances pratiques",
                    ThirdLevelDescription = "Maitrise",
                    FourthLevelDescription = "Expertise",
                },
                new Skill
                {
                    SkillId = 23,
                    CategoryId = 5,
                    SubCategoryId = 33,
                    Code = "PJT : Méthodologie CACIB TAO",
                    Label = "Méthodologie de Gestion de Projets ARPEGIO / TAO",
                    CriticalityLevel = "P2",
                    ProjectRequiresSkill = true,
                    RequiredLevel = 2,
                    FirstLevelDescription =
                        "Supervise un projet à la fois / N'a pas encore obtenu de certification dans le cadre de la Project Academy.",
                    SecondLevelDescription =
                        "Supervise un projet à la fois / A été certifié Chef de projet SILVER dans le cadre de la Project Academy.",
                    ThirdLevelDescription =
                        "Supervise un ou plusieurs projet / A été certifié Chef de projet GOLD dans le cadre de la Project Academy.",
                    FourthLevelDescription =
                        "Supervise plusieurs projets à la fois / A été certifié PROGRAM DIRECTOR dans le cadre de la Project Academy.",
                },
                new Skill
                {
                    SkillId = 24,
                    CategoryId = 2,
                    SubCategoryId = 3,
                    Code = "SI : Chiffrage",
                    Label = "Chiffrage",
                    CriticalityLevel = "P2",
                    ProjectRequiresSkill = false,
                    RequiredLevel = 2,
                    FirstLevelDescription = "Connaissance des fondamentaux de chiffrage",
                    SecondLevelDescription = "Chiffrer des tâches unitaires (<2j/h)",
                    ThirdLevelDescription =
                        "Découper des chantiers (<10j/h) en tâches unitaires et chiffrer ces dernières",
                    FourthLevelDescription =
                        "Découper des projets (taille dépendante du profil de la ressource: experte fonctionnelle ou CP) en chantiers et chiffrer ces derniers",
                },
                new Skill
                {
                    SkillId = 25,
                    CategoryId = 2,
                    SubCategoryId = 3,
                    Code = "SI : Analyse d'impact",
                    Label = "Etude d'impact",
                    CriticalityLevel = "P2",
                    ProjectRequiresSkill = false,
                    RequiredLevel = 2,
                    FirstLevelDescription =
                        "Identifier les impacts de modification sur une tâche allouée",
                    SecondLevelDescription =
                        "Identifier les impacts de modification d'une fonctionnalité simple ou moyennement complexe",
                    ThirdLevelDescription =
                        "Identifier les impacts de modification d'une fonctionnalité complexe ou d'un ensemble de fonctionnalités simples ou moyennement complexes",
                    FourthLevelDescription =
                        "Identifier les impacts de modification d'un ensemble de fonctionnalités de toute complexité",
                },
                new Skill
                {
                    SkillId = 26,
                    CategoryId = 2,
                    SubCategoryId = 3,
                    Code = "SI : Conception générale",
                    Label = "Etude préalable / Conception générale (Architecture, …)",
                    CriticalityLevel = "P2",
                    ProjectRequiresSkill = false,
                    RequiredLevel = 2,
                    FirstLevelDescription = "Notions",
                    SecondLevelDescription = "Connaissances pratiques",
                    ThirdLevelDescription = "Maitrise",
                    FourthLevelDescription = "Expertise",
                },
                new Skill
                {
                    SkillId = 27,
                    CategoryId = 4,
                    SubCategoryId = 17,
                    Code = "TEC : .NET",
                    Label = "Langage de programmation .NET",
                    CriticalityLevel = "P2",
                    ProjectRequiresSkill = false,
                    RequiredLevel = 2,
                    FirstLevelDescription =
                        "Notions. Développe sur la base de Spec Technique détaillée fournies. A besoin d'un accompagnement rapproché",
                    SecondLevelDescription =
                        "Connaissances pratiques. Développe sur la base de Spec Technique détaillée fournies. A besoin d'un accompagnement ponctuel",
                    ThirdLevelDescription = "Maitrise. Développe en toute autonomie",
                    FourthLevelDescription =
                        "Expertise. Développe en toute autonomie. Est reconnu comme référent sur ce langage. Maîtrise les outils de développement associés et leurs options avancées",
                },
                new Skill
                {
                    SkillId = 28,
                    CategoryId = 4,
                    SubCategoryId = 18,
                    Code = "TEC : AFT",
                    Label = "AFT",
                    CriticalityLevel = "P2",
                    ProjectRequiresSkill = false,
                    RequiredLevel = 2,
                    FirstLevelDescription =
                        "Notions. Développe sur la base de Spec Technique détaillée fournies. A besoin d'un accompagnement rapproché",
                    SecondLevelDescription =
                        "Connaissances pratiques. Développe sur la base de Spec Technique détaillée fournies. A besoin d'un accompagnement ponctuel",
                    ThirdLevelDescription = "Maitrise. Développe en toute autonomie",
                    FourthLevelDescription =
                        "Expertise. Développe en toute autonomie. Est reconnu comme référent sur ce langage. Maîtrise les outils de développement associés et leurs options avancées",
                },
                new Skill
                {
                    SkillId = 29,
                    CategoryId = 4,
                    SubCategoryId = 19,
                    Code = "TEC : Ansible",
                    Label = "Ansible",
                    CriticalityLevel = "P2",
                    ProjectRequiresSkill = false,
                    RequiredLevel = 2,
                    FirstLevelDescription =
                        "Notions. Développe sur la base de Spec Technique détaillée fournies. A besoin d'un accompagnement rapproché",
                    SecondLevelDescription =
                        "Connaissances pratiques. Développe sur la base de Spec Technique détaillée fournies. A besoin d'un accompagnement ponctuel",
                    ThirdLevelDescription = "Maitrise. Développe en toute autonomie",
                    FourthLevelDescription =
                        "Expertise. Développe en toute autonomie. Est reconnu comme référent sur ce langage. Maîtrise les outils de développement associés et leurs options avancées",
                }
            );
        }
    }
}
