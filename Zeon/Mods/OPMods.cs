using BepInEx;
using GorillaTagScripts;
using Photon.Pun;
using POpusCodec.Enums;
using Zeon.Patches;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zeon.Mods;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Zeon.Classes;
using HarmonyLib;
using static UnityEngine.ProBuilder.AutoUnwrapSettings;
using Zeon.Notifications;
using GorillaGameModes;

/* 
Zeon
Created by athena (@trix9x)
Zeon falls under the GPL-3.0 license. Do not take credit for the work as it remains copyrighted under the terms of this license.

Any comments here are completely for me, do not take my code without permission
*/
namespace Zeon.Mods
{
    internal class OPMods
    {
        public static void DestroyAll()
        {
            foreach (VRRig plr in GorillaParent.instance.vrrigs)
            {
                PhotonNetwork.OpRemoveCompleteCacheOfPlayer(plr.Creator.ActorNumber);
            }
        }
        public static TappableGuardianIdol[] aaa = null;
        public static TappableGuardianIdol[] allthinhgys()
        {
            if (Time.time > lasttine)
            {
                aaa = null;
                lasttine = Time.time + 30f;
            }
            if (aaa == null)
            {
                aaa = UnityEngine.Object.FindObjectsOfType<TappableGuardianIdol>();
            }
            return aaa;
        }
        public static float lasttine = 0f;
        public static float lastExecutionTime = 0f;
        public static float TryGetGuardiandelay = 0.2f;

        public static void GetGuardian()
        {
            if (Time.time < lastExecutionTime + TryGetGuardiandelay) return; 

            lastExecutionTime = Time.time;

            GorillaGuardianManager hgghffg = GameObject.Find("GT Systems/GameModeSystem/Gorilla Guardian Manager").GetComponent<GorillaGuardianManager>();
            if (hgghffg != null)
            {
                if (!hgghffg.IsPlayerGuardian(NetworkSystem.Instance.LocalPlayer))
                {
                    foreach (TappableGuardianIdol garoauialidea in allthinhgys())
                    {
                        if (!garoauialidea.isChangingPositions)
                        {
                            GorillaGuardianManager hhh = GameObject.Find("GT Systems/GameModeSystem/Gorilla Guardian Manager").GetComponent<GorillaGuardianManager>();
                            if (!hhh.IsPlayerGuardian(NetworkSystem.Instance.LocalPlayer))
                            {
                                NotifiLib.SendNotification("<color=red>NOT GAURDIAN, PLEASE WAIT...</color>");
                                GorillaTagger.Instance.offlineVRRig.enabled = false;
                                GorillaTagger.Instance.offlineVRRig.transform.position = garoauialidea.transform.position;

                                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = garoauialidea.transform.position;
                                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = garoauialidea.transform.position;

                                garoauialidea.manager.photonView.RPC("SendOnTapRPC", RpcTarget.All, garoauialidea.tappableId, UnityEngine.Random.Range(0.2f, 0.4f));
                                NormalMods.RPCProt();

                            }
                        }
                        else
                        {
                            GorillaTagger.Instance.offlineVRRig.enabled = true;
                        }
                    }
                }
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
                NotifiLib.SendNotification("<color=red>FAILED, YOU ARE ALREADY GUARDIAN</color>");
                ZeonMain.getGuardian = false;
            }
        }
        public static void CrashAllAsGuardian()
        {
            GorillaGuardianManager hgghffg = GameObject.Find("GT Systems/GameModeSystem/Gorilla Guardian Manager").GetComponent<GorillaGuardianManager>();
            if (hgghffg.IsPlayerGuardian(NetworkSystem.Instance.LocalPlayer))
            {
                TryGetGuardiandelay = 0f;
                foreach (VRRig a in GorillaParent.instance.vrrigs)
                {
                    if (!a.isLocal)
                    {
                        NetworkView aaa = (NetworkView)Traverse.Create(typeof(GameMode)).Field("activeNetworkHandler").Field("netView").GetValue();
                        aaa.SendRPC("GrabbedByPlayer", a.Creator, new object[] { true, false, false });

                        Debug.Log($"Flinging {a.Creator.NickName} out of the game.");

                        aaa.SendRPC("DroppedByPlayer", a.Creator, new object[] { new Vector3(float.NaN, float.NaN, float.NaN) });
                        GorillaTagger.Instance.offlineVRRig.enabled = true;

                    }
                    if (PhotonNetwork.PlayerList.Length < 2)
                    {
                        NotifiLib.SendNotification("<color=green>SUCCESS, CRASHED LOBBY</color>");
                        TryGetGuardiandelay = 0.2f;
                        ZeonMain.GCrasAll = false;
                    }
                }
            }
        }
        public static void BlockSpammer()
        {
            if ((double)ControllerInputPoller.instance.rightControllerIndexFloat >= 0.1 || UnityInput.Current.GetKey(KeyCode.E))
            {
                NormalMods.lastSpammerTime = Time.time;
                CoroutineManager.RunCoroutine(ProcessPiece());
                NormalMods.RPCProt();
            }
        }
        public static bool justdidthat = false;
        public static float lastExecutionTime2 = 0f;
        public static float lagAllDel = 0.2f;
        public static void TestLagAll()
        {
            string[] messages = new string[]
            {
                "trying to inappropriately create game managers",
                "trying to create multiple game managers",
                "possible kick attempt",
                "room host force changed",
                "inappropriate tag data being sent",
                "taking master to ban player",
                "gorvity bisdabled",
                "wack rad.",
                "empty rig",
                "evading the name ban",
                "extremely far tag",
                "inappropriate tag data being sent multiple vrrigs",
                "inappropriate tag data being sent creating multiple vrrigs",
                "inappropriate tag data being sent init noob",
                "inappropriate tag data being sent set tagged time",
                "inappropriate tag data being sent set slowed time",
                "inappropriate tag data being sent set join tagged time",
                "inappropriate tag data being sent play tag sound",
                "inappropriate tag data being sent bonk",
                "inappropriate tag data being sent drum",
                "inappropriate tag data being sent self only instrument",
                "inappropriate tag data being sent hand tap",
                "inappropriate tag data being sent update cosmetics",
                "inappropriate tag data being sent update cosmetics with tryon",
                "invalid projectile state",
                "invalid impact state",
                "too many rpc calls!",
                "invalid world shareable",
                "Sent an SetOwnershipFromMasterClient when they weren't the master client",
                "projectile error",
                "invalid tag",
                "creating rigs as room objects",
                "creating rigs for someone else",
                "splash effect",
                "geode effect"
            };

            string[] array = new string[GameObject.Find("Networking Scripts/GorillaReporter").GetComponent<GorillaNot>().cachedPlayerList.Length];
            int num = 0;

            foreach (NetPlayer netPlayer in GameObject.Find("Networking Scripts/GorillaReporter").GetComponent<GorillaNot>().cachedPlayerList)
            {
                array[num] = netPlayer.UserId;
                num++;
            }

            foreach (VRRig a in GorillaParent.instance.vrrigs)
            {
                if (!a.isLocal)
                {
                    string randomMessage = messages[UnityEngine.Random.Range(0, messages.Length)];

                    PhotonNetwork.RaiseEvent(
                        8,
                        new object[]
                        {
                    NetworkSystem.Instance.RoomStringStripped(),
                    array,
                    NetworkSystem.Instance.MasterClient.UserId,
                    a.OwningNetPlayer.UserId,
                    a.OwningNetPlayer.NickName,
                    randomMessage,
                    NetworkSystemConfig.AppVersion
                        },
                        new RaiseEventOptions { CachingOption = EventCaching.AddToRoomCache },
                        new ExitGames.Client.Photon.SendOptions { Encrypt = false }

                    );
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
                    WebFlags flags = new WebFlags(1);
                    raiseEventOptions.Flags = flags;
                    object[] eventContent = new object[0];
                    PhotonNetwork.RaiseEvent(10, eventContent, new RaiseEventOptions { CachingOption = EventCaching.AddToRoomCache }, SendOptions.SendReliable);

                    GameObject.Find("Networking Scripts/GorillaReporter").GetComponent<GorillaNot>().SendReport(randomMessage, a.OwningNetPlayer.UserId, a.OwningNetPlayer.NickName);
                }
            }
        }
        public static void TestCrash()
        {
            if ((double)ControllerInputPoller.instance.rightControllerIndexFloat >= 0.1 || UnityInput.Current.GetKey(KeyCode.E))
            {
                NormalMods.lastSpammerTime = Time.time;
                CoroutineManager.RunCoroutine(ProcessTreePiece());
                Debug.Log("spawn tree block plz!");
                NormalMods.RPCProt();
            }
        }

        public static void AntiCrashBlock()
        {
            foreach (BuilderPiece piece in GameObject.FindObjectsOfType<BuilderPiece>())
            {
                if (piece.gameObject.active == true)
                {
                    piece.gameObject.SetActive(false);
                }
            }
        }
        public static void BlockSpammerTest()
        {
            if ((double)ControllerInputPoller.instance.rightControllerIndexFloat >= 0.1 || UnityInput.Current.GetKey(KeyCode.E))
            {
                NormalMods.lastSpammerTime = Time.time;
                CoroutineManager.RunCoroutine(ProcessPieceTest());
                NormalMods.RPCProt();
            }
        }
        public static void SSCube()
        {
            if ((double)ControllerInputPoller.instance.rightControllerIndexFloat >= 0.1 || UnityInput.Current.GetKey(KeyCode.E))
            {
                NormalMods.lastSpammerTime = Time.time;
                CoroutineManager.RunCoroutine(ProcessSquarePiece(4, 4, 1f));
                NormalMods.RPCProt();
            }
        }
        public static void BlueBlockSpammer()
        {
            if ((double)ControllerInputPoller.instance.rightControllerIndexFloat >= 0.1 || UnityInput.Current.GetKey(KeyCode.E))
            {
                NormalMods.lastSpammerTime = Time.time;
                CoroutineManager.RunCoroutine(ProcessBluePiece());
                NormalMods.RPCProt();
            }
        }
        public static IEnumerator CreateGetPieceIIDK(int pieceType, System.Action<BuilderPiece> onComplete)
        {
            BuilderPiece target = null;
            CreatePatch.enabled = true;
            CreatePatch.pieceTypeSearch = pieceType;
            yield return null;
            BuilderTable.instance.RequestCreatePiece(pieceType, GorillaTagger.Instance.headCollider.transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity, 0);
            while (pieceId < 0)
            {
                yield return null;
            }
            yield return null;
            target = BuilderTable.instance.GetPiece(pieceId);
            pieceId = -1;
            CreatePatch.enabled = false;
            CreatePatch.pieceTypeSearch = 0;
            if (onComplete != null)
            {
                onComplete(target);
            }
            yield break;
        }

        public static void BlockLine()
        {
            if ((double)ControllerInputPoller.instance.rightControllerIndexFloat >= 0.1 || UnityInput.Current.GetKey(KeyCode.E))
            {
                NormalMods.lastShooterTime = Time.time;
                CoroutineManager.RunCoroutine(ProcessLinePiece());
                NormalMods.RPCProt();
            }
        }
        public static void BlockAura()
        {
            if ((double)ControllerInputPoller.instance.rightControllerIndexFloat >= 0.1 || UnityInput.Current.GetKey(KeyCode.E))
            {
                NormalMods.lastShooterTime = Time.time;
                CoroutineManager.RunCoroutine(AuraBlockCor());
                NormalMods.RPCProt();
            }
        }
        public static void BlockShootRandom()
        {
            if ((double)ControllerInputPoller.instance.rightControllerIndexFloat >= 0.1 || UnityInput.Current.GetKey(KeyCode.E))
            {
                NormalMods.lastShooterTime = Time.time;
                CoroutineManager.RunCoroutine(ProcessRanLinePiece());
                NormalMods.RPCProt();
            }
        }
        public static void BlockAuraRandom()
        {
            if ((double)ControllerInputPoller.instance.rightControllerIndexFloat >= 0.1 || UnityInput.Current.GetKey(KeyCode.E))
            {
                NormalMods.lastShooterTime = Time.time;
                CoroutineManager.RunCoroutine(AuraRanBlockCor());
                NormalMods.RPCProt();
            }
        }
        public static void BlockRain()
        {
            if ((double)ControllerInputPoller.instance.rightControllerIndexFloat >= 0.1 || UnityInput.Current.GetKey(KeyCode.E))
            {
                NormalMods.lastShooterTime = Time.time;
                CoroutineManager.RunCoroutine(RainBlockCor());
                NormalMods.RPCProt();
            }
        }
        public static void DestroyBlocks()
        {
            if ((double)ControllerInputPoller.instance.rightControllerIndexFloat >= 0.1 || UnityInput.Current.GetKey(KeyCode.E))
            {
                NormalMods.lastShooterTime = Time.time;
                CoroutineManager.RunCoroutine(ProcessTreePiece());
                BuilderTable.instance.builderNetworking.PlayerEnterBuilder();
                NormalMods.RPCProt();
            }
        }
        public static void BlockRandom()
        {
            if ((double)ControllerInputPoller.instance.rightControllerIndexFloat >= 0.1 || UnityInput.Current.GetKey(KeyCode.E))
            {
                NormalMods.lastShooterTime = Time.time;
                CoroutineManager.RunCoroutine(SpamRandomPiece());
                NormalMods.RPCProt();
            }
        }
        public static void BlockRandomRain()
        {
            if ((double)ControllerInputPoller.instance.rightControllerIndexFloat >= 0.1 || UnityInput.Current.GetKey(KeyCode.E))
            {
                NormalMods.lastShooterTime = Time.time;
                CoroutineManager.RunCoroutine(SpamRandomPieceRain());
                NormalMods.RPCProt();
            }
        }
        public static void BlockExplode()
        {
            if ((double)ControllerInputPoller.instance.rightControllerIndexFloat >= 0.1 || UnityInput.Current.GetKey(KeyCode.E))
            {
                NormalMods.lastShooterTime = Time.time;
                CoroutineManager.RunCoroutine(RainBlockCor());
                NormalMods.RPCProt();
            }
        }

        public static int pieceId = -1;
        public static IEnumerator CreateGetPiece(int pieceType, System.Action<BuilderPiece> onComplete)
        {
            BuilderPiece target = null;

            Patches.CreatePatch.enabled = true;
            Patches.CreatePatch.pieceTypeSearch = pieceType;

            yield return null;

            BuilderTable.instance.RequestCreatePiece(pieceType, GorillaTagger.Instance.rightHandTransform.position, GorillaTagger.Instance.rightHandTransform.rotation, 0);

            while (pieceId < 0)
            {
                yield return null;
            }
            yield return null;

            target = BuilderTable.instance.GetPiece(pieceId);
            pieceId = -1;
            Patches.CreatePatch.enabled = false;
            Patches.CreatePatch.pieceTypeSearch = 0;

            onComplete?.Invoke(target);
        }
        public static IEnumerator CubeGetPiece(int pieceType, Vector3 spawnPosition, Quaternion spawnRotation, System.Action<BuilderPiece> onComplete)
        {
            BuilderPiece target = null;

            Patches.CreatePatch.enabled = true;
            Patches.CreatePatch.pieceTypeSearch = pieceType;

            yield return null;

            BuilderTable.instance.RequestCreatePiece(pieceType, spawnPosition, spawnRotation, 0);

            while (pieceId < 0)
            {
                yield return null;
            }
            yield return null;

            target = BuilderTable.instance.GetPiece(pieceId);
            pieceId = -1;
            Patches.CreatePatch.enabled = false;
            Patches.CreatePatch.pieceTypeSearch = 0;

            onComplete?.Invoke(target);
        }
        private static IEnumerator ProcessSquarePiece(int rows, int cols, float spacing)
        {
            Vector3 startPosition = GorillaTagger.Instance.rightHandTransform.position + GorillaTagger.Instance.rightHandTransform.forward * 5f; // Adjust distance in front of the hand
            Quaternion spawnRotation = GorillaTagger.Instance.rightHandTransform.rotation;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    Vector3 spawnPosition = startPosition
                                            + GorillaTagger.Instance.rightHandTransform.right * (col * spacing)
                                            + GorillaTagger.Instance.rightHandTransform.up * (row * spacing);

                    BuilderPiece piece = null;

                    yield return CubeGetPiece(1858470402, spawnPosition, spawnRotation, createdPiece =>
                    {
                        piece = createdPiece;
                    });

                    while (piece == null)
                    {
                        yield return null;
                    }

                    Vector3 grabPosition = GorillaTagger.Instance.rightHandTransform.position;
                    Quaternion grabRotation = GorillaTagger.Instance.rightHandTransform.rotation;
                    bool isLeftHand = true;
                    BuilderTable.instance.RequestGrabPiece(piece, isLeftHand, grabPosition, grabRotation);

                    Vector3 dropPosition = spawnPosition;
                    Quaternion dropRotation = spawnRotation;
                    Vector3 dropVelocity = GorillaTagger.Instance.rightHandTransform.forward * 1f;
                    Vector3 dropAngularVelocity = new Vector3(UnityEngine.Random.Range(-36f, 36f), UnityEngine.Random.Range(-36f, 36f), UnityEngine.Random.Range(-36f, 36f));
                    BuilderTable.instance.RequestDropPiece(piece, dropPosition, dropRotation, dropVelocity, dropAngularVelocity);
                }
            }
        }

        public static IEnumerator CreateAboveGetPiece(int pieceType, System.Action<BuilderPiece> onComplete)
        {
            BuilderPiece target = null;

            Patches.CreatePatch.enabled = true;
            Patches.CreatePatch.pieceTypeSearch = pieceType;

            yield return null;

            BuilderTable.instance.RequestCreatePiece(pieceType, GorillaTagger.Instance.rightHandTransform.position, GorillaTagger.Instance.rightHandTransform.rotation, 0);

            while (pieceId < 0)
            {
                yield return null;
            }
            yield return null;

            target = BuilderTable.instance.GetPiece(pieceId);
            pieceId = -1;
            Patches.CreatePatch.enabled = false;
            Patches.CreatePatch.pieceTypeSearch = 0;

            onComplete?.Invoke(target);
        }
        public static IEnumerator CreateLineGetPiece(int pieceType, Vector3 dropPosition, Quaternion dropRotation, System.Action<BuilderPiece> onComplete)
        {
            BuilderPiece target = null;

            Patches.CreatePatch.enabled = true;
            Patches.CreatePatch.pieceTypeSearch = pieceType;

            yield return null;

            BuilderTable.instance.RequestCreatePiece(pieceType, dropPosition, dropRotation, 0);

            while (pieceId < 0)
            {
                yield return null;
            }
            yield return null;

            target = BuilderTable.instance.GetPiece(pieceId);
            pieceId = -1;
            Patches.CreatePatch.enabled = false;
            Patches.CreatePatch.pieceTypeSearch = 0;

            onComplete?.Invoke(target);
        }

        public static IEnumerator CreateAuraGetPiece(int pieceType, System.Action<BuilderPiece> onComplete)
        {
            BuilderPiece target = null;

            Patches.CreatePatch.enabled = true;
            Patches.CreatePatch.pieceTypeSearch = pieceType;

            yield return null;

            BuilderTable.instance.RequestCreatePiece(pieceType, GorillaTagger.Instance.offlineVRRig.transform.position + new Vector3(UnityEngine.Random.Range(-1.5f, 1.5f), UnityEngine.Random.Range(-0.5f, 1.5f), UnityEngine.Random.Range(-1.5f, 1.5f)), GorillaTagger.Instance.rightHandTransform.rotation, 0);

            while (pieceId < 0)
            {
                yield return null;
            }
            yield return null;

            target = BuilderTable.instance.GetPiece(pieceId);
            pieceId = -1;
            Patches.CreatePatch.enabled = false;
            Patches.CreatePatch.pieceTypeSearch = 0;

            onComplete?.Invoke(target);
        }
        public static IEnumerator CreateRainGetPiece(int pieceType, System.Action<BuilderPiece> onComplete)
        {
            BuilderPiece target = null;

            Patches.CreatePatch.enabled = true;
            Patches.CreatePatch.pieceTypeSearch = pieceType;

            yield return null;

            BuilderTable.instance.RequestCreatePiece(pieceType, GorillaTagger.Instance.offlineVRRig.transform.position + new Vector3(UnityEngine.Random.Range(-3f, 3f), 4f, UnityEngine.Random.Range(-3f, 3f)), GorillaTagger.Instance.rightHandTransform.rotation, 0);

            while (pieceId < 0)
            {
                yield return null;
            }
            yield return null;

            target = BuilderTable.instance.GetPiece(pieceId);
            pieceId = -1;
            Patches.CreatePatch.enabled = false;
            Patches.CreatePatch.pieceTypeSearch = 0;

            onComplete?.Invoke(target);
        }
        private static IEnumerator ProcessPiece()
        {
            BuilderPiece piece = null;

            yield return CreateGetPiece(-1927069002, createdPiece =>
            {
                piece = createdPiece;
            });

            while (piece == null)
            {
                yield return null;
            }

            Vector3 grabPosition = GorillaTagger.Instance.rightHandTransform.position;
            Quaternion grabRotation = GorillaTagger.Instance.rightHandTransform.rotation;
            bool isLeftHand = true;
            BuilderTable.instance.RequestGrabPiece(piece, isLeftHand, grabPosition, grabRotation);

            Vector3 dropPosition = GorillaTagger.Instance.rightHandTransform.position;
            Quaternion dropRotation = GorillaTagger.Instance.rightHandTransform.rotation;
            Vector3 dropVelocity = GorillaTagger.Instance.rightHandTransform.forward * 1f;
            Vector3 dropAngularVelocity = new Vector3(UnityEngine.Random.Range(-36f, 36f), UnityEngine.Random.Range(-36f, 36f), UnityEngine.Random.Range(-36f, 36f));
            BuilderTable.instance.RequestDropPiece(piece, dropPosition, dropRotation, dropVelocity, dropAngularVelocity);
        }
        private static IEnumerator ProcessTreePiece()
        {
            BuilderPiece piece = null;

            yield return CreateGetPiece(2059548340, createdPiece =>
            {
                piece = createdPiece;
            });

            while (piece == null)
            {
                yield return null;
            }

            Vector3 grabPosition = GorillaTagger.Instance.rightHandTransform.position;
            Quaternion grabRotation = GorillaTagger.Instance.rightHandTransform.rotation;
            bool isLeftHand = true;
            BuilderTable.instance.RequestGrabPiece(piece, isLeftHand, grabPosition, grabRotation);

            Vector3 dropPosition = GorillaTagger.Instance.rightHandTransform.position;
            Quaternion dropRotation = GorillaTagger.Instance.rightHandTransform.rotation;
            Vector3 dropVelocity = GorillaTagger.Instance.rightHandTransform.forward * 1f;
            Vector3 dropAngularVelocity = new Vector3(UnityEngine.Random.Range(-36f, 36f), UnityEngine.Random.Range(-36f, 36f), UnityEngine.Random.Range(-36f, 36f));
            BuilderTable.instance.RequestDropPiece(piece, dropPosition, dropRotation, dropVelocity, dropAngularVelocity);
        }
        private static List<int> castlePieceTypes = new List<int>
        {
            -57667458, -1310012383, -2116490732, -1961511959, 1459246571, 1008893018, 38505905,
            2104851213, 405537871, -1508642259, -666919797, -1206998199, 19594543, 1477544431,
            1102387446, 654087561, -482116696, -297072615, 1685601286
        };

        private static IEnumerator ProcessPieceTest()
        {
            BuilderPiece piece = null;
            int index = 0;

            while (true)
            {
                yield return CreateGetPiece(castlePieceTypes[index], createdPiece =>
                {
                    piece = createdPiece;
                });

                while (piece == null)
                {
                    yield return null;
                }

                Vector3 grabPosition = GorillaTagger.Instance.rightHandTransform.position;
                Quaternion grabRotation = GorillaTagger.Instance.rightHandTransform.rotation;
                bool isLeftHand = true;
                BuilderTable.instance.RequestGrabPiece(piece, isLeftHand, grabPosition, grabRotation);

                Vector3 dropPosition = GorillaTagger.Instance.rightHandTransform.position;
                Quaternion dropRotation = GorillaTagger.Instance.rightHandTransform.rotation;
                Vector3 dropVelocity = GorillaTagger.Instance.rightHandTransform.forward * 1f;
                Vector3 dropAngularVelocity = new Vector3(UnityEngine.Random.Range(-36f, 36f), UnityEngine.Random.Range(-36f, 36f), UnityEngine.Random.Range(-36f, 36f));
                BuilderTable.instance.RequestDropPiece(piece, dropPosition, dropRotation, dropVelocity, dropAngularVelocity);

                index = (index + 1) % castlePieceTypes.Count;
                yield return null;
            }
        }

        private static IEnumerator ProcessBluePiece()
        {
            BuilderPiece piece = null;

            yield return CreateAboveGetPiece(1858470402, createdPiece =>
            {
                piece = createdPiece;
            });

            while (piece == null)
            {
                yield return null;
            }

            Vector3 grabPosition = GorillaTagger.Instance.rightHandTransform.position;
            Quaternion grabRotation = GorillaTagger.Instance.rightHandTransform.rotation;
            bool isLeftHand = true;
            BuilderTable.instance.RequestGrabPiece(piece, isLeftHand, grabPosition, grabRotation);

            Vector3 dropPosition = GorillaTagger.Instance.rightHandTransform.position;
            Quaternion dropRotation = GorillaTagger.Instance.rightHandTransform.rotation;
            Vector3 dropVelocity = GorillaTagger.Instance.rightHandTransform.forward * 1f;
            Vector3 dropAngularVelocity = new Vector3(UnityEngine.Random.Range(-36f, 36f), UnityEngine.Random.Range(-36f, 36f), UnityEngine.Random.Range(-36f, 36f));
            BuilderTable.instance.RequestDropPiece(piece, dropPosition, dropRotation, dropVelocity, dropAngularVelocity);
        }
        private static IEnumerator ProcessLinePiece()
        {
            Vector3 startPosition = GorillaTagger.Instance.rightHandTransform.position;
            Vector3 dropPosition = startPosition;
            Quaternion dropRotation = GorillaTagger.Instance.rightHandTransform.rotation;
            float distanceMoved = 0f;

            BuilderPiece piece = null;

            yield return CreateGetPieceIIDK(-1927069002, createdPiece =>
            {
                piece = createdPiece;
            });


            while (piece == null)
            {
                yield return null;
            }

            Vector3 grabPosition = GorillaTagger.Instance.rightHandTransform.position;
            Quaternion grabRotation = GorillaTagger.Instance.rightHandTransform.rotation;
            bool isLeftHand = true;
            BuilderTable.instance.RequestGrabPiece(piece, isLeftHand, grabPosition, grabRotation);

            Vector3 dropVelocity = GorillaTagger.Instance.rightHandTransform.forward * 10f;
            Vector3 dropAngularVelocity = GorillaTagger.Instance.rightHandTransform.forward * 50f;
            BuilderTable.instance.RequestDropPiece(piece, NormalMods.TrueRightHand().position + NormalMods.TrueRightHand().forward * 0.65f + NormalMods.TrueRightHand().right * 0.03f + NormalMods.TrueRightHand().up * 0.05f, NormalMods.TrueRightHand().rotation, NormalMods.TrueRightHand().forward * 19.9f, Vector3.zero);
        }

        private static IEnumerator AuraBlockCor()
        {
            BuilderPiece piece = null;

            yield return CreateAuraGetPiece(-1927069002, createdPiece =>
            {
                piece = createdPiece;
            });

            while (piece == null)
            {
                yield return null;
            }

            Vector3 grabPosition = GorillaTagger.Instance.rightHandTransform.position;
            Quaternion grabRotation = GorillaTagger.Instance.rightHandTransform.rotation;
            bool isLeftHand = true;
            BuilderTable.instance.RequestGrabPiece(piece, isLeftHand, grabPosition, grabRotation);

            Vector3 dropPosition = GorillaTagger.Instance.offlineVRRig.transform.position + new Vector3(UnityEngine.Random.Range(-1.5f, 1.5f), UnityEngine.Random.Range(-0.5f, 1.5f), UnityEngine.Random.Range(-1.5f, 1.5f));
            Quaternion dropRotation = GorillaTagger.Instance.rightHandTransform.rotation;
            Vector3 dropVelocity = GorillaTagger.Instance.rightHandTransform.forward * 1f;
            Vector3 dropAngularVelocity = new Vector3(UnityEngine.Random.Range(-36f, 36f), UnityEngine.Random.Range(-36f, 36f), UnityEngine.Random.Range(-36f, 36f));
            BuilderTable.instance.RequestDropPiece(piece, dropPosition, dropRotation, dropVelocity, dropAngularVelocity);
        }
        private static IEnumerator RainBlockCor()
        {
            BuilderPiece piece = null;

            yield return CreateRainGetPiece(-1927069002, createdPiece =>
            {
                piece = createdPiece;
            });

            while (piece == null)
            {
                yield return null;
            }

            Vector3 grabPosition = GorillaTagger.Instance.rightHandTransform.position;
            Quaternion grabRotation = GorillaTagger.Instance.rightHandTransform.rotation;
            bool isLeftHand = true;
            BuilderTable.instance.RequestGrabPiece(piece, isLeftHand, grabPosition, grabRotation);

            Vector3 dropPosition = GorillaTagger.Instance.offlineVRRig.transform.position + new Vector3(UnityEngine.Random.Range(-3f, 3f), 4f, UnityEngine.Random.Range(-3f, 3f));
            Quaternion dropRotation = GorillaTagger.Instance.rightHandTransform.rotation;
            Vector3 dropVelocity = GorillaTagger.Instance.rightHandTransform.forward * 1f;
            Vector3 dropAngularVelocity = new Vector3(UnityEngine.Random.Range(-36f, 36f), UnityEngine.Random.Range(-36f, 36f), UnityEngine.Random.Range(-36f, 36f));
            BuilderTable.instance.RequestDropPiece(piece, dropPosition, dropRotation, dropVelocity, dropAngularVelocity);
        }
        private static IEnumerator ProcessRanLinePiece()
        {
            BuilderPiece piece = null;
            if (pieceTypes.Count == 0)
            {
                GetAllSigma();
                yield return null;
            }
            else
            {
                Vector3 startPosition = GorillaTagger.Instance.rightHandTransform.position;
                Vector3 dropPosition = startPosition;
                Quaternion dropRotation = GorillaTagger.Instance.rightHandTransform.rotation;
                int randomPieceType = pieceTypes[UnityEngine.Random.Range(0, pieceTypes.Count)];
                yield return CreateLineGetPiece(randomPieceType, startPosition, dropRotation, createdPiece =>
                {
                    piece = createdPiece;
                });

                while (piece == null)
                {
                    yield return null;
                }

                Vector3 grabPosition = GorillaTagger.Instance.rightHandTransform.position;
                Quaternion grabRotation = GorillaTagger.Instance.rightHandTransform.rotation;
                bool isLeftHand = true;
                BuilderTable.instance.RequestGrabPiece(piece, isLeftHand, grabPosition, grabRotation);

                Vector3 dropVelocity = GorillaTagger.Instance.rightHandTransform.forward * 10f;
                Vector3 dropAngularVelocity = GorillaTagger.Instance.rightHandTransform.forward * 50f;
                BuilderTable.instance.RequestDropPiece(piece, NormalMods.TrueRightHand().position + NormalMods.TrueRightHand().forward * 0.65f + NormalMods.TrueRightHand().right * 0.03f + NormalMods.TrueRightHand().up * 0.05f, NormalMods.TrueRightHand().rotation, NormalMods.TrueRightHand().forward * 19.9f, Vector3.zero);
            }
        }

        private static IEnumerator AuraRanBlockCor()
        {
            BuilderPiece piece = null;
            if (pieceTypes.Count == 0)
            {
                GetAllSigma();
                yield return null;
            }
            else
            {
                int randomPieceType = pieceTypes[UnityEngine.Random.Range(0, pieceTypes.Count)];
                yield return CreateAuraGetPiece(randomPieceType, createdPiece =>
                {
                    piece = createdPiece;
                });

                while (piece == null)
                {
                    yield return null;
                }

                Vector3 grabPosition = GorillaTagger.Instance.rightHandTransform.position;
                Quaternion grabRotation = GorillaTagger.Instance.rightHandTransform.rotation;
                bool isLeftHand = true;
                BuilderTable.instance.RequestGrabPiece(piece, isLeftHand, grabPosition, grabRotation);

                Vector3 dropPosition = GorillaTagger.Instance.offlineVRRig.transform.position + new Vector3(UnityEngine.Random.Range(-1.5f, 1.5f), UnityEngine.Random.Range(-0.5f, 1.5f), UnityEngine.Random.Range(-1.5f, 1.5f));
                Quaternion dropRotation = GorillaTagger.Instance.rightHandTransform.rotation;
                Vector3 dropVelocity = GorillaTagger.Instance.rightHandTransform.forward * 20f;
                Vector3 dropAngularVelocity = new Vector3(UnityEngine.Random.Range(-36f, 36f), UnityEngine.Random.Range(-36f, 36f), UnityEngine.Random.Range(-36f, 36f));
                BuilderTable.instance.RequestDropPiece(piece, dropPosition, dropRotation, dropVelocity, dropAngularVelocity);
            }
        }
        private static IEnumerator ExplodeBlockCor()
        {
            BuilderPiece piece = null;

            yield return CreateGetPiece(-1927069002, createdPiece =>
            {
                piece = createdPiece;
            });

            while (piece == null)
            {
                yield return null;
            }

            Vector3 grabPosition = GorillaTagger.Instance.rightHandTransform.position;
            Quaternion grabRotation = GorillaTagger.Instance.rightHandTransform.rotation;
            bool isLeftHand = true;
            BuilderTable.instance.RequestGrabPiece(piece, isLeftHand, grabPosition, grabRotation);

            Vector3 dropPosition = GorillaTagger.Instance.offlineVRRig.transform.position;
            Quaternion dropRotation = GorillaTagger.Instance.rightHandTransform.rotation;
            Vector3 dropVelocity = new Vector3(UnityEngine.Random.Range(-20f, 20f), UnityEngine.Random.Range(-20f, 20f), UnityEngine.Random.Range(-20f, 20f));
            Vector3 dropAngularVelocity = new Vector3(UnityEngine.Random.Range(-36f, 36f), UnityEngine.Random.Range(-36f, 36f), UnityEngine.Random.Range(-36f, 36f));
            BuilderTable.instance.RequestDropPiece(piece, dropPosition, dropRotation, dropVelocity, dropAngularVelocity);
        }
        public static List<int> pieceTypes = new List<int>();

        public static void GetAllSigma()
        {
            pieceTypes.Clear();

            foreach (BuilderPiece builderPiece in GameObject.FindObjectsOfType<BuilderPiece>())
            {
                if (builderPiece != null)
                {
                    pieceTypes.Add(builderPiece.pieceType);
                }
            }

            Debug.Log($"Collected {pieceTypes.Count} pieceTypes.");
        }
        public static IEnumerator SpamRandomPiece()
        {
            BuilderPiece piece = null;
            if (pieceTypes.Count == 0)
            {
                GetAllSigma();
                yield return null;
            }
            else
            {
                int randomPieceType = pieceTypes[UnityEngine.Random.Range(0, pieceTypes.Count)];
                yield return CreateGetPiece(randomPieceType, createdPiece =>
                {
                    piece = createdPiece;
                });

                while (piece == null)
                {
                    yield return null;
                }

                Vector3 grabPosition = GorillaTagger.Instance.rightHandTransform.position;
                Quaternion grabRotation = GorillaTagger.Instance.rightHandTransform.rotation;
                bool isLeftHand = true;
                BuilderTable.instance.RequestGrabPiece(piece, isLeftHand, grabPosition, grabRotation);

                Vector3 dropPosition = GorillaTagger.Instance.rightHandTransform.position;
                Quaternion dropRotation = GorillaTagger.Instance.rightHandTransform.rotation;
                Vector3 dropVelocity = GorillaTagger.Instance.rightHandTransform.forward * 20f;
                Vector3 dropAngularVelocity = new Vector3(UnityEngine.Random.Range(-36f, 36f), UnityEngine.Random.Range(-36f, 36f), UnityEngine.Random.Range(-36f, 36f));
                BuilderTable.instance.RequestDropPiece(piece, dropPosition, dropRotation, dropVelocity, dropAngularVelocity);
            }
        }
        public static IEnumerator SpamRandomPieceRain()
        {
            BuilderPiece piece = null;
            if (pieceTypes.Count == 0)
            {
                GetAllSigma();
                yield return null;
            }
            else
            {
                int randomPieceType = pieceTypes[UnityEngine.Random.Range(0, pieceTypes.Count)];
                yield return CreateRainGetPiece(randomPieceType, createdPiece =>
                {
                    piece = createdPiece;
                });

                while (piece == null)
                {
                    yield return null;
                }

                Vector3 grabPosition = GorillaTagger.Instance.rightHandTransform.position;
                Quaternion grabRotation = GorillaTagger.Instance.rightHandTransform.rotation;
                bool isLeftHand = true;
                BuilderTable.instance.RequestGrabPiece(piece, isLeftHand, grabPosition, grabRotation);

                Vector3 dropPosition = GorillaTagger.Instance.rightHandTransform.position;
                Quaternion dropRotation = GorillaTagger.Instance.rightHandTransform.rotation;
                Vector3 dropVelocity = GorillaTagger.Instance.rightHandTransform.forward * 20f;
                Vector3 dropAngularVelocity = new Vector3(UnityEngine.Random.Range(-36f, 36f), UnityEngine.Random.Range(-36f, 36f), UnityEngine.Random.Range(-36f, 36f));
                BuilderTable.instance.RequestDropPiece(piece, dropPosition, dropRotation, dropVelocity, dropAngularVelocity);
            }
        }
        private static IEnumerator ShootBlockCor()
        {
            BuilderPiece piece = null;

            yield return CreateGetPiece(-1927069002, createdPiece =>
            {
                piece = createdPiece;
            });

            while (piece == null)
            {
                yield return null;
            }

            Vector3 grabPosition = GorillaTagger.Instance.rightHandTransform.position;
            Quaternion grabRotation = GorillaTagger.Instance.rightHandTransform.rotation;
            bool isLeftHand = true;
            BuilderTable.instance.RequestGrabPiece(piece, isLeftHand, grabPosition, grabRotation);

            Vector3 dropPosition = GorillaTagger.Instance.rightHandTransform.forward * 20f;
            Quaternion dropRotation = GorillaTagger.Instance.rightHandTransform.rotation;
            Vector3 dropVelocity = GorillaTagger.Instance.rightHandTransform.forward * 20f;
            Vector3 dropAngularVelocity = GorillaTagger.Instance.rightHandTransform.forward * 20f;
            BuilderTable.instance.RequestDropPiece(piece, dropPosition, dropRotation, dropVelocity, dropAngularVelocity);
        }

    }
}
