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
                }
            );
        }
    }
}
