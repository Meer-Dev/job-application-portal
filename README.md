🎯 Job Application Portal (Full-Stack)

SCREENSHOT ; (https://ibb.co/hJM54P2t)

A comprehensive job application management system built with ASP.NET Core, Angular, Entity Framework Core, SQL Server, and Hangfire, following ABP (ASP.NET Boilerplate) architecture.

✨ Features

📝 Job Position Management – CRUD with search + pagination

👨‍💼 Candidate Management – Full lifecycle + resume upload

📂 File Upload System – Secure uploads with validation (.pdf, .docx, .jpg, .png)

🔐 Role-Based Permissions – Fine-grained access control

⚙️ Background Jobs – Automated job monitoring via Hangfire

🏢 Multi-Tenant Architecture – Supports multiple organizations

💻 Modern UI – Responsive with Bootstrap & ngx-bootstrap modals

🔔 Real-time Notifications – SignalR for live updates

🛠 Technology Stack
Backend

🚀 ASP.NET Core 6.0 – Web API

📦 Entity Framework Core – ORM

🏗 ABP Framework – Enterprise architecture

🗄 SQL Server LocalDB – Database engine

⏳ Hangfire – Background job processing

📖 Swagger/OpenAPI – API docs

Frontend

🌐 Angular 12 – Frontend framework

🟦 TypeScript – Strongly typed JS

🎨 Bootstrap 4 – UI styling

⚡ ngx-bootstrap – Angular Bootstrap components

🔄 RxJS – Reactive programming

🤖 NSwag – Auto API client generation

📂 Project Structure
D:\7.3.0\
├── aspnet-core\               # Backend solution
│   ├── src\
│   │   ├── solvefy.task.Core\             # Domain layer
│   │   ├── solvefy.task.Application\      # Application services
│   │   ├── solvefy.task.EntityFrameworkCore\ # EF + DB access
│   │   ├── solvefy.task.Web.Core\         # Web core
│   │   └── solvefy.task.Web.Host\         # Web API host
│   └── test\                              # Unit tests
└── angular\                  # Frontend app
    └── src\
        ├── app\              # Components
        ├── shared\           # Shared modules
        └── assets\           # Static assets

⚡ Quick Start
🔧 Backend Setup
# Go to backend directory
cd D:\7.3.0\aspnet-core

# Go to EF project
cd src\solvefy.task.EntityFrameworkCore

# Create & update DB
dotnet ef migrations add "Initial_Migration" --startup-project ..\solvefy.task.Web.Host\
dotnet ef database update --startup-project ..\solvefy.task.Web.Host\


👉 Then:

Open solvefy.task.sln in Visual Studio 2022

Set solvefy.task.Web.Host as startup

Run ▶ (F5) → Backend runs at http://localhost:5000

Check API at http://localhost:5000/swagger

🎨 Frontend Setup
# Go to Angular project
cd D:\7.3.0\angular

# Install dependencies
npm install --legacy-peer-deps

# Generate API clients
npm run nswag

# Run dev server
npm start


👉 App runs at: http://localhost:4200

🔑 Default Login

🏢 Tenant: Default

👤 Username: admin

🔑 Password: 123qwe

🌐 Application URLs

🎨 Frontend: http://localhost:4200

🛠 Backend API: http://localhost:5000

📖 Swagger Docs: http://localhost:5000/swagger

📊 Hangfire Dashboard: http://localhost:5000/hangfire

🧩 Core Features
📌 Job Position Management

CRUD with search + filters

Active/inactive toggle

Auto time tracking

👨‍💼 Candidate Management

Full profile management

Resume upload + validation

Link to job positions

📂 File Upload System

PDF/DOCX/JPG/PNG support

File size validation

Resume download

🔐 Permissions

Pages.JobPositions – Manage jobs

Pages.Candidates – Manage candidates

Role-based menus

⚙️ Background Jobs

Daily monitoring of job apps

Auto warnings for low apps

Hangfire dashboard

⚙️ Configuration
📌 Database Connection (appsettings.json)
"ConnectionStrings": {
  "Default": "Server=(localdb)\\MSSQLLocalDB;Database=taskDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}

🌍 CORS Setup (Startup.cs)
"App": {
  "CorsOrigins": "http://localhost:4200,http://localhost:8080"
}

🚀 Development Workflow

Create entities → Core layer

Add services → Application layer

Update DB → Migrations

Regenerate clients → npm run nswag

Add Angular components/services

🛑 Troubleshooting

❌ NSwag fails?

npm run nswag


❌ Build errors?

rm -rf node_modules package-lock.json
npm install --legacy-peer-deps


❌ DB issues?

dotnet ef database drop --startup-project ..\solvefy.task.Web.Host\
dotnet ef database update --startup-project ..\solvefy.task.Web.Host\

🏛 Architecture Notes

📐 Domain Driven Design (DDD)

📦 Repository Pattern

🛠 Application Services

📤 DTOs for data transfer

🧩 Dependency Injection

🔄 Unit of Work

📜 License

MIT License – free for personal & commercial use.

💬 Support

✅ Check Troubleshooting



✅ Inspect Swagger endpoints

✅ Use browser DevTools

✅ Ensure DB migrations applied
