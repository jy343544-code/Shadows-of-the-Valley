# Shadows of the Valley — Godot Android Prototype

This is a separate Godot implementation of the Black Gate vertical-slice prototype. The original Unity project files remain untouched.

## Engine and build

- Godot 4.7.2
- GDScript (no Unity license or C# runtime required)
- Android ARM64 debug APK
- GitHub Actions workflow: `.github/workflows/godot-android-build.yml`
- No Unity license secret is required for this workflow.

## Prototype features

- Third-person player movement and follow camera
- Touch movement area and camera drag
- Attack, jump, dodge, and shadow-pull buttons
- Four basic shadow raiders with chase and attack behavior
- Health bar, arena, pillars, and Black Gate placeholder geometry
- Offline single-player prototype

## Build

Run the **Godot Android APK Build** workflow from GitHub Actions. The build uses the pinned `barichello/godot-ci:4.7.2` container, which includes Godot, Android SDK/JDK configuration, and matching export templates.

This is an early playable prototype, not a finished commercial game. Character art, animation, sound, boss encounters, save systems, performance tuning, and device testing remain future work.
