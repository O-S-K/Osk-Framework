# Framework Overview

This framework is designed to manage various aspects of a Unity project through a modular and organized structure. It provides comprehensive systems for game functionalities such as game data management, events, localization, networking, sound management, and more. Each module is encapsulated for maintainability, performance, and scalability.

The framework includes systems like:
- **Object Pooling**: Efficiently reuses objects to reduce memory allocation and improve performance.
- **Observer Pattern**: Implements an event system for decoupled event handling, allowing flexible communication between components.
- **State Machines**: Supports state-driven behavior, making it easier to manage game logic and transitions.
- **Event Handling**: A powerful event system for managing interactions and communication between different game components.
- **Data Management**: Handles persistent data storage and retrieval, including saving/loading game states, player preferences, and more.
- **Entity Management**: Manages in-game entities (characters, NPCs, etc.) with dynamic and reusable object systems.
- **Game Configuration**: Stores and manages game settings and parameters via ScriptableObjects, making them easily adjustable.
- **Localization**: Simplifies managing translations for multiple languages, with support for importing/exporting from CSV/Excel files.
- **Platform-Specific Functionality**: Includes features like vibration, GPS, and notifications, tailored for specific platforms.
- **Networking**: Handles multiplayer, leaderboards, and cloud synchronization to support online game features.
- **Performance Optimization**: Provides utilities like memory management, framerate monitoring, and object pooling to optimize game performance.
- **Procedural Content Generation**: Allows for dynamic generation of levels, environments, or other game content.
- **Resource Management**: Manages assets and resources to optimize memory usage and improve loading times.
- **Sound Management**: Controls background music, sound effects, and audio events with ScriptableObject-based configuration.
- **UI Management**: Simplifies user interface management, including screens, popups, HUDs, and input handling with dynamic transitions.
- **Timer System**: A flexible system for managing countdowns, events, and time-based logic, with support for pausing and looping.
- **Save/Load System**: Robust saving and loading mechanisms for both local and cloud storage.
- **Scene Management**: Handles scene transitions, loading, and unloading of levels and menus, ensuring smooth gameplay flow.

These systems are designed to improve the modularity, maintainability, and performance of your Unity project.

---

## How to Use

### 1. **Install Required Packages**

Before using the framework, install the following essential packages:
- Odin Inspector (Requirements)
- DOTween (Requirements)
- UIFeel (optional)
- UIParticle (optional)

### 2. **Initialize the Framework**

Once the required packages are installed, follow these steps to initialize the framework in your Unity project:

- Go to **Window** -> **OSK-Framework** -> **CreateFramework** to generate the framework structure for your project.
- Then click **Create Module** and **Create Config** to create the initial modules and configurations for the system.

### 3. **Enable Modules**

- Go to **MainModules** in `OSK-Framework` and enable the modules you want to use in your game.

### 4. **Configure Init Settings**

- In **ConfigInit**, create and configure **ScriptableObject (SO)** for managing resources like **ListMusicSO**, **ListSoundSO**, and **UIParticleSO** for the game:
  - Right-click in the `Assets` folder -> **Create** -> **OSK** to create these SOs.
  - You can then edit initial values for these components directly in the SOs, allowing you to customize settings for your game.

### 5. **Using the Main Object**

The `Main` object is the central access point for all systems within the framework. After enabling the necessary modules, you can use the following methods to interact with the various systems in your game:

- **Object Pooling**: Use `Main.Pool.Spawn<T>()` to spawn objects from the pool, optimizing memory usage.
- **UI Management**: Use `Main.UI.Open<T>()` to open UI screens or popups in the game.
- **Event Handling**: Use `Main.Event.Add("EventName", callback)` to register and handle events in the game.
- **Sound Management**: Use `Main.Sound.Play()` to play sound effects or background music in the game.
- **Storage Management**: Use `Main.Storage.Save<T, U>()` to save game data to disk.
more module .....
    

---

### **Benefits of Using This Framework:**

- **Flexibility**: The framework can be extended and customized for various types of games.
- **Reusability**: Modules can be reused across different projects.
- **Efficient Management**: The framework helps manage different game components (like UI, sound, events) in an organized and modular way.

By following the above steps, you can effectively utilize this framework to develop your Unity game, ensuring a well-structured and maintainable project.
 
@support : gamecoding1999@gmail.com
@facebook: https://www.facebook.com/xOskx/
