using System;
using GorillaLocomotion;
using HarmonyLib;

/* 
Zeon
Created by athena (@trix9x)
Zeon falls under the GPL-3.0 license. Do not take credit for the work as it remains copyrighted under the terms of this license.

Any comments here are completely for me, do not take my code without permission
*/
namespace Zeon.Patches
{
	[HarmonyPatch(typeof(Player))]
	[HarmonyPatch("Awake", MethodType.Normal)]
	internal class ExamplePatch
	{
		private static void Postfix(Player __instance)
		{
			Console.WriteLine(__instance.maxJumpSpeed);
		}
	}
}
