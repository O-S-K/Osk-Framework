# <span style="color:yellow;">**OSK Framework Overview for Realme****</span>

The **OSK Framework** is a modular Unity framework designed to streamline game development, optimized for Realme devices. It provides tools to manage game systems like events, localization, sound, and more, ensuring high performance, scalability, and maintainability.

**version 2.0

---

## **🌟 Key Features**

**link: https://github.com/O-S-K/Osk-Framework/tree/main/Runtime/Scripts/Core

1. [<span style="color:green;">**MonoManager**</span>]: Centralized management for MonoBehaviours, enabling streamlined execution and lifecycle control.  
2. [<span style="color:green;">**ServiceLocatorManager**</span>]: Provides a service locator pattern for dependency resolution.  
3. [<span style="color:green;">**ObserverManager**</span>]: Implements the Observer Pattern for decoupled event-driven communication.  
4. [<span style="color:green;">**EventBusManager**</span>]: Facilitates event broadcasting and subscription across systems.  
5. [<span style="color:green;">**FSMManager**</span>]: Manages Finite State Machines for state-driven behaviors.  
6. [<span style="color:green;">**PoolManager**</span>]: Efficiently handles object pooling to improve memory usage and performance.  
7. [<span style="color:green;">**CommandManager**</span>]: Supports command pattern for undoable actions and player input recording.  
8. [<span style="color:green;">**SceneManager**</span>]: Manages smooth scene transitions and scene-specific logic.  
9. [<span style="color:green;">**ResourceManager**</span>]: Handles resource loading, caching, and unloading.  
10. [<span style="color:green;">**StorageManager**</span>]: Provides mechanisms for saving and loading persistent data.  
11. [<span style="color:green;">**DataManager**</span>]: Manages runtime and persistent game data.  
12. [<span style="color:green;">**NetworkManager**</span>]: Handles networking, multiplayer, and server communication.  
13. [<span style="color:green;">**WebRequestManager**</span>]: Simplifies making HTTP requests and processing responses.  
14. [<span style="color:green;">**GameConfigsManager**</span>]: Centralized management of game configuration settings.  
15. [<span style="color:green;">**UIManager**</span>]: Manages UI screens, transitions, and dynamic content.  
16. [<span style="color:green;">**SoundManager**</span>]: Controls background music, sound effects, and audio events.  
17. [<span style="color:green;">**LocalizationManager**</span>]: Handles multi-language support and localization.  
18. [<span style="color:green;">**EntityManager**</span>]: Manages game entities and their lifecycle.  
19. [<span style="color:green;">**TimeManager**</span>]: Provides advanced time tracking, countdowns, and scheduling.  
20. [<span style="color:green;">**NativeManager**</span>]: Supports platform-specific features like vibration, GPS, and notifications.  
21. [<span style="color:green;">**BlackboardManager**</span>]: Facilitates shared data storage for AI and gameplay logic.  
22. [<span style="color:green;">**ProcedureManager**</span>]: Manages game procedures and workflows for structured gameplay logic.  

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
