# dodgegame
A fast-paced, 2D multiplayer arena shooter where players battle to be the last one standing. Games can range from 2-4 players. The game features a unique "ricochet" combat system where projectiles bounce off environment walls, turning the map into an increasingly chaotic bullet hell.

![Project Status](https://img.shields.io/badge/Status-Alpha-red) ![Unity Version](https://img.shields.io/badge/Unity-6000.0.61f1-blue) ![Net Core](https://img.shields.io/badge/.NET-9.0-purple)

## Key Features
* **Dedicated Client-Server Architecture:** All critical game logic (movement validation, bullet collisions, room management) is authoritative and handled on the server.
* **Lobby & Room System:** Players can host public game rooms and see real-time updates in the lobby.
* **Secure Authentication:** Uses a "Token" login system requiring web app authentication to access the game.
* **Bidirectional Networking:** Serverbound (requests) and Clientbound (state updates) packets.

## Architecture
The project is split into three major components:
1.  **Unity Client (Frontend):** The main game interface and renderer.
2.  **C# Server (Backend):** Handles networking, gameplay, and user token validation.
3.  **Web Portal (Website):** Handles user account creation and authentication via Supabase.


## Installation & Setup

### Prerequisites
* **Unity Hub** & **Unity Editor** version `6000.0.61f1` (Silicon LTS for Mac).
* **Dotnet SDK 9.0** or **Runtime 9.0**.
* **Node.js & npm** (for the website).
* **Git** installed on your terminal.

### 1. Unity Client (The Game)
1.  Clone the repository:
    ```bash
    git clone https://github.com/KRazo777/dodgegame.git
    ```
2.  Open **Unity Hub** -> **Add Project from Disk** -> Select the `dodgegame` folder.
3.  Ensure the Unity version matches `6000.0.61f1`.
4.  Open the project. You can run it directly in the editor using the **Play** button.
    * *Note:* WebGL builds are unsupported due to UDP socket usage. Windows and Linux builds are tested and working.

### 2. Backend Server
1.  Navigate to the repository folder in your terminal.
2.  Switch to the backend branch:
    ```bash
    git checkout backend
    ```
3.  Build the server:
    ```bash
    dotnet build
    ```
4.  Run the server:
    ```bash
    dotnet DodgeGame.Server.dll
    ```
    * *Note:* If hosting externally, update the IP/Port in `Assets/Scripts/Networking/ConnectionHandler.cs` in the Unity Client.

### 3. Web Portal (Authentication)
1.  Clone the web portal repository, you can also find the repo [here](https://github.com/taahs-backlog/dodgegame-web-portal)
:
    ```bash
    git clone https://github.com/taahs-backlog/dodgegame-web-portal.git
    ```
2.  Install dependencies:
    ```bash
    npm i
    ```
3.  **Environment Setup:** You must create a (free) Supabase project for authentication. Create an `.env` file with your Supabase credentials. Once supabase is set up, you can find the layout for the .env file [here](https://github.com/taahs-backlog/dodgegame-web-portal/blob/main/README.md). The rest of the instructions for the deployment of the website are on the link as well.

4.  Run the development server:
    ```bash
    npm run dev
    ```
Open http://localhost:3000 with your browser to see the result.
You can start editing the page by modifying app/page.tsx. The page auto-updates as you edit the file.

## Testing
The project employs both automated Unit Tests and manual Integration Tests.

**How to run Unit Tests:**
1.  Open the project in Unity.
2.  Navigate to **Window** > **General** > **Test Runner**.
3.  Select **EditMode** tab.
4.  Click **Run All**.

---
