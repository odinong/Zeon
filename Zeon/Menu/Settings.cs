using Zeon.Classes;
using UnityEngine;
using static Zeon.Menu.Main;

/* 
Zeon
Created by athena (@trix9x)
Zeon falls under the GPL-3.0 license. Do not take credit for the work as it remains copyrighted under the terms of this license.

Any comments here are completely for me, do not take my code without permission
*/
namespace Zeon.Settings
{
    internal class Settings
    {
        public static ExtGradient backgroundColor = new ExtGradient { colors = GetSolidGradient(new Color(0 / 255f, 0 / 255f, 0 / 255f)) };
        public static ExtGradient[] buttonColors = new ExtGradient[]
        {
            new ExtGradient{colors = GetSolidGradient(new Color(0/255f, 0/255f, 0/255f))}, // Disabled
            new ExtGradient{colors = GetSolidGradient(new Color(5/255f, 5/255f, 5/255f))} // Enabled
        };
        public static Color[] textColors = new Color[]
        {
            Color.white, // Disabled
            Color.white // Enabled
        };

        public static Font currentFont = (Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font);

        public static bool fpsCounter = true;
        public static bool disconnectButton = true;
        public static bool rightHanded = true;
        public static bool disableNotifications = false;

        public static KeyCode keyboardButton = KeyCode.Q;

        public static Vector3 menuSize = new Vector3(0.1f, 0.95f, 1f); // Depth, Width, Height
        public static int buttonsPerPage = 6;
    }
}
