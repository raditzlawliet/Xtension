## Unity Engine - Xtension

a collection mini scripts of helper for general unity engine project.

### What's inside?

Some of script are required to have other package like Corgi Engine, Dialogue System for Unity, Odin Inspector.

This script tested on:

- Unity Engine 2022.3.9f1 (URP Mode)

#### Xtension

- Animations

  - [AnimatorEventListener](Scripts/Animation/AnimatorEventListener.cs) Event receiver for Animation using key-event values _(Req: Odin Inspector 3)_
  - [AnimatorUnityEvent](Scripts/Animation/AnimatorUnityEvent.cs) Call Animator Param with single string (e.g., SetIntenger('param 1'))
  - [AnimatorUnityEventSingleParam](Scripts/Animation/AnimatorUnityEventSingleParam.cs) Similar with AnimatorUnityEvent but for single param. Also good for setter in UnityEvent Inspector to change animator param like transition progress, blended or else

- EditorTools

  - [RaycastPaddingTool](Scripts/EditorTools/Editor/RaycastPaddingTool.cs) Show gizmos for raycast padding property of any UI.Graphic component

- Scene

  - [SceneReference](Scripts/Scene/SceneReference.cs) Is Unity Scene Reference for Inspector based on this gist (https://gist.github.com/JohannesMP/ec7d3f0bcf167dab3d0d3bb480e0e07b)
  - [SceneTools](Scripts/Scene/SceneTools.cs) a Tools to load/unload scene via Unity Inspector or Unity Event

    - [LoadingManager](Scripts/Scene/LoadingManager.cs) (Required for SceneTools), to handle Loading Screen. Put this script on your Loading Scene Gamem Object

- Singleton

  - [Singleton](Scripts/Singleton/Singleton.cs) Base Singleton Pattern, can be use with any component
  - [PersistentSingleton](Scripts/Singleton/PersistentSingleton.cs) Base Presistent Singleton Pattern, can be use with any component

- Timeline

  - [PlayableDirectorControl](Scripts/Timeline/PlayableDirectorControl.cs) Shortcut Pause, Resume and SetSpeed a Playable Director, can be use with UnityEvent

- [PlayableScene](Scripts/PlayableScene/)

  - Run a Timeline/Director Playable on different scene and run on it automatically, (if needed dispose after end).
  - PlayableSceneData holding the ScriptableObject data
  - PlayableSceneLoader for the Loader, you will use it to handling play/stop here
  - PlayableScene for the Scene will be load, Every scene need this attached one

- SplashScreen

  - Basic Common Splash Screen with separation Mobile & Standalonen Mode using [SceneTools](Scripts/Scene/SceneTools.cs)

#### Shader

a collection shaders created using ShaderGraph

- URP
  - Sprite Unlit Dissolve
  - Sprite Unlit Transition

#### ThirdParty

- Corgi Engine

### How to

1. Copy Folder `Assets/TheFlavare` to your `Assets/` folders
2. OR Copy individual script that you will need use

### Contributor

- [@raditzlawliet](https://github.com/raditzlawliet)
