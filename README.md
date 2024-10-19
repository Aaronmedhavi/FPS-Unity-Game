## 🔫 About
Zero Hour Assault is a first-person shooter where you play as a lone soldier battling enemy forces in a city. Navigate streets, engage in close-quarters combat, and use the urban environment to outsmart your enemies.

## 🕹️ Installation

### 📁 Clone the Repository

1. Make sure you have Unity (version 2022.3.9f1 or later) installed on your machine.
2. Clone this repository:
   ```
   git clone https://github.com/Aaronmedhavi/FPS-Unity-Game.git
   ```
3. Open the project in Unity.
4. Open the game scene located in the "Assets/Scenes" folder.
5. Press the Play button in Unity Editor to start the game.

## 🎮 Controls

- Move Forward: W
- Move Backward: S
- Move Left: A
- Move Right: D 
- Jump: Spacebar
- Shoot: Left Mouse Button
- Pick Up Weapon: F
- Switch Weapon: 1 / 2

## 📺 Gameplay Footage / Screenshot

## ⚙️ Mechanics
<h3>Netcode For GameObjects</h3>
<p align="justify">Experience online multiplayer experience made possible with Netcode. Through the use of a network manager, it allows players to join the game as a host or a client in a menu. The game will start when there is 2 players in the game, the ball will spawn once all the players have joined. The built in network manager only provide one slot for the player prefab but with the use of an index based on the client ID, it's now possible for players to play with distinct sprites.</p>

<h3>Post Processing</h3>
<p align="justify">Implementation of basic post processing which includes bloom and color grading to increase visual fidelity and enhance the player experience without sacrificing any performance.</p>

## 📚 Features and Script
- Play With Multiple Weapons
- Patrolling Enemies using NavMesh
- Stunning visuals and immersive sound design
- Smooth character animations and responsive controls

|  Script       | Description                                                  |
| ------------------- | ------------------------------------------------------------ |
| `WeaponManager.cs` | Manages the weapon slot, active weapon, and ammo. |
| `EnemyPatrollingState.cs` | Controls the animation state for patrolling enemies. |
| `SoundManager.cs`  | Controls the audio used for sound effects for guns. |
| `UIManager.cs`  | Manages various UI elements and assign them to the appropiate elements. |
| `etc`  | |
