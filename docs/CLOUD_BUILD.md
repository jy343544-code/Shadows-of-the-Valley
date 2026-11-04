# Cloud Android Build — Shadows of the Valley

## What is now automated
The repository contains a Unity 6.3 LTS project manifest, a generated greybox scene builder, and a GitHub Actions workflow that can build an Android APK and store it as a downloadable workflow artifact.

The generated scene is a functional placeholder arena (capsule hero, training enemies, Gatekeeper boss, basic lighting). It is not the final realistic 3D art pass.

## One-time Unity license requirement
Unity's editor must be licensed even on a cloud build runner. The workflow expects a Unity Personal license XML stored as a GitHub Actions repository secret named `UNITY_LICENSE`.

Do not paste license XML, passwords, or access tokens into ChatGPT or commit them to the repository.

To add it:
1. Open the Unity ID account in a browser and activate Unity Personal on the account.
2. Generate/export the Unity license XML using Unity's official manual activation process.
3. Open this repository on GitHub.
4. Go to **Settings → Secrets and variables → Actions → New repository secret**.
5. Name it exactly `UNITY_LICENSE`, paste the XML as the value, and save.

The GitHub Actions workflow will then be able to use that secret without exposing it in the repository.

## Start the build
1. Open the repository's **Actions** tab.
2. Select **Android APK Build**.
3. Press **Run workflow** on branch `main`.
4. When the run succeeds, open the run and download the artifact named `Shadows-of-the-Valley-Android-APK`.
5. Extract the ZIP and install the APK on an Android device (allow installation from the browser/files app if Android asks).

A push to the Unity project files on `main` also triggers the workflow.

## Current limitations
- The first build is a greybox gameplay prototype, not a finished PlayStation-quality game.
- The repository's C# scripts have not yet been compiled in this environment.
- Android APK generation requires the Unity license secret and a successful hosted runner.
- Realistic character/monster models, final animation clips, sound, VFX, touch HUD, and device optimization are still separate production tasks.
