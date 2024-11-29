using HarmonyLib;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/* 
Zeon
Created by athena (@trix9x)
Zeon falls under the GPL-3.0 license. Do not take credit for the work as it remains copyrighted under the terms of this license.

Any comments here are completely for me, do not take my code without permission
*/
namespace Zeon.Patches
{
    [HarmonyPatch(typeof(GorillaLocomotion.Player))]
    [HarmonyPatch("DoLaunch")]
    public class DoLaunchPatch
    {
        static bool Prefix(ref Vector3 velocity) // this is for shibas guardian crash all, i think it works but i havent tested it yet.
        {
            if (velocity.x > 50 || velocity.y > 50 || velocity.z > 50 ||
                float.IsNaN(velocity.x) || float.IsNaN(velocity.y) ||
                float.IsInfinity(velocity.x) || float.IsInfinity(velocity.y) || float.IsInfinity(velocity.z))
                return false;

            return true;
        }
    }
}
