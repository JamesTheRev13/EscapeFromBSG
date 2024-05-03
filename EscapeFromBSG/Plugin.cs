// PLUGIN BY BOB SAGET
// FUCK BATTLESTATE GAMES

using BepInEx;
using BepInEx.Logging;
using EscapeFromBSG.Patches;
using UnityEngine;

namespace EscapeFromBSG
{
    [BepInPlugin("BobSaget.EscapeFromBSG", "EscapeFromBSG", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        internal static Plugin Instance { get; private set; }
        internal static ManualLogSource PluginLogger { get; private set; }
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

            new ApplyDamagePatch().Enable();
            new PlayerUpdateTickPatch().Enable();
        }

        
    }
}
