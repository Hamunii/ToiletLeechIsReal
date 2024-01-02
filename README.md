# Toilet Leech Is Real

> [!NOTE]  
> This project is still a WIP. See the progress at the bottom of this page

This repository contains the full source code for the Toilet Leech enemy for Lethal Company, including the Unity project which can be used to make an asset bundle. This project is designed to be used as a template/reference for creating your own modded enemy, and even comes with [a guide](/Assets/AboutAssets.md)! The guide explains the asset bundle side of Toilet Leech, as it is not as straight forward as the coding side. The code of this project can be found under the `src` directory, and as you can see, there doesn't need to be much code for a custom enemy.

### Setting Up The Project For Development

After copying this repo for yourself, run [SETUP-PROJECT.py](/SETUP-PROJECT.py) (make sure you have Python installed) to copy all the DLL files from the game files that the C# project and the Unity project depend on. However, make sure you have https://thunderstore.io/c/lethal-company/p/Evaisa/HookGenPatcher/ installed and ran the game at least once with it so it can generate a few files. That should be all the required setup for this project, and now you can move to coding AI or making your own 3D models for your custom enemy. Good luck! And make sure you read [the guide](/Assets/AboutAssets.md)!

### Looking for a mod install link?
This is the GitHub repository where the source code of this mod is hosted. **Mod files are not found here**, but will be uploaded to Thunderstore once this mod is ready enough.

### AboutAssets.md Resource Progress

> [!NOTE]  
> Contributions are welcome! This will help new modders get started with making custom enemies, so it would be super awesome if you contributed your knowledge and shared some useful resources! Feel free to open an issue or ping me on discord under the toilet leech post or DM me! This resource could possibly find its way to the Lethal Company Modding Wiki.

- [ ] Blender resources
    - [x] Basics
    - [ ] Modeling
    - [ ] Materials & texturing, UV Unwrapping
    - [x] Rigging
    - [x] Animation
    - [x] How to export model for Unity
    - [ ] Common issues and how to fix them
- [ ] Unity resources
    - [x] What are asset bundles?
    - [x] How to add stuff to asset bundles
    - [x] Building asset bundles
    - [x] How Toilet Leech is configured
    - [x] Importing fbx models (your model from blender)
    - [ ] What are prefabs
    - [ ] Referencing our [AI script](src/ToiletLeechAI.cs) in a prefab

### Mod Progress

- [x] Mod builds, enemy works in game, even if the AI is super basic
- [x] All enemy components are there (right click shows enemy name, enemy info page gets added to terminal)
- [ ] More complex, interesting AI
    - [ ] likes music?
    - [ ] 2 or 3 spit attacks used in short succession: enemy gets exhausted and the head gets smaller and it will be at its weakest. A good moment to flush the toilet
    - and more...
- [ ] Enemy attacks
    - [ ] spit attack: will slow player movement
    - [ ] suck attack: will suck players who are stuck or get too close
    - [ ] fire attack????