# Unity 6 Setup — Android Prototype

## Required workstation
The standard Unity Editor workflow requires Windows, macOS, or Linux. A phone can edit repository files but cannot run the normal Unity Editor or local Android build pipeline. A cloud workstation is an alternative if it provides a graphical desktop and Android Build Support.

## Create the project
1. Install Unity Hub on a supported desktop/cloud workstation.
2. Install a Unity 6 Editor version and include Android Build Support, Android SDK & NDK Tools, and OpenJDK.
3. Create a new **3D (URP)** project.
4. Copy this repository's `Assets` folder into the new project's root folder.
5. Create and save a scene named `BlackGate_Prototype`.

## Build a greybox scene
1. Create a Plane for the ground.
2. Create a Capsule named `Sekhem`, position it at (0, 1, 0).
3. Add a CharacterController. Suggested values: Height 1.8, Radius 0.35, Center (0, 0.9, 0).
4. Add `PlayerController` and `PlayerCombat` to Sekhem.
5. Create a Camera. Add `ThirdPersonCamera`; assign Sekhem as Target.
6. Assign the camera Transform to PlayerController's Camera Transform.
7. Create a Cube named `TrainingEnemy`, add a Collider and `EnemyHealth`.
8. Create a layer named `Enemy`, assign it to TrainingEnemy, and select it in PlayerCombat's Enemy Layers.
9. Add an empty child to Sekhem in front of the character and assign it as Attack Point.
10. Create a Canvas with a joystick base/handle and an EventSystem.
11. Attach `VirtualJoystick` to the joystick base and assign the handle and player.
12. Add a transparent right-side UI panel with Raycast Target enabled; attach `CameraTouchArea` and assign the camera.
13. Add an Attack button. In its OnClick event, call Sekhem -> PlayerCombat -> Attack().

## Mobile input notes
- The joystick and camera panel use Unity UI pointer events.
- Ensure the Canvas has a GraphicRaycaster and the scene has an EventSystem.
- Use an Image component on touch panels so they receive pointer events; alpha can be near zero, but Raycast Target must stay enabled.

## Android player settings
- Switch Platform to Android.
- Start with ARM64 enabled.
- Use IL2CPP for release builds when the toolchain is configured.
- Set orientation to Landscape.
- Start with a conservative render scale and test on the actual phone.

## Performance checklist
- Prefer baked lighting for static temple geometry.
- Keep real-time shadow-casting lights to a minimum.
- Use compressed textures and appropriate texture sizes.
- Avoid large transparent particle effects.
- Use object pooling for repeated combat effects when added.
- Profile on a real mid-range device before adding visual complexity.

## Current limitations
This repository contains starter scripts and design documentation, not a generated Unity scene, final character model, animation set, or APK. Scene assembly and compilation must be performed in Unity Editor on a supported workstation.
