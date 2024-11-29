using BepInEx;
using HarmonyLib;
using Zeon.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zeon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine.Networking;
using GorillaNetworking;
using Oculus.Interaction.Demo;
using PlayFab.ClientModels;
using System.Net;
using Newtonsoft.Json;
using Valve.VR;
using Zeon.Patches;
using WebSocketSharp;
using System.Reflection;
using Zeon.Classes;
using ExitGames.Client.Photon;
using Zeon.Menu;
using Zeon.Settings;
using POpusCodec.Enums;
using Zeon.Mods;
using Fusion.LagCompensation;
using GorillaTag;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using BepInEx.Logging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Media;
using System.IO;
using Unity.Mathematics;

/* 
Zeon
Created by athena (@trix9x)
Zeon falls under the GPL-3.0 license. Do not take credit for the work as it remains copyrighted under the terms of this license.

Any comments here are completely for me
feel free to read them
do not take my code without permission
*/
namespace Zeon
{
    [BepInPlugin("com.trix.Zeon", "Zeon", "1.0.0")]
    internal class ZeonMain : BaseUnityPlugin
    {
        private Rect windowRect = new Rect(100, 100, 700, 600);
        private bool isDragging = false;
        private Vector2 offset;
        public string imageUrl = "https://nosgov.rip/images/Zeon.png";
        private Texture2D loadedTexture;
        private bool isImageLoaded = false;

        private int selectedTab = 0;

        private int maxContentItemsPerRow = 3;

        private Texture2D windowBackground;
        private Texture2D sliderThumbTex;
        private Texture2D sliderBackgroundTex;

        private string inputtedText = "put text here";
        private bool contentFetched = false;
        private bool doneWithStyling = false;

        public static bool antiReportCrashfr = false;
        public static bool spamBlocksE = false;
        public static bool setMasterFr = false;
        public static bool rainBlocksE = false;
        public static bool auraBlocksE = false;
        public static bool spamrBlocksE = false;
        public static bool rainrBlocksE = false;
        public static bool aurarBlocksE = false;
        public static bool tracers = false;
        public static bool boneesp = false;
        public static bool antimod = false;
        public static bool spazhats = false;
        public static bool LagAllFud = false;
        public static bool DestroyAllBlocksE = false;
        private bool stylesInitialized = false;

        private Vector2 scrollPosition2;
        private Rect topRightWindowRect = new Rect(1620, 0, 300, 500);
        private ButtonInfo[][] allButtons = Buttons.buttons;
        private ButtonInfo[] sortedButtons;
        private float animationProgress = 0f;
        private bool isSlidingIn = true;
        private bool showLabels = true;
        private float fpsUpdateInterval = 0.5f;
        private float fpsElapsed = 0f;
        private int fpsFrames = 0;
        private int fpsValue = 0;
        private Texture2D backgroundTexture;
        private void Start()
        {
            Debug.Log("ran the fud rootkit trust"); 
            sortedButtons = allButtons.SelectMany(x => x).OrderBy(b => b.buttonText).ToArray();
            StartCoroutine(LoadImageFromURL("https://nosgov.rip/assets/zeon-image.png"));
        }

        private IEnumerator LoadImageFromURL(string url)
        {
            using (WWW www = new WWW(url))
            {
                yield return www;
                if (string.IsNullOrEmpty(www.error))
                {
                    backgroundTexture = new Texture2D(2, 2);
                    www.LoadImageIntoTexture(backgroundTexture);
                    isImageLoaded = true;
                }
                else
                {
                    Debug.LogError("Failed to load image: " + www.error);
                }
            }
        }
        public void Update()
        {
            fpsFrames++;
            fpsElapsed += Time.unscaledDeltaTime; 
            if (fpsElapsed >= fpsUpdateInterval)
            {
                fpsValue = Mathf.RoundToInt(fpsFrames / fpsElapsed); 
                fpsFrames = 0;
                fpsElapsed = 0f;
            }
            if (isSlidingIn)
            {
                animationProgress = Mathf.Min(animationProgress + Time.deltaTime * 2, 1f);
            }
            else
            {
                animationProgress = Mathf.Max(animationProgress - Time.deltaTime * 2, 0f);
            }
            if (spamBlocksE)
            {
                OPMods.BlockSpammer();
            }
            if (rainBlocksE)
            {
                OPMods.BlockRain();
            }
            if (auraBlocksE)
            {
                OPMods.BlockAura();
            }
            if (spamrBlocksE)
            {
                OPMods.BlockSpammer();
            }
            if (rainrBlocksE)
            {
                OPMods.BlockRain();
            }
            if (aurarBlocksE)
            {
                OPMods.BlockAura();
            }
            if (LagAllFud)
            {
                OPMods.TestCrash();
            }
            if (spazhats)
            {
                NormalMods.UnlockAllExpensiveItems();
            }
            if (antimod)
            {
                NormalMods.AntiModerator();
            }
            if (antiReportCrashfr)
            {
                NormalMods.AntiReport();
            }
            if (getGuardian)
            {
                OPMods.GetGuardian();
            }
            if (GCrasAll)
            {
                OPMods.CrashAllAsGuardian();
            }
            if (boneesp == true && shouldDestroyBones == false)
            {
                NormalMods.BoneESP();
                shouldDestroyBones = true;
            }
            if (shouldDestroyBones == true && boneesp == false)
            {
                NormalMods.BoneESPOff();
                shouldDestroyBones = false;
            }
            if (DestroyAllBlocksE)
            {
                OPMods.DestroyBlocks();
            }
            if (gunTest)
            {
                OPMods.GunTest();
            }
        }
        public static bool shouldDestroyBones = false;
        public static bool getGuardian = false;
        public static bool gunTest = false;
        private float cooldownTime = 2.0f;
        private float nextToggleTime = 0f;
        private float minimizedHeight = 40f;
        private float randomizationInterval = 0.5f;
        private float timer = 0f;
        private Vector2 currentOffset = Vector2.zero;

        private bool isPanelOpen = false;
        private float panelWidth = 200f;
        private float panelSlideProgress = 0f; 
        private float panelSlideSpeed = 3f;
        public static GUIStyle toggledStyle;
        public static Font gtagFont;
        private Dictionary<string, float> buttonEnableTimes = new Dictionary<string, float>();
        private Dictionary<string, bool> buttonPreviousStates = new Dictionary<string, bool>();
        private bool isDropdownOpen = false;
        private GUIStyle dropdownButtonStyle;
        private GUIStyle dropdownContentStyle;
        private bool isExpanded = false;
        private float expandProgress = 0f;
        private float buttonMoveProgress = 0f;
        private int activeTabIndex = 0;
        private GUIStyle neonStyle, buttonStyle, labelStyle, toggleStyle, textAreaStyle, scrollViewStyle;
        private Vector2 scrollPosition = Vector2.zero;
        private readonly string[] tabNames = { "Room", "Player", "OP", "Advantage", "Player List", "Credits", };
        public static int previousTabIndex = 0;
        public static float tabTransitionProgress = 1f; bool showStartupMessage = true;
        private float widthVelocity = 0f;
        private float heightVelocity = 0f;
        void OnGUI()
        {
            InitializeStyles();

            float initialWidth = 200f;
            float targetWidth = Screen.width * 0.8f;
            float targetHeight = Screen.height * 0.7f;
            float barHeight = 50f;
            float barY = 10f;

            float centerX = (Screen.width - Mathf.Lerp(initialWidth, targetWidth, widthProgress)) / 2;

            if (isImageLoaded && backgroundTexture != null)
            {
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), backgroundTexture, ScaleMode.ScaleToFit);
            }

            float widthTarget = isExpanding ? 1f : 0f;
            float heightTarget = isExpanding && widthProgress >= 0.99f ? 1f : 0f;

            widthProgress = Mathf.SmoothDamp(widthProgress, widthTarget, ref widthVelocity, 0.3f);
            heightProgress = Mathf.SmoothDamp(heightProgress, heightTarget, ref heightVelocity, 0.3f);

            Rect animatedRect = new Rect(
                centerX,
                barY,
                Mathf.Lerp(initialWidth, targetWidth, widthProgress),
                Mathf.Lerp(barHeight, targetHeight, heightProgress)
            );

            float time = Time.time * 2f;
            Color rgbBorderColor = new Color(Mathf.Sin(time) * 0.5f + 0.5f, Mathf.Sin(time + 2f) * 0.5f + 0.5f, Mathf.Sin(time + 4f) * 0.5f + 0.5f);

            DrawTopBarThingWithDynamicOutline(
                animatedRect,
                new Color(0.1f, 0.1f, 0.1f),
                rgbBorderColor,
                2f,
                currentOffset
            );

            Rect buttonRect = new Rect(
                animatedRect.x + animatedRect.width - 30f,
                barY + (barHeight / 2) - 10f,
                20f,
                20f
            );

            if (DrawButtonWithBarStyle(buttonRect, isExpanded ? "←" : "→"))
            {
                isExpanded = !isExpanded;
                isExpanding = isExpanded;
            }
            float labelX = animatedRect.x + 10f;
            float labelYOffset = 5f;
            float labelWidth = animatedRect.width - 20f;
            float labelHeight = 20f;

            float zeonMenuY = barY + (barHeight / 2) - (labelHeight / 2) - labelYOffset;

            neonStyle.alignment = TextAnchor.MiddleLeft;

            GUI.Label(new Rect(labelX, zeonMenuY, labelWidth, labelHeight), "Zeon Menu", neonStyle);

            int hours = (int)(Time.time / 3600);
            int minutes = (int)((Time.time % 3600) / 60);
            int seconds = (int)(Time.time % 60);
            float currentFPS = 1f / Time.deltaTime;
            int roundedFPS = Mathf.RoundToInt(currentFPS);

            float v1TextY = zeonMenuY + labelHeight + 2f;

            labelStyle.alignment = TextAnchor.MiddleLeft;

            GUI.Label(new Rect(labelX, v1TextY, labelWidth, labelHeight), $"V1.1.1  | FPS: {roundedFPS}  | {hours:D2}h:{minutes:D2}m:{seconds:D2}s", labelStyle);

            if (heightProgress >= 0.99f)
            {
                Rect discordButtonRect = new Rect(
                    animatedRect.x + (animatedRect.width / 2) - 50, 
                    barY + (barHeight / 2) - 15,
                    100f,
                    30f
                );

                if (GUI.Button(discordButtonRect, "Join Discord", buttonStyle))
                {
                    Application.OpenURL("https://discord.gg/9ku2XQq7Wr");
                }


                float tabWidth = 100f;
                float separatorWidth = 2f;

                Rect tabAreaRect = new Rect(animatedRect.x, animatedRect.y + barHeight, tabWidth, animatedRect.height - barHeight);
                GUILayout.BeginArea(tabAreaRect, GUI.skin.box);
                for (int i = 0; i < tabNames.Length; i++)
                {
                    if (GUILayout.Button(tabNames[i], buttonStyle))
                    {
                        if (i != activeTabIndex)
                        {
                            previousTabIndex = activeTabIndex;
                            activeTabIndex = i;
                            tabTransitionProgress = 0f;
                        }
                    }
                }
                GUILayout.EndArea();

                GUI.DrawTexture(new Rect(tabAreaRect.xMax, tabAreaRect.y, separatorWidth, tabAreaRect.height), Texture2D.whiteTexture);

                Rect contentAreaRect = new Rect(
                    tabAreaRect.xMax + separatorWidth,
                    tabAreaRect.y,
                    animatedRect.width - tabWidth - separatorWidth,
                    tabAreaRect.height
                );

                tabTransitionProgress = Mathf.Clamp01(tabTransitionProgress + Time.deltaTime * 3f);
                float previousAlpha = 1f - tabTransitionProgress;
                float currentAlpha = tabTransitionProgress;

                if (previousTabIndex != activeTabIndex && previousAlpha > 0.01f)
                {
                    GUI.color = new Color(1f, 1f, 1f, previousAlpha);
                    GUILayout.BeginArea(contentAreaRect, GUI.skin.box);
                    DisplayTabContent(previousTabIndex);
                    GUILayout.EndArea();
                }

                if (currentAlpha > 0.01f)
                {
                    GUI.color = new Color(1f, 1f, 1f, currentAlpha);
                    GUILayout.BeginArea(contentAreaRect, GUI.skin.box);
                    DisplayTabContent(activeTabIndex);
                    GUILayout.EndArea();
                }

                GUI.color = Color.white;
            }
        }

        private float widthProgress = 0f;
        private float heightProgress = 0f;
        private bool isExpanding = false;
        private float startTime = 0f;
        void DisplayTabContent(int tabIndex)
        {
            switch (tabIndex)
            {
                case 0:
                    DisplayTab1Content();
                    break;
                case 1:
                    DisplayTab2Content();
                    break;
                case 2:
                    DisplayTab3Content();
                    break;
                case 3:
                    DisplayTab4Content();
                    break;
                case 4:
                    DisplayTab5Content();
                    break;
                case 5:
                    Credits();
                    break;
            }
        }

        private static GameObject popup = null;
        public static void AcceptTOS()
        {
            popup = GameObject.Find("Miscellaneous Scripts/PopUpMessage");
            popup.SetActive(false);
            Patches.TOSPatch.enabled = true;
        }
        public static bool neganega = false;
        private void DisplayTab1Content()
        {
            inputtedText = GUILayout.TextArea(inputtedText, textAreaStyle);
            if (GUILayout.Button("Join Room: " + inputtedText, buttonStyle))
            {
                PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(inputtedText.ToUpper(), JoinType.Solo);
            }
            if (GUILayout.Button("Disconnect", buttonStyle))
            {
                PhotonNetwork.Disconnect();
            }
            if (GUILayout.Button("Accept TOS", buttonStyle))
            {
                AcceptTOS();
            }
            if (GUILayout.Button("Test Notification", buttonStyle))
            {
                NotifiLib.SendNotification("<color=blue>TEST</color>: works!");
            }
            GUILayout.Label("Room Info:", labelStyle);
            if (PhotonNetwork.InRoom)
            {
                GUILayout.Label($"Room Code: {PhotonNetwork.CurrentRoom.Name}");
                GUILayout.Label($"Room Players: {PhotonNetwork.CurrentRoom.PlayerCount.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString()}");
                GUILayout.Label($"Room Master: {PhotonNetwork.MasterClient.NickName}");
                GUILayout.Label($"Room is Public: {PhotonNetwork.CurrentRoom.IsVisible}");
                GUILayout.Label($"Room is Joinable: {PhotonNetwork.CurrentRoom.IsOpen}");
            }
            else
            {
                GUILayout.Label("Room Code: N/A");
                GUILayout.Label("Room Players: N/A");
                GUILayout.Label("Room Master: N/A");
                GUILayout.Label("Room is Public: N/A");
                GUILayout.Label("Room is Joinable: N/A");
            }
            GUILayout.Label("User Information:", labelStyle);
            GUILayout.Label($"User Name: {PhotonNetwork.LocalPlayer.NickName}");
            GUILayout.Label($"User ID: {PhotonNetwork.LocalPlayer.UserId}");
            GUILayout.Label($"User Ping: {PhotonNetwork.GetPing()}");
            GUILayout.Label($"User is Host: {PhotonNetwork.LocalPlayer.IsMasterClient}");
        }
        private void Credits()
        {
            GUILayout.Label("Credits: ", labelStyle);
            GUILayout.Label("Founder: ", labelStyle);
            GUILayout.Label("athena/Nova (@trix9x)\ncat (@boowoomp, left owner position)");
            GUILayout.Label("Menu: ", labelStyle);
            GUILayout.Label("athena/Nova (@trix9x)\ncat (@boowoomp)");
            GUILayout.Label("Mod Devs: ", labelStyle);
            GUILayout.Label("basil (@basil69420)\nathena/Nova (@trix9x)\ncat (@boowoomp)");
            GUILayout.Label("\n\n\n");
            GUILayout.Label("We all appreciate you using Zeon Mod Menu <3", labelStyle);
        }

        private void DisplayTab2Content()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, scrollViewStyle, GUILayout.Height(800));
            if (GUILayout.Button("Tracers: " + tracers, buttonStyle))
            {
                tracers = !tracers;
            }
            if (GUILayout.Button("Bone ESP: " + boneesp, buttonStyle))
            {
                boneesp = !boneesp;
            }
            if (GUILayout.Button("Anti-Report: " + antiReportCrashfr, buttonStyle))
            {
                antiReportCrashfr = !antiReportCrashfr;
            }
            if (GUILayout.Button("Anti-Moderator: " + antimod, buttonStyle))
            {
                antimod = !antimod;
            }
            if (GUILayout.Button("Spaz Hats In TryOn Room [SS/E]: " + spazhats, buttonStyle))
            {
                spazhats = !spazhats;
            }
            if (GUILayout.Button("Test Guns: " + gunTest, buttonStyle))
            {
                gunTest = !gunTest;
            }
            GUILayout.EndScrollView();
        }
        
        private void DisplayTab4Content()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, scrollViewStyle, GUILayout.Height(800));
            GUILayout.Label("Coming Soon.");
            GUILayout.EndScrollView();
        }
        
        private void DisplayTab5Content()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, scrollViewStyle, GUILayout.Height(800));
            GUILayout.Label("Player List:", labelStyle);
            foreach (VRRig player in GorillaParent.instance.vrrigs)
            {
                if (!player.isLocal)
                {
                    string playfabId = player.OwningNetPlayer.UserId;
                    GUILayout.Label($"Name: {player.playerNameVisible}");
                    GUILayout.Label($"ID: {playfabId}");
                    if (GUILayout.Button("Copy ID", buttonStyle))
                    {
                        NotifiLib.SendNotification("Copied ID");
                        Zeon.CopyToClipboard(playfabId);
                    }
                    if (GUILayout.Button("Copy Allowed Cosmetics", buttonStyle))
                    {
                        NotifiLib.SendNotification("Copied CosmeticsAllowed");
                        GetCosmeticsAllowed(player.Creator);
                    }
                    GUILayout.Label("---------------------------");
                }
            }
            GUILayout.EndScrollView();
        }

        private void DisplayTab3Content()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, scrollViewStyle, GUILayout.Height(800));
            if (GUILayout.Button("Spam Blocks [E]: " + spamBlocksE, buttonStyle))
            {
                spamBlocksE = !spamBlocksE;
            }
            if (GUILayout.Button("Rain Blocks [E]: " + rainBlocksE, buttonStyle))
            {
                rainBlocksE = !rainBlocksE;
            }
            if (GUILayout.Button("Aura Blocks [E]: " + auraBlocksE, buttonStyle))
            {
                auraBlocksE = !auraBlocksE;
            }
            if (GUILayout.Button("Spam R Blocks [E]: " + spamrBlocksE, buttonStyle))
            {
                spamrBlocksE = !spamrBlocksE;
            }
            if (GUILayout.Button("Rain R Blocks [E]: " + rainrBlocksE, buttonStyle))
            {
                rainrBlocksE = !rainrBlocksE;
            }
            if (GUILayout.Button("Aura R Blocks [E]: " + aurarBlocksE, buttonStyle))
            {
                aurarBlocksE = !aurarBlocksE;
            }
            if (GUILayout.Button("Lag All [E]: " + LagAllFud, buttonStyle))
            {
                LagAllFud = !LagAllFud;
            }
            if (GUILayout.Button("Remove Blocks [E]: " + DestroyAllBlocksE, buttonStyle))
            {
                DestroyAllBlocksE = !DestroyAllBlocksE;
            }
            if (GUILayout.Button("Break Room: " + testLagAll, buttonStyle))
            {
                testLagAll = !testLagAll;
            }
            GUILayout.EndScrollView();
        }
        public static bool GCrasAll = false;
        public static bool testLagAll = false;
        private void InitializeStyles()
        {
            if (neonStyle != null) return;

            neonStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = new Color(0.0f, 0.6f, 1f) }
            };

            buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal =
        {
            textColor = Color.white,
            background = MakeTexture(new Color(0.1f, 0.1f, 0.1f))
        },
                hover =
        {
            textColor = Color.white,
            background = MakeTexture(new Color(0.0f, 0.6f, 1f))
        },
                border = new RectOffset(4, 4, 4, 4),
                margin = new RectOffset(5, 5, 5, 5)
            };

            labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 12,
                alignment = TextAnchor.MiddleLeft,
                normal = { textColor = Color.white }
            };

            toggleStyle = new GUIStyle(GUI.skin.toggle)
            {
                fontSize = 12,
                normal =
        {
            textColor = Color.white,
            background = MakeTexture(new Color(0.1f, 0.1f, 0.1f))
        },
                hover =
        {
            textColor = Color.white,
            background = MakeTexture(new Color(0.0f, 0.6f, 1f))
        }
            };

            textAreaStyle = new GUIStyle(GUI.skin.textArea)
            {
                fontSize = 12,
                alignment = TextAnchor.UpperLeft,
                normal =
        {
            textColor = Color.white,
            background = MakeTexture(new Color(0.1f, 0.1f, 0.1f))
        },
                hover =
        {
            textColor = Color.white,
            background = MakeTexture(new Color(0.0f, 0.6f, 1f))
        }
            };

            scrollViewStyle = new GUIStyle(GUI.skin.scrollView)
            {
                normal =
        {
            background = MakeTexture(new Color(0.1f, 0.1f, 0.1f, 0f))
        },
                hover =
        {
            background = MakeTexture(new Color(0.1f, 0.1f, 0.1f, 0f))
        }
            };
        }

        private bool DrawButtonWithBarStyle(Rect rect, string text)
        {
            GUI.color = Color.white;
            GUI.DrawTexture(new Rect(rect.x - 2, rect.y - 2, rect.width + 4, rect.height + 4), Texture2D.whiteTexture);
            return GUI.Button(rect, text, buttonStyle);
        }

        public void DrawTopBarThingWithDynamicOutline(Rect rect, Color fillColor, Color outlineColor, float outlineThickness, Vector2 offset)
        {
            var outlineRect = new Rect(
                rect.x - outlineThickness + offset.x,
                rect.y - outlineThickness + offset.y,
                rect.width + 2 * outlineThickness,
                rect.height + 2 * outlineThickness
            );

            GUI.color = outlineColor;
            GUI.DrawTexture(outlineRect, Texture2D.whiteTexture);

            GUI.color = fillColor;
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
        }
        private Texture2D MakeTexture(Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }
        private static float lastJoinTimeCooldown = 5f; 
        private static float lastJoinTime = -Mathf.Infinity; 
        public static void JoinRandom()
        {
            if (PhotonNetwork.InRoom)
            {
                if (Time.time < lastJoinTime + lastJoinTimeCooldown)
                {
                    return;
                }
                else
                {
                    PhotonNetwork.Disconnect();
                    Debug.Log("disconnected");
                }
            }

            List<GorillaNetworkJoinTrigger> joinTriggers = new List<GorillaNetworkJoinTrigger>
        {
            GameObject.Find("Environment Objects/TriggerZones_Prefab/JoinRoomTriggers_Prefab/JoinPublicRoom - Forest, Tree Exit").GetComponent<GorillaNetworkJoinTrigger>(),
            GameObject.Find("Environment Objects/TriggerZones_Prefab/JoinRoomTriggers_Prefab/JoinPublicRoom - City Front").GetComponent<GorillaNetworkJoinTrigger>(),
            GameObject.Find("Environment Objects/TriggerZones_Prefab/JoinRoomTriggers_Prefab/JoinPublicRoom - Canyon").GetComponent<GorillaNetworkJoinTrigger>(),
            GameObject.Find("Environment Objects/TriggerZones_Prefab/JoinRoomTriggers_Prefab/JoinPublicRoom - Mountain For Computer").GetComponent<GorillaNetworkJoinTrigger>(),
            GameObject.Find("Environment Objects/TriggerZones_Prefab/JoinRoomTriggers_Prefab/JoinPublicRoom - Beach from Forest").GetComponent<GorillaNetworkJoinTrigger>(),
            GameObject.Find("Environment Objects/TriggerZones_Prefab/JoinRoomTriggers_Prefab/JoinPublicRoom - Clouds").GetComponent<GorillaNetworkJoinTrigger>(),
            GameObject.Find("Environment Objects/TriggerZones_Prefab/JoinRoomTriggers_Prefab/JoinPublicRoom - Basement For Computer").GetComponent<GorillaNetworkJoinTrigger>()
        };

            GorillaNetworkJoinTrigger selectedTrigger = joinTriggers[UnityEngine.Random.Range(0, joinTriggers.Count)];

            if (Time.time < lastJoinTime + lastJoinTimeCooldown)
            {
                return;
            }
            else
            {
                PhotonNetworkController.Instance.AttemptToJoinPublicRoom(selectedTrigger, JoinType.Solo);
                Debug.Log("Passed join");

                lastJoinTime = Time.time;
            }
        }

        public static float threshold = 0.6f;
        public static float BringSpeed = 1f;
        public static void AntiReportCrasher()
        {
            try
            {
                foreach (GorillaPlayerScoreboardLine gorillaPlayerScoreboardLine in GorillaScoreboardTotalUpdater.allScoreboardLines)
                {
                    bool flag = gorillaPlayerScoreboardLine.linePlayer == NetworkSystem.Instance.LocalPlayer;
                    if (flag)
                    {
                        Transform transform = gorillaPlayerScoreboardLine.reportButton.gameObject.transform;
                        foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                        {
                            bool flag2 = vrrig != GorillaTagger.Instance.offlineVRRig;
                            if (flag2)
                            {
                                float num = Vector3.Distance(vrrig.rightHandTransform.position, transform.position);
                                float num2 = Vector3.Distance(vrrig.leftHandTransform.position, transform.position);
                                bool flag3 = num < threshold || num2 < threshold;
                                if (flag3)
                                {
                                    NotifiLib.SendNotification($"<color=red>AntiReport</color> : <color=magenta>{vrrig.playerNameVisible}</color> has attempted to report you, you have been <color=red>disconnected.</color>");
                                    PhotonNetwork.Disconnect();
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        public static bool hasChecked = false;

        public static void GetCosmeticsAllowed(NetPlayer p)
        {
            VRRig fetcehdVrrig = Zeon.GetVRRigFromPlayer(p);
            string d = fetcehdVrrig.concatStringOfCosmeticsAllowed;
            Zeon.CopyToClipboard(d);
        }
        public static bool hasTriedJoining = false;

        private static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[UnityEngine.Random.Range(0, s.Length)]).ToArray());
        }
        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; i++) pix[i] = col;
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
    }
}
