# 🏰 Dungeon

> A grid-based, turn-based tactical combat game built with Unity. 

Welcome to the **Dungeon** repository! This project features a highly modular architecture, leveraging A* pathfinding, the Factory design pattern, and a robust turn-based combat system to create a dynamic tactical experience.

---

## ⚙️ Core Systems & Architecture

To maintain a scalable and clean codebase, the project is divided into specialized managers and core systems:

| System | Description | Key Classes |
| :--- | :--- | :--- |
| **AI & Pathfinding** | Controls enemy movement and tactical targeting. | `AStarPathfinding`, `EnemyAI_Organizer` |
| **Turn-Based Combat** | Manages the flow of combat between player and enemies. | `TurnBasedManager`, `PlayerTurn`, `EnemyAttack` |
| **Grid Management** | Generates the battlefield and handles cell interactions. | `GridManager`, `Grid`, `GridClickable` |
| **Object Creation** | Ensures optimized instantiation using the Factory pattern. | `FactoryManager`, `EnemyFactory`, `PlayerFactory` |
| **Mission Tracking** | Monitors player progression toward level objectives. | `MissionManager`, `KillFourEnemyMission` |

---

## 👾 Entities & Combat Mechanics

The combat system is designed to support diverse enemy types and dynamic skills:

* **Enemy Variations:** Behaviors are controlled by specialized AI brains (e.g., `SpiderEnemyBrain`, `WizardEnemyBrain`, `RangerEnemyBrain`).
* **Combat Styles:** Distinct calculators dictate tactical decisions (e.g., `EnemyAI_Calculator_Archer`, `EnemyAI_Calculator_Warrior`).
* **Skills & Projectiles:** Features dynamic abilities like `Fireball`, `Heal`, and `ThunderSkill` with specific trajectory controllers like `ParabolicLineController`.
* **Game Feel:** The `FeelManager` handles visual and auditory feedback to enhance combat impact.

---

## 🔌 Core Interfaces

To reduce coupling, entities rely on standardized contracts:

* `IDamagable` / `IHealth`: Applied to any entity that has health points and can take damage.
* `IClickable`: Used for objects and grid cells that can be interacted with via mouse clicks.
* `IPushable`: Applied to entities affected by knockback or force mechanics.
* `IRotatable`: Used for objects that need to update their facing direction.

---

## 💾 Data Management

The project heavily utilizes **ScriptableObjects** for clean, scalable data storage and configuration:

* **Level Data:** Layouts and configurations are stored in assets like `LevelSo` and `TutorialLevelSO`.
* **Entity Stats:** Attributes are easily adjustable via `EnemySO` and `VillageSO`.
