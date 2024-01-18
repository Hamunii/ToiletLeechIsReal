# About Assets

This guide is a part of the full project, which includes full source code to our example enemy: the Toilet Leech. You can grab your own copy of this repository and start working on your modded enemy, using this guide and everyting included as a reference for how to get things working!  
While this is still a Work In Progress, I hope it will be useful!

> [!NOTE]  
> Check [README](/README.md) for a somewhat accurate state of the progress so far on this resource!  
> Also, this is a temporary place for this guide while it's still unfinished.

The contents in this directory are not directly used during the building process, and are excluded in the csproj file.  
In the assets folder, you will find a folder “UnityProject”. This unity project file contains everything needed, and we use Unity 2022.3.9f1 to build an asset bundle named "toiletleech", using the accompanying file "toiletleech.manifest". 

All of the software used in this process is available for free download on both Windows and Linux. However, on Linux, Unity 2022.3.9f1 has a bug where might output audio to the wrong output. If this happens, you can try using [pavucontrol](https://flathub.org/apps/org.pulseaudio.pavucontrol) to change Unity's audio output (appears as `FMOD Ex App`) to the correct one.

## Blender

> https://www.blender.org/about/  
Blender is a free and open source 3D creation suite. It supports the entirety of the 3D pipeline—modeling, rigging, animation, simulation, rendering, compositing and motion tracking, even video editing and game creation.

Blender is an amazing program and it can do everything you want when making your 3D model. Files that end with .blend or .blend1 (a backup) are blender projects. While you work with these files, you need to export your model as fbx when importing it to Unity. However, in order to model, rig and animate your models, you will need to learn blender first.  

You can install Blender from https://www.blender.org/download/, or if you are on Linux, I recommend installing the [Flatpak](https://flathub.org/apps/org.blender.Blender) package.

**For a list of resources to get started with Blender and to learn how to properly export your assets for Unity, see the [page on Blender](./Blender.md).**

## Unity

> [!IMPORTANT]  
> Lethal Company uses Unity version 2022.3.9f1, and therefore we must use it too in order to avoid any issues with version differences when exporting our asset bundles.  

You can download Unity Hub (which is where you install 2022.3.9f1) from https://unity.com/download, https://unity.com/releases/editor/whats-new/2022.3.9, or if you are on Linux, you should probably use the unofficial [Flatpak](https://flathub.org/apps/com.unity.UnityHub) package, or follow [these install instructions](https://docs.unity3d.com/hub/manual/InstallHub.html#install-hub-linux) if you truly despise Flatpak.

**For the Unity part of this guide, see the [page on Unity](./Unity.md).**