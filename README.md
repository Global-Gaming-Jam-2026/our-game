```
 ██████╗ ██╗   ██╗ █████╗  ██████╗██╗  ██╗    ███████╗██████╗ ███████╗███╗   ██╗███████╗██╗   ██╗
██╔═══██╗██║   ██║██╔══██╗██╔════╝██║ ██╔╝    ██╔════╝██╔══██╗██╔════╝████╗  ██║╚══███╔╝╚██╗ ██╔╝
██║   ██║██║   ██║███████║██║     █████╔╝     █████╗  ██████╔╝█████╗  ██╔██╗ ██║  ███╔╝  ╚████╔╝
██║▄▄ ██║██║   ██║██╔══██║██║     ██╔═██╗     ██╔══╝  ██╔══██╗██╔══╝  ██║╚██╗██║ ███╔╝    ╚██╔╝
╚██████╔╝╚██████╔╝██║  ██║╚██████╗██║  ██╗    ██║     ██║  ██║███████╗██║ ╚████║███████╗   ██║
 ╚══▀▀═╝  ╚═════╝ ╚═╝  ╚═╝ ╚═════╝╚═╝  ╚═╝    ╚═╝     ╚═╝  ╚═╝╚══════╝╚═╝  ╚═══╝╚══════╝   ╚═╝
```

### A Duck Jones Adventure

![Unity](https://img.shields.io/badge/Unity-6000.0.62f1-000000?style=for-the-badge&logo=unity&logoColor=white)
![CSharp](https://img.shields.io/badge/C%23-10.0-239120?style=for-the-badge&logo=csharp&logoColor=white)
![Global Game Jam](https://img.shields.io/badge/Global%20Game%20Jam-2026-E4405F?style=for-the-badge)
![License](https://img.shields.io/badge/License-MIT-blue?style=for-the-badge)

---

## What Is This

A duck with a whip versus a shape-shifting boss. The boss wears masks. Each mask means a different attack pattern. You learn the patterns or you die. There is no middle ground.

Built in 48 hours for Global Game Jam 2026. The theme was masks. We took it literally.

```
+===================================================================+
|  GAME STATS                                                       |
+===================================================================+
|                                                                   |
|  Boss Forms        [█████░░░░░░░░░░░░░░░]   5 masks               |
|  Attack Patterns   [██████░░░░░░░░░░░░░░]   6 ways to die         |
|  Player States     [██████░░░░░░░░░░░░░░]   6 states              |
|  Duck Frames       [██████████████░░░░░░]  14 animation frames    |
|  Dev Time          [████████████████████]  48 hours               |
|                                                                   |
+===================================================================+
```

---

## Quick Start

### What You Need
- Unity 6000.0.62f1 or later
- Git
- Patience

### How to Run
```bash
git clone https://github.com/Global-Gaming-Jam-2026/our-game.git
cd our-game/src/GGJ26
```
Open in Unity. Load `Scenes/Almog Prototype.unity`. Hit Play. Die a few times. Get better.

---

## Controls

| Action | Keyboard | Gamepad |
|--------|----------|---------|
| Move | WASD / Arrows | Left Stick |
| Jump | Space | A Button |
| Attack | Left Click / Enter | X Button |
| Pause | Escape | Start |

The whip direction depends on your vertical input. Hold W and attack for an upward strike. Otherwise you swing sideways.

---

## How to Win

1. Do not get hit
2. Hit the boss
3. Repeat until the boss health bar is empty

Simple in theory. The boss has other plans.

---

## The Boss

Five masks. Five forms. Six attacks. One very angry entity.

Every time the boss takes damage, it switches masks. New mask means new attack pattern. You thought you had it figured out? Start over.

### Attack Patterns

| Attack | Mask | How Often | Danger | What Happens |
|--------|------|-----------|--------|--------------|
| **Slam** | Bear | Very Common | HIGH | Rises up, shadow tracks you, drops on your head |
| **Spears** | Human | Common | MEDIUM | Five projectiles arc toward where you are standing |
| **Swoop** | Bird | Medium | MEDIUM | Figure-8 flight pattern, spawns projectiles along the way |
| **Charge** | Bull | Rare | HIGH | Full screen charge, left to right or right to left |
| **Roar** | Lion | Rare | HIGH | AOE damage pulse, short telegraph before it hits |
| **Laser** | None | Medium | EXTREME | 90 degree arc sweep, continuous damage, do not stand still |

### The Catch

The boss gets faster as it loses health. Attack cooldowns scale from 1.0x at full HP to 0.3x at low HP. The moment you think you are winning is the moment the game gets three times harder.

---

<details>
<summary><strong>The Player (click to expand)</strong></summary>

### Duck Jones

A duck. With a whip. That is the entire backstory.

The whip has two attacks:
- **Side Whip**: Horizontal arc, default attack
- **Up Whip**: Hold up while attacking, vertical arc

You can move while attacking. This is intentional. Use it to dodge and deal damage at the same time.

### State Machine

```
IDLE ---------> RUN ---------> JUMP
  |              |              |
  |              |              v
  |              |            FALL
  |              |              |
  v              v              v
           WHIP <--------------+
             |
             v
           DEATH
```

Six states. Whip can be triggered from any of the first four. Death is final.

</details>

---

<details>
<summary><strong>Architecture (click to expand)</strong></summary>

### State Machine Pattern

Both the player and boss run on state machines. Each state handles its own logic: enter, update, fixed update, exit. Clean separation. Easy to extend.

```
Entity (Base Class)
    |
    +-- PlayerController
    |       +-- StateMachine
    |               +-- PlayerIdleState
    |               +-- PlayerRunState
    |               +-- PlayerJumpState
    |               +-- PlayerFallState
    |               +-- PlayerWhipState
    |               +-- PlayerDeathState
    |
    +-- BossController
            +-- StateMachine
                    +-- BossIdleState
                    +-- BossAttackState
                    |       +-- BirdAttack
                    |       +-- BullAttack
                    |       +-- LionAttack
                    |       +-- HumanAttack
                    |       +-- LaserAttack
                    |       +-- SlamAttack
                    |               +-- TelegraphPhase
                    |               +-- SlamPhase
                    +-- BossDeathState
```

### Event Bus

Central event system for game-wide communication:
- `OnPlayerHealthChange` - HP updates
- `OnBossHealthChange` - Boss HP updates
- `OnBossDefeat` - Victory condition
- `OnPlayerDeath` - Restart trigger

### Health System

`HealthModule` handles all damage logic: HP tracking, invincibility frames, percentage thresholds for phase transitions. Attach it to anything that needs to take damage.

</details>

---

<details>
<summary><strong>Project Structure (click to expand)</strong></summary>

```
src/GGJ26/Assets/
|
+-- Scripts/
|   +-- Boss/
|   |   +-- BossController.cs
|   |   +-- StateMachine.cs
|   |   +-- States/
|   |       +-- BossIdleState.cs
|   |       +-- BossDeathState.cs
|   |       +-- Attack States/
|   |           +-- BirdAttack.cs
|   |           +-- BullAttack.cs
|   |           +-- LionAttack.cs
|   |           +-- HumanAttack.cs
|   |           +-- LaserAttack.cs
|   |           +-- SlamState/
|   +-- Player/
|   |   +-- PlayerController.cs
|   |   +-- States/
|   +-- Core/
|   |   +-- GameManager.cs
|   |   +-- EventBus.cs
|   |   +-- HealthModule.cs
|   +-- UI/
|       +-- MainMenuUI.cs
|       +-- PlayerHPBar.cs
|       +-- BossHPBar.cs
|       +-- GameOverUI.cs
|
+-- Scenes/
|   +-- MainMenu.unity
|   +-- Almog Prototype.unity
|
+-- Sprites/
|   +-- Duck/ (14 frames)
|   +-- Masks/ (5 masks)
|
+-- Animations/
```

</details>

---

## Tech Stack

| Package | Version | What It Does |
|---------|---------|--------------|
| Unity | 6000.0.62f1 | The engine |
| DOTween | Latest | Smooth tweening and animation |
| Input System | 1.14.2 | Keyboard and gamepad support |
| URP | 17.0.4 | Universal Render Pipeline |
| TextMeshPro | Built-in | UI text rendering |

---

## Building

### Development Build
1. Open in Unity 6000.0.62f1 or later
2. File, then Build Settings
3. Pick your platform
4. Build

### Scenes
- **MainMenu** - Title screen, Play button, Quit button
- **Almog Prototype** - The actual game

---

## Known Issues

| Issue | Status | Impact |
|-------|--------|--------|
| Whip has no visual sprite | Gameplay works | Damage collider functions, animation invisible |
| Some particle textures missing | Cosmetic | Particles render as squares |

---

## Global Game Jam 2026

Built during the 48 hour jam. The theme was masks. We made a boss that literally changes masks. Sometimes the obvious approach is the right one.

### Team Tools
- Unity 6000.0.62f1
- DOTween by Demigiant
- Visual Studio
- Rider
- Coffee

---

## Links

| Resource | Description |
|----------|-------------|
| [Game Jam Agent](https://github.com/Global-Gaming-Jam-2026/game-jam-agent) | AI dev tools we built |
| [Skills Library](https://github.com/Global-Gaming-Jam-2026/game-dev-skills) | Game dev knowledge base |

---

## License

MIT. Take the code. Use it. Modify it. Ship it. Credit is appreciated but not required.

---

```
+===================================================================+
|                                                                   |
|   "The ancient mask awakens. Face your destiny."                  |
|                                                                   |
|   ...or just press Space to jump. That works too.                 |
|                                                                   |
+===================================================================+
```
