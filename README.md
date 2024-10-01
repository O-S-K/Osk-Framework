Framework Overview
This framework is designed to manage various aspects of a Unity project through a modular, organized structure. It provides comprehensive systems for game functionalities such as abilities, data management, events, localization, networking, and more. Each module is encapsulated for maintainability, performance, and scalability.

Modules
Ability
Manages character abilities and power-ups. It allows for creating, modifying, and applying abilities in the game using ScriptableObjects.

Data
Handles persistent data storage and retrieval. Supports player preferences, game saves, and general data management.

DesignPattern
Implements commonly used design patterns like Singleton, Object Pooling, Dependency Injection, and State Machines to organize and streamline game code.

Entity
Manages game entities such as characters, NPCs, enemies, and other in-game objects. Entities are represented in a dynamic, reusable way.

EventManager
A centralized system for handling game events using an Observer pattern. This allows for loosely coupled communication between different game components.

GameConfigs
Stores configuration settings and gameplay-related parameters. Use ScriptableObjects to manage and tweak game variables without hardcoding them in scripts.

Localization
Manages translations and localization for different languages. Supports importing/exporting localization data from files, like Excel or CSV.

Native
Provides platform-specific functionalities for handling native device features such as vibration, GPS, or notifications.

Networking
Manages online features such as multiplayer, leaderboards, and cloud synchronization.

Performance
Optimizes game performance with utilities like Object Pooling, memory management, and framerate monitoring.

Procedure
Manages procedural generation techniques, allowing for the dynamic creation of levels, environments, or other game content.

Resources
Handles resource loading and asset management, optimizing memory usage by managing resource lifecycle efficiently.

Save
Provides a robust system for saving and loading game states, both locally and via cloud storage.

Scene
Manages scene transitions, loading, and unloading of game levels or menus, providing smooth flow between scenes.

Sound
Manages game audio, including background music, sound effects, and environmental sounds. Easily integrate and manage audio using ScriptableObjects.

Stats
Manages player and entity stats, such as health, attack, defense, and speed. Allows for stat calculation and modification through modifiers like flat and percentage-based boosts.

Timer
Provides a flexible system for managing timers, countdowns, and in-game time-based events. Supports features like pausing, resuming, and looping timers.

UI
Manages user interface components like popups, screens, and HUD elements. Provides functions for dynamic UI transitions and input handling.

World
The main manager that oversees the entire game system. Provides access to core functionalities like the Object Pool, Observer, Event System, and more.

How to Use
Drag Prefab to Scene
To initialize the framework in your project, drag the World prefab into your scene.

Create ScriptableObject (SO) Data

Screens and Popups: Create ScriptableObjects to define different screens (e.g., MainMenu, GameOver) and popups (e.g., Settings, Notifications).
Sound and Music: Create SO for sound effects and music tracks, linking them to corresponding events in your game.
Using the World Functionality
The World object is the central hub for accessing various systems.

Object Pooling: Use World.Pool.Create<T>() to instantiate objects using the pooling system, ensuring optimal performance.
Observer: Use World.Observer.Add("ScoreUpdate", callback) to subscribe to game events and manage the flow of information between game components.
You can also use World.Command, World.Timer, and other World systems for advanced game management.

This framework provides a structured, reusable foundation for managing various game systems efficiently. The flexibility and modularity of the framework ensure that it can be tailored to different game genres and requirements.
