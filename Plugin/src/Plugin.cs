using System.Reflection;
using UnityEngine;
using BepInEx;
using HarmonyLib;
using LethalLib.Modules;
using static LethalLib.Modules.Levels;
using static LethalLib.Modules.Enemies;
using System;

namespace ToiletLeechIsReal {
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class ToiletLeechPlugin : BaseUnityPlugin {
        public static Harmony _harmony;
        public static EnemyType toiletLeech;

        private void Awake() {
            Assets.PopulateAssets();

            toiletLeech = Assets.MainAssetBundle.LoadAsset<EnemyType>("ToiletLeech");
            var tlTerminalNode = Assets.MainAssetBundle.LoadAsset<TerminalNode>("ToiletLeechTN");
            var tlTerminalKeyword = Assets.MainAssetBundle.LoadAsset<TerminalKeyword>("ToiletLeechTK");
            
            // Network Prefabs need to be registered first. See https://docs-multiplayer.unity3d.com/netcode/current/basics/object-spawning/
            NetworkPrefabs.RegisterNetworkPrefab(toiletLeech.enemyPrefab);
			RegisterEnemy(toiletLeech, 100, LevelTypes.All, SpawnType.Outside, tlTerminalNode, tlTerminalKeyword);
            
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

    // credit to upsidedowncatfish for the asset loader class
    public static class Assets {
        public static string mainAssetBundleName = "toiletleech";
        public static AssetBundle MainAssetBundle = null;

        private static string GetAssemblyName() => Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        public static void PopulateAssets() {
            if(MainAssetBundle == null) {
                using(var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(GetAssemblyName() + "." + mainAssetBundleName)) {
                    MainAssetBundle = AssetBundle.LoadFromStream(assetStream);
                }
            }
        }
    }
}