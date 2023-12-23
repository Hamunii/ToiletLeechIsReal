# About Assets

The goal of this project is to make a resource guide and reference documentation to help explain the steps behind the creation and implementation of a custom enemy.
While this is still very much a Work In Progress, I hope it will be useful to someone.


### Note:

Check [README](/README.md) for a somewhat accurate state of the progress so far on this resource!

The contents in this directory are not directly used during the building process, and are excluded in the csproj file.
In the assets folder, you will find a folder “UnityProject”. This unity project file contains everything needed, and gets turned into an asset bundle (when opened with Unity 2022.3.9f1) named "toiletleech", using the accompanying file "toiletleech.manifest". 

All of the software used in this process is available for free download on both Windows and Linux. However, on Linux, it is possible that Unity 2022.3.9f1 might output sounds into the wrong place, which is a known bug in Unity. I don't know a solution for this, but if you don't hear anything in the Unity editor, this might be the issue. Fortunately, the sounds should still work once the asset bundle is exported and the game is launched with your new mod installed.

## Blender

> https://www.blender.org/about/  
Blender is a free and open source 3D creation suite. It supports the entirety of the 3D pipeline—modelling, rigging, animation, simulation, rendering, compositing and motion tracking, even video editing and game creation.

Blender is an amazing program and it can do everything you want when making your 3D model. Files that end with .blend or .blend1 (a backup) are blender projects. While you work with these files, you need to export your model as fbx when importing it to Unity. However, in order to model, rig and animate your models, you will need to learn blender first.  

You can install Blender from https://www.blender.org/download/, or if you are on Linux, I recommend installing the [Flatpak](https://flathub.org/apps/org.blender.Blender) package.

Here are some resources to get started with Blender:

### Basics

**Pro Tip!** Don't press random keys, as Blender has a lot of keyboard shortcuts and you might have no idea what you just did or how to undo it. That said, keyboard shortcuts can speed up your workflow by a lot, and you can use this [Blender Shortcuts Cheat Sheet](https://docs.google.com/document/d/1zPBgZAdftWa6WVa7UIFUqW_7EcqOYE0X743RqFuJL3o/edit?pli=1#heading=h.ftqi9ub1gec3) by Blender Guru, which can be useful.

If you have absolutely no experience with Blender, the 4 first parts of this series will be relevant.  
[Blender 4.0 Beginner Donut Tutorial](https://www.youtube.com/playlist?list=PLjEaoINr3zgEPv5y--4MKpciLaoQYZB1Z) (playlist) - Blender Guru
- [Part 1: Introduction](https://youtu.be/B0J27sf9N1Y?list=PLjEaoINr3zgEPv5y--4MKpciLaoQYZB1Z) - Introduces the very basics of Blender
- [Part 2: Basic Modelling](https://youtu.be/tBpnKTAc5Eo?list=PLjEaoINr3zgEPv5y--4MKpciLaoQYZB1Z) - Self-explanatory (Learn to model a Donut)
- [Part 3: Modelling the Icing](https://youtu.be/AqJx5TJyhes?list=PLjEaoINr3zgEPv5y--4MKpciLaoQYZB1Z&t=42) - introduces more advanced modelling techniques (Detailing)
- [Part 4: Sculpting](https://youtu.be/--GVNZnSROc?list=PLjEaoINr3zgEPv5y--4MKpciLaoQYZB1Z) - Sculpting can be especially useful when modelling organic things

### Modelling

- [Fast Character Modelling with the Skin Modifier in Blender](https://youtu.be/DAAwy_l4jw4) - Joey Carlino
    - Introduces a super cool and easy modelling technique. I recommend this a lot for learning to make basic initial meshes for characters or creatures.

// TODO: add more resources

### Modelling - Common Issues

**My mesh looks inverted in Blender or when imported to Unity.**  
This is because your normals got inverted in one way or another. Select your mesh in Edit Mode, press A to select everything, press Shift+N to recalculate your normals (do not select "Inside", that is the flipped state).  
If this doesn't fix the problem after importing to Unity, you likely have accidentally resized your object by a negative amount. This looks normal in Blender, but not in Unity. To fix this, go into Object Mode, select your object, press Ctrl+A, select apply scale. Now your normals should have flipped in Blender. Now, recalculate normals.

// TODO: add more stuff

### Materials & Texturing, UV Unwrapping

**Note:** Unity does not understand Blender's shader node system. If you use it for anything other than the princibled BSDF, you will have to bake your material as a texture before it will work in Unity. Also make note of the fact that Lethal Company automatically adds its own "style" to everything, so you don't need to worry about that. However, textures are not necessary so you can basically skip this section entirely.

// TODO: add resources

### Rigging

- [Tutorial: My New Rigging Workflow in Blender](https://youtu.be/BiPoPMnU2VI) - Polyfjord
    - Inverse kinematics on a mechanical character. Very useful for rigging legs.
- [Rigging for impatient people - Blender Tutorial](https://youtu.be/DDeB4tDVCGY) - Joey Carlino
    - Includes a lot of useful information about rigging, but it's a very fast paced video. Can be fairly hard to follow for a complete beginner. Likely a better watch after you've seen the more basic introductory type tutorials first.
- [How to Rig and Animate in BLENDER!](https://youtu.be/1khSuB6sER0) - ProductionCrate
    - Learn how to make a rig for a humanoid character, fix issues with Blender's automatic weights feature, as well as inverse kinematics. 

// TODO: add more resources

### Animation & NLA (Nonlinear Animation) Editor

**Note:** We put our individual animations in the NLA Editor so we can use them separately in Unity. The length of the animation in Unity will be the length that you set in the NLA editor. This is important to know if you set an animation cycle to repeat a certain amount of times in Blender when you want to for example preview it in combination with your other animations.

- [The Nuts and Bolts of Blender's animation system](https://youtu.be/p3m57yAcsi0) - CGDive
    - Introduces concepts in a very in-depth way. Introduces Timeline, Dope Sheet, Graph Editor, NLA Editor, Actions.
- [Un-confusing the NLA Editor (Nonlinear Animation)](https://youtu.be/tAo7HxxxA08) - GCDive
    - A more in-depth video about the NLA Editor. Do note though, we do not need to do anything complex with the NLA Editor.
- [Become a PRO at Animation in 25 Minutes | Blender Tutorial](https://youtu.be/_C2ClFO3FAY) - CG Geek
    - Animating a walk cycle. Uses Timeline, Dope Sheet and Graph Editor. Uses references for animation.
- [Character animation for impatient people - Blender Tutorial](https://youtu.be/GAIZkIfXXjQ) - Joey Carlino
    - If you don't want to make and rig your own models.

// TODO: add more resources

### Exporting Assets For Unity

Export the model as fbx
// TODO: write the rest

## Unity

IMPORTANT! Lethal Company uses Unity version 2022.3.9f1, and therefore we must use it too in order to avoid any issues with version differences when exporting our asset bundles.  
You can download Unity Hub (which is where you install 2022.3.9f1) from https://unity.com/download, https://unity.com/releases/editor/whats-new/2022.3.9 or if you are on Linux, you should probably use the unofficial [Flatpak](https://flathub.org/apps/com.unity.UnityHub) package, or follow [these install instructions](https://docs.unity3d.com/hub/manual/InstallHub.html#install-hub-linux) if you truly despise Flatpak.

You can open the Unity project by choosing to open a project from disk, and selecting the UnityProject folder. When Unity has loaded the project, look into the ToiletLeech folder for the assets that make up the asset bundle.

### Setting Up The Unity Project

**Note:** The [SETUP-PROJECT.py](/SETUP-PROJECT.py) script will copy all of the dlls files for you! So if you run it, you can ignore this section almost entirely.

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

Also, you would probably normally want to add `BepInEx.Harmony.dll` too from the same location, but it causes Unity to crash when building asset bundles, so we don't add it. But we don't seem to need that file anyways for making our asset bundle. If you know why this is, please tell about it!

We also depend on LethalLib by Evaisa (which is already included in the project), and it depends on MMHOOK, so you need to run the game once with MMHOOK so these dll files are generated:
>- MMHOOK_AmazingAssets.TerrainToMesh.dll
>- MMHOOK_Assembly-CSharp.dll
>- MMHOOK_ClientNetworkTransform.dll
>- MMHOOK_DissonanceVoip.dll
>- MMHOOK_Facepunch.Steamworks.Win64.dll
>- MMHOOK_Facepunch Transport for Netcode for GameObjects.dll

The dll file of this mod also needs to be there so we can reference ToiletLeechAI from a component of the Toilet Leech prefab in Unity. It needs to be from the dll file, you cannot just copy and paste the ToiletLeechAI.cs file in the Unity project because asset bundles cannot contain scripts, and it just doesn't get the reference otherwise. You know it doesn't get the reference in the form of a yellow warning text if you launch the game with the mod and you have unity logging enabled in the `BepInEx.cfg` file.

### What Are Asset Bundles?

> https://docs.unity3d.com/Manual/AssetBundlesIntro.html  
An AssetBundle is an archive file that contains platform-specific non-code Assets (such as Models, Textures, Prefabs, Audio clips, and even entire Scenes) that Unity can load at run time. AssetBundles can express dependencies between each other; for example, a Material in one AssetBundle can reference a Texture in another AssetBundle. For efficient delivery over networks, you can compress AssetBundles with a choice of built-in algorithms depending on use case requirements (LZMA and LZ4).
>
> AssetBundles can be useful for downloadable content (DLC), reducing initial install size, loading assets optimized for the end-user’s platform, and reduce runtime memory pressure.
>
> Note: An AssetBundle can contain the serialized data of an instance of a code object, such as a ScriptableObject. However, the class definition itself is compiled into one of the Project assemblies. When you load a serialized object in an AssetBundle, Unity finds the matching class definition, creates an instance of it, and sets that instance’s fields using the serialized values. This means that you can introduce new items to your game in an AssetBundle as long as those items do not require any changes to your class definitions.

Asset bundles are a way for us to basically transfer our enemy from our Unity project to Lethal Company.

### Our Toilet Leech Assets In Unity

We have made a ToiletLeech folder in our Unity project. Everything that goes into our asset bundle is in there.
The first thing we did was import our fbx model into Unity. It contains all the materials, textures and animations of our model when first imported, but it is good to separate some of that stuff into their own folders. We have extracted our materials into the `Materials` folder.

We have also copied the individual animations into the `Animations` folder, because I don't know how to separate them properly, but we can just ignore the animations embedded in the fbx file and use the copies inside the `Animations` folder anyways.

And at this point of writing this, I realised we have two, probably identical versions of the fbx model in the project, one in the `Models` folder, and one directly in `ToiletLeech`, which is basically our root folder. We are meant to only have the one in `Models` and use that, **but I will fix that later. Also note to self: remove the music.cs file too. It does nothing.** At least, it should.

Anyways, how do we make the game see our assets as an enemy? Well, we create a new Scriptable Object of type EnemyType. This is what the game uses, so we need it too. Do note that these thingies come from the Lethal Company Unity Template this is based off of. Do also note that our UnityProject in this repository is already configured properly *(for other than the missing stuff, like right click showing enemy name and enemy info page getting added to terminal. It will be added when I have time to do so)*.  
![Screenshot: Create object Enemy Type](./ForTutorial/CreateObjectEnemyType.png)

The EnemyType thingy has some configuration options, and the most important thing is the "Enemy Prefab" part of it. This is where we tell it what the model and whatever stuff our EnemyType has. Also note the "Enemy Name" thingy, this will be the name of the ToiletLeech enemy in the coding side of things.

// TODO: write the rest

### Adding Things To An Asset Bundle

To add a thing to an asset bundle, you first need to select the object you want to add, and then on the asset bundle dropdown, select "New..." and write the name of your asset bundle. Or if you already have an asset bundle, you can just select that. You don't actually need to assign everything you need to the asset bundle as long as the thing you assigned to the asset bundle depends on the rest of the things, which in our case is true.  
![Screenshot: assign to asset bundle](./ForTutorial/AssignToAssetBundle.png)

### How To Build An Asset Bundle:

1. Open asset bundle browser (this plugin is included in the Lethal Company Unity Template):  
![Screenshot: open asset bundle browser](./ForTutorial/OpenAssetBundleBrowser.png)
2. Here we can see files that are included in our bundle. The ones that have the bundle as "auto" are things that our thing we have assigned to the asset bundle depends on, so they will be included as well. I don't know what modassets really is, it came with the Lethal Company Unity Template too. Should probably ask Evaisa, but anyways we can ignore it.  
![Screenshot: Toilet Leech bundle preview](./ForTutorial/ToiletLeechBundlePreview.png)
3. This is where we build our asset bundle. The asset bundle will be found where output path specifies, which in this case exists in a directory in the root of the Unity project.  
![Screenshot: build asset bundle](./ForTutorial/BuildAssetBundle.png)
4. Then we copy both `toiletleech` and `toiletleech.manifest` to the root of this repository. (Actually, we could probably just reference them without copy pasting them as they exist in this repository already. If you try this and it works, and you might have to edit the csproj file for that, please open an issue or a pull request. I don't have time to do that right now.)

Note: if you don't have Windows standalone build support installed in your Unity installation, close unity and install it from Unity Hub. I'm not 100% sure if this is actually needed, but I had no luck getting the materials of the model working in the asset bundle when I had my build target set to Linux, which I didn't realize could affect anything.

// TODO: write the rest