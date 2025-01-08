# **OSK Framework Overview for Realme**

The **OSK Framework** is a modular Unity framework designed to streamline game development, optimized for Realme devices. It provides tools to manage game systems like events, localization, sound, and more, ensuring high performance, scalability, and maintainability.

**version 2.0

---

## **🌟 Key Features**

**link: https://github.com/O-S-K/Osk-Framework/tree/main/Runtime/Scripts/Core

1. [**MonoManager**]: Centralized management for MonoBehaviours, enabling streamlined execution and lifecycle control.  
2. [**ServiceLocatorManager**]: Provides a service locator pattern for dependency resolution.  
3. [**ObserverManager**]: Implements the Observer Pattern for decoupled event-driven communication.  
4. [**EventBusManager**]: Facilitates event broadcasting and subscription across systems.  
5. [**FSMManager**]: Manages Finite State Machines for state-driven behaviors.  
6. [**PoolManager**]: Efficiently handles object pooling to improve memory usage and performance.  
7. [**CommandManager**]: Supports command pattern for undoable actions and player input recording.  
8. [**SceneManager**]: Manages smooth scene transitions and scene-specific logic.  
9. [**ResourceManager**]: Handles resource loading, caching, and unloading.  
10. [**StorageManager**]: Provides mechanisms for saving and loading persistent data.  
11. [**DataManager**]: Manages runtime and persistent game data.  
12. [**NetworkManager**]: Handles networking, multiplayer, and server communication.  
13. [**WebRequestManager**](: Simplifies making HTTP requests and processing responses.  
14. [**GameConfigsManager**]: Centralized management of game configuration settings.  
15. [**UIManager**]: Manages UI screens, transitions, and dynamic content.  
16. [**SoundManager**]: Controls background music, sound effects, and audio events.  
17. [**LocalizationManager**]: Handles multi-language support and localization.  
18. [**EntityManager**]: Manages game entities and their lifecycle.  
19. [**TimeManager**]: Provides advanced time tracking, countdowns, and scheduling.  
20. [**NativeManager**]: Supports platform-specific features like vibration, GPS, and notifications.  
21. [**BlackboardManager**]: Facilitates shared data storage for AI and gameplay logic.  
22. [**ProcedureManager**]: Manages game procedures and workflows for structured gameplay logic.  

---

## **🚀 Quick Start**

### **1. Install Dependencies**
- Required: **Odin Inspector**, **DOTween**  
- Optional: **UIFeel**, **UIParticle**

### **2. Initialize Framework**
1. Go to **Window → OSK-Framework → CreateFramework** to set up the structure.  
2. Use **Create Module** and **Create Config** to enable and configure modules.

### **3. Enable Modules**
- Navigate to **MainModules** and activate the features you need.

### **4. Configure Settings**
- Create ScriptableObjects for resources:
  - Right-click in `Assets` → **Create → OSK** → Select the desired SO type (e.g., **ListMusicSO**, **ListSoundSO**).

### **5. Access Framework**
Use the `Main` object to interact with the systems:
- **Pooling**: `Main.Pool.Spawn<T>()`  
- **UI**: `Main.UI.Open<T>()`  
- **Events**: `Main.Event.Add("EventName", callback)`  
- **Sound**: `Main.Sound.Play()`  
- **Storage**: `Main.Storage.Save<T, U>()`  

---

## **🎯 Benefits**
- **Modular**: Enable only the features you need for your project.  
- **Reusable**: Modules can be reused across multiple games.  
- **Optimized**: Organized management of game systems for better performance.  

---

## **📞 Support**
- **Email**: gamecoding1999@gmail.com  
- **Facebook**: [OSK Framework](https://www.facebook.com/xOskx/)
