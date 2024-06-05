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
        public static ConfigEntry<KeyboardShortcut> NoClipShortcut;
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
            NoClip = Config.Bind(new ConfigDefinition("Cheats", "NoClip"), false, new ConfigDescription("Enable NoClip (Recommended to be used alongside GodMode to prevent collision and/or fall damage death)", null, new ConfigurationManagerAttributes { IsAdvanced = false, Order = 2 }));
            NoClipSpeed = Config.Bind(new ConfigDefinition("Cheats", "NoClipSpeed"), 8.0f, new ConfigDescription("Configure NoClip Speed", new AcceptableValueRange<float>(1, 500), new ConfigurationManagerAttributes { IsAdvanced = false, Order = 3 }));
            NoClipShortcut = Config.Bind(new ConfigDefinition("Cheats", "NoClipShortcut"), new KeyboardShortcut(UnityEngine.KeyCode.Slash), new ConfigDescription("NoClip Shortcut", null, new ConfigurationManagerAttributes { IsAdvanced = false, Order = 4 }));
        }
    }
}
