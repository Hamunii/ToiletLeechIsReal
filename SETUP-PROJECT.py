#!/usr/bin/env python
import os
import shutil

# This is an automated script for copying required dll files into this project.
# Supports both Windows* and Linux.
# *not tested on Windows
#
# Also, this script got kinda out of hand towards the end. I won't waste more time on this though, it works anyways.
# Feel free to contribute to this script

class color:
   reset = '\033[0m'
   green = '\033[32m'
   yellow = '\033[93m'
   red = '\033[31m'
   lightblue = '\033[94m'
   lightcyan = '\033[96m'

# Locate our dlls directory in this repo
thisPath = os.getcwd()
dllsRelative = 'dlls'
dllDestination = f'{thisPath}/{dllsRelative}'
if not os.path.exists(dllDestination):
   print(color.red + f'Setup script could not find path: {dllDestination}.{color.yellow}\nMake sure you run this script from the root of the repo directory.')
   exit()

# Locate the game's data folder
gameFilesPath = None
expectedGamePaths = ['C:/Program Files (x86)/Steam/steamapps/common/Lethal Company', f'{os.path.expanduser('~')}/.local/share/Steam/steamapps/common/Lethal Company']
for path in expectedGamePaths:
    if os.path.exists(path):
      gameFilesPath = path
      break

if gameFilesPath is None:
   print(color.yellow + "Could not locate Lethal Company game files!\nPlease paste the full path to 'Lethal Company':" + color.reset)
   userInputGamePath = input()
   if os.path.exists(userInputGamePath):
      gameFilesPath = userInputGamePath
   else:
      print(color.red + "Could not find location. Exiting the program.")
      exit()
print(color.lightblue + f'Game data path found: {gameFilesPath}' + color.reset)

# Copy dlls for C# project
print('Copying for C# project:')
gameDataRelative = 'Lethal Company_Data/Managed'
neededGameDllFiles = ["Assembly-CSharp.dll", "Unity.Netcode.Runtime.dll", "UnityEngine.CoreModule.dll"]
for dllFile in neededGameDllFiles:
   shutil.copy2(f'{gameFilesPath}/{gameDataRelative}/{dllFile}', f'{dllDestination}/{dllFile}')
   print(f'Got: {dllsRelative}/{dllFile}')

print(color.green + f'Done copying to {dllDestination}!' + color.reset)

# Make sure our Unity project still exists
unityProjectPath = f'{thisPath}/Assets/UnityProject'
unityPluginsRelative = 'Assets/Plugins'
if not os.path.exists(unityProjectPath):
   print(color.yellow + f'Could not find Unity project at {unityProjectPath}! Paste the full path to your Unity project:' + color.reset)
   userInputUnityPath = input()
   if os.path.exists(userInputUnityPath):
      unityProjectPath = userInputUnityPath
   else:
      print(color.red + "Could not find location. Exiting the program.")
      exit()

if not os.path.exists(f'{unityProjectPath}/{unityPluginsRelative}'):
   print(color.red + f"Your Unity Project does not have a {unityPluginsRelative} folder!\nMake sure your Unity project is based off of Evaisa's Lethal Company Unity Template. Exiting program.")
   exit()
print(color.lightblue + f'Unity Plugins path found: {unityProjectPath}/{unityPluginsRelative}' + color.reset)

# Copying dlls for Unity project
print('Copying game DLLs for Unity project:')
neededPluginDllFiles =[
   "AmazingAssets.TerrainToMesh.dll",
   "ClientNetworkTransform.dll",
   "DissonanceVoip.dll",
   "Facepunch Transport for Netcode for GameObjects.dll",
   "Facepunch.Steamworks.Win64.dll",
   "Newtonsoft.Json.dll",
   "Assembly-CSharp-firstpass.dll"
]
for dllFile in neededPluginDllFiles:
   shutil.copy2(f'{gameFilesPath}/{gameDataRelative}/{dllFile}', f'{unityProjectPath}/{unityPluginsRelative}')
   print(f'Got: {unityPluginsRelative}/{dllFile}')

print(color.green + f'Done copying game DLLs to {unityProjectPath}/{unityPluginsRelative}!' + color.reset)

#######################################################################################
# This is non-game dll territory
r2modmanPath = None
expectedr2modmanPaths = [f'{os.path.expanduser('~')}/AppData/Roaming/r2modmanPlus-local/LethalCompany/profiles', f'{os.path.expanduser('~')}/.config/r2modmanPlus-local/LethalCompany/profiles']
for path in expectedr2modmanPaths:
    if os.path.exists(path):
      r2modmanPath = path
      print(color.lightblue + f'r2modman Lethal Company path found: {r2modmanPath}' + color.reset)
      break

neededCoreDlls = [
   "0Harmony20.dll",
   "0Harmony.dll",
   "BepInEx.dll",
#  "BepInEx.Harmony.dll", Unity does not like this dll and causes a crash when building asset bundle
   "BepInEx.Preloader.dll",
   "HarmonyXInterop.dll",
   "Mono.Cecil.dll",
   "Mono.Cecil.Mdb.dll",
   "Mono.Cecil.Pdb.dll",
   "Mono.Cecil.Rocks.dll",
   "MonoMod.RuntimeDetour.dll",
   "MonoMod.Utils.dll"
]
gotCoreFiles = None
gotMMHOOKFiles = None
if r2modmanPath is not None:
   for dir in os.listdir(r2modmanPath):
      if os.path.exists(f'{r2modmanPath}/{dir}/BepInEx/core'):
         gotFiles = True
         for dllFile in neededCoreDlls:
            shutil.copy2(f'{r2modmanPath}/{dir}/BepInEx/core/{dllFile}', f'{unityProjectPath}/{unityPluginsRelative}')
            print(f'Got: {unityPluginsRelative}/{dllFile}')
      if os.path.exists(f'{r2modmanPath}/{dir}/BepInEx/Plugins/MMHOOK'):
         gotMMHOOKFiles = True
         for dllFile in os.listdir(f'{r2modmanPath}/{dir}/BepInEx/Plugins/MMHOOK'):
            shutil.copy2(f'{r2modmanPath}/{dir}/BepInEx/Plugins/MMHOOK/{dllFile}', f'{unityProjectPath}/{unityPluginsRelative}')
            print(f'Got: {unityPluginsRelative}/{dllFile}')

# Testing against non-r2modman installation if the mods exist there
if not gotCoreFiles:
   if os.path.exists(f'{gameFilesPath}/BepInEx/core'):
      gotCoreFiles = True
      for dllFile in neededCoreDlls:
         shutil.copy2(f'{gameFilesPath}/BepInEx/core/{dllFile}', f'{unityProjectPath}/{unityPluginsRelative}')
         print(f'Got: {unityPluginsRelative}/{dllFile}')

if not gotMMHOOKFiles:
   if os.path.exists(f'{gameFilesPath}/BepInEx/Plugins/MMHOOK'):
      gotMMHOOKFiles = True
      for dllFile in os.listdir(f'{gameFilesPath}/BepInEx/Plugins/MMHOOK'):
         shutil.copy2(f'{gameFilesPath}/BepInEx/Plugins/MMHOOK/{dllFile}', f'{unityProjectPath}/{unityPluginsRelative}')
         print(f'Got: {unityPluginsRelative}/{dllFile}')

if not gotCoreFiles:
   print(color.red + f"No BepInEx/core directory found! Please install r2modman or BepInEx manually.")
   exit()

if not gotMMHOOKFiles:
   print(color.red + f"No MMHOOK directory found! Please do the following:\n"
   "1) Install https://thunderstore.io/c/lethal-company/p/Evaisa/HookGenPatcher/\n"
   "2) Run the game once to generate the folder and its contents\n"
   "3) Run this script again\n"
   f"{color.yellow}Or if you have a separate installation of the game for testing which has the MMHOOK directory,\ninput the full path of the Lethal Company folder: (otherwise press enter)" + color.reset)
   userInputGamePath = input()
   if os.path.exists(userInputGamePath):
      gameFilesPath = userInputGamePath
      if os.path.exists(f'{gameFilesPath}/BepInEx/Plugins/MMHOOK'):
         print(color.lightblue + f'Game installation found: {gameFilesPath}' + color.reset)
         gotMMHOOKFiles = True
         for dllFile in os.listdir(f'{gameFilesPath}/BepInEx/Plugins/MMHOOK'):
            shutil.copy2(f'{gameFilesPath}/BepInEx/Plugins/MMHOOK/{dllFile}', f'{unityProjectPath}/{unityPluginsRelative}')
            print(f'Got: {unityPluginsRelative}/{dllFile}')  

if not gotMMHOOKFiles:
   print(color.red + "Could not find location. Exiting the program.")
   exit()

print(color.lightblue + f'Project Setup Complete!{color.lightcyan}\n> You should now be able to build the C# project, including the Asset Bundle!')