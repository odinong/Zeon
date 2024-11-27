using BepInEx;
using HarmonyLib;
using Photon.Pun;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;
using GorillaNetworking;
using GorillaLocomotion.Gameplay;
using System.Net.NetworkInformation;
using System.IO;
using System.Linq;
using OVR;
using Photon.Voice.Unity;
using System.Reflection;
using System;
using UnityEngine.Networking;
using Photon.Voice.Unity.UtilityScripts;
using POpusCodec.Enums;
using GorillaTag;
using UnityEngine.InputSystem;
using static NetworkSystem;
using System.Security.Policy;
using Photon.Realtime;
using PlayFab.ClientModels;
using UnityEngine.XR;
using System.Collections;
using Pathfinding.Util;
using Cinemachine;
using Zeon.Notifications;
using Zeon;
using ExitGames.Client.Photon;
using GorillaTagScripts;
using ExitGames.Client.Photon.StructWrapping;
using NanoSockets;
using TMPro;
using Zeon.Notifications;

/* 
Zeon
Created by athena (@trix9x)
Zeon falls under the GPL-3.0 license. Do not take credit for the work as it remains copyrighted under the terms of this license.

Any comments here are completely for me, do not take my code without permission
*/
namespace Zeon
{
    public class Zeon : BaseUnityPlugin
    {
        private float deltaTime = 0.0f;
        private int selectedTab = 0;
        private static string keyauthId = "";
        static Color MainColor = new Color(94f / 255f, 125f / 255f, 171f / 255f);

        static float randomX = UnityEngine.Random.Range(-10f, 10f);
        static float randomY = UnityEngine.Random.Range(-10f, 10f);
        static float randomZ = UnityEngine.Random.Range(-10f, 10f);
        private Rect windowRect = new Rect(100, 100, 800, 500);

        public static void CopyToClipboard(string text)
        {
            GUIUtility.systemCopyBuffer = text;
        }

        public static float startX = -1f;
        public static float startY = -1f;
        public static float subThingy = 0f;
        public static float subThingyZ = 0f;
        public static float moveSpeed = 1f;
        public static float rotationSpeed = 1.0f;
        public static float flySpeed = 10f;
        public static void CreateRoom(string roomName, bool isPublic)
        {
            PhotonNetworkController.Instance.currentJoinTrigger = GorillaComputer.instance.GetJoinTriggerForZone("forest");
            UnityEngine.Debug.Log((string)typeof(PhotonNetworkController).GetField("platformTag", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(PhotonNetworkController.Instance));
            RoomConfig roomConfig = new RoomConfig()
            {
                createIfMissing = true,
                isJoinable = true,
                isPublic = isPublic,
                MaxPlayers = PhotonNetworkController.Instance.GetRoomSize(PhotonNetworkController.Instance.currentJoinTrigger.networkZone),
                CustomProps = new ExitGames.Client.Photon.Hashtable()
                {
                    { "gameMode", PhotonNetworkController.Instance.currentJoinTrigger.GetFullDesiredGameModeString() },
                    { "platform", (string)typeof(PhotonNetworkController).GetField("platformTag", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(PhotonNetworkController.Instance) }
                }
            };
            NetworkSystem.Instance.ConnectToRoom(roomName, roomConfig);
        }
        public static string RandomRoomName()
        {
            string text = "";
            for (int i = 0; i < 4; i++)
            {
                text += NetworkSystem.roomCharacters.Substring(UnityEngine.Random.Range(0, NetworkSystem.roomCharacters.Length), 1);
            }
            if (GorillaComputer.instance.CheckAutoBanListForName(text))
            {
                return text;
            }
            return RandomRoomName();
        }
        public static void CreatePublic()
        {
            CreateRoom(RandomRoomName(), true);
        }
        public static void RPCProt()
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
            raiseEventOptions.CachingOption = EventCaching.RemoveFromRoomCache;
            raiseEventOptions.TargetActors = new int[]
            {
                PhotonNetwork.LocalPlayer.ActorNumber
            };
            PhotonNetwork.NetworkingClient.OpRaiseEvent(200, null, raiseEventOptions, SendOptions.SendReliable);
            GorillaNot.instance.rpcErrorMax = int.MaxValue;
            GorillaNot.instance.rpcCallLimit = int.MaxValue;
            GorillaNot.instance.logErrorMax = int.MaxValue;
            PhotonNetwork.MaxResendsBeforeDisconnect = int.MaxValue;
            PhotonNetwork.QuickResends = int.MaxValue;
            PhotonNetwork.RemoveRPCs(PhotonNetwork.LocalPlayer);
            PhotonNetwork.RemoveBufferedRPCs(GorillaTagger.Instance.myVRRig.ViewID, null, null);
            PhotonNetwork.RemoveRPCsInGroup(int.MaxValue);
            PhotonNetwork.SendAllOutgoingCommands();
            GorillaNot.instance.OnPlayerLeftRoom(PhotonNetwork.LocalPlayer);
        }

        public static VRRig GetVRRigFromPlayer(NetPlayer p)
        {
            return GorillaGameManager.instance.FindPlayerVRRig(p);
        }

        public static VRRig GetRandomVRRig(bool includeSelf)
        {
            Photon.Realtime.Player randomPlayer;
            if (includeSelf)
            {
                randomPlayer = PhotonNetwork.PlayerList[UnityEngine.Random.Range(0, PhotonNetwork.PlayerList.Length - 1)];
            }
            else
            {
                randomPlayer = PhotonNetwork.PlayerListOthers[UnityEngine.Random.Range(0, PhotonNetwork.PlayerListOthers.Length - 1)];
            }
            return GetVRRigFromPlayer(randomPlayer);
        }

        public static VRRig GetClosestVRRig()
        {
            float num = float.MaxValue;
            VRRig outRig = null;
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (Vector3.Distance(GorillaTagger.Instance.bodyCollider.transform.position, vrrig.transform.position) < num && vrrig != GorillaTagger.Instance.offlineVRRig)
                {
                    num = Vector3.Distance(GorillaTagger.Instance.bodyCollider.transform.position, vrrig.transform.position);
                    outRig = vrrig;
                }
            }
            return outRig;
        }

        public static PhotonView GetPhotonViewFromVRRig(VRRig p)
        {
            return (PhotonView)Traverse.Create(p).Field("photonView").GetValue();
        }

        public static Photon.Realtime.Player GetRandomPlayer(bool includeSelf)
        {
            if (includeSelf)
            {
                return PhotonNetwork.PlayerList[UnityEngine.Random.Range(0, PhotonNetwork.PlayerList.Length - 1)];
            }
            else
            {
                return PhotonNetwork.PlayerListOthers[UnityEngine.Random.Range(0, PhotonNetwork.PlayerListOthers.Length - 1)];
            }
        }

        public static Player NetPlayerToPlayer(NetPlayer p)
        {
            return p.GetPlayerRef();
        }

        public static NetPlayer GetPlayerFromVRRig(VRRig p)
        {
            return p.Creator;
        }

        public static NetPlayer GetPlayerFromID(string id)
        {
            NetPlayer found = null;
            foreach (Photon.Realtime.Player target in PhotonNetwork.PlayerList)
            {
                if (target.UserId == id)
                {
                    found = target;
                    break;
                }
            }
            return found;
        }
        public static VRRig GetVRRigFromPlayer(Player p)
        {
            return GorillaGameManager.instance.FindPlayerVRRig(p);
        }


        public static VRRig FindVRRigForPlayer(Photon.Realtime.Player player)
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                bool flag = !vrrig.isOfflineVRRig && vrrig.GetComponent<PhotonView>().Owner == player;
                if (flag)
                {
                    return vrrig;
                }
            }
            return null;
        }
        public static void Flight()
        {
            if (ControllerInputPoller.instance.rightGrab)
            {
                GorillaLocomotion.Player.Instance.transform.position += GorillaLocomotion.Player.Instance.headCollider.transform.forward * Time.deltaTime * 15;
                GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
        public static void Tracers()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (!vrrig.isMyPlayer)
                {
                    GameObject line = new GameObject("TracerLine");
                    LineRenderer linerender = line.AddComponent<LineRenderer>();
                    linerender.material.shader = Shader.Find("GUI/Text Shader");
                    linerender.startColor = MainColor;
                    linerender.endColor = MainColor;
                    linerender.startWidth = 0.015f;
                    linerender.endWidth = 0.015f;
                    linerender.positionCount = 2;
                    linerender.useWorldSpace = true;
                    linerender.SetPosition(0, GorillaTagger.Instance.rightHandTransform.position);
                    linerender.SetPosition(1, vrrig.transform.position);
                    UnityEngine.Object.Destroy(line, Time.deltaTime);
                }
            }
        }
        public static void DisableTracers()
        {
            foreach (GameObject a in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (a.name == "TracerLine")
                {
                    Destroy(a);
                }
            }
        }
    }
}