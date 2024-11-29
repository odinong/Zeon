using Zeon.Classes;
using Zeon.Mods;
using Zeon.Mods;
using PlayFab.MultiplayerModels;
using Zeon.Settings;
using PlayFab.ExperimentationModels;
using Zeon;

/* 
Zeon
Created by athena (@trix9x)
Zeon falls under the GPL-3.0 license. Do not take credit for the work as it remains copyrighted under the terms of this license.

Any comments here are completely for me, do not take my code without permission
*/
namespace Zeon.Menu
{
    internal class Buttons
    {
        public static ButtonInfo[][] buttons = new ButtonInfo[][]
        {
            new ButtonInfo[] { // pages [0]
                new ButtonInfo { buttonText = "Open Settings", method =() => SettingsMods.EnterSettings(), isTogglable = false},
                new ButtonInfo { buttonText = "Player Mods", method =() => SettingsMods.EnterMain(), isTogglable = false},
                new ButtonInfo { buttonText = "Safety Mods", method =() => SettingsMods.EnterRoom(), isTogglable = false},
                new ButtonInfo { buttonText = "Fun Mods", method =() => SettingsMods.EnterFun(), isTogglable = false},
                new ButtonInfo { buttonText = "OverPowered Mods", method =() => SettingsMods.EnterOP(), isTogglable = false},
                new ButtonInfo { buttonText = "Gun Mods", method =() => SettingsMods.EnterGun(), isTogglable = false},
            },

            new ButtonInfo[] { // Settings [1]
                new ButtonInfo { buttonText = "Menu", method =() => SettingsMods.MenuSettings(), isTogglable = false},
                new ButtonInfo { buttonText = "Main", method =() => SettingsMods.MovementSettings(), isTogglable = false},
                new ButtonInfo { buttonText = "Safety", method =() => SettingsMods.ProjectileSettings(), isTogglable = false},
            },

            new ButtonInfo[] { // Menu Settings [2]
                new ButtonInfo { buttonText = "Right Hand", enableMethod =() => SettingsMods.RightHand(), disableMethod =() => SettingsMods.LeftHand(), enabled = true},
                new ButtonInfo { buttonText = "Notifications", enableMethod =() => SettingsMods.EnableNotifications(), disableMethod =() => SettingsMods.DisableNotifications(), enabled = !Settings.Settings.disableNotifications},
                new ButtonInfo { buttonText = "FPS Counter", enableMethod =() => SettingsMods.EnableFPSCounter(), disableMethod =() => SettingsMods.DisableFPSCounter(), enabled = Settings.Settings.fpsCounter},
                new ButtonInfo { buttonText = "Disconnect Button", enableMethod =() => SettingsMods.EnableDisconnectButton(), disableMethod =() => SettingsMods.DisableDisconnectButton(), enabled = Settings.Settings.disconnectButton},
            },

            new ButtonInfo[] { // room Settings [3]
                new ButtonInfo { buttonText = "ESP On Self", enableMethod =() => NormalMods.ShowESPSelfOn(), disableMethod =() => NormalMods.ShowESPSelfOff()},
            },

            new ButtonInfo[] { // room Settings [4]
                new ButtonInfo { buttonText = "carrg", overlapText = "Change Anti Report Distance [Normal]", method =() => SettingsMods.ChangeReportDistance(), isTogglable = false},
            },

            new ButtonInfo[] { // main mods [5]
                new ButtonInfo { buttonText = "Fly", method =() => NormalMods.Flight()},
                new ButtonInfo { buttonText = "Ghost [RT]", method =() => NormalMods.Ghost()},
                new ButtonInfo { buttonText = "Invis [RT]", method =() => NormalMods.Invis()},
                new ButtonInfo { buttonText = "Quit Game", method =() => NormalMods.Quit(), isTogglable = false},
                new ButtonInfo { buttonText = "Tracers [Laggy]", method =() => NormalMods.Tracers(), disableMethod =() => NormalMods.DisableTracers()},
                new ButtonInfo { buttonText = "Bone ESP [Not Laggy]", method =() => NormalMods.BoneESP(), disableMethod =() => NormalMods.BoneESPOff()},
            },

            new ButtonInfo[] { // room mods [6]
                new ButtonInfo { buttonText = "Disconnect", method =() => NormalMods.Disconnect(), isTogglable = false},
                new ButtonInfo { buttonText = "Anti-Report", method =() => NormalMods.AntiReport(), disableMethod =() => NormalMods.AntiReportDisable()},
                new ButtonInfo { buttonText = "Anti-Moderator", method =() => NormalMods.AntiModerator()},
                new ButtonInfo { buttonText = "Block Anti-Cheat Reports [FUD]", method =() => NormalMods.BlockACEventsOn(), disableMethod =() => NormalMods.BlockACEventsOff()},
            },
            new ButtonInfo[] { // op mods [7]
                new ButtonInfo { buttonText = "Steal Cosmetics", method =() => NormalMods.TryOnAnywhere(), isTogglable = false},
                new ButtonInfo { buttonText = "Mood Ring Always On", method =() => NormalMods.MoodRingAlwaysOn(), disableMethod =() => NormalMods.MoodRingAlwaysOff()},
                new ButtonInfo { buttonText = "Spaz Hats In TryOn Room [SS/RT/E]", method =() => NormalMods.UnlockAllExpensiveItems()},
                new ButtonInfo { buttonText = "Clear Cart", method =() => NormalMods.ClearCart(), isTogglable = false},
            },

            new ButtonInfo[] { // op mods [8]
                new ButtonInfo { buttonText = "Destroy All", method =() => OPMods.DestroyAll(), isTogglable = false},
                new ButtonInfo { buttonText = "Shoot Blocks [RT/E]", method =() => OPMods.BlockLine()},
                new ButtonInfo { buttonText = "Spam Blocks [RT/E]", method =() => OPMods.BlockSpammer()},
                new ButtonInfo { buttonText = "Aura Blocks [RT/E]", method =() => OPMods.BlockAura()},
                new ButtonInfo { buttonText = "Rain Blocks [RT/E]", method =() => OPMods.BlockRain()},
                new ButtonInfo { buttonText = "Spam Blue Blocks [RT/E]", method =() => OPMods.BlueBlockSpammer()},
                new ButtonInfo { buttonText = "Spam Random Blocks [RT/E]", method =() => OPMods.BlockRandom()},
                new ButtonInfo { buttonText = "Rain Random Blocks [RT/E]", method =() => OPMods.BlockRandomRain()},
                new ButtonInfo { buttonText = "Shoot Random Blocks [RT/E]", method =() => OPMods.BlockShootRandom()},
                new ButtonInfo { buttonText = "Aura Random Blocks [RT/E]", method =() => OPMods.BlockAuraRandom()},
                new ButtonInfo { buttonText = "Spam Castle Blocks [RT/E]", method =() => OPMods.BlockSpammer()},
                new ButtonInfo { buttonText = "Lag All [BLOCKS/RT/E]", method =() => OPMods.TestCrash()},
                new ButtonInfo { buttonText = "AntiLag - USE", method =() => OPMods.AntiCrashBlock()},
                new ButtonInfo { buttonText = "Destroy All Blocks [RT/E]", method =() => OPMods.DestroyBlocks()},
            },

            new ButtonInfo[] { // gun mods [9]
            },

        };
    }
}
