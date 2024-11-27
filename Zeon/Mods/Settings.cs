using static Zeon.Menu.Main;
using static Zeon.Settings.Settings;

/* 
Zeon
Created by athena (@trix9x)
Zeon falls under the GPL-3.0 license. Do not take credit for the work as it remains copyrighted under the terms of this license.

Any comments here are completely for me, do not take my code without permission
*/
namespace Zeon.Mods
{
    internal class SettingsMods
    {
        public static void EnterSettings()
        {
            buttonsType = 1;
        }

        public static void MenuSettings()
        {
            buttonsType = 2;
        }

        public static void MovementSettings()
        {
            buttonsType = 3;
        }
        public static void EnterMain()
        {
            buttonsType = 5;
        }
        public static void EnterRoom()
        {
            buttonsType = 6;
        }
        public static void EnterFun()
        {
            buttonsType = 7;
        }
        public static void EnterOP()
        {
            buttonsType = 8;
        }
        public static void EnterGun()
        {
            buttonsType = 9;
        }

        public static void ProjectileSettings()
        {
            buttonsType = 4;
        }

        public static void RightHand()
        {
            rightHanded = true;
        }

        public static void LeftHand()
        {
            rightHanded = false;
        }

        public static void EnableFPSCounter()
        {
            fpsCounter = true;
        }

        public static void DisableFPSCounter()
        {
            fpsCounter = false;
        }

        public static void EnableNotifications()
        {
            disableNotifications = false;
        }

        public static void DisableNotifications()
        {
            disableNotifications = true;
        }

        public static void EnableDisconnectButton()
        {
            disconnectButton = true;
        }

        public static void DisableDisconnectButton()
        {
            disconnectButton = false;
        }
        public static int antireportrangeindex = 0;
        public static int threshSpeedindex = 0;
        public static void ChangeReportDistance()
        {
            {
                string[] names = new string[]
                {
                "Default", 
                "Large", 
                "Big AF" 
                };
                float[] distances = new float[]
                {
                0.35f,
                0.7f,
                1.5f
                };
                antireportrangeindex++;
                if (antireportrangeindex > names.Length - 1)
                {
                    antireportrangeindex = 0;
                }

                ZeonMain.threshold = distances[antireportrangeindex];
                GetIndex("carrg").overlapText = "Change Anti Report Distance [" + names[antireportrangeindex] + "]";
            }
        }
        public static void ChangeBringThresh()
        {
            {
                string[] names = new string[]
                {
                "Slow", 
                "Normal", 
                "Medium",
                "Fast",
                "Faster",
                "Insanely Fast",
                "Too Fast"
                };
                float[] distances = new float[]
                {
                1f,
                5f,
                10f,
                20f,
                40f,
                60f,
                100f
                };
                threshSpeedindex++;
                if (threshSpeedindex > names.Length - 1)
                {
                    threshSpeedindex = 0;
                }

                ZeonMain.BringSpeed = distances[threshSpeedindex];
                GetIndex("cbgs").overlapText = "Change Bring/Fly/Car Mods Speed <color=grey>[</color><color=green>" + names[threshSpeedindex] + "</color><color=grey>]</color>";
            }
        }
    }
}
