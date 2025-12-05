🌟 Employee Performance & Attendance Tracking API
ASP.NET Core • JWT Auth • Identity • SQL Server • EF Core

A modern, secure and scalable ASP.NET Core Web API for managing Employees, Attendance, Performance, Departments & Leave Workflow.
Built using clean architecture principles & industry-best practices.

-------🚀 Tech Stack--------
Technology	                          Usage
ASP.NET Core 8 Web API	            Backend Framework
Entity Framework Core	                Database ORM
SQL Server	                            Database
ASP.NET Identity	               User & Role Management
JWT Authentication	          Secure Login & Role-based Access
Repository+Service Pattern	    Clean Architecture
AutoMapper	                          DTO Mapping

**✨ Key Features**
**🔐 Authentication & Security**

JWT Token-based Authentication

Role-based Authorization (Admin, Manager, Employee)

Password Hashing using Identity

**👥 Employee Management**

**Create / Update / Delete Employees**

Identity Integrated User Accounts

Auto Assign Role on Creation

Manager + Admin Access Controls

**🕒 Attendance Tracking**

Punch IN / OUT

Prevent duplicate day entries

User-wise attendance history

**📊 Performance Module**

Add / Review Employee Performance

Ratings + Comments by Manager

Performance history tracking

**🗂 Department Management**

Add Departments

Assign employees to departments

**📝 Leave System**

Apply for leave

Manager approval workflow

Track leave status

**🧱 Project Architecture**

📦 EmployeePerformance_AttendanceTracking
│
├── 📁 Controllers/
├── 📁 Data/
│     ├── ApplicationDbContext.cs
│     ├── DbSeed.cs
│
├── 📁 DTOs/
├── 📁 Models/
├── 📁 Service/
│     ├── Interfaces/
│     └── Implementations/
│
├── 📁 Migrations/
├── Program.cs
└── appsettings.json

**📡 Sample API Endpoints
🔐 Authentication**
POST /api/auth/register
POST /api/auth/login

**👤 Employees**
GET    /api/employee/GetAll
GET    /api/employee/GetById{id}
POST   /api/employee/create
PUT    /api/employee/Update/{id}
DELETE /api/employee/Delete/{id}

**🕒 Attendance**
Attendance
GET   /api/Attendance/GetAll
GET   /api/Attendance/ById/{id}
POST  /api/Attendance/Create
PUT   /api/Attendance/Update/{id}
DELETE /api/Attendance/Delete/{id}

**📝 Leave**
GET /api/leave/GetAll
GET /api/leave/GetById{id}
POST /api/leave/Create
PUT /api/leave/Approve{id}
PUT  /api/leave/Reject/{id}
PUT /api/leave/UPDATE{id}
DELETE /api/leave/Delete{id}

**Performance**
GET /api/Performance/GetAll
GET /api/Performance/GetBy/{id}
POST /api/Performance/Create
PUT /api/Performance/Update/{id}
DELETE  /api/Performance/DELETEB/{id}

**Departments**
GET /api/Department/GetAll
GET /api/Department/GetBy/{id}
POST /api/Department/Create
PUT /api/Department/Update/{id}
DELETE  /api/Department/DELETEB/{id}
