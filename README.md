# 🧠 Skill Management System  
### Role-Based Skill Tracking App (Admin / Leader / User)

A full-stack application for managing and filtering user skills based on expertise levels.  
Built with **ASP.NET Core API**, **Entity Framework Core**, and **Razor Pages**.

---

## 🚀 Overview

This application allows users to record and maintain their professional and technical skills, while leaders and admins can search, filter, and manage teams based on these skills.

### 🎯 Objectives
- Centralize all user skills in one platform.  
- Enable leaders to find users by skill or skill level.  
- Allow users to update their own competencies.  
- Provide admins with full control over users, roles, and skills.

---

## 👥 Roles and Permissions

| Role | Description | Permissions |
|------|--------------|--------------|
| **Admin** | Manages users, roles, and all skills. | Create/update/delete users & skills, import data from Excel. |
| **Manager** | Manages users, roles, and all skills. | Create/update users & skills, import data from Excel. |
| **TeamLeader** | Views and filters users based on skills. | View and search users by skill and skill level. |
| **User** | Manages their own skills and levels. | Add, update, or delete their own skills. |
---

## 🧩 Core Features

✅ Skill CRUD operations (Create, Read, Update, Delete)  
✅ User skill tracking and level updates  
✅ Role-based access control (Admin, Leader, User)  
✅ Skill-based filtering and search  
✅ Excel import/export for Admin  
✅ Many-to-many relationship between Users and Skills  
✅ Authentication & Authorization using Windows Authentication 
✅ RESTful API + Angular Frontend Integration  

---

## 🏗️ Tech Stack

### Backend (API)
- **ASP.NET Core 8 Web API**
- **Entity Framework Core** (Code-First)
- **SQL Server**
- **Authentication: Windows Authentication (Active Directory)**
- **Dependency Injection: Built-in ASP.NET Core DI**
- **EPPlus** (Excel import/export)

### Frontend
## Frontend

The frontend of the Skill Manager application is built using **Razor Pages** with **Bootstrap 5** for responsive layouts and **FontAwesome / Bootstrap Icons** for UI elements.

### Key Features

- **Pages/Teams**
  - Create, edit, and delete teams
  - Assign team leads and members
  - Link teams to projects
  - Display team member count with badges
  - Show success/error alerts for operations
  - **Role & Project Restrictions:** 
    - Only users with the **Manager** role can create, update, or delete teams
    - Users can only manage teams within **their assigned projects**

- **Pages/Projects**
  - Create, edit, and delete projects
  - Assign teams to projects
  - Display project details in tables
  - **Role Restrictions:** Only administrators or authorized users can manage projects

- **Pages/Users**
  - Manage users and roles
  - Assign and approve skills
  - Track Subject Matter Experts (SMEs)
  - View user summaries and status
  - **Project Restrictions:** Users can only assign skills or manage users within their project scope

### UI Elements

- **Forms**
  - Bootstrap forms with client-side validation
  - Dropdown selects for team leads, users, and projects
  - Text inputs and textareas for names, descriptions, and skills
  - Inputs dynamically disabled if the current user does not have the required role or project access

- **Tables**
  - Responsive tables with Bootstrap styling
  - Actions include edit, delete, and assign
  - Highlighting and badges for team member counts
  - Action buttons hidden or disabled for unauthorized users

- **Icons**
  - FontAwesome or Bootstrap Icons used for buttons (save, edit, delete, back)
  - Alerts and status indicators

### Scripts

- `_ValidationScriptsPartial.cshtml` for client-side validation  
- Custom JS for:
  - Clearing edit mode forms
  - Populating select elements in edit forms
  - Confirmation dialogs for deletions
  - Conditional display of action buttons based on role/project

### Styling

- Uses **Bootstrap 5** for responsive layouts  
- Custom CSS in `wwwroot/css` for branding and overrides  
- Icons from **FontAwesome** and **Bootstrap Icons**


## 🗄️ Database Design

### Entities

#### `ApplicationSkill`
- Represents a skill in the system linked to a **category and level**.  
- **Fields:** `ApplicationSkillId`, `SkillName`, `CategoryId`, `LevelId`  
- **Relationships:**  
  - One-to-many with `UserSkill` (users possessing this skill)  
  - Many-to-one with `Category`  

#### `ApplicationSuite`
- Represents a suite of applications/tools.  
- **Fields:** `ApplicationSuiteId`, `SuiteName`, `Description`  

#### `Category`
- Groups skills into broader categories.  
- **Fields:** `CategoryId`, `CategoryName`, `CategoryTypeId`  
- **Relationships:**  
  - One-to-many with `SubCategory`  
  - One-to-many with `ApplicationSkill`  

#### `CategoryType`
- Represents the type of category (e.g., Technical, Soft Skill).  
- **Fields:** `CategoryTypeId`, `TypeName`  
- **Relationships:**  
  - One-to-many with `Category`  

#### `Level`
- Represents proficiency level (Beginner, Intermediate, Expert).  
- **Fields:** `LevelId`, `LevelName`, `Description`  
- **Relationships:**  
  - One-to-many with `ApplicationSkill`  

#### `Project`
- Represents a project in the organization.  
- **Fields:** `ProjectId`, `ProjectName`, `Description`  
- **Relationships:**  
  - Many-to-many with `Team` via `ProjectTeam`  

#### `ProjectTeam`
- Join entity representing a team assigned to a project.  
- **Fields:** `TeamId`, `ProjectId`  

#### `Skill`
- General skill entity that can be assigned to users.  
- **Fields:** `SkillId`, `SkillName`, `Description`  
- **Relationships:**  
  - Many-to-many with `User` via `UserSkill`  

#### `SubCategory`
- Subdivision of a category.  
- **Fields:** `SubCategoryId`, `SubCategoryName`, `CategoryId`  

#### `Team`
- Represents a team of users.  
- **Fields:** `TeamId`, `TeamName`, `TeamDescription`, `TeamLeadId`  
- **Relationships:**  
  - One-to-many with `User`  
  - Many-to-many with `Project` via `ProjectTeam`  

#### `User`
- Represents a system user.  
- **Fields:** `UserId`, `FirstName`, `LastName`, `Eid`, `TeamId`, `UserStatusId`  
- **Relationships:**  
  - Many-to-one with `Team`  
  - Many-to-many with `Skill` via `UserSkill`  
  - One-to-one with `UserSme`  

#### `UserRole`
- Represents roles assigned to users (Manager, Team Lead, etc.).  
- **Fields:** `UserRoleId`, `RoleName`  

#### `UserSkill`
- Join entity linking a user to a skill.  
- **Fields:** `UserId`, `SkillId`, `LevelId`, `ApprovalStatus`  

#### `UserSme`
- Indicates if a user is a Subject Matter Expert.  
- **Fields:** `UserSmeId`, `UserId`, `CategoryId`  

#### `UserStatus`
- Indicates status of the user (Active, Inactive, On Leave).  
- **Fields:** `UserStatusId`, `StatusName`  

#### `UserSummary`
- Aggregated information about a user’s skills and assignments.  
- **Fields:** `UserSummaryId`, `UserId`, `TotalSkills`, `ApprovedSkills`, `PendingSkills`  


### Relationship

User 1 — * UserSkill * — 1 Skill


### Skill Levels
| Level | Description |
|--------|--------------|
| 1 | Notion |
| 2 | Pratique |
| 3 | Maîtrise |
| 4 | Expert |

---

## 🧱 Project Architecture
# Windows Authentication

The application supports **Windows Authentication**, allowing users to login with their Active Directory accounts.  

## Folder Structure

SkillManager/
│
├─ SkillManager.Application/
│ ├─ Dependencies/
│ │ └─ DependencyInjection.cs
│ ├─ DTOs/
│ │ ├─ Category/
│ │ ├─ Level/
│ │ ├─ Project/
│ │ ├─ Skill/
│ │ ├─ SubCategory/
│ │ ├─ Team/
│ │ └─ User/
│ ├─ Exceptions/
│ ├─ Interfaces/
│ │ ├─ Repositories/
│ │ └─ Services/
│ ├─ Mappers/
│ ├─ Models/
│ └─ Validators/
│
├─ SkillManager.Application.UnitTest/
│ ├─ Dependencies/
│ └─ Tests/
│ └─ GlobalUsing.cs
│
├─ SkillManager.Domain/
│ ├─ Dependencies/
│ └─ Entities/
│ └─ DependencyInjection.cs
│
├─ SkillManager.Infrastructure/
│ ├─ Dependencies/
│ ├─ Identity/
│ ├─ Migrations/
│ └─ Repositories/
│ └─ DependencyInjection.cs
│
└─ SkillManager.Web/
├─ Connected Services/
├─ Dependencies/
├─ Properties/
├─ wwwroot/
└─ Pages/
