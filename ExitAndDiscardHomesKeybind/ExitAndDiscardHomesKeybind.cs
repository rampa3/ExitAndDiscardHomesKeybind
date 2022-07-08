using HarmonyLib;
using NeosModLoader;
using FrooxEngine;
using System.Reflection;
using System;

namespace ExitAndDiscardHomesKeybind
{
    public class ExitAndDiscardHomesKeybind : NeosMod
    {
        public override string Name => "ExitAndDiscardHomesKeybind";
        public override string Author => "rampa3";
        public override string Version => "1.0.0";
        public override string Link => "https://github.com/rampa3/ExitAndDiscardHomesKeybind/";
        private static ModConfiguration Config;

        public override void OnEngineInit()
        {
            Config = GetConfiguration();
            Config.Save(true);
            Harmony harmony = new Harmony("net.rampa3.ExitAndDiscardHomesKeybind");
            addExitAndDiscardHomesKeybind(harmony);
        }

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> MOD_ENABLED = new ModConfigurationKey<bool>("ModEnabled", "Enable Ctrl+Alt+F4 exit without saving homes shortcut ", () => true);

        private static void addExitAndDiscardHomesKeybind(Harmony harmony)
        {
            MethodInfo original = AccessTools.DeclaredMethod(typeof(Userspace), "OnCommonUpdate", new Type[] { });
            MethodInfo postfix = AccessTools.DeclaredMethod(typeof(ExitAndDiscardHomesKeybind), nameof(AddShortcutPostfix));
            harmony.Patch(original, postfix: new HarmonyMethod(postfix));
            Debug("Exit without saving shortcut added successfully!");
        }

        private static void AddShortcutPostfix(Userspace __instance)
        {
            if (Config.GetValue(MOD_ENABLED))
            {
                if (__instance.InputInterface.GetKey(Key.Control) && (__instance.InputInterface.GetKey(Key.Alt) || __instance.InputInterface.GetKey(Key.AltGr)) && __instance.InputInterface.GetKeyDown(Key.F4))
                {
                    Userspace.ExitNeos(false);
                }
            }
            
        }
    }
}