using Zeon;
using Zeon.Notifications;
using Fusion;
using GorillaLocomotion;
using GorillaNetworking;
using HarmonyLib;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using ExitGames.Client.Photon;
using Photon.Realtime;
using GorillaTagScripts;
using UnityEngine.ProBuilder.Shapes;
using BepInEx;
using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using Zeon.Patches;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;
using static BodyDockPositions;
using Zeon.Menu;
using Zeon.Patches;
using System.Runtime.CompilerServices;
using OVR;
using static MonoMod.Cil.RuntimeILReferenceBag.FastDelegateInvokers;
using ExitGames.Client.Photon;
using GorillaLocomotion.Gameplay;
using GorillaNetworking;
using GorillaTagScripts;
using GorillaTagScripts.ObstacleCourse;
using HarmonyLib;
using Photon.Pun;
using Photon.Voice.Unity;
using Photon.Voice.Unity.UtilityScripts;
using POpusCodec.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

/* 
Zeon
Created by athena (@trix9x)
Zeon falls under the GPL-3.0 license. Do not take credit for the work as it remains copyrighted under the terms of this license.

Any comments here are completely for me, do not take my code without permission
*/
namespace Zeon.Mods
{
    internal class NormalMods
    {
        public static bool showEspSelf = false;
        public static void Ghost()
        {
            if ((double)ControllerInputPoller.instance.rightControllerIndexFloat >= 0.1)
            {
                BallsOnHands();
                GorillaTagger.Instance.offlineVRRig.enabled = false;
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }
        public static void BallsOnHands()
        {
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
            UnityEngine.Object.Destroy(gameObject.GetComponent<SphereCollider>());
            gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            gameObject.transform.position = GorillaTagger.Instance.leftHandTransform.position;
            GameObject gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            UnityEngine.Object.Destroy(gameObject2.GetComponent<Rigidbody>());
            UnityEngine.Object.Destroy(gameObject2.GetComponent<SphereCollider>());
            gameObject2.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            gameObject2.transform.position = GorillaTagger.Instance.rightHandTransform.position;
            gameObject.GetComponent<Renderer>().material.color = Color.red;
            gameObject2.GetComponent<Renderer>().material.color = Color.red;
            UnityEngine.Object.Destroy(gameObject, Time.deltaTime);
            UnityEngine.Object.Destroy(gameObject2, Time.deltaTime);
        }
        public static int ProjCount;
        public static void LaunchProjectile(Vector3 pos, Vector3 vel)
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(ag => ag.GetName().Name == "Assembly-CSharp");
            var clas = assembly?.GetType("RoomSystem");
            var st = clas?.GetNestedType("aura", BindingFlags.NonPublic);
            var meth = clas?.GetMethod("SendLaunchProjectile", BindingFlags.NonPublic | BindingFlags.Static);
            bool rc = false;
            byte r = 255, g = 0, b = 0, a = 255;
            object aura = Enum.Parse(st, "SlingShot");
            meth.Invoke(null, new object[] { pos, vel, aura, (int)(ProjCount + 1), rc, r, g, b, a });
            Debug.Log("NIGGGAAAAA");
        }
        public static void Disconnect()
        {
            PhotonNetwork.Disconnect(); // bruh
        }
        public static void Home()
        {
            Global.ReturnHome(); // bruh
        }
        public static void Quit()
        {
            Application.Quit(); // bruh
        }
        public static void AntiReport()
        {
            ZeonMain.antiReportCrashfr = true;
        }
        public static void AntiReportDisable()
        {
            ZeonMain.antiReportCrashfr = false;
        }
        public static void ProjSpam()
        {
            Vector3 vel = GorillaTagger.Instance.offlineVRRig.rightHandTransform.forward;
            LaunchProjectile(GorillaTagger.Instance.rightHandTransform.position, vel);
        }
        public static (Vector3 position, Quaternion rotation, Vector3 up, Vector3 forward, Vector3 right) TrueLeftHand()
        {
            Quaternion rot = GorillaTagger.Instance.leftHandTransform.rotation * GorillaLocomotion.Player.Instance.leftHandRotOffset;
            return (GorillaTagger.Instance.leftHandTransform.position + GorillaTagger.Instance.leftHandTransform.rotation * GorillaLocomotion.Player.Instance.leftHandOffset, rot, rot * Vector3.up, rot * Vector3.forward, rot * Vector3.right);
        }

        public static (Vector3 position, Quaternion rotation, Vector3 up, Vector3 forward, Vector3 right) TrueRightHand()
        {
            Quaternion rot = GorillaTagger.Instance.rightHandTransform.rotation * GorillaLocomotion.Player.Instance.rightHandRotOffset;
            return (GorillaTagger.Instance.rightHandTransform.position + GorillaTagger.Instance.rightHandTransform.rotation * GorillaLocomotion.Player.Instance.rightHandOffset, rot, rot * Vector3.up, rot * Vector3.forward, rot * Vector3.right);
        }

        public static void TagAll()
        {
            {
                if (!PlayerIsTagged(GorillaTagger.Instance.offlineVRRig))
                {
                    NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> <color=white>You must be tagged.</color>");
                    Menu.Main.GetIndex("Tag All [<color=red>50/50</color>]").enabled = false;
                }
                else
                {
                    bool isInfectedPlayers = false;
                    foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                    {
                        if (!PlayerIsTagged(vrrig))
                        {
                            isInfectedPlayers = true;
                            break;
                        }
                    }
                    if (isInfectedPlayers == true)
                    {
                        foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                        {
                            if (!PlayerIsTagged(vrrig))
                            {
                                GorillaTagger.Instance.offlineVRRig.enabled = false;
                                {
                                    Vector3 position = vrrig.transform.position + new Vector3(UnityEngine.Random.Range(-10f, 10f) / 10f, UnityEngine.Random.Range(-10f, 10f) / 10f, UnityEngine.Random.Range(-10f, 10f) / 10f);

                                    GorillaTagger.Instance.offlineVRRig.transform.position = position;
                                    GorillaTagger.Instance.myVRRig.transform.position = position;

                                    Quaternion rotation = Quaternion.Euler(new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360)));
                                    GorillaTagger.Instance.offlineVRRig.transform.rotation = rotation;
                                    GorillaTagger.Instance.myVRRig.transform.rotation = rotation;

                                    GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform.rotation = Quaternion.Euler(new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360)));
                                    GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = vrrig.transform.position + new Vector3(UnityEngine.Random.Range(-10f, 10f) / 10f, UnityEngine.Random.Range(-10f, 10f) / 10f, UnityEngine.Random.Range(-10f, 10f) / 10f);
                                    GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = vrrig.transform.position + new Vector3(UnityEngine.Random.Range(-10f, 10f) / 10f, UnityEngine.Random.Range(-10f, 10f) / 10f, UnityEngine.Random.Range(-10f, 10f) / 10f);

                                    GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.rotation = Quaternion.Euler(new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360)));
                                    GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.rotation = Quaternion.Euler(new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360)));

                                    GorillaTagger.Instance.offlineVRRig.leftIndex.calcT = 0f;
                                    GorillaTagger.Instance.offlineVRRig.leftMiddle.calcT = 0f;
                                    GorillaTagger.Instance.offlineVRRig.leftThumb.calcT = 0f;

                                    GorillaTagger.Instance.offlineVRRig.leftIndex.LerpFinger(1f, false);
                                    GorillaTagger.Instance.offlineVRRig.leftMiddle.LerpFinger(1f, false);
                                    GorillaTagger.Instance.offlineVRRig.leftThumb.LerpFinger(1f, false);

                                    GorillaTagger.Instance.offlineVRRig.rightIndex.calcT = 0f;
                                    GorillaTagger.Instance.offlineVRRig.rightMiddle.calcT = 0f;
                                    GorillaTagger.Instance.offlineVRRig.rightThumb.calcT = 0f;

                                    GorillaTagger.Instance.offlineVRRig.rightIndex.LerpFinger(1f, false);
                                    GorillaTagger.Instance.offlineVRRig.rightMiddle.LerpFinger(1f, false);
                                    GorillaTagger.Instance.offlineVRRig.rightThumb.LerpFinger(1f, false);
                                }

                                GorillaLocomotion.Player.Instance.rightControllerTransform.position = vrrig.transform.position;
                            }
                        }
                    }
                    else
                    {
                        NotifiLib.SendNotification("<color=grey></color><color=green>SUCCESS</color> : <color=grey></color> <color=white>Everyone is tagged!</color>");
                        GorillaTagger.Instance.offlineVRRig.enabled = true;
                        Menu.Main.GetIndex("Tag All [<color=red>50/50</color>]").enabled = false;
                    }
                }
            }
        }
        public static bool PlayerIsTagged(VRRig who)
        {
            string name = who.mainSkin.material.name.ToLower();
            return name.Contains("fected") || name.Contains("it") || name.Contains("stealth") || !who.nameTagAnchor.activeSelf;
        }

        public static List<NetPlayer> InfectedList()
        {
            List<NetPlayer> infected = new List<NetPlayer> { };
            string gamemode = GorillaGameManager.instance.GameModeName().ToLower();
            if (gamemode.Contains("infection") || gamemode.Contains("tag"))
            {
                GorillaTagManager tagman = GameObject.Find("GT Systems/GameModeSystem/Gorilla Tag Manager").GetComponent<GorillaTagManager>();
                if (tagman.isCurrentlyTag)
                {
                    infected.Add(tagman.currentIt);
                }
                else
                {
                    foreach (NetPlayer plr in tagman.currentInfected)
                    {
                        infected.Add(plr);
                    }
                }
            }
            if (gamemode.Contains("ghost"))
            {
                GorillaAmbushManager tagman = GameObject.Find("GT Systems/GameModeSystem/Gorilla GhostTag Manager").GetComponent<GorillaAmbushManager>();
                if (tagman.isCurrentlyTag)
                {
                    infected.Add(tagman.currentIt);
                }
                else
                {
                    foreach (NetPlayer plr in tagman.currentInfected)
                    {
                        infected.Add(plr);
                    }
                }
            }
            if (gamemode.Contains("ambush") || gamemode.Contains("stealth"))
            {
                GorillaAmbushManager tagman = GameObject.Find("GT Systems/GameModeSystem/Gorilla Stealth Manager").GetComponent<GorillaAmbushManager>();
                if (tagman.isCurrentlyTag)
                {
                    infected.Add(tagman.currentIt);
                }
                else
                {
                    foreach (NetPlayer plr in tagman.currentInfected)
                    {
                        infected.Add(plr);
                    }
                }
            }
            return infected;
        }
        public static void EnableVRRig()
        {
            GorillaTagger.Instance.offlineVRRig.enabled = true;
        }
        public static void TagSelf()
        {
            {
                if (InfectedList().Contains(PhotonNetwork.LocalPlayer))
                {
                    NotifiLib.SendNotification("<color=grey>[</color><color=green>SUCCESS</color><color=grey>]</color> <color=white>You have been tagged.</color>");
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                    Menu.Main.GetIndex("Tag Self [<color=red>50/50</color>]").enabled = false;
                }
                else
                {
                    foreach (VRRig rig in GorillaParent.instance.vrrigs)
                    {
                        if (PlayerIsTagged(rig))
                        {
                            GorillaTagger.Instance.offlineVRRig.enabled = false;
                            {
                                Quaternion rotation = Quaternion.Euler(new Vector3(0, UnityEngine.Random.Range(0, 360), 0));
                                GorillaTagger.Instance.offlineVRRig.transform.rotation = rotation;
                                GorillaTagger.Instance.myVRRig.transform.rotation = rotation;

                                GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform.rotation = Quaternion.Euler(new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360)));
                                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = GorillaTagger.Instance.offlineVRRig.transform.position + new Vector3(UnityEngine.Random.Range(-10f, 10f) / 10f, UnityEngine.Random.Range(-10f, 10f) / 10f, UnityEngine.Random.Range(-10f, 10f) / 10f);
                                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = GorillaTagger.Instance.offlineVRRig.transform.position + new Vector3(UnityEngine.Random.Range(-10f, 10f) / 10f, UnityEngine.Random.Range(-10f, 10f) / 10f, UnityEngine.Random.Range(-10f, 10f) / 10f);

                                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.rotation = Quaternion.Euler(new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360)));
                                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.rotation = Quaternion.Euler(new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360)));

                                GorillaTagger.Instance.offlineVRRig.leftIndex.calcT = 0f;
                                GorillaTagger.Instance.offlineVRRig.leftMiddle.calcT = 0f;
                                GorillaTagger.Instance.offlineVRRig.leftThumb.calcT = 0f;

                                GorillaTagger.Instance.offlineVRRig.leftIndex.LerpFinger(1f, false);
                                GorillaTagger.Instance.offlineVRRig.leftMiddle.LerpFinger(1f, false);
                                GorillaTagger.Instance.offlineVRRig.leftThumb.LerpFinger(1f, false);

                                GorillaTagger.Instance.offlineVRRig.rightIndex.calcT = 0f;
                                GorillaTagger.Instance.offlineVRRig.rightMiddle.calcT = 0f;
                                GorillaTagger.Instance.offlineVRRig.rightThumb.calcT = 0f;

                                GorillaTagger.Instance.offlineVRRig.rightIndex.LerpFinger(1f, false);
                                GorillaTagger.Instance.offlineVRRig.rightMiddle.LerpFinger(1f, false);
                                GorillaTagger.Instance.offlineVRRig.rightThumb.LerpFinger(1f, false);
                            }
                        }
                    }
                }
            }
        }
        public static void AntiModerator()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                bool flag = vrrig == GorillaTagger.Instance.offlineVRRig;
                if (!flag)
                {
                    bool flag2 = vrrig.concatStringOfCosmeticsAllowed.Contains("LBAAK.");
                    if (flag2)
                    {
                        NotifiLib.SendNotification("Moderator Detected");
                        PhotonNetwork.Disconnect();
                    }
                }
            }
        }
        public static VRRig VrRigPlayers = null;
        private static bool CopyPlayer;
        public static Color MainColor = new Color(0.50980395f, 0.22352941f, 0.9137255f);
        private static float blockDelay = 0f; 
        public static void BetaDropBlock(BuilderPiece piece, Vector3 pos, Quaternion rot)
        {
            BuilderTable.instance.RequestCreatePiece(piece.pieceType, pos, rot, piece.materialType);
            RPCProt();
        }

        public static List<BuilderPiece> archivepieces = null;
        private static float lastRecievedTime = -1f;

        public static BuilderPiece[] GetPieces()
        {
            if (Time.time > lastRecievedTime)
            {
                archivepieces = new List<BuilderPiece>();
                lastRecievedTime = Time.time + 5f;
            }
            if (archivepieces.Count == 0)
            {
                foreach (BuilderPiece lol in UnityEngine.Object.FindObjectsOfType<BuilderPiece>())
                {
                    if (!lol.isBuiltIntoTable)
                    {
                        archivepieces.Add(lol);
                    }
                }
            }
            return archivepieces.ToArray();
        }
        public static void TestBlockDrop(object builderTableInstance, BuilderTable.BuilderCommand cmd)
        {

            var traverse = Traverse.Create(builderTableInstance);

            traverse.Method("ExecuteBuildCommand", cmd).GetValue();
        }
        public static void TestBlockGrab(object builderTableInstance)
        {
            var traverse = Traverse.Create(builderTableInstance);

            traverse.Method("CreateLocalCommandId").GetValue();
        }
        public static float lastSpammerTime = 0f;
        public static float lastShooterTime = 0f;
        public static readonly float delay = 0f;
        public static bool shotgunRapid = false;
        public static void RapidFireShotgun()
        {
            shotgunRapid = true;
        }
        public static void RapidFireShotgunOff()
        {
            shotgunRapid = false;
        }
        public static bool lastHit = false;
        private static bool ghostMonke = false;
        public static void BlockGun()
        {
            bool rightGrab = ControllerInputPoller.instance.rightGrab;
            if (rightGrab)
            {
                RaycastHit raycastHit;
                Physics.Raycast(GorillaTagger.Instance.rightHandTransform.position, -GorillaTagger.Instance.rightHandTransform.up, out raycastHit);
                GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                gameObject.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                gameObject.GetComponent<Renderer>().material.color = MainColor;
                gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                gameObject.transform.position = (CopyPlayer ? VrRigPlayers.transform.position : raycastHit.point);
                UnityEngine.Object.Destroy(gameObject.GetComponent<BoxCollider>());
                UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
                UnityEngine.Object.Destroy(gameObject.GetComponent<Collider>());
                UnityEngine.Object.Destroy(gameObject, Time.deltaTime);
                GameObject gameObject2 = new GameObject("Line");
                LineRenderer lineRenderer = gameObject2.AddComponent<LineRenderer>();
                lineRenderer.material.shader = Shader.Find("GUI/Text Shader");
                lineRenderer.startColor = MainColor;
                lineRenderer.endColor = MainColor;
                lineRenderer.startWidth = 0.025f;
                lineRenderer.endWidth = 0.025f;
                lineRenderer.positionCount = 2;
                lineRenderer.useWorldSpace = true;
                lineRenderer.SetPosition(0, GorillaTagger.Instance.rightHandTransform.position);
                lineRenderer.SetPosition(1, CopyPlayer ? VrRigPlayers.transform.position : raycastHit.point);
                UnityEngine.Object.Destroy(gameObject2, Time.deltaTime);
                bool flag2 = (double)ControllerInputPoller.instance.rightControllerIndexFloat >= 0.1;
                if (flag2)
                {
                    BuilderPiece that = GetBlocks("snappiececolumn01")[0];
                    BetaDropBlock(that, raycastHit.transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
                    RPCProt();
                }
            }
            else
            {
                CopyPlayer = false;
            }
        }
        public static void OrbitBlocks()
        {
            if ((double)ControllerInputPoller.instance.rightControllerIndexFloat >= 0.1 || UnityInput.Current.GetKey(KeyCode.E))
            {
                BuilderPiece that = GetBlocks("snappiececolumn01")[0];
                BetaDropBlock(
                    that,
                    GorillaTagger.Instance.headCollider.transform.position + new Vector3(
                        (float)Math.Cos((double)Time.frameCount / 30),
                        0f,
                        (float)Math.Sin((double)Time.frameCount / 30)
                    ),
                    Quaternion.identity
                );
                RPCProt();
            }
        }
        public static List<BuilderPiece> GetBlocks(string blockname)
        {
            List<BuilderPiece> blocks = new List<BuilderPiece> { };

            foreach (BuilderPiece lol in GetPiecesFiltered())
            {
                if (lol.name.ToLower().Contains(blockname))
                {
                    blocks.Add(lol);
                }
            }

            return blocks;
        }
        private static float lastReceivedTime = 0f;
        private static List<BuilderPiece> archivepiecesfiltered = new List<BuilderPiece>() { };
        public static BuilderPiece[] GetPiecesFiltered()
        {
            if (Time.time > lastReceivedTime)
            {
                archivepiecesfiltered = null;
                lastReceivedTime = Time.time + 5f;
            }
            if (archivepiecesfiltered == null)
            {
                archivepiecesfiltered = new List<BuilderPiece>() { };
                foreach (BuilderPiece piece in GetPieces())
                {
                    if (piece.pieceType > 0)
                    {
                        archivepiecesfiltered.Add(piece);
                    }
                }
            }
            return archivepieces.ToArray();
        }
        public static void DisableGhostTog()
        {
            GorillaTagger.Instance.offlineVRRig.enabled = true;
        }
        public static void RPCProt()
        {
            GorillaNot.instance.rpcErrorMax = int.MaxValue;
            GorillaNot.instance.rpcCallLimit = int.MaxValue;
            GorillaNot.instance.logErrorMax = int.MaxValue;
            PhotonNetwork.MaxResendsBeforeDisconnect = int.MaxValue;
            PhotonNetwork.QuickResends = int.MaxValue;
            PhotonNetwork.OpCleanActorRpcBuffer(PhotonNetwork.LocalPlayer.ActorNumber);
            PhotonNetwork.SendAllOutgoingCommands();
            GorillaNot.instance.OnPlayerLeftRoom(PhotonNetwork.LocalPlayer);
        }
        private static string[] archiveCosmetics = null;
        public static void TryOnAnywhere()
        {
            archiveCosmetics = CosmeticsController.instance.currentWornSet.ToDisplayNameArray();
            string[] itjustworks = new string[] { "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU." };
            CosmeticsController.instance.currentWornSet = new CosmeticsController.CosmeticSet(itjustworks, CosmeticsController.instance);
            GorillaTagger.Instance.offlineVRRig.cosmeticSet = new CosmeticsController.CosmeticSet(itjustworks, CosmeticsController.instance);
            GorillaTagger.Instance.myVRRig.SendRPC("RPC_UpdateCosmeticsWithTryon", RpcTarget.All, new object[] { itjustworks, CosmeticsController.instance.tryOnSet.ToDisplayNameArray() });
            RPCProt();
            NotifiLib.SendNotification("<color=green>SUCCESS</color> : Stole Cosmetics: " + CosmeticsController.instance.currentWornSet.ToString());
        }
        public static void MoodRingAlwaysOn()
        {
            ControllerInputPoller.instance.rightControllerIndexFloat = 1f;
        }
        public static void BlockACEventsOn()
        {
            Menu.Main.blockAcEvents = true;
        }
        public static void BlockACEventsOff()
        {
            Menu.Main.blockAcEvents = false;
        }
        public static void UnlckMod()
        {
            archiveCosmetics = CosmeticsController.instance.currentWornSet.ToDisplayNameArray();
            string[] itjustworks = new string[] { "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU." };
            CosmeticsController.instance.currentWornSet = new CosmeticsController.CosmeticSet(itjustworks, CosmeticsController.instance);
            GorillaTagger.Instance.offlineVRRig.cosmeticSet = new CosmeticsController.CosmeticSet(itjustworks, CosmeticsController.instance);
            GorillaTagger.Instance.myVRRig.SendRPC("RPC_UpdateCosmeticsWithTryon", RpcTarget.All, new object[] { itjustworks, "LMAMM." });
            RPCProt();
        }
        public static void MoodRingAlwaysOff()
        {
            ControllerInputPoller.instance.rightControllerIndexFloat = 0f;
        }
        private static float CoolIt = 0.1f;
        private static float CoolItTimer;

        public static void SpazHats()
        {
            try
            {
                if (Time.time > CoolIt + CoolItTimer)
                {
                    CoolItTimer = Time.time;

                    GameObject cityWorkingPrefab = GameObject.Find("Environment Objects/LocalObjects_Prefab/City_WorkingPrefab");
                    if (cityWorkingPrefab != null)
                        cityWorkingPrefab.SetActive(true);

                    string categoryButtonsPath = "Environment Objects/LocalObjects_Prefab/City_WorkingPrefab/CosmeticsRoomAnchor/outsidestores_prefab/Bottom Layer/OutsideBuildings/Wardrobe Hut/SatelliteWardrobe/UI/CategoryButtons";
                    GameObject categoryButtonsObject = GameObject.Find(categoryButtonsPath);

                    if (categoryButtonsObject != null)
                    {
                        GorillaPressableButton[] categoryButtons = categoryButtonsObject.GetComponentsInChildren<GorillaPressableButton>();
                        foreach (var button in categoryButtons)
                        {
                            button.testPress = true;
                            button.ButtonActivationWithHand(false);
                        }
                    }

                    string scrollPrevPath = "Environment Objects/LocalObjects_Prefab/City_WorkingPrefab/CosmeticsRoomAnchor/outsidestores_prefab/Bottom Layer/OutsideBuildings/Wardrobe Hut/SatelliteWardrobe/UI/ScrollButtons/ScrollButton_Prev";
                    string scrollNextPath = "Environment Objects/LocalObjects_Prefab/City_WorkingPrefab/CosmeticsRoomAnchor/outsidestores_prefab/Bottom Layer/OutsideBuildings/Wardrobe Hut/SatelliteWardrobe/UI/ScrollButtons/ScrollButton_Next";

                    ClickButton(scrollPrevPath);
                    ClickButton(scrollNextPath);

                    for (int i = 1; i <= 5; i++)
                    {
                        string selectionButtonPath = $"Environment Objects/LocalObjects_Prefab/City_WorkingPrefab/CosmeticsRoomAnchor/outsidestores_prefab/Bottom Layer/OutsideBuildings/Wardrobe Hut/SatelliteWardrobe/UI/SelectionButtons/SelectionButton{i}";
                        ClickButton(selectionButtonPath);
                    }
                }
            }
            catch { Exception ex; }
        }
        private static float fast = 0.01f;
        private static float fafsdf = 0f;
        private static int currentItemIndex = 0;
        private static float fasts = 0.001f;
        private static float fafsdfs = 0f;
        private static int currentItemIndexBalloon = 0;
        public static void ClearCart()
        {
            CosmeticsController.instance.currentCart.Clear();
        }
        public static bool isRightHand = false;
        public static void UnlockAllExpensiveItems()
        {
            if ((double)ControllerInputPoller.instance.rightControllerIndexFloat >= 0.1 || UnityInput.Current.GetKey(KeyCode.E))
            {
                RPCProt();
                if (CosmeticsController.instance != null)
                {
                    if (currentItemIndex >= CosmeticsController.instance.allCosmetics.Count)
                    {
                        CosmeticsController.instance.currentCart.Clear();
                        currentItemIndex = 0;
                    }

                    for (int i = currentItemIndex; i < CosmeticsController.instance.allCosmetics.Count; i++)
                    {
                        var cosmeticItem = CosmeticsController.instance.allCosmetics[i];

                        if (cosmeticItem.cost > 10 && cosmeticItem.canTryOn == true)
                        {
                            if (Time.time > fast + fafsdf)
                            {
                                fafsdf = Time.time;

                                if (CosmeticsController.instance.currentCart.Contains(cosmeticItem))
                                {
                                    CosmeticsController.instance.currentCart.Remove(cosmeticItem);
                                }

                                CosmeticsController.instance.currentCart.Insert(0, cosmeticItem);

                                GorillaTelemetry.PostShopEvent(GorillaTagger.Instance.offlineVRRig, GTShopEventType.item_try_on, cosmeticItem);
                                GorillaTelemetry.PostShopEvent(GorillaTagger.Instance.offlineVRRig, GTShopEventType.cart_item_add, cosmeticItem);
                                GorillaTelemetry.PostShopEvent(GorillaTagger.Instance.offlineVRRig, GTShopEventType.item_select, cosmeticItem);
                                GorillaTelemetry.PostShopEvent(GorillaTagger.Instance.offlineVRRig, GTShopEventType.checkout_start, cosmeticItem);

                                GameObject.Find("Environment Objects/LocalObjects_Prefab/City_WorkingPrefab").SetActive(true);
                                GameObject.Find("Environment Objects/LocalObjects_Prefab/City_WorkingPrefab/CosmeticsRoomAnchor/nicegorillastore_prefab/DressingRoom_Mirrors_Prefab/ShoppingCart/Anchor/FittingRoomButton")
                                    .GetComponent<FittingRoomButton>().ButtonActivationWithHand(isRightHand);
                                isRightHand = !isRightHand;
                                currentItemIndex = i + 1;
                                return;
                            }
                        }
                    }
                }
            }
        }
        public static void SpamPopBallons()
        {
            RPCProt();
            if (CosmeticsController.instance != null)
            {
                if (currentItemIndexBalloon >= CosmeticsController.instance.allCosmetics.Count)
                {
                    CosmeticsController.instance.currentCart.Clear();
                    currentItemIndexBalloon = 0; 
                    return;
                }

                for (int i = currentItemIndexBalloon; i < CosmeticsController.instance.allCosmetics.Count; i++)
                {
                    var cosmeticItem = CosmeticsController.instance.allCosmetics[i];

                    if (cosmeticItem.cost > 10 &&
                        (cosmeticItem.displayName.Contains("LMAAT.") || cosmeticItem.displayName.Contains("LMAHE.") ||
                         cosmeticItem.displayName.Contains("LMAAR.") || cosmeticItem.displayName.Contains("LMABD.") ||
                         cosmeticItem.displayName.Contains("LMABW.") || cosmeticItem.displayName.Contains("LMAAP.")))
                    {
                        if (Time.time > fasts + fafsdfs)
                        {
                            fafsdfs = Time.time;

                            CosmeticsController.instance.currentCart.Clear();

                            CosmeticsController.instance.currentCart.Insert(0, cosmeticItem);

                            GorillaTelemetry.PostShopEvent(GorillaTagger.Instance.offlineVRRig, GTShopEventType.item_try_on, cosmeticItem);
                            GorillaTelemetry.PostShopEvent(GorillaTagger.Instance.offlineVRRig, GTShopEventType.cart_item_add, cosmeticItem);
                            GorillaTelemetry.PostShopEvent(GorillaTagger.Instance.offlineVRRig, GTShopEventType.item_select, cosmeticItem);
                            GorillaTelemetry.PostShopEvent(GorillaTagger.Instance.offlineVRRig, GTShopEventType.checkout_start, cosmeticItem);

                            GameObject.Find("Environment Objects/LocalObjects_Prefab/City_WorkingPrefab").SetActive(true);
                            GameObject.Find("Environment Objects/LocalObjects_Prefab/City_WorkingPrefab/CosmeticsRoomAnchor/nicegorillastore_prefab/DressingRoom_Mirrors_Prefab/ShoppingCart/Anchor/FittingRoomButton")
                                .GetComponent<FittingRoomButton>().ButtonActivationWithHand(true);

                            currentItemIndexBalloon = i + 1;
                            return;
                        }
                    }
                }
            }
        }
        public static bool isLefThandRR = false;
        private static void ClickButton(string path)
        {
            GameObject buttonObject = GameObject.Find(path);
            if (buttonObject != null)
            {
                GorillaPressableButton buttonComponent = buttonObject.GetComponent<GorillaPressableButton>(); 
                if (buttonComponent != null)
                {
                    buttonComponent.ButtonActivationWithHand(isLefThandRR);
                    isLefThandRR = !isLefThandRR;
                }
            }
        }
        public static void Invis()
        {
            bool flag = ControllerInputPoller.instance.rightControllerIndexFloat > 0.4f;
            if (flag)
            {
                BallsOnHands();
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(GorillaLocomotion.Player.Instance.headCollider.transform.position.x, -646.46466f, GorillaLocomotion.Player.Instance.headCollider.transform.position.z);
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
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
                    linerender.startColor = Color.magenta;
                    linerender.endColor = Color.magenta;
                    linerender.startWidth = 0.015f;
                    linerender.endWidth = 0.015f;
                    linerender.positionCount = 2;
                    linerender.useWorldSpace = true;
                    linerender.SetPosition(0, GorillaTagger.Instance.rightHandTransform.position);
                    linerender.SetPosition(1, vrrig.transform.position);
                    UnityEngine.Object.Destroy(line, Time.deltaTime);
                }
            }
            if (showEspSelf)
            {
                VRRig vrrig = GorillaTagger.Instance.offlineVRRig;
                {
                    GameObject line = new GameObject("TracerLineSelf");
                    LineRenderer linerender = line.AddComponent<LineRenderer>();
                    linerender.material.shader = Shader.Find("GUI/Text Shader");
                    linerender.startColor = Color.magenta;
                    linerender.endColor = Color.magenta;
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
                    UnityEngine.Object.Destroy(a);
                }
            }
        }
        public static void BoneESP()
        {
            Material material = new Material(Shader.Find("GUI/Text Shader"));
            material.color = Color.Lerp(Color.white, MainColor, Mathf.PingPong(Time.time, 1));

            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                {

                    if (!vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>())
                    {
                        vrrig.head.rigTarget.gameObject.AddComponent<LineRenderer>();
                    }



                    vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>().endWidth = 0.025f;
                    vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>().startWidth = 0.025f;
                    vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>().material = material;
                    vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>().SetPosition(0, vrrig.head.rigTarget.transform.position + new Vector3(0, 0.160f, 0));
                    vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>().SetPosition(1, vrrig.head.rigTarget.transform.position - new Vector3(0, 0.4f, 0));



                    for (int i = 0; i < bones.Count(); i += 2)
                    {
                        if (!vrrig.mainSkin.bones[bones[i]].gameObject.GetComponent<LineRenderer>())
                        {
                            vrrig.mainSkin.bones[bones[i]].gameObject.AddComponent<LineRenderer>();
                        }
                        vrrig.mainSkin.bones[bones[i]].gameObject.GetComponent<LineRenderer>().endWidth = 0.025f;
                        vrrig.mainSkin.bones[bones[i]].gameObject.GetComponent<LineRenderer>().startWidth = 0.025f;
                        vrrig.mainSkin.bones[bones[i]].gameObject.GetComponent<LineRenderer>().material = material;
                        vrrig.mainSkin.bones[bones[i]].gameObject.GetComponent<LineRenderer>().SetPosition(0, vrrig.mainSkin.bones[bones[i]].position);
                        vrrig.mainSkin.bones[bones[i]].gameObject.GetComponent<LineRenderer>().SetPosition(1, vrrig.mainSkin.bones[bones[i + 1]].position);
                    }
                }

            }
            if (showEspSelf)
            {
                VRRig vrrig2 = GorillaTagger.Instance.offlineVRRig;
                {

                    if (!vrrig2.head.rigTarget.gameObject.GetComponent<LineRenderer>())
                    {
                        vrrig2.head.rigTarget.gameObject.AddComponent<LineRenderer>();
                    }



                    vrrig2.head.rigTarget.gameObject.GetComponent<LineRenderer>().endWidth = 0.025f;
                    vrrig2.head.rigTarget.gameObject.GetComponent<LineRenderer>().startWidth = 0.025f;
                    vrrig2.head.rigTarget.gameObject.GetComponent<LineRenderer>().material = material;
                    vrrig2.head.rigTarget.gameObject.GetComponent<LineRenderer>().SetPosition(0, vrrig2.head.rigTarget.transform.position + new Vector3(0, 0.160f, 0));
                    vrrig2.head.rigTarget.gameObject.GetComponent<LineRenderer>().SetPosition(1, vrrig2.head.rigTarget.transform.position - new Vector3(0, 0.4f, 0));



                    for (int i = 0; i < bones.Count(); i += 2)
                    {
                        if (!vrrig2.mainSkin.bones[bones[i]].gameObject.GetComponent<LineRenderer>())
                        {
                            vrrig2.mainSkin.bones[bones[i]].gameObject.AddComponent<LineRenderer>();
                        }
                        vrrig2.mainSkin.bones[bones[i]].gameObject.GetComponent<LineRenderer>().endWidth = 0.025f;
                        vrrig2.mainSkin.bones[bones[i]].gameObject.GetComponent<LineRenderer>().startWidth = 0.025f;
                        vrrig2.mainSkin.bones[bones[i]].gameObject.GetComponent<LineRenderer>().material = material;
                        vrrig2.mainSkin.bones[bones[i]].gameObject.GetComponent<LineRenderer>().SetPosition(0, vrrig2.mainSkin.bones[bones[i]].position);
                        vrrig2.mainSkin.bones[bones[i]].gameObject.GetComponent<LineRenderer>().SetPosition(1, vrrig2.mainSkin.bones[bones[i + 1]].position);
                    }
                }

            }
        }
        public static int[] bones = { 4, 3, 5, 4, 19, 18, 20, 19, 3, 18, 21, 20, 22, 21, 25, 21, 29, 21, 31, 29, 27, 25, 24, 22, 6, 5, 7, 6, 10, 6, 14, 6, 16, 14, 12, 10, 9, 7 };
        public static void BoneESPOff()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                {
                    for (int i = 0; i < bones.Count(); i += 2)
                    {
                        if (vrrig.mainSkin.bones[bones[i]].gameObject.GetComponent<LineRenderer>())
                        {
                            UnityEngine.Object.Destroy(vrrig.mainSkin.bones[bones[i]].gameObject.GetComponent<LineRenderer>());
                        }
                        if (vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>())
                        {
                            UnityEngine.Object.Destroy(vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>());
                        }
                    }
                }
            }
            if (showEspSelf)
            {
                VRRig vrrig2 = GorillaTagger.Instance.offlineVRRig;
                {
                    for (int i = 0; i < bones.Count(); i += 2)
                    {
                        if (vrrig2.mainSkin.bones[bones[i]].gameObject.GetComponent<LineRenderer>())
                        {
                            UnityEngine.Object.Destroy(vrrig2.mainSkin.bones[bones[i]].gameObject.GetComponent<LineRenderer>());
                        }
                        if (vrrig2.head.rigTarget.gameObject.GetComponent<LineRenderer>())
                        {
                            UnityEngine.Object.Destroy(vrrig2.head.rigTarget.gameObject.GetComponent<LineRenderer>());
                        }
                    }
                }
            }
        }
        public static void ShowESPSelfOn()
        {
            showEspSelf = true;
        }
        public static void ShowESPSelfOff()
        {
            showEspSelf = false;
            VRRig vrrig2 = GorillaTagger.Instance.offlineVRRig;
            {
                for (int i = 0; i < bones.Count(); i += 2)
                {
                    if (vrrig2.mainSkin.bones[bones[i]].gameObject.GetComponent<LineRenderer>())
                    {
                        UnityEngine.Object.Destroy(vrrig2.mainSkin.bones[bones[i]].gameObject.GetComponent<LineRenderer>());
                    }
                    if (vrrig2.head.rigTarget.gameObject.GetComponent<LineRenderer>())
                    {
                        UnityEngine.Object.Destroy(vrrig2.head.rigTarget.gameObject.GetComponent<LineRenderer>());
                    }
                }
            }

            foreach (GameObject a in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (a.name == "TracerLineSelf")
                {
                    UnityEngine.Object.Destroy(a);
                }
            }
        }
        public static void Flight()
        {
            if (ControllerInputPoller.instance.rightGrab)
            {
                GorillaLocomotion.Player.Instance.transform.position += GorillaLocomotion.Player.Instance.headCollider.transform.forward * Time.deltaTime * 15;
                GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }
}
