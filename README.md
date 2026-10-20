# TechScape India

TechScape India is an Android augmented reality application that uses a Vuforia image target to display an interactive map of India. Users can select supported states and explore technology ecosystems, major cities, sectors, companies, innovation initiatives, and research highlights.

## Final Android application

The installable ARM64 Android APK is available at:

`Builds/TechScapeIndia-9Markers.apk`

Copy the APK to an Android phone, allow installation from the file manager when prompted, and install it. Use the project scan target in `Documentation/TechScapeIndia_ScanTarget.jpg` to start the AR experience.

## Open the project in Unity

1. Clone the repository with Git LFS enabled:
   ```bash
   git lfs install
   git clone https://github.com/YashrajSh/ARVR_CA3-TechScape.git
   ```
2. Open the project folder in Unity 2022.3 LTS.
3. Allow Unity Package Manager to restore the project dependencies.
4. Add valid Vuforia development credentials in the Vuforia configuration if a new license is needed for testing or rebuilding.
5. Switch the build platform to Android and build for ARM64.

## Project structure

- `Assets/` — Unity scenes, scripts, AR assets, data, images, and models.
- `Packages/` — Unity package manifest and Vuforia package dependency.
- `ProjectSettings/` — Unity project and Android configuration.
- `Documentation/` — scan target and technical documentation.
- `Builds/TechScapeIndia-9Markers.apk` — final Android executable.

## Technology stack

- Unity 2022 LTS
- Vuforia Engine image-target tracking
- C# scripts and JSON-based state information
- Android ARM64 build
