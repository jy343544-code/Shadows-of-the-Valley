# Cloud Android Build — Shadows of the Valley

## What is prepared
- Unity 6.3 LTS project configuration.
- Editor automation that creates a greybox Black Gate arena.
- Android ARM64 APK build script.
- GitHub Actions workflow that can build an APK and upload it as a downloadable artifact.

The generated scene is a functional placeholder arena (capsule hero, training enemies, Gatekeeper boss, basic lighting). It is not the final realistic 3D art pass.

## Why the first cloud build stopped
The GitHub Actions runner started successfully, then stopped before checking out or compiling the project because the repository does not yet contain a `UNITY_LICENSE` secret. The run log explicitly reported that the secret was empty. This is a licensing gate, not evidence of a C# compile error.

The legacy GameCI activation-file action was tested and returned a deprecation error, so it has been removed rather than left as a broken workflow.

## One required account setup
Unity Editor must be activated before a hosted runner can build. A valid Unity Personal license XML must be obtained through Unity's supported activation flow and stored as a GitHub Actions secret named `UNITY_LICENSE`.

Do not paste license XML, Unity passwords, or access tokens into ChatGPT or commit them to the repository.

On GitHub mobile:
1. Open this repository: https://github.com/jy343544-code/Shadows-of-the-Valley
2. Open **Settings**.
3. Open **Secrets and variables** → **Actions**.
4. Tap **New repository secret**.
5. Set the name to `UNITY_LICENSE`.
6. Paste the valid Unity license XML as the value and save.

If you do not already have a valid license XML, use Unity's currently supported licensing/activation process for your Unity Personal account. Unity's current GameCI guidance is here: https://game.ci/docs/github/activation

## Start the APK build after licensing
1. Open the repository's **Actions** tab.
2. Select **Android APK Build**.
3. Press **Run workflow** on branch `main`.
4. If the run succeeds, open it and download the artifact named `Shadows-of-the-Valley-Android-APK`.
5. Extract the ZIP and install the APK on Android.

The build workflow is manual-only until the Unity license secret is configured, so ordinary code updates will not create repeated failed build runs.

## Current limitations
- The first build is a greybox gameplay prototype, not a finished PlayStation-quality game.
- The repository's C# scripts have not yet been compiled in this environment.
- APK generation is blocked until a valid Unity license secret is configured and the hosted build succeeds.
- Realistic character/monster models, final animation clips, sound, VFX, touch HUD, and device optimization remain production tasks.
