using GorillaTagScripts;
using HarmonyLib;
using Zeon.Mods;
using Zeon.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* 
Zeon
Created by athena (@trix9x)
Zeon falls under the GPL-3.0 license. Do not take credit for the work as it remains copyrighted under the terms of this license.

Any comments here are completely for me, do not take my code without permission
*/
namespace Zeon.Patches
{
    [HarmonyPatch(typeof(BuilderTableNetworking), "PieceCreatedRPC")]
    public class CreatePatch
    {
        public static bool enabled = false;
        public static int pieceTypeSearch = 0;

        private static void Postfix(int pieceType, int pieceId)
        {
            if (enabled)
            {
                if (pieceTypeSearch == pieceType)
                {
                    OPMods.pieceId = pieceId;
                    enabled = false;
                }
            }
        }
    }
}
