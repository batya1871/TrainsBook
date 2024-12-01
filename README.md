# TrainsBook: Train and Route Management System

## Overview
TrainsBook is a desktop application developed with WPF (C#) for managing train and route data. The project provides functionalities for viewing, sorting, and maintaining data about trains, routes, and users, while ensuring user-friendly interaction and data persistence.

---

## Features
- **Train Management:**
  - Add, view, and sort trains based on conditions such as technical state and route status.
  - Maintain descriptive and technical details for each train.

- **Route Management:**
  - Create and manage route information, including assignment of trains to routes.
  - Archive completed routes for reference.

- **User Management:**
  - Supports multiple user roles: Administrator, Head, and Regular User.
  - Tracks active users and ensures authentication and role-based access control.

- **Data Persistence:**
  - Save and load train, route, and event data to/from JSON files.
  - Store user credentials and manage active sessions.

- **Notifications:**
  - Display real-time notifications about train conditions or system events.

---

## Technical Details
1. **Core Functionality:**
   - Centralized data handling through the `Container` class for managing shared data (trains, routes, events, and window states).
   - Data serialization and deserialization handled by `FileIOService` for persistence between sessions.

2. **Key Models:**
   - **`Train`:** Encapsulates train properties and behaviors, such as determining route status and cloning.
   - **`Route`:** Represents train routes and their attributes (implementation inferred but integrates seamlessly with trains).
   - **`User`:** Manages user details, authentication, and role-based logic.

3. **Windows & UI:**
   - Modular windows for train tables, route tables, and user notifications, with drag-and-drop support and customizable sorting.

---

## File Structure
- **`MainWindow.xaml.cs`**: Entry point of the application, managing navigation between different windows.
- **`LoginWindow.xaml.cs`**: Handles user authentication.
- **`TrainTable.xaml.cs` & `RouteTable.xaml.cs`**: Provide data grids for viewing and sorting trains/routes.
- **`Notifications.xaml.cs`**: Manages notifications related to train conditions.
- **`Container.cs`**: Centralized static class for shared data and application state.
- **`FileIOService.cs`**: Utility class for saving and loading JSON data files.
- **`Train.cs` & `Route.cs`**: Define the models for trains and routes, including their behaviors and properties.
- **`User.cs`**: Defines user authentication and management logic.

---

## Technologies Used
- **Language:** C#  
- **Framework:** WPF  
- **Data Handling:** JSON (via `Newtonsoft.Json`)  
- **Collections:** `BindingList`, `LinkedList`  

---

## Setup & Usage
1. Clone the repository to your local machine.
2. Open the solution in Visual Studio.
3. Run the application to access features such as train and route management, user authentication, and notifications.

---

## Future Enhancements
- Add real-time data synchronization for multi-user environments.
- Enhance the UI with advanced filtering and search functionalities.
- Introduce data analytics for predicting train maintenance schedules.

---

## Contributors
Developed by Escapist_1871. Contributions are welcome to extend the functionality of the TrainsBook system.
