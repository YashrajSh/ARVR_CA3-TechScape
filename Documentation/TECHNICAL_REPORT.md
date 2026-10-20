# TechScape India — Technical Report

## Project technology and version

- **Engine:** Unity `2022.3.11f1` (LTS)
- **Render pipeline:** Built-in Render Pipeline (no URP/HDRP package is declared)
- **AR SDK:** Vuforia Engine `10.21.3`
- **Primary target:** Android; minimum SDK is API 27 and ARM64 is enabled.

## Original architecture

The original KalaKunj app is a Unity/Vuforia image-target AR application. `Main.unity` contains the Vuforia `ImageTarget` configured against `StreamingAssets/Vuforia/KalaKriti.xml`. The India map is composed of separately selectable scene-linked state objects. A click uses Unity EventSystem pointer events through `StateToScene.cs` and opens a dedicated state scene.

State information was not data-driven: each state scene serialized its own culture heading, paragraph and image. The chatbot screen used `ChatGPT.cs` and the OpenAI Unity package, with a culture prompt and `Assets/context.txt`. Its API key was a placeholder embedded in code.

## Main scenes and folders

- `Assets/Scenes/Onboarding.unity` — launch/onboarding.
- `Assets/Scenes/Main.unity` — Vuforia AR camera, image target, India map and clickable states.
- `Assets/Scenes/AskSanskriti.unity` — chatbot UI (now branded TechMitra at runtime).
- `Assets/Other Scenes/Scenes/` — one legacy detail scene per selectable state.
- `Assets/StreamingAssets/Vuforia/` — Vuforia image target datasets. Do not change these without reauthoring and testing the target.
- `Assets/Resources/VuforiaConfiguration.asset` — Vuforia configuration/licence asset. Preserve it.
- `Assets/Other Scenes/Images/` — original culture imagery; preserved for non-destructive migration but no longer shown by TechScape state presentation.

## Migration implemented

`TechScapeStateData.json` is the source of truth for ten detailed state profiles: Karnataka, Maharashtra, Telangana, Tamil Nadu, Kerala, Gujarat, Uttar Pradesh, West Bengal and Rajasthan. All other selectable existing regions receive a clearly labelled generic technology placeholder rather than their former art text.

`TechScapeRuntimePresenter.cs` replaces only runtime-facing state headings, content, calls to action, chatbot labels and the state visual. It leaves scene hierarchy, Vuforia target, transforms, map models, colliders and serialized button bindings unchanged. `TechScapeIndiaVisual.png` is a generated, project-local technology visual used in state screens.

`StateToScene.cs` now records the selected state before loading its existing scene. `SceneNavigator.cs` now returns a state-detail screen to `Main`, avoiding the original build-index navigation bug. `ChatGPT.cs` retains the original UI binding/class name, becomes TechMitra, receives selected-state context, and falls back to a concise local knowledge response when no key/network/API response is available.

## External services and security

- Vuforia requires the existing licence configuration and image-target files.
- The OpenAI Unity package is declared in `Packages/manifest.json` from its Git repository.
- TechMitra API access is optional. Enter a real key into the **ChatGPT component's Api Key** field in `AskSanskriti.unity` through the Unity Inspector, or set `OPENAI_API_KEY` for editor testing. Do not commit a key.
- The currently configured model is `gpt-6-astra` through the existing Chat Completions integration. Verify that the account has access before a live demo; if it does not, TechMitra automatically uses the built-in offline guide. The installed Unity package is older, so a later enhancement can migrate it to the Responses API rather than changing the AR project now.

## Files that should not be modified casually

Do not alter `Assets/StreamingAssets/Vuforia/`, `Assets/Resources/VuforiaConfiguration.asset`, the Vuforia package/archive, `ProjectSettings/ProjectSettings.asset`, map transforms/colliders in `Main.unity`, or generated directories (`Library`, `Temp`, `Logs`, `obj`). A new Unity import may recreate generated directories automatically.

## Risks and safe strategy

The map's ImageTarget, state object names and click bindings are serialized Unity references, so wholesale scene rewrites can break AR tracking or taps. Keep future changes data-first: extend `TechScapeStateData.json`, then adjust `TechScapeRuntimePresenter` only when the visible layout needs a new field. Verify on a real camera target after any Vuforia dataset or scene change.

## Manual AR test checklist

1. Open the project with Unity 2022.3.11f1 and let packages import.
2. In Build Settings, select Android and keep the existing API level/ARM64 settings.
3. Run `Onboarding`, open the AR experience and grant camera permission on the phone.
4. Point at the existing KalaKunj image target; confirm the India map appears in the same location and scale.
5. Tap Karnataka, Maharashtra, Telangana, Tamil Nadu, Kerala, Gujarat, Uttar Pradesh, West Bengal or Rajasthan; confirm technology information and the blue TechScape visual appear.
6. Tap Back; confirm the app returns to the AR map.
7. Open TechMitra, ask a state question after selecting Karnataka, then select Telangana and ask "What are the major startups here?" Confirm the context changes.
8. Test without an API key/network; confirm TechMitra gives an offline response and the app remains usable.
9. Build an APK or AAB using Unity's Android Build Settings and install on an ARM64 Android device.

## Viva explanation

TechScape India uses a mobile camera and Vuforia image tracking to anchor an interactive India map in augmented reality. Each state object raises a pointer-click event, stores a state identifier and opens the existing detail experience. A JSON dataset supplies technology hubs, sectors, companies, startups, institutions and initiatives; the runtime presentation layer converts that data into mobile UI without rebuilding the AR scene. TechMitra receives the selected-state context so ambiguous questions such as “What are the startups here?” can be interpreted geographically. Unlike a static information app, AR places the geographically organised data in the user's physical environment and combines spatial interaction with context-aware assistance.

`Mobile Camera → Vuforia Image Tracking → India Map → State Interaction Layer → State Data Manager → Dynamic Information UI → TechMitra`
