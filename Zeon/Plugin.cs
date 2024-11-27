using System;
using System.Reflection;
using Zeon;
using BepInEx;
using HarmonyLib;
using UnityEngine;

/* 
Zeon
Created by athena (@trix9x)
Zeon falls under the GPL-3.0 license. Do not take credit for the work as it remains copyrighted under the terms of this license.

Any comments here are completely for me, do not take my code without permission
*/
namespace Zeon
{
    public class Plugin : BaseUnityPlugin
    {
        private Plugin()
        {
            new Harmony("Zeon").PatchAll(Assembly.GetExecutingAssembly());
            Plugin.instance = this;
        }

        public void OnGameInitialised()
        {
            Debug.Log("Game Init.");
            new GameObject().AddComponent<Zeon>().gameObject.name = "Zeon";
            new GameObject().AddComponent<ZeonMain>().gameObject.name = "Zeon";
            new GameObject().AddComponent<Menu.Main>().gameObject.name = "Zeon";
        }

        public static Plugin instance;
    }
}
