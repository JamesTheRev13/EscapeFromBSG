// PLUGIN BY BOB SAGET
// FUCK BATTLESTATE GAMES

using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using EscapeFromBSG.Patches;

namespace EscapeFromBSG
{
    [BepInPlugin("BobSaget.EscapeFromBSG", "EscapeFromBSG", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        internal static Plugin Instance { get; private set; }
        internal static ManualLogSource PluginLogger { get; private set; }

        public static ConfigEntry<bool> GodMode;
        public static ConfigEntry<bool> NoClip;
        public static ConfigEntry<float> NoClipSpeed;

        private void Awake()
        {
            PluginLogger = Logger;
            PluginLogger.LogInfo("Escape From BattleState Games!!!");

            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            BindConfigs();

            new ApplyDamagePatch().Enable();
            new PlayerUpdateTickPatch().Enable();
        }

        private void BindConfigs()
        {
            GodMode = Config.Bind(new ConfigDefinition("Cheats", "GodMode"), false, new ConfigDescription("Enable God Mode", null, new ConfigurationManagerAttributes { IsAdvanced = false, Order = 1}));
            NoClip = Config.Bind(new ConfigDefinition("Cheats", "NoClip"), false, new ConfigDescription("Enable NoClip", null, new ConfigurationManagerAttributes { IsAdvanced = false, Order = 2 }));
            NoClipSpeed = Config.Bind(new ConfigDefinition("Cheats", "NoClipSpeed"), 10.0f, new ConfigDescription("Configure NoClip Speed", null, new ConfigurationManagerAttributes { IsAdvanced = false, Order = 3 }));
        }
    }
}
