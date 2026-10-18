# rts-unity-client

This repository contains the Unity project scaffold for a mobile RTS game inspired by classic base-building strategy games, but redesigned for touch-first Android gameplay.

## Features planned
- Mobile-first touchscreen camera controls
- Tap-to-select / drag selection
- RTS unit and building flow
- Resource gathering and base building
- 1v1 / 2v2 / 3v3 / 4v4 lobby system
- Shop integration and promo unlock flow
- Server-backed account and member page
- Multiplayer chat and moderation hooks

## Unity setup
1. Open this folder in Unity Hub.
2. Create or open a Unity 2022 LTS project using this repo as the project root.
3. Set the project to use URP.
4. Install required packages:
   - Input System
   - Unity UI
   - TextMeshPro
   - Cinemachine
   - Navigation
5. Create a scene named `GameScene` and add the core gameplay objects.

## Recommended folder structure
- `Assets/Scripts/Core`
- `Assets/Scripts/Gameplay`
- `Assets/Scripts/UI`
- `Assets/Scripts/Networking`
- `Assets/Scripts/Shop`
- `Assets/Scripts/Chat`
- `Assets/Scripts/AI`
- `Assets/Scenes`
- `Assets/Prefabs`

## Next steps
- Add input handlers for touch and mouse
- Add an RTS camera controller
- Add a selectable base and unit
- Add a minimap stub
- Hook to backend member and promo APIs

## License
MIT
