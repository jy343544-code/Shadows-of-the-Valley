# Unity 6 Setup — Android Prototype

## Required workstation
The standard Unity Editor workflow requires Windows, macOS, or Linux. A phone can edit repository files but cannot run the normal Unity Editor or local Android build pipeline. A cloud workstation is an alternative if it provides a graphical desktop and Android Build Support.

## Create the project
1. Install Unity 6 with Android Build Support, Android SDK & NDK Tools, and OpenJDK.
2. Create a new **3D (URP)** project and copy this repository's `Assets` folder into it.
3. Create a scene named `BlackGate_Prototype`.

## Player setup
1. Create a Capsule named `Sekhem`; add CharacterController, PlayerController, PlayerHealth, PlayerCombat, AirComboCombat, ShadowTether, and TargetLock.
2. Assign the Main Camera Transform to PlayerController and TargetLock.
3. Add an Animator only after importing a compatible humanoid rig and controller. Scripts remain functional without it.
4. Create an Attack Point child in front of Sekhem and assign it to PlayerCombat and AirComboCombat.
5. Create a lock marker (small world-space ring/sprite) and assign it to TargetLock; keep it disabled initially.

## Standard enemy setup
1. Add Collider, Rigidbody, EnemyHealth, and MonsterChaser to each basic enemy.
2. For enemies affected by pull/launch physics, set Rigidbody Is Kinematic OFF, Use Gravity ON, Interpolate to Interpolate, and freeze rotation X/Z if needed.
3. Create an `Enemy` layer and assign it to every enemy collider. Set PlayerCombat, AirComboCombat, ShadowTether, and TargetLock masks to Enemy.
4. Assign the Sekhem Transform to MonsterChaser's Target.

## Gatekeeper boss setup
1. Create the boss object with a suitable Collider, Rigidbody (optional for this scripted prototype), and EnemyHealth.
2. Set EnemyHealth Max Health to 600 and Destroy On Defeat OFF if you want to play a defeat animation before disabling it.
3. Add GatekeeperBossAI. Assign Sekhem as Target and set Player Layers to the layer used by Sekhem's Collider.
4. Give the boss a large silhouette, clear attack windups, and a weak-point visual. Keep the boss collider on the Enemy layer so Sekhem can target it.
5. Tune shockwave radius and windup in the Inspector; the shockwave is a circular proximity hit, not a rendered ground wave yet.

## Mobile controls
Create a Canvas with GraphicRaycaster and an EventSystem. Add a joystick base/handle and attach VirtualJoystick. Add a transparent camera drag panel with Raycast Target enabled and attach CameraTouchArea.

Connect Button OnClick:
- Attack -> PlayerCombat.Attack
- Jump -> PlayerController.Jump
- Dodge -> PlayerController.Dodge
- Shadow Pull -> ShadowTether.Cast
- Air Launcher -> AirComboCombat.LaunchTarget
- Air Attack -> AirComboCombat.AirAttack
- Ground Slam -> AirComboCombat.GroundSlam
- Lock Target -> TargetLock.ToggleLock

## 3D art direction
Use Unity 6 URP, PBR materials, baked global illumination, reflection probes, atmospheric dust, selective fog/light shafts, and original detailed character/monster models. Use LOD groups, texture compression, occlusion culling, GPU instancing, pooled VFX, and scalable quality presets. A Cinemachine camera, layered combat animation, impact hit-stop, camera shake, and authored sound design are planned polish passes.

## Android performance
Start with ARM64, Landscape, conservative render scale, limited real-time shadows, and compressed textures. Profile CPU, GPU, memory, heat, and battery on the actual phone. Aim for a console-inspired art direction scaled to mobile, not identical console fidelity.

## Current limitations
The repository contains starter scripts and documentation, not a complete Unity project, finished 3D models, animation assets, assembled scene, or APK. Compilation, animation wiring, art import, and device testing require Unity Editor on a supported desktop or cloud workstation.
