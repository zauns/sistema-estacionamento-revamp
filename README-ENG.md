# parking-system-revamp
## Advanced Parking System - Full Stack .NET
This repository documents the development journey of a complete Parking Management System, built with the latest technologies from the .NET ecosystem. The project started as a simple console application and evolved into a robust and modern solution, following a clean and scalable software architecture.

More than a finished project, this is a learning diary, highlighting the challenges faced and solutions found along the way.

### 🚀 Project Vision
The main objective was to transform a basic application into a professional-level system, covering:

- Robust Backend: A RESTful API to manage all business logic.

- Native Desktop Client: An administrative WPF application for daily operations.

- Modern Web Dashboard: A web interface in Blazor for statistics visualization and reports.

- Clean Architecture: Clear separation of responsibilities between Domain, Infrastructure, API, and Presentation layers.

### 🛠️ Technology Stack
- Backend: .NET 9, ASP.NET Core Web API, Entity Framework Core, SignalR

- Database: SQLite (for development)

- Desktop Frontend: WPF, MVVM Pattern with CommunityToolkit.Mvvm

- Web Frontend: Blazor Server, MudBlazor, ChartJs.Blazor

- Reports: QuestPDF, ClosedXML

### 🌱 Learning Journey and Challenges
Each phase of the project represented a new layer of learning and a unique set of challenges.

#### Phase 1 & 2: The Foundation with API and EF Core
The transition from a console project to a multi-project architecture was the first major step.

- Learning Goals:

  - Understand the structure of a .NET Solution with multiple projects (Class Library, Web API).

  - Implement Entity Framework Core for object-relational mapping.

  - Build RESTful endpoints and separate business logic into a service layer.

- Challenges Overcome:

  - Database Configuration: The initial choice of LocalDB proved challenging when using VS Code. Migration to SQLite drastically simplified the development environment.

  - Refactoring to Service Layer: The initial logic in TestController worked but was hard to maintain. Refactoring to a pattern with IParkingService and ParkingService was a crucial exercise in software design and dependency injection.

#### Phase 3: Diving into the Desktop World with WPF
This was the first foray into GUI development with WPF, representing the steepest learning curve.

- Learning Goals:

  - Understand the MVVM (Model-View-ViewModel) pattern.

  - Use data binding to connect the UI (View) to the logic (ViewModel).

  - Implement commands and manage UI state with CommunityToolkit.Mvvm.

- Challenges Overcome:

  - Initialization Errors: Encountering errors like InitializeComponent() doesn't exist and StaticResource not found was a major challenge. The solution involved understanding the importance of synchronization between .xaml and .xaml.cs files, especially after moving them to folders like Views.

  - API Communication: The application not loading "silently" was a frustrating problem, solved by removing the StartupUri from App.xaml and adding global exception handling to get clear error messages.

#### Phase 4 & 5: The Web with Blazor and Advanced Features
Building the web dashboard brought new challenges, mainly related to client-server communication in the web environment.

- Learning Goals:

  - Structure a Blazor Server application and use component libraries like MudBlazor.

  - Implement real-time notifications with SignalR.

  - Generate files dynamically (PDF/Excel) from the API.

- Challenges Overcome:

  - CORS and HTTPS: The most persistent challenge was making the Blazor application (HTTPS) communicate with the API (initially on HTTP). The solution required a deep understanding of CORS, development certificates (dotnet dev-certs), and explicit configuration of listening protocols (Kestrel) in the API.

  - Library Licensing: The QuestPDF exception was a surprise, but served as a great reminder of the importance of reading documentation and understanding licensing models of open-source packages.

### ✅ Project Progress
#### This checklist reflects the current state of development compared to the original roadmap.

- [x] Phase 1: Base Structure

- [x] Phase 2: REST API

- [x] Phase 3: WPF Desktop Interface

- [x] Phase 4: Blazor Web Dashboard

- [x] Phase 5: Advanced Features

  - [x] Reporting System

  - [x] Data Export (PDF/Excel)

  - [ ] Real-time Notifications (SignalR)

  - [ ] Cache and Performance

- [ ] Phase 6: Testing and Quality

  - [ ] Unit Tests

  - [ ] Integration Tests

- [ ] Phase 7: Deploy and DevOps

  - [ ] Containerization with Docker

  - [ ] CI/CD with GitHub Actions