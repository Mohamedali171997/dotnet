# Running the Ges-sco Application on Windows with Visual Studio

This guide provides detailed instructions on how to set up and run the Ges-sco backend (ASP.NET Core Web API) using Visual Studio and the frontend (Angular) using Node.js and Angular CLI on a Windows machine.

## Prerequisites

Before you begin, ensure you have the following installed on your system:

1.  **Visual Studio 2022 (Community, Professional, or Enterprise):**
    *   Make sure to install the "ASP.NET and web development" workload.
    *   Download from: [https://visualstudio.microsoft.com/downloads/](https://visualstudio.microsoft.com/downloads/)

2.  **.NET 8.0 SDK:**
    *   This is typically included with Visual Studio 2022 if you selected the web development workload. You can verify or install it separately.
    *   Download from: [https://dotnet.microsoft.com/download/dotnet/8.0](https://dotnet.microsoft.com/download/dotnet/8.0)

3.  **Node.js and npm:**
    *   Node.js (LTS version recommended) comes with npm (Node Package Manager).
    *   Download from: [https://nodejs.org/en/download/](https://nodejs.org/en/download/)

4.  **Angular CLI:**
    *   Once Node.js and npm are installed, open a command prompt or PowerShell and install Angular CLI globally:
        ```bash
        npm install -g @angular/cli
        ```

5.  **Git:**
    *   To clone the repository.
    *   Download from: [https://git-scm.com/download/win](https://git-scm.com/download/win)

## Setup Instructions

### 1. Clone the Repository

Open Git Bash, Command Prompt, or PowerShell and clone the project:

```bash
git clone <repository_url>
cd <project_directory> # e.g., cd dotnet
```
*(Replace `<repository_url>` with the actual URL of your Git repository and `<project_directory>` with the name of the folder created by cloning)*

### 2. Run the Backend (ASP.NET Core Web API)

The backend is an ASP.NET Core 8.0 Web API project.

1.  **Open in Visual Studio:**
    *   Navigate to the `backend/` folder in the cloned repository.
    *   Open the `Ges-sco.sln` solution file in Visual Studio 2022.

2.  **Restore NuGet Packages:**
    *   Visual Studio should automatically restore the necessary NuGet packages. If not, right-click on the `Ges-sco.API` project in the Solution Explorer and select "Restore NuGet Packages".

3.  **Run the Backend Application:**
    *   In Visual Studio, ensure `Ges-sco.API` is set as the startup project (right-click on the project -> "Set as Startup Project").
    *   Press `F5` or click the "IIS Express" (or "Ges-sco.API") button in the toolbar to run the application.
    *   A browser window should open, typically showing the Swagger UI (API documentation) at `http://localhost:5106/swagger` (or a similar port).
    *   Keep this Visual Studio instance running to keep the backend active.

### 3. Run the Frontend (Angular)

The frontend is an Angular 17+ application.

1.  **Open a New Terminal:**
    *   Open a new Command Prompt or PowerShell window, separate from Visual Studio.

2.  **Navigate to the Frontend Directory:**
    *   Change your current directory to the Angular project folder:
        ```bash
        cd <project_directory>/frontend/ges-sco-angular
        ```
        *(Replace `<project_directory>` with your project's root folder name, e.g., `cd dotnet/frontend/ges-sco-angular`)*

3.  **Install Node.js Dependencies:**
    *   Install all required npm packages:
        ```bash
        npm install
        ```

4.  **Start the Angular Development Server:**
    *   Run the Angular application in development mode:
        ```bash
        ng serve --open
        ```
    *   The `--open` flag will automatically open your default browser to `http://localhost:4200/`.

### 4. Access the Full Application

*   Once both the backend (Swagger UI at `http://localhost:5106/swagger`) and the frontend (Angular app at `http://localhost:4200/`) are running, you can interact with the complete application through the frontend at `http://localhost:4200/`.
*   The frontend will communicate with the backend API running on `http://localhost:5106`.

### Troubleshooting

*   **"Address already in use" error (Backend):** If you encounter this, another program is using port `5106`.
    *   You can try to find and stop the process using Task Manager or `netstat -ano | findstr :5106` in Command Prompt.
    *   Alternatively, change the port in `backend/Ges-sco.API/Properties/launchSettings.json`.
*   **"Cannot connect to API" (Frontend):** Ensure the backend is running and accessible. Check the browser's developer console for network errors. Verify the `environment.ts` in the Angular project has the correct backend API URL.
*   **Compilation Errors:** Ensure all prerequisites are installed correctly and their versions are compatible. Perform `npm install` again for the frontend.