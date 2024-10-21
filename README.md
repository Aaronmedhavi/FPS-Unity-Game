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
  <tr>
    <td><img src="https://github.com/Aaronmedhavi/ProjectClips/blob/main/fPSSS - Made with Clipchamp.gif?raw=true" width="500"></td>
  </tr>
<table>
  <tr>
    <td><img src="https://github.com/Aaronmedhavi/ProjectClips/blob/main/Screenshot 2024-10-20 234429.png?raw=true" width="400"></td>
    <td><img src="https://github.com/Aaronmedhavi/ProjectClips/blob/main/Screenshot 2024-10-20 234506.png?raw=true" width="400"></td>
  </tr>
</table>

## ⚙️ Mechanics

### NavMesh and State Machine
Enemies are controlled using NavMesh to determine the patrol area in the map and waypoints for the patrolling path. Animations for the enemy are determined using state machine that dictates what animation should play when the enemy are idle, walking, attacking, or dying. Once the enemy detects the player, it will chase them around.

### URP Post Processing
This game using the built in URP post processing feature which includes bloom, vignette, depth of field, film grain, and color adjustment to enhance the visuals. Paired with occlusion culling, it provides an exciting and visually appealing experience without a drop in performance.

### Weapon Management
The weapon is stored inside of a weapon slot located in the game scene, which works as a container to hold the weapon for the player. When switching or picking up a new weapon, the old weapon is removed and switched with the new one. Weapon essentialy worked as a child of the weapon slot.

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

## 📫 Contact
If you want to provide feedback or report bugs, feel free to reach out to me here:
- Email: aaronmedhavi@gmail.com
