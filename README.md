# FitTrack Gym App 🏋️‍♂️💪

A comprehensive, beginner-friendly Gym Management System built with **.NET 8 MAUI** and **C#**, using **SQLite & Entity Framework Core** for a robust local database.

This project was built step-by-step to track members, manage membership plans, monitor payments, log daily attendance, discover exercises via public API, and even generate custom AI workout plans! 🚀

---

## ✨ Features
1. **📊 Interactive Dashboard**: See live statistics for Total Members, Active Members, Pending Payments, and Today's Check-ins.
2. **👥 Member Management**: Full CRUD (Create, Read, Update, Delete) functionality to manage your gym's clients.
3. **💳 Membership Plans**: Create customizable plans (e.g., "Monthly Pro", "Annual Standard") with pricing and durations.
4. **💰 Payment Tracking**: Log payments, track next due dates, and monitor who is "Paid", "Pending", or "Overdue".
5. **✅ Attendance Logging**: A quick check-in system that logs the exact time a member enters the gym, with duplicate check-in prevention.
6. **🔍 Exercise Discovery**: Integrated with the **API-Ninjas Exercise API** to dynamically search for exercises by muscle group and fetch instructions over the internet.
7. **🤖 AI Workout Coach**: A custom AI generator! Tell it your goal, age, and experience, and it will build a tailored workout plan for you.

---

## 🛠️ Tech Stack
- **Framework**: .NET MAUI (.NET 8.0)
- **Language**: C#
- **UI Markup**: XAML (with MVVM Architecture)
- **Database**: SQLite (Local Database)
- **ORM**: Entity Framework Core 8.0
- **External Services**: HttpClient, JSON Deserialization, API-Ninjas, Google Gemini AI

---

## 🚀 Setup Instructions

### Prerequisites
1. Install [Visual Studio 2022](https://visualstudio.microsoft.com/) or Visual Studio Code.
2. Ensure you have the **.NET Multi-platform App UI development** workload installed.
3. Ensure you have the **.NET 8.0 SDK** installed.

### How to Build & Run
1. Clone this repository:
   ```bash
   git clone https://github.com/HANSAJA122/FitTrackGymApp.git
   ```
2. Open the project in your IDE.
3. Select **Android** or **Windows** or **Mac Catalyst** as your target emulator/device.
4. Hit **Run**! Entity Framework is configured to automatically create the local SQLite database file on your device the very first time you launch it.

---

## 🔑 How to Add API Keys (Optional but Recommended)

For the viva or testing, the app has built-in safe fallbacks, meaning it will perfectly function without API keys. However, for the full experience, add the keys!

### 1. Exercise API (API-Ninjas)
1. Go to [api-ninjas.com](https://api-ninjas.com/) and create a free account.
2. Copy your API Key.
3. Open `Services/ExerciseApiService.cs`.
4. Paste your key into the `ApiKey` variable:
   ```csharp
   private const string ApiKey = "YOUR_KEY_HERE";
   ```

### 2. AI Workout Generator (Google Gemini)
1. Go to [Google AI Studio](https://aistudio.google.com/) and create a free Gemini API Key.
2. Open `Services/AiWorkoutService.cs`.
3. Paste your key into the `ApiKey` variable:
   ```csharp
   private const string ApiKey = "YOUR_KEY_HERE";
   ```
   *Note: If you leave this blank, the app will automatically generate a simulated fallback plan locally, so it never crashes during a live demo!*

---

## 👨‍💻 Developed By
Shashith Hansaja 
