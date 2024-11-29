using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using BepInEx;
using System.Collections.Generic;
using System;
using System.Linq;

/* 
NotificationLib 2.0
Original HUD Made by Lars
OnGUI Created and Styled by athena (@trix9x)
NotificationLib 2.0 falls under the GPL-3.0 license. Do not take credit for the work as it remains copyrighted under the terms of this license.

Any comments here are completely for you, please pay close attention to any comments if you are looking to change it
Feel free to use this for menus and or mods
Written originally by lars, OnGUI made by trix9x
*/
namespace Zeon.Notifications
{
    [BepInPlugin("org.athena.notifications2.0", "NotificationLibrary V2", "2.0.0")]
    public class NotifiLib : BaseUnityPlugin
    {
        private static GUIStyle notificationStyle;
        private static GUIStyle headerStyle;
        private static GUIStyle borderStyle;
        private static Queue<string> notifications = new Queue<string>();
        private static List<NotificationData> notificationDataList = new List<NotificationData>();

        private const float SwipeInDuration = 1f; // in speed
        private const float VisibleDuration = 3f; // how long it stays
        private const float SwipeOutDuration = 1f; // out speed
        private const float NotificationLifetime = SwipeInDuration + VisibleDuration + SwipeOutDuration; // the lifetime (dont rlly change this)

        private static bool showStartupMessage = true;
        private string startupMessage = "<color=blue>ZEON</color> : Welcome to Zeon Menu!"; // replace with ur welcome text

        private class NotificationData //dont change this
        {
            public string message;
            public float spawnTime;
            public Vector2 position;
            public Vector2 velocity;
            public Vector2 targetPosition;
            public bool isSlidingIn;
            public Color borderColor;
        }
        private void OnGUI() // gui notifs
        {
            if (notificationStyle == null)
                InitializeStyles();

            if (showStartupMessage)
            {
                SendNotification(startupMessage);
                showStartupMessage = false;
            }

            for (int i = 0; i < notificationDataList.Count; i++)
            {
                NotificationData notificationData = notificationDataList[i];
                float elapsedTime = Time.time - notificationData.spawnTime;

                if (elapsedTime < SwipeInDuration)
                {
                    notificationData.targetPosition.x = 10; 
                }
                else if (elapsedTime > SwipeInDuration + VisibleDuration)
                {
                    notificationData.targetPosition.x = -300; 
                }
                notificationData.position = Vector2.SmoothDamp(
                    notificationData.position,
                    new Vector2(notificationData.targetPosition.x, notificationData.position.y),
                    ref notificationData.velocity,
                    0.2f 
                );

                notificationData.targetPosition.y = 10 + (i * 80); 
                notificationData.position.y = Mathf.Lerp(notificationData.position.y, notificationData.targetPosition.y, 0.1f);

                DisplayNotification(notificationData, i);
            }

            notificationDataList.RemoveAll(n => (Time.time - n.spawnTime) > NotificationLifetime);
        }
        public static void SendPlayerNotification(string playerName, string playerEvent, string roomCode)
        {
            string notificationMessage = $"Event For Player: {playerName}\n<size=12>Event: {playerEvent}\nRoom Code: {roomCode}</size>";
            SendNotification(notificationMessage);
        }
        private void DisplayNotification(NotificationData notificationData, int index)
        {
            float boxWidth = 300;
            float boxHeight = 70;
            float barWidth = 2;

            notificationData.position.y = 10 + (index * (boxHeight + 10));

            Rect borderRect = new Rect(notificationData.position.x, notificationData.position.y, barWidth, boxHeight);
            GUI.color = Color.blue;
            GUI.Box(borderRect, "");
            GUI.color = Color.white;

            Rect backgroundRect = new Rect(borderRect.x + barWidth, notificationData.position.y, boxWidth - barWidth, boxHeight); 
            GUI.Box(backgroundRect, "", borderStyle);

            Rect headerRect = new Rect(backgroundRect.x + 10, backgroundRect.y + 5, backgroundRect.width - 20, 20);
            GUI.Label(headerRect, "Notification", headerStyle);

            Rect textRect = new Rect(backgroundRect.x + 10, backgroundRect.y + 25, backgroundRect.width - 20, 50); 
            GUI.Label(textRect, notificationData.message, notificationStyle);
        }


        private static void InitializeStyles() // dont remove this since its needed, you can change colors if you like
        {
            notificationStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 14,
                fontStyle = FontStyle.Italic,
                alignment = TextAnchor.MiddleLeft,
                normal = { textColor = Color.white }
            };

            headerStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 16,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.UpperLeft,
                normal = { textColor = Color.white }
            };

            borderStyle = new GUIStyle(GUI.skin.box)
            {
                normal = { background = CreateGreyTexture() }
            };
        }

        private static Texture2D CreateGreyTexture() // dont remove this but change it if you want a different bg color
        {
            Texture2D texture = new Texture2D(1, 100);
            for (int y = 0; y < 100; y++)
            {
                texture.SetPixel(0, y, new Color(23 / 255f, 23 / 255f, 23 / 255f, 0.8f));
            }
            texture.Apply();
            return texture;
        }


        public static Font Verdana = Font.CreateDynamicFontFromOSFont("Candara", 24);
        private void UpdateHUDText() // u dont rlly need this
        {
            Testtext.text = string.Join("\n", notificationDataList.Select(n => n.message));
        }

        private void Init() // dont mess with this unless u wanna change the position
        {
            this.MainCamera = GameObject.Find("Main Camera");
            this.HUDObj = new GameObject();
            this.HUDObj2 = new GameObject();
            this.HUDObj2.name = "NOTIFICATIONLIB_HUD_OBJ";
            this.HUDObj.name = "NOTIFICATIONLIB_HUD_OBJ";
            this.HUDObj.AddComponent<Canvas>();
            this.HUDObj.AddComponent<CanvasScaler>();
            this.HUDObj.AddComponent<GraphicRaycaster>();
            this.HUDObj.GetComponent<Canvas>().enabled = true;
            this.HUDObj.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
            this.HUDObj.GetComponent<Canvas>().worldCamera = this.MainCamera.GetComponent<Camera>();
            this.HUDObj.GetComponent<RectTransform>().sizeDelta = new Vector2(5f, 5f);
            this.HUDObj.GetComponent<RectTransform>().position = new Vector3(this.MainCamera.transform.position.x, this.MainCamera.transform.position.y, this.MainCamera.transform.position.z);
            this.HUDObj2.transform.position = new Vector3(this.MainCamera.transform.position.x, this.MainCamera.transform.position.y, this.MainCamera.transform.position.z - 4.6f);
            this.HUDObj.transform.parent = this.HUDObj2.transform;
            this.HUDObj.GetComponent<RectTransform>().localPosition = new Vector3(0.4f, 0.1f, 1.5f); // hud position (for notif area)
            Vector3 eulerAngles = this.HUDObj.GetComponent<RectTransform>().rotation.eulerAngles;
            eulerAngles.y = -270f;
            this.HUDObj.transform.localScale = new Vector3(1f, 1f, 1f);
            this.HUDObj.GetComponent<RectTransform>().rotation = Quaternion.Euler(eulerAngles);
            this.Testtext = new GameObject
            {
                transform =
                {
                    parent = this.HUDObj.transform
                }
            }.AddComponent<Text>();
            this.Testtext.text = "";
            this.Testtext.fontSize = 30;
            this.Testtext.font = Verdana;
            this.Testtext.rectTransform.sizeDelta = new Vector2(450f, 210f);
            this.Testtext.alignment = TextAnchor.LowerCenter;
            this.Testtext.rectTransform.localScale = new Vector3(0.00333333333f, 0.00333333333f, 0.33333333f);
            this.Testtext.rectTransform.localPosition = new Vector3(-1f, -1f, -0.5f);
            this.Testtext.material = this.AlertText;
            NotifiLib.NotifiText = this.Testtext;
        }

        private void FixedUpdate() // dont change this
        {
            bool flag = !this.HasInit && GameObject.Find("Main Camera") != null;
            if (flag)
            {
                this.Init();
                this.HasInit = true;
            }
            this.HUDObj2.transform.position = new Vector3(this.MainCamera.transform.position.x, this.MainCamera.transform.position.y, this.MainCamera.transform.position.z);
            this.HUDObj2.transform.rotation = this.MainCamera.transform.rotation;
            if (this.Testtext.text != "")
            {
                this.NotificationDecayTimeCounter++;
                if (this.NotificationDecayTimeCounter > NotificationDecayTime)
                {
                    this.Notifilines = null;
                    this.newtext = "";
                    this.NotificationDecayTimeCounter = 0;
                    this.Notifilines = Enumerable.ToArray<string>(Enumerable.Skip<string>(this.Testtext.text.Split(Environment.NewLine.ToCharArray()), 1));
                    foreach (string text in this.Notifilines)
                    {
                        if (text != "")
                        {
                            this.newtext = this.newtext + text + "\n";
                        }
                    }
                    this.Testtext.text = this.newtext;
                }
            }
            else
            {
                this.NotificationDecayTimeCounter = 0;
            }
        }
        public static bool playSound = true;
        public static void SendNotification(string message) // dont change this
        {
            if (Instance != null && !string.IsNullOrEmpty(message))
            {
                Color borderColor = ParseColorFromMessage(message);

                if (!showStartupMessage)
                {
                    var notificationData = new NotificationData
                    {
                        message = message,
                        spawnTime = Time.time,
                        position = new Vector2(-300, 10),
                        velocity = Vector2.zero,
                        targetPosition = new Vector2(10, 10),
                        isSlidingIn = false,
                        borderColor = borderColor
                    };
                    notificationDataList.Add(notificationData);

                    if (notificationDataList.Count == 1 && playSound)
                    {
                        Instance.PlayNotificationSound();
                    }

                    try
                    {
                        if (NotifiLib.IsEnabled && NotifiLib.PreviousNotifi != message)
                        {
                            if (!message.Contains(Environment.NewLine))
                            {
                                message += Environment.NewLine;
                            }
                            NotificationDecayTime = 300;
                            NotifiLib.NotifiText.text = NotifiLib.NotifiText.text + message;
                            NotifiLib.NotifiText.supportRichText = true;
                            NotifiLib.PreviousNotifi = message;
                        }
                    }
                    catch
                    {
                        UnityEngine.Debug.LogError("Notification failed, object probably nil due to third person ; " + message);
                    }
                }
                else
                {
                    var notificationData = new NotificationData
                    {
                        message = message,
                        spawnTime = Time.time,
                        position = new Vector2(-300, 10),
                        velocity = Vector2.zero,
                        targetPosition = new Vector2(10, 10),
                        isSlidingIn = false,
                        borderColor = borderColor
                    };
                    notificationDataList.Add(notificationData);
                    try
                    {
                        if (NotifiLib.IsEnabled && NotifiLib.PreviousNotifi != message)
                        {
                            if (!message.Contains(Environment.NewLine))
                            {
                                message += Environment.NewLine;
                            }
                            NotificationDecayTime = 1000;
                            NotifiLib.NotifiText.text = NotifiLib.NotifiText.text + message;
                            NotifiLib.NotifiText.supportRichText = true;
                            NotifiLib.PreviousNotifi = message;
                        }
                    }
                    catch
                    {
                        UnityEngine.Debug.LogError("Notification failed, object probably nil due to third person ; " + message);
                    }
                }
            }
        }
        public static void ClearAllNotifications() // clears notifs
        {
            NotifiLib.NotifiText.text = "";
        }


        private GameObject HUDObj;

        private GameObject HUDObj2;

        private GameObject MainCamera;

        private Text Testtext;

        private Material AlertText = new Material(Shader.Find("GUI/Text Shader"));

        private static int NotificationDecayTime = 300;

        private int NotificationDecayTimeCounter;

        public static int NoticationThreshold = 30;

        private string[] Notifilines;

        private string newtext;

        public static string PreviousNotifi;

        private bool HasInit;

        private static Text NotifiText;

        public static bool IsEnabled = true;
        public static void ClearPastNotifications(int amount) // use this to clear the notifications 
        {
            string text = "";
            foreach (string text2 in Enumerable.ToArray<string>(Enumerable.Skip<string>(NotifiLib.NotifiText.text.Split(Environment.NewLine.ToCharArray()), amount)))
            {
                if (text2 != "")
                {
                    text = text + text2 + "\n";
                }
            }
            NotifiLib.NotifiText.text = text;
        }
        private static Color ParseColorFromMessage(string message) // this doesnt work
        {
            try
            {
                int startIndex = message.IndexOf("<color=") + 7;
                int endIndex = message.IndexOf(">", startIndex);
                if (startIndex > 6 && endIndex > startIndex)
                {
                    string colorCode = message.Substring(startIndex, endIndex - startIndex).Trim();
                    if (ColorUtility.TryParseHtmlString(colorCode, out Color color))
                    {
                        return color;
                    }
                    else if (ColorUtility.TryParseHtmlString($"#{colorCode}", out Color hexColor)) 
                    {
                        return hexColor;
                    }
                }
            }
            catch
            {
                Debug.LogWarning("Failed to parse color from message.");
            }
            return Color.white;
        }

        private void PlayNotificationSound() // u can change this sound if u want idrc
        {
            StartCoroutine(DownloadAndPlaySound("https://github.com/odinong/BioFr/raw/refs/heads/main/buttonpress.ogg"));
        }
        
        private void PlayStartupSound() // u can change this sound if u want idrc
        {
            StartCoroutine(DownloadAndPlaySound("https://github.com/odinong/BioFr/raw/refs/heads/main/Startup.mp3"));
        }

        private IEnumerator DownloadAndPlaySound(string url) // dont remove this
        {
            using (var request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Failed to download sound: {request.error}");
                    yield break;
                }

                var clip = DownloadHandlerAudioClip.GetContent(request);
                AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
            }
        }

        private static NotifiLib Instance;

        private void Awake() // dont remove this
        {
            Instance = this;
            {
                base.Logger.LogInfo("Plugin NotificationLibrary is loaded!");
            }
        }

        private void OnDestroy() // u can prob remove this
        {
            Instance = null;
        }
    }
}
