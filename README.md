# 🏰 Dungeon

> A grid-based, turn-based tactical combat game built with Unity. 

Welcome to the **Dungeon** repository! [cite_start]This project features a highly modular architecture, leveraging A* pathfinding, the Factory design pattern, and a robust turn-based combat system to create a dynamic tactical experience[cite: 14, 98, 222].

---

## ⚙️ Core Systems & Architecture

[cite_start]To maintain a scalable and clean codebase, the project is divided into specialized managers and core systems[cite: 81, 98, 110, 150, 222]:

| System | Description | Key Classes |
| :--- | :--- | :--- |
| **AI & Pathfinding** | Controls enemy movement and tactical targeting. | [cite_start]`AStarPathfinding`, `EnemyAI_Organizer` [cite: 14, 81] |
| **Turn-Based Combat** | Manages the flow of combat between player and enemies. | [cite_start]`TurnBasedManager`, `PlayerTurn`, `EnemyAttack` [cite: 213, 219, 222] |
| **Grid Management** | Generates the battlefield and handles cell interactions. | [cite_start]`GridManager`, `Grid`, `GridClickable` [cite: 109, 110] |
| **Object Creation** | Ensures optimized instantiation using the Factory pattern. | [cite_start]`FactoryManager`, `EnemyFactory`, `PlayerFactory` [cite: 96, 98, 102] |
| **Mission Tracking** | Monitors player progression toward level objectives. | [cite_start]`MissionManager`, `KillFourEnemyMission` [cite: 148, 150] |

---

## 👾 Entities & Combat Mechanics

The combat system is designed to support diverse enemy types and dynamic skills:

* [cite_start]**Enemy Variations:** Behaviors are controlled by specialized AI brains (e.g., `SpiderEnemyBrain`, `WizardEnemyBrain`, `RangerEnemyBrain`)[cite: 50, 53, 60].
* [cite_start]**Combat Styles:** Distinct calculators dictate tactical decisions (e.g., `EnemyAI_Calculator_Archer`, `EnemyAI_Calculator_Warrior`)[cite: 64, 77].
* [cite_start]**Skills & Projectiles:** Features dynamic abilities like `Fireball`, `Heal`, and `ThunderSkill` with specific trajectory controllers like `ParabolicLineController`[cite: 46, 183, 185, 199].
* [cite_start]**Game Feel:** The `FeelManager` handles visual and auditory feedback to enhance combat impact[cite: 105].

---

## 🔌 Core Interfaces

To reduce coupling, entities rely on standardized contracts:

* [cite_start]`IDamagable` / `IHealth`: Applied to any entity that has health points and can take damage[cite: 115, 116].
* [cite_start]`IClickable`: Used for objects and grid cells that can be interacted with via mouse clicks[cite: 115].
* [cite_start]`IPushable`: Applied to entities affected by knockback or force mechanics[cite: 130].
* [cite_start]`IRotatable`: Used for objects that need to update their facing direction[cite: 131].

---

## 💾 Data Management

The project heavily utilizes **ScriptableObjects** for clean, scalable data storage and configuration:

* [cite_start]**Level Data:** Layouts and configurations are stored in assets like `LevelSo` and `TutorialLevelSO`[cite: 139, 140].
* [cite_start]**Entity Stats:** Attributes are easily adjustable via `EnemySO` and `VillageSO`[cite: 45, 244].
