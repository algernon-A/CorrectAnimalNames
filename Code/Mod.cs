using CitiesHarmony.API;
using ICities;


namespace CleanAnimalNames
{
    /// <summary>
    /// The base mod class for instantiation by the game.
    /// </summary>
    public class Mod : IUserMod
    {
        public static string ModName => "Correct Animal Names";

        public string Name => ModName + " " + ModUtils.CurrentVersion;
        public string Description => "Cleans up the names of custom animal prefabs";


        /// <summary>
        /// Called by the game when the mod is enabled.
        /// </summary>
        public void OnEnabled()
        {
            // Apply Harmony patches via Cities Harmony.
            // Called here instead of OnCreated to allow the auto-downloader to do its work prior to launch.
            HarmonyHelper.DoOnHarmonyReady(() => Patcher.PatchAll());
        }


        /// <summary>
        /// Called by the game when the mod is disabled.
        /// </summary>
        public void OnDisabled()
        {
            // Unapply Harmony patches via Cities Harmony.
            if (HarmonyHelper.IsHarmonyInstalled)
            {
                Patcher.UnpatchAll();
            }
        }
    }
}