using System.Reflection;
using UnityEngine;
using BepInEx;
using HarmonyLib;
using LethalLib.Modules;
using static LethalLib.Modules.Levels;
using static LethalLib.Modules.Enemies;
using BepInEx.Logging;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace ToiletLeechIsReal {
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class ToiletLeechPlugin : BaseUnityPlugin {
        public static Harmony _harmony;
        public static EnemyType toiletLeech;
        public static ToiletLeechIsRealConfig config { get; private set; } // prevent from accidently overriding the config
        internal static new ManualLogSource Logger;

        private void Awake() {
            Logger = base.Logger;
            Assets.PopulateAssets();
            config = new ToiletLeechIsRealConfig(this.Config); // Create the config with the file from here.

            toiletLeech = Assets.MainAssetBundle.LoadAsset<EnemyType>("ToiletLeech");
            var tlTerminalNode = Assets.MainAssetBundle.LoadAsset<TerminalNode>("ToiletLeechTN");
            var tlTerminalKeyword = Assets.MainAssetBundle.LoadAsset<TerminalKeyword>("ToiletLeechTK");
            
            // Network Prefabs need to be registered first. See https://docs-multiplayer.unity3d.com/netcode/current/basics/object-spawning/
            NetworkPrefabs.RegisterNetworkPrefab(toiletLeech.enemyPrefab);

            string toiletLeechSpawnratesConfig = config.configSpawnrateToiletLeech.Value;
            // Initialize dictionaries to hold spawn rates for predefined and custom levels.
            Dictionary<LevelTypes, int> spawnRateByLevelType = new Dictionary<LevelTypes, int>();
            Dictionary<string, int> spawnRateByCustomLevelType = new Dictionary<string, int>();
            if (spawnRateByCustomLevelType != null && spawnRateByLevelType != null) {
                foreach (string entry in toiletLeechSpawnratesConfig.Split(',').Select(s => s.Trim()))
                {
                    string[] entryParts = entry.Split('@');

                    if (entryParts.Length != 2)
                    {
                        continue;
                    }

                    string name = entryParts[0];
                    int spawnrate;

                    if (!int.TryParse(entryParts[1], out spawnrate))
                    {
                        continue;
                    }

                    if (System.Enum.TryParse<LevelTypes>(name, true, out LevelTypes levelType))
                    {
                        spawnRateByLevelType[levelType] = spawnrate;
                        Logger.LogInfo($"Registered spawn rate for level type {levelType} to {spawnrate}");
                    }
                    else
                    {
                        spawnRateByCustomLevelType[name] = spawnrate;
                        Logger.LogInfo($"Registered spawn rate for custom level type {name} to {spawnrate}");
                    }
                }
                RegisterEnemy(toiletLeech, spawnRateByLevelType, spawnRateByCustomLevelType, tlTerminalNode, tlTerminalKeyword);
            } else {
                RegisterEnemy(toiletLeech, 100, LevelTypes.All, SpawnType.Outside, tlTerminalNode, tlTerminalKeyword);
            }
            
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            // Required by https://github.com/EvaisaDev/UnityNetcodePatcher maybe?
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);
                    if (attributes.Length > 0)
                    {
                        method.Invoke(null, null);
                    }
                }
            }
        }
    }

    public static class Assets {
        public static AssetBundle MainAssetBundle = null;
        public static void PopulateAssets() {
            string sAssemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            MainAssetBundle = AssetBundle.LoadFromFile(Path.Combine(sAssemblyLocation, "toiletleech"));
            if (MainAssetBundle == null) {
                ToiletLeechPlugin.Logger.LogError("Failed to load custom assets.");
                return;
            }
        }
    }
}
