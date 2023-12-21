# About Assets

Here you will find information regarding our assets and how to work with them.

### Note:

The contents in this directory are not directly used during the building process, and are exluded in the csproj file.  
The unity project constains everything that gets turned into an asset bundle named "toiletleech", with the accompanying file "toiletleech.manifest".

All of the software used in this process are available on both Windows and Linux. However, on Linux, it is possible that Unity 2022.3.9f might output sounds into the wrong place, which is a bug in Unity. I don't know a solution for this, but if you don't hear anything in the Unity editor, this might be the issue. The sounds should still work when the asset bundle is exported and the game is launched with the mod.

## Blender

> https://www.blender.org/about/  
Blender is the free and open source 3D creation suite. It supports the entirety of the 3D pipeline—modeling, rigging, animation, simulation, rendering, compositing and motion tracking, even video editing and game creation.

Files that end with .blend or .blend1 (a backup) are blender projects. In order to model, rig and animate your models, you need to learn blender first. Here are some resources to get started with Blender:

### Modeling

// TODO: add resources

### Rigging

// TODO: add resources

### Animation & Nonlinear Animation editor

// TODO: add resources

### Exporting assets for Unity

Export the model as fbx
// TODO: write the rest

## Unity

Important! Lethal Company uses Unity version 2022.3.9f, and therefore we use it too in order to avoid any issues with version differences when exporting our asset bundles.

You can open the Unity project by choosing to open a project from disk, and selecting the UnityProject folder. When Unity has loaded the project, look into the ToiletLeech folder for the assets that make up the asset bundle.

### Setting up the Unity project

The Unity project we have is based off of Evaisa's [Lethal Company Unity Template](https://github.com/EvaisaDev/LethalCompanyUnityTemplate/) (hence the LICENSE file in that folder. I have no idea if I can remove it or not). HOWEVER, just like with our dlls in the root directory of this repository, we need to add some dll files into our `UnityProject/Assets/Plugins` folder. These are listed in the README of Evaisa's repository, but here's the list so you don't miss it:
>- AmazingAssets.TerrainToMesh.dll
>- ClientNetworkTransform.dll
>- DissonanceVoip.dll
>- Facepunch Transport for Netcode for GameObjects.dll
>- Facepunch.Steamworks.Win64.dll
>- Newtonsoft.Json.dll
>- Assembly-CSharp-firstpass.dll

This part needs confirmation, as I have not tested the project without these dll files and I don't know anything about this stuff.
Anyways, you might also need to add these files from `Lethal Company/BepInEx/core`:
>- 0Harmony20.dll
>- 0Harmony.dll
>- BepInEx.dll
>- BepInEx.Preloader.dll
>- HarmonyXInterop.dll
>- Mono.Cecil.dll
>- Mono.Cecil.Mdb.dll
>- Mono.Cecil.Pdb.dll
>- Mono.Cecil.Rocks.dll
>- MonoMod.RuntimeDetour.dll
>- MonoMod.Utils.dll

Also, you might want to add `BepInEx.Harmony.dll` too from the same location, but for me it causes Unity to crash so I don't have it. Idk if this is for everyone, but we don't seem to need that file anyways for making the asset bundle.

We also depend on LethalLib by Evaisa (which is already included in the project), and it depends on MMHOOK, so I think you need to run the game once with MMHOOK add their dlls too I guess. (actually I have no idea about any of this and I'm probably wrong considering we already have non-MMHOOK dlls included, so please try building the asset bundle without these, and if the mod works with that asset bundle, message me. I don't feel like wasting time on this right now):
>- MMHOOK_AmazingAssets.TerrainToMesh.dll
>- MMHOOK_Assembly-CSharp.dll
>- MMHOOK_ClientNetworkTransform.dll
>- MMHOOK_DissonanceVoip.dll
>- MMHOOK_Facepunch.Steamworks.Win64.dll
>- MMHOOK_Facepunch Transport for Netcode for GameObjects.dll

The dll file of this mod also needs to be there so we can reference ToiletLeechAI from a component of the Toilet Leech prefab in Unity. It needs to be from the dll file, you cannot just copy and paste the ToiletLeechAI.cs file in the Unity project because asset bundles cannot contain scripts, and it just doesn't get the reference otherwise. You know it doesn't get the reference in the form of a yellow warning text if you launch the game with the mod and you have unity logging enabled in the `BepInEx.cfg` file.


### How to build an asset bundle:

1. Open asset bundle browser (this plugin is included in the Lethal Company Unity Template):  
![Screenshot: open asset bundle browser](./ForTutorial/OpenAssetBundleBrowser.png)
2. Here we can see files that are included in our bundle. I don't know what modassets really is, it came with the Lethal Company Unity Template too. Should probably ask Evaisa, but anyways we can ignore it.  
![Screenshot: Toilet Leech bundle preview](./ForTutorial/ToiletLeechBundlePreview.png)
3. This is where we build our asset bundle. The asset bundle will be found where output path specifies, which in this case exists in a directory in the root of the Unity project.  
![Screenshot: build asset bundle](./ForTutorial/BuildAssetBundle.png)
4. Then we copy both `toiletleech` and `toiletleech.manifest` to the root of this repository. (Actually, we could probably just reference them without copy pasting them as they exist in this repository already. If you try this and it works, and you might have to edit the csproj file for that, please open an issue or a pull request. I don't have time to do that right now.)

Note: if you don't have Windows standalone build support installed in your Unity installation, close unity and install it from Unity Hub. I'm not 100% sure if this is actually needed, but I had no luck getting the materials of the model working in the asset bundle when I had my build target set to Linux, which I didn't realize could affect anything.

// TODO: write the rest