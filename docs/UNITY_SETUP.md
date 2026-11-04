# Unity 6 Setup — Android Prototype

## Required workstation
The standard Unity Editor workflow requires Windows, macOS, or Linux. A phone can edit repository files but cannot run the normal Unity Editor or local Android build pipeline. A cloud workstation is an alternative if it provides a graphical desktop and Android Build Support.

## Create the project
1. Install Unity Hub on a supported desktop/cloud workstation.
2. Install Unity 6 Editor with Android Build Support, Android SDK & NDK Tools, and OpenJDK.
3. Create a new **3D (URP)** project.
4. Copy this repository's `Assets` folder into the project root.
5. Create and save a scene named `BlackGate_Prototype`.

## Greybox player and camera
1. Create a Plane for the ground and a Capsule named `Sekhem` at (0, 1, 0).
2. Add CharacterController (Height 1.8, Radius 0.35, Center (0, 0.9, 0)).
3. Add `PlayerController`, `PlayerCombat`, `AirComboCombat`, and `ShadowTether` to Sekhem.
4. Add a third-person camera and `ThirdPersonCamera`; assign Sekhem as Target.
5. Assign the camera Transform to PlayerController's Camera Transform.
6. Add an Animator only after importing a compatible humanoid rig and animation controller. Until then, scripts work without an Animator.

## Enemy setup (important for physical pulls and launch attacks)
1. Create a Cube/Capsule named `TrainingEnemy`, add a Collider and `EnemyHealth`.
2. Add a Rigidbody. For the first physics prototype, set Is Kinematic OFF, Use Gravity ON, Interpolate to Interpolate, and Collision Detection to Continuous.
3. Freeze Rigidbody rotation X and Z if the enemy tips over; do not freeze Y position.
4. Create an `Enemy` layer, assign it to the enemy and select it in PlayerCombat, AirComboCombat, and ShadowTether layer masks.
5. Add `MonsterChaser` to the enemy and assign the player Transform. Tune its movement so it does not overpower the Rigidbody launch/pull reactions.
6. Create an empty child in front of Sekhem and assign it as Attack Point / Attack Origin / Cast Origin as appropriate.
7. Keep the enemy Collider on the Enemy layer. If the enemy uses multiple colliders, put them on the same layer.

## Mobile controls
Create a Canvas with GraphicRaycaster and an EventSystem. Add a joystick base and handle; attach `VirtualJoystick` and assign the player. Add a transparent right-side camera touch panel with Raycast Target enabled; attach `CameraTouchArea` and assign the camera.

Create UI Buttons and connect OnClick:
- Attack -> PlayerCombat.Attack
- Jump -> PlayerController.Jump
- Dodge -> PlayerController.Dodge
- Shadow Pull -> ShadowTether.Cast
- Air Launcher -> AirComboCombat.LaunchTarget
- Air Attack -> AirComboCombat.AirAttack (support press/tap repetition)
- Ground Slam -> AirComboCombat.GroundSlam

For a production feel, use a hold/repeat input handler for Air Attack rather than relying on one tap only. Add UI cooldown indicators for Shadow Pull and Dodge.

## Visual quality target (console-inspired, mobile-scalable)
- Use URP with a cinematic but readable color grade: warm sunlit sandstone, cool deep shadows, restrained turquoise energy accents.
- Use PBR materials, baked global illumination, reflection probes, fog, decals, and carefully authored particle effects.
- Use high-quality humanoid/monster models, retargeted animation clips, facial/weapon detail, and layered locomotion/combat animation.
- Use a Cinemachine follow camera with damping, collision handling, camera shake on heavy impacts, and a subtle FOV kick during dodges.
- Use LOD groups, occlusion culling, texture compression, GPU instancing, baked lighting, and pooled VFX. Provide scalable quality presets for mobile.
- Do not promise console-level fidelity on every Android phone: target a strong stylized-realistic look and profile on actual mid-range devices.

## Android settings and performance
- Switch Platform to Android; start with ARM64 and Landscape orientation.
- Use IL2CPP for release builds when configured.
- Keep render scale and shadow distance conservative initially; test on the actual target phone.
- Profile CPU, GPU, memory, heat, and battery during a sustained combat session.

## Current limitations
This repository contains gameplay starter scripts and setup/design documentation, not a complete Unity project, finished 3D models, animation assets, assembled scene, or APK. Compilation, animation wiring, visual asset import, and device testing still need Unity Editor on a supported desktop or cloud workstation.
