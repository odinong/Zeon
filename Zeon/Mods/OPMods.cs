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
using PlayFab.ClientModels;
using UnityEngine.XR;
using UnityEngine.InputSystem;

/* 
Zeon
Created by athena (@trix9x)
Zeon falls under the GPL-3.0 license. Do not take credit for the work as it remains copyrighted under the terms of this license.

Any comments here are completely for me
feel free to read them
do not take my code without permission
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

        public static void GetGuardian() // dont use this it doesnt work 
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
        public static void CrashAllAsGuardian() // dont use this it doesnt work
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
        /*
        public static void CrashTest2()
        {
            foreach (VRRig a in GorillaParent.instance.vrrigs)
            {
                if (!a.isLocal)
                {
                    PhotonNetwork.NetworkingClient.OpRaiseEvent(254, a.OwningNetPlayer.ActorNumber, null, SendOptions.SendReliable);
                }

            }
        }
        */
        /*
        public static void CrashTest3()
        {
            foreach (VRRig a in GorillaParent.instance.vrrigs)
            {
                if (!a.isLocal)
                {
                    Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
                    dictionary.Add(233, false);
                    dictionary.Add(252, a.OwningNetPlayer.ActorNumber);
                    PhotonNetwork.NetworkingClient.OpRaiseEvent(254, dictionary, null, SendOptions.SendReliable);
                    PhotonNetwork.CurrentRoom.LoadBalancingClient.LoadBalancingPeer.SendOperation(254, dictionary, new SendOptions { Reliability = true, DeliveryMode = DeliveryMode.ReliableUnsequenced, Encrypt = true });
                    PhotonNetwork.CurrentRoom.LoadBalancingClient.LoadBalancingPeer.SendOperation(212, dictionary, new SendOptions { Reliability = true, DeliveryMode = DeliveryMode.ReliableUnsequenced, Encrypt = true });
                }

            }
        }
        */
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
        /*
        public static void SetMaster() // please never call this it was a test cat did
        {
            ExitGames.Client.Photon.Hashtable gameProperties = new ExitGames.Client.Photon.Hashtable
            {
                {
                    248,
                    PhotonNetwork.LocalPlayer.ActorNumber
                }
            };
            ExitGames.Client.Photon.Hashtable expectedProperties = new ExitGames.Client.Photon.Hashtable
            {
                {
                    248,
                    PhotonNetwork.MasterClient.ActorNumber
                }
            };
            Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
            dictionary.Add(251, gameProperties);
            dictionary.Add(250, true);
            if (expectedProperties != null && expectedProperties.Count != 0)
            {
                dictionary.Add(231, expectedProperties);
            }
            PhotonNetwork.CurrentRoom.LoadBalancingClient.LoadBalancingPeer.SendOperation(252, dictionary, SendOptions.SendReliable);
        }
        */
        private static GameObject pointer;
        private static VRRig lockedVRRig; 
        private static Camera desktopCamera;
        private static GameObject triggerZones;

        private static void GunLib(Action<VRRig> method, bool shouldLock) // half of this was chatgpt cause i dont know how to mask shit and stuff, this isnt used at all lol
        {
            if (desktopCamera == null)
            {
                desktopCamera = GameObject.Find("Player Objects/Third Person Camera/Shoulder Camera")?.GetComponent<Camera>();
                if (desktopCamera == null)
                {
                    Debug.LogError("Shoulder Camera not found. Ensure it exists in the scene.");
                    return;
                }
            }

            if (triggerZones == null)
            {
                triggerZones = GameObject.Find("Environment Objects/TriggerZones_Prefab");
                if (triggerZones == null)
                {
                    Debug.LogError("TriggerZones_Prefab not found. Ensure it exists in the scene.");
                    return;
                }
            }

            if (Mouse.current.rightButton.isPressed)
            {
                Ray ray = desktopCamera.ScreenPointToRay(UnityEngine.InputSystem.Mouse.current.position.ReadValue());
                RaycastHit raycastHit;

                if (Physics.Raycast(ray, out raycastHit))
                {
                    if (pointer == null)
                    {
                        pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        UnityEngine.Object.Destroy(pointer.GetComponent<Rigidbody>());
                        UnityEngine.Object.Destroy(pointer.GetComponent<SphereCollider>());
                        pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

                        SetTransparency(pointer, 0.25f);
                    }

                    pointer.transform.position = raycastHit.point;

                    Transform current = raycastHit.collider.transform;
                    bool isUnderTriggerZones = false;

                    while (current != null)
                    {
                        if (current.gameObject == triggerZones)
                        {
                            isUnderTriggerZones = true;
                            break;
                        }
                        current = current.parent;
                    }

                    if (!isUnderTriggerZones)
                    {
                        if (Mouse.current.leftButton.isPressed)
                        {
                            if (shouldLock)
                            {
                                VRRig vrrig = raycastHit.collider.GetComponentInParent<VRRig>();
                                if (vrrig != null)
                                {
                                    lockedVRRig = vrrig;
                                    pointer.GetComponent<Renderer>().material.color = Color.green;
                                    method?.Invoke(vrrig);
                                }
                            }
                            else
                            {
                                method?.Invoke(null);
                            }
                        }
                    }
                }
            }
            else
            {
                lockedVRRig = null;
            }
        }

        private static void SetTransparency(GameObject obj, float transparency)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer == null) return;

            Material material = renderer.material;
            material.SetOverrideTag("RenderType", "Transparent");
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;

            Color color = material.color;
            color.a = transparency;
            material.color = color;
        }


        public static void GunTest()
        {
            GunLib(vrrig =>
            {
                Debug.Log("gun works i think");
            }, false);
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
