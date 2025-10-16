# 🧠 Skill Management System  
### Role-Based Skill Tracking App (Admin / Leader / User)

A full-stack application for managing and filtering user skills based on expertise levels.  
Built with **ASP.NET Core API**, **Entity Framework Core**, and **Angular**.

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
| **Leader** | Views and filters users based on skills. | View and search users by skill and skill level. |
| **User** | Manages their own skills and levels. | Add, update, or delete their own skills. |

---

## 🧩 Core Features

✅ Skill CRUD operations (Create, Read, Update, Delete)  
✅ User skill tracking and level updates  
✅ Role-based access control (Admin, Leader, User)  
✅ Skill-based filtering and search  
✅ Excel import/export for Admin  
✅ Many-to-many relationship between Users and Skills  
✅ Authentication & Authorization using JWT  
✅ RESTful API + Angular Frontend Integration  

---

## 🏗️ Tech Stack

### Backend (API)
- **ASP.NET Core 8 Web API**
- **Entity Framework Core** (Code-First)
- **SQL Server**
- **JWT Authentication**
- **EPPlus** (Excel import/export)

### Frontend
- **Angular 17+**
- **Angular Material** for UI components
- **RxJS**, **HttpClient**
- **TypeScript**

---

## 🗄️ Database Design

### Tables
- **User** → represents application users  
- **Skill** → master list of all skills (functional, technical, etc.)  
- **UserSkill** → bridge table linking users and skills (many-to-many)

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

SkillManager/
├── backend/
│ ├── Controllers/
│ │ ├── UsersController.cs
│ │ ├── SkillsController.cs
│ │ ├── ImportController.cs
│ ├── Models/
│ │ ├── User.cs
│ │ ├── Skill.cs
│ │ ├── UserSkill.cs
│ ├── Services/
│ │ ├── IUserSkillService.cs
│ │ ├── UserSkillService.cs
│ ├── Data/
│ │ └── AppDbContext.cs
│ ├── DTOs/
│ ├── Program.cs
│ └── README.md
│
├── frontend/
│ ├── src/app/
│ │ ├── services/
│ │ │ ├── users.service.ts
│ │ │ ├── skills.service.ts
│ │ │ ├── auth.service.ts
│ │ ├── components/
│ │ │ ├── leader-dashboard/
│ │ │ ├── user-profile/
│ │ │ ├── admin-skill-manager/
│ └── README.md
│
└── README.md

```bash
git clone https://github.com/girishj12/SkillManager.git
cd SkillManagement


🔒 Authentication & Roles

JWT-based authentication (/api/auth/login)

Admin creates users and assigns roles.

Role-based access with [Authorize(Roles = "...")] attributes.
