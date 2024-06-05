using Aki.Reflection.Patching;
using EFT.HealthSystem;
using System.Reflection;

namespace EscapeFromBSG.Patches
{
    // Simple God mode
    // TODO: Investigate weird insta kill bug (seemingly only happens if the player takes significant damage BEFORE enabling god mode)
    // (Maybe find the actual kill logic and patch that as well as ApplyDamage)
    public class ApplyDamagePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => typeof(ActiveHealthController).GetMethod(nameof(ActiveHealthController.ApplyDamage));

        [PatchPrefix]
        private static bool Prefix(ActiveHealthController __instance, ref float damage, DamageInfo damageInfo)
        {
            if (__instance.Player == null || !__instance.Player.IsYourPlayer || !Plugin.GodMode.Value)
            {
                return true;
            }

            Plugin.PluginLogger.LogInfo($"Damage: {damage}, Source: {damageInfo.SourceId}");

            damage = 0f;
            return false;
        }
    }
}
