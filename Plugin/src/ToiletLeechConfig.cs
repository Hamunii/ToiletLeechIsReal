using System.Collections.Generic;
using System.Reflection;
using BepInEx.Configuration;

namespace ToiletLeechIsReal {
    public class ToiletLeechIsRealConfig
    {
        public ConfigEntry<string> configSpawnrateToiletLeech { get; private set; }

        // Here we make a new object, passing in the config file from Plugin.cs
        public ToiletLeechIsRealConfig(ConfigFile configFile) 
        {
            configSpawnrateToiletLeech = configFile.Bind("ToiletLeech Spawnrates",   // The section under which the option is shown
                                                "Enemy Spawnrate",  // The key of the configuration option in the configuration file
                                                "Modded@100,ExperimentationLevel@50,AssuranceLevel@100,VowLevel@200,OffenseLevel@100,MarchLevel@200,RendLevel@200,DineLevel@100,TitanLevel@200",
                                                "Spawn Weight of the Toilet Leech in all vanilla moons + a modded option (Doesn't work for LLL moons).");

            ClearUnusedEntries(configFile);
            ToiletLeechPlugin.Logger.LogInfo("Setting up config for Toilet Leech plugin...");
        }

        private void ClearUnusedEntries(ConfigFile configFile) {
            // Normally, old unused config entries don't get removed, so we do it with this piece of code. Credit to Kittenji.
            PropertyInfo orphanedEntriesProp = configFile.GetType().GetProperty("OrphanedEntries", BindingFlags.NonPublic | BindingFlags.Instance);
            var orphanedEntries = (Dictionary<ConfigDefinition, string>)orphanedEntriesProp.GetValue(configFile, null);
            orphanedEntries.Clear(); // Clear orphaned entries (Unbinded/Abandoned entries)
            configFile.Save(); // Save the config file to save these changes
        }
    }
}