﻿using Zeon.Notifications;
using HarmonyLib;
using Photon.Pun;
using System;
using UnityEngine;
using Zeon.Menu;
using ExitGames.Client.Photon;
using Photon.Realtime;
using System.Text;

/* 
Zeon
Created by athena (@trix9x)
Zeon falls under the GPL-3.0 license. Do not take credit for the work as it remains copyrighted under the terms of this license.

Any comments here are completely for me, do not take my code without permission
*/
namespace Zeon.Patches
{
    [HarmonyPatch(typeof(GorillaNot), "SendReport")]
    internal class AntiCheat : MonoBehaviour
    {
        private static bool Prefix(string susReason, string susId, string susNick)
        {
            if (susId == PhotonNetwork.LocalPlayer.UserId && Main.blockAcEvents == true)
            {
                // Debug.Log("<color=red>GorillaNot</color> : <color=red>BLOCKED AC REPORT FROM BEING SENT TO YOU:</color><color=red>REASON: " + susReason + "</color>");
                Mods.NormalMods.RPCProt();
            }
            if (susId == PhotonNetwork.LocalPlayer.UserId)
            {
                // Debug.Log("<color=red>GorillaNot</color> : <color=red>AC REPORT HAS BEEN SENT TO YOU:</color><color=red>REASON: " + susReason + "</color>");
                Mods.NormalMods.RPCProt();
                PhotonNetwork.Disconnect();
            }
            if (susId == PhotonNetwork.LocalPlayer.UserId == false)
            {
                // Debug.Log("<color=red>GorillaNot</color> : <color=red>AC REPORT:</color> <color=red>USER: " + susId + "</color><color=red>NAME: " + susNick + ", </color><color=red>REASON: " + susReason + "</color>");
            }
            return false;
        }
    }
    [HarmonyPatch(typeof(PhotonNetwork))]
    [HarmonyPatch(typeof(LoadBalancingClient), "OnEvent")]
    class RaiseEventPatch
    {
        static void Postfix(EventData __0)
        {
            if (__0.Code != 0 && __0.Code != 203 && __0.Code != 201 && __0.Code != 200)
            {
                Photon.Realtime.Player sender = PhotonNetwork.CurrentRoom?.GetPlayer(__0.Sender);
                string senderName = sender != null ? sender.NickName : "Unknown";
                var eventInfo = new StringBuilder();
                eventInfo.AppendLine("[RaiseEvent] Details:");
                eventInfo.AppendLine($"Code: {__0.Code}");
                eventInfo.AppendLine($"Sender: {senderName} ({__0.Sender})");
                eventInfo.AppendLine($"Custom Data: {__0.CustomData ?? "None"}");
                eventInfo.AppendLine($"Parameters Count: {__0.Parameters?.Count ?? 0}");

                if (__0.Parameters != null)
                {
                    foreach (var param in __0.Parameters)
                    {
                        eventInfo.AppendLine($"Parameter Key: {param.Key}, Value: {param.Value}");
                    }
                }
                Debug.Log(eventInfo.ToString());
            }
        }

    }
    [HarmonyPatch(typeof(GorillaNot), "LogErrorCount")]
    public class NoLogErrorCount : MonoBehaviour
    {
        private static bool Prefix(string logString, string stackTrace, LogType type)
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(GorillaNot), "CloseInvalidRoom")]
    public class NoCloseInvalidRoom : MonoBehaviour
    {
        private static bool Prefix()
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(GorillaNot), "CheckReports", MethodType.Enumerator)]
    public class NoCheckReports : MonoBehaviour
    {
        private static bool Prefix()
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(GorillaNot), "QuitDelay", MethodType.Enumerator)]
    public class NoQuitDelay : MonoBehaviour
    {
        private static bool Prefix()
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(GorillaNot), "IncrementRPCCallLocal")]
    public class NoIncrementRPCCallLocal : MonoBehaviour
    {
        private static bool Prefix(PhotonMessageInfoWrapped infoWrapped, string rpcFunction)
        {
            
            Debug.Log(infoWrapped.Sender.NickName + " sent rpc: " + rpcFunction + " master: " + infoWrapped.Sender.IsMasterClient);
            return false;
        }
    }

    [HarmonyPatch(typeof(GorillaNot), "GetRPCCallTracker")]
    internal class NoGetRPCCallTracker : MonoBehaviour
    {
        private static bool Prefix()
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(GorillaNot), "IncrementRPCCall", new Type[] { typeof(PhotonMessageInfo), typeof(string) })]
    public class NoIncrementRPCCall : MonoBehaviour
    {
        private static bool Prefix(PhotonMessageInfo info, string callingMethod = "")
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(VRRig), "IncrementRPC", new Type[] { typeof(PhotonMessageInfoWrapped), typeof(string) })]
    public class NoIncrementRPC : MonoBehaviour
    {
        private static bool Prefix(PhotonMessageInfoWrapped info, string sourceCall)
        {
            return false;
        }
    }
}