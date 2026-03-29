# SPACESHOOTER by Orca (G2)

## Group Members
| Name | Student ID |
|------|------------|
| Dania Batrisya binti Mohd Nizam | 24001438 |
| Aql bin Abdul Samad | 24006507 |
| Abu Al Rub Ahmad Mohammed Mahmoud | 25005930 |

## Project Description
Our space shooter game is a 2D arcade style space shooter based on the popular game 
‘ Galaga ’. The game is developed in C# using Windows Forms (.NET Framework). Our game uses direct inspiration from classic arcade shooter mechanisms, placing the player in control of a spaceship navigating through increasingly hostile waves of enemies and obstacles. The application was built to demonstrate how real-world software design patterns and how Object Oriented Programming principles can be applied in interactive game development.

Our game operates as a Windows desktop application and features moving ships, background music (via WMPLib), sound effects, a scrolling starfield background, and a complete game loop including level progression, score tracking, power-ups, and game-over and replay functionality. Our graphics are using direct inspiration and design from the famous movie series ‘ Star Wars ‘.

## System Features
- Player movement using arrow keys
- Auto-shooting system with enemy collision detection
- 3 enemy types with increasing difficulty across 5 levels
- Obstacle system with basic and flaming meteors
- Health and lives system with 3 hearts
- Power-ups: restore life, rapid fire, double points
- Final boss (Death Star) with health bar at level 5
- Score and level tracking
- Pause and replay functionality

## OOP Concepts Used
### Encapsulation
- All class fields are declared as **private**
- Data is accessed and modified only through **public properties**
- Example: `Player` class has private `health` field accessed via `Health` property

### Classes Implemented
- `Player` — manages health, lives, score and movement
- `Enemy` — manages damage, speed and attack patterns
- `Obstacle` — manages collision damage and movement types

### Inheritance
- `BasicEnemy`, `HardEnemy` and `BossEnemy` all inherit from base `Enemy` class

## How to Run
1. Install **Visual Studio 2022**
2. Clone or download this repository
3. Open `SPACESHOOTER_ORCA.sln` in Visual Studio
4. Make sure the `assets` and `songs` folders are in the project's debug directory (...\bin\debug)
5. Press **F5** or click **Start** to run the game

## Controls
| Key | Action |
|-----|--------|
| Arrow Keys | Move player |
| Space | Pause / Unpause |

## Project File Structure
```
SPACESHOOTER_ORCA/
├── assets/          # Images (enemies, player, meteors, power-ups)
├── songs/           # Audio files (background music, sound effects)
├── Form1.cs         # Main game logic and UI
├── GameClasses.cs   # OOP class implementations
├── Program.cs       # Entry point and Task 3 demonstration
└── README.md        # Project documentation
```
