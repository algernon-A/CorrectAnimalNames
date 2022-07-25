using HarmonyLib;


namespace CleanAnimalNames
{
    /// <summary>
    /// Harmony patch to return cleaned-up names for custom animal prefabs.
    /// </summary>
    [HarmonyPatch(typeof(AnimalAI), "GenerateName")]
    public static class GenerateName
    {
        /// <summary>
        /// Harmony Postfix patch to AnimalAI.GenerateName to return cleaned-up name for custom prefabs.
        /// </summary>
        /// <param name="__instance">AI instance reference</param>
        /// <param name="__result">Method result reference</param>
        /// <returns>False (don't execute original method) if a cleaned-up name was generated, true (continue execution chain) otherwise</returns>
        public static bool Prefix(AnimalAI __instance, ref string __result)
        {
            // Check for period in prefab name.
            int periodPos = __instance.m_info.name.IndexOf('.');
            if (periodPos >= 0)
            {
                // Found a period - strip off leading package ID and any trailing _Data and returned the cleaned-up name.
                __result = __instance.m_info.name.Substring(periodPos + 1).Replace("_Data", "");
                return false;
            }
            // No period - check for traiing _Data for local assets.
            else if (__instance.m_info.name.EndsWith("_Data"))
            {
                // Trailing _Data found - trim and return the trimmed name.
                __result = __instance.m_info.name.Substring(0, __instance.m_info.name.Length - 5);
                return false;
            }

            // Not a custom asset - continue to game method.
            return true;
        }
    }
}