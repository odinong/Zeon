using BepInEx;
using HarmonyLib;
using Zeon.Classes;
using Zeon.Notifications;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static Zeon.Menu.Buttons;
using static Zeon.Settings.Settings;
using Photon.Pun;
using ExitGames.Client.Photon;
using GorillaNetworking;
using OVR;
using Photon.Realtime;
using PlayFab.ExperimentationModels;
using Zeon.Patches;
using static MonoMod.Cil.RuntimeILReferenceBag.FastDelegateInvokers;
using System.Collections.Generic;
using Zeon.Mods;
using static Fusion.Sockets.NetBitBuffer;

/* 
Zeon
Created by athena (@trix9x)
Zeon falls under the GPL-3.0 license. Do not take credit for the work as it remains copyrighted under the terms of this license.

Any comments here are completely for me, do not take my code without permission
*/
namespace Zeon.Menu
{
    [HarmonyPatch(typeof(GorillaLocomotion.Player))]
    [HarmonyPatch("LateUpdate", MethodType.Normal)]
    public class Main : MonoBehaviour
    {
        // Constant
        public static bool lastMasterClient = false;
        public static bool blockAcEvents = false;
        public static Vector3 leftPageButton = new Vector3(0.6f, -0.004935512f, 0.3324285f);
        public static Vector3 leftPageButtonText = new Vector3(0.07707082f, 0, 0.1223036f);
        public static Vector3 rightPageButton = new Vector3(0.6f, -0.0007184388f, 0.2374102f);
        public static Vector3 rightPageButtonSize = new Vector3(0.03996109f, 0.9199839f, 0.07821f);
        public static Vector3 homeButtonSize = new Vector3(0.03996109f, 0.4474708f, 0.07821f);
        public static Vector3 rightPageButtonText = new Vector3(0.07707082f, 0, 0.0892088f);
        public static Vector3 disconnectButtonPos = new Vector3(0.56f, 0f, 0.5639689f);
        public static Vector3 homeButtonPos = new Vector3(0.5175098f, 0.007782096f, -0.5547471f);
        public static Vector3 disconnectButtonTextPos = new Vector3(0.064f, 0f, 0.2183269f);
        public static Vector3 homeButtonTextPos = new Vector3(0.064f, 0.003891052f, -0.2062257f);
        public static Vector3 disconnectButtonPos2 = new Vector3(0.56f, 0f, -0.5206226f);
        public static Vector3 disconnectButtonTextPos2 = new Vector3(0.064f, 0f, -0.20f);
        public static void Prefix()
        {
            float time = Time.time * 2f; // Adjust multiplier for speed
            outlinecolor = new Color(
                Mathf.Sin(time) * 0.5f + 0.5f,
                Mathf.Sin(time + 2f) * 0.5f + 0.5f,
                Mathf.Sin(time + 4f) * 0.5f + 0.5f
            );
            // Initialize Menu
            try
                {
                    bool toOpen = (!rightHanded && ControllerInputPoller.instance.leftControllerSecondaryButton) || (rightHanded && ControllerInputPoller.instance.rightControllerSecondaryButton);
                    bool keyboardOpen = UnityInput.Current.GetKey(keyboardButton);

                    if (menu == null)
                    {
                        if (toOpen || keyboardOpen)
                        {
                            CreateMenu();
                            RecenterMenu(rightHanded, keyboardOpen);
                            if (reference == null)
                            {
                                CreateReference(rightHanded);
                            }
                        }
                    }
                    else
                    {
                        if ((toOpen || keyboardOpen))
                        {
                            RecenterMenu(rightHanded, keyboardOpen);
                        }
                        else
                        {
                            Rigidbody comp = menu.AddComponent(typeof(Rigidbody)) as Rigidbody;
                            if (rightHanded)
                            {
                                comp.velocity = GorillaLocomotion.Player.Instance.rightHandCenterVelocityTracker.GetAverageVelocity(true, 0);
                            }
                            else
                            {
                                comp.velocity = GorillaLocomotion.Player.Instance.leftHandCenterVelocityTracker.GetAverageVelocity(true, 0);
                            }

                            UnityEngine.Object.Destroy(menu, 2);
                            menu = null;

                            UnityEngine.Object.Destroy(reference);
                            reference = null;
                        }
                    }
                }
                catch (Exception exc)
                {
                    UnityEngine.Debug.LogError(string.Format("{0} // Error initializing at {1}: {2}", "butt", exc.StackTrace, exc.Message));
                }

            try
            {
                if (PhotonNetwork.InRoom)
                {
                    if (PhotonNetwork.LocalPlayer.IsMasterClient && !lastMasterClient)
                    {
                        NotifiLib.SendNotification("<color=yellow>MASTER</color> : you are now master client.");
                    }
                    lastMasterClient = PhotonNetwork.LocalPlayer.IsMasterClient;
                }
            }
            catch { }
            // Constant
                try
                {
                    // Pre-Execution
                        if (fpsObject != null)
                        {
                            fpsObject.text = "FPS: " + Mathf.Ceil(1f / Time.unscaledDeltaTime).ToString();
                        }

                    // Execute Enabled mods
                        foreach (ButtonInfo[] buttonlist in buttons)
                        {
                            foreach (ButtonInfo v in buttonlist)
                            {
                                if (v.enabled)
                                {
                                    if (v.method != null)
                                    {
                                        try
                                        {
                                            v.method.Invoke();
                                        }
                                        catch (Exception exc)
                                        {
                                            UnityEngine.Debug.LogError(string.Format("{0} // Error with mod {1} at {2}: {3}", "butt", v.buttonText, exc.StackTrace, exc.Message));
                                        }
                                    }
                                }
                            }
                        }
                } catch (Exception exc)
                {
                    UnityEngine.Debug.LogError(string.Format("{0} // Error with executing mods at {1}: {2}", "butt", exc.StackTrace, exc.Message));
                }
        }
        public static Vector3 outlinescale = new Vector3(0.09f, 0.9779137f, -1.027338f);
        public static Vector3 outlinepos = new Vector3(0.05f, 0f, -0.003090815f);
        public static Color outlinecolor = new Color(1, 1, 1);
        public static void CreateMenu()
        {
            // Menu Holder
            menu = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(menu.GetComponent<Rigidbody>());
            UnityEngine.Object.Destroy(menu.GetComponent<BoxCollider>());
            UnityEngine.Object.Destroy(menu.GetComponent<Renderer>());
            menu.transform.localScale = new Vector3(0.1f, 0.3f, 0.3825f);

            // Menu Background
            menuBackground = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(menuBackground.GetComponent<Rigidbody>());
            UnityEngine.Object.Destroy(menuBackground.GetComponent<BoxCollider>());
            menuBackground.transform.parent = menu.transform;
            menuBackground.transform.rotation = Quaternion.identity;
            menuBackground.transform.localScale = menuSize;
            menuBackground.GetComponent<Renderer>().material.color = backgroundColor.colors[0].color;
            menuBackground.transform.position = new Vector3(0.05f, 0f, 0f);

            ColorChanger colorChanger = menuBackground.AddComponent<ColorChanger>();
            colorChanger.colorInfo = backgroundColor;
            colorChanger.Start();


            GameObject Outline = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(Outline.GetComponent<Rigidbody>());
            UnityEngine.Object.Destroy(Outline.GetComponent<BoxCollider>());
            Outline.transform.parent = menu.transform;
            Outline.GetComponent<Renderer>().material.shader = Shader.Find("UI/Default");
            Outline.GetComponent<Renderer>().material.color = outlinecolor;
            Outline.transform.rotation = Quaternion.identity;
            Outline.transform.localScale = outlinescale;
            Outline.name = "MenuOutline";
            Outline.transform.position = outlinepos;

            // Canvas
            canvasObject = new GameObject();
            canvasObject.transform.parent = menu.transform;
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            CanvasScaler canvasScaler = canvasObject.AddComponent<CanvasScaler>();
            canvasObject.AddComponent<GraphicRaycaster>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvasScaler.dynamicPixelsPerUnit = 1000f;

            // Title and FPS
            Text text = new GameObject
            {
                transform =
        {
            parent = canvasObject.transform
        }
            }.AddComponent<Text>();
            text.font = currentFont;
            text.text = "Zeon V1.1.1";
            text.fontSize = 1;
            text.color = textColors[0];
            text.supportRichText = true;
            text.fontStyle = FontStyle.Italic;
            text.alignment = TextAnchor.MiddleCenter;
            text.resizeTextForBestFit = true;
            text.resizeTextMinSize = 0;
            RectTransform component = text.GetComponent<RectTransform>();
            component.localPosition = Vector3.zero;
            component.sizeDelta = new Vector2(0.24f, 0.02f);
            component.localPosition = new Vector3(0.06f, 0f, 0.165f);
            component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));

            // Buttons
            if (buttonsType != 0)
            {
                GameObject homebutton = GameObject.CreatePrimitive(PrimitiveType.Cube);
                if (!UnityInput.Current.GetKey(KeyCode.Q))
                {
                    homebutton.layer = 2;
                }
                UnityEngine.Object.Destroy(homebutton.GetComponent<Rigidbody>());
                homebutton.GetComponent<BoxCollider>().isTrigger = true;
                homebutton.transform.parent = menu.transform;
                homebutton.transform.rotation = Quaternion.identity;
                homebutton.transform.localScale = homeButtonSize;
                homebutton.transform.localPosition = homeButtonPos;
                homebutton.GetComponent<Renderer>().material.color = buttonColors[0].colors[0].color;
                homebutton.AddComponent<Classes.Button>().relatedText = "Home";

                colorChanger = homebutton.AddComponent<ColorChanger>();
                colorChanger.colorInfo = buttonColors[0];
                colorChanger.Start();

                Text homeontext = new GameObject
                {
                    transform =
            {
                parent = canvasObject.transform
            }
                }.AddComponent<Text>();
                homeontext.text = "Home";
                homeontext.font = currentFont;
                homeontext.fontSize = 1;
                homeontext.color = textColors[0];
                homeontext.alignment = TextAnchor.MiddleCenter;
                homeontext.resizeTextForBestFit = true;
                homeontext.resizeTextMinSize = 0;

                RectTransform recttt = homeontext.GetComponent<RectTransform>();
                recttt.localPosition = Vector3.zero;
                recttt.sizeDelta = new Vector2(0.15f, 0.025f);
                recttt.localPosition = homeButtonTextPos;
                recttt.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
            }
            // Page Buttons
            // Previous Page Button
            GameObject previousPageButton = GameObject.CreatePrimitive(PrimitiveType.Cube);
            if (!UnityInput.Current.GetKey(KeyCode.Q))
            {
                previousPageButton.layer = 2;
            }
            UnityEngine.Object.Destroy(previousPageButton.GetComponent<Rigidbody>());
            previousPageButton.GetComponent<BoxCollider>().isTrigger = true;
            previousPageButton.transform.parent = menu.transform;
            previousPageButton.transform.rotation = Quaternion.identity;
            previousPageButton.transform.localScale = rightPageButtonSize;
            previousPageButton.transform.localPosition = leftPageButton;
            previousPageButton.GetComponent<Renderer>().material.color = buttonColors[0].colors[0].color;
            previousPageButton.AddComponent<Classes.Button>().relatedText = "PreviousPage";

            colorChanger = previousPageButton.AddComponent<ColorChanger>();
            colorChanger.colorInfo = buttonColors[0];
            colorChanger.Start();

            Text previousText = new GameObject
            {
                transform =
        {
            parent = canvasObject.transform
        }
            }.AddComponent<Text>();
            previousText.font = currentFont;
            previousText.text = "<<<<<<<<<";
            previousText.fontSize = 1;
            previousText.color = textColors[0];
            previousText.alignment = TextAnchor.MiddleCenter;
            previousText.resizeTextForBestFit = true;
            previousText.resizeTextMinSize = 0;
            RectTransform prevComponent = previousText.GetComponent<RectTransform>();
            prevComponent.localPosition = leftPageButtonText;
            prevComponent.sizeDelta = new Vector2(0.2f, 0.03f);
            prevComponent.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));

            // Next Page Button
            GameObject nextPageButton = GameObject.CreatePrimitive(PrimitiveType.Cube);
            if (!UnityInput.Current.GetKey(KeyCode.Q))
            {
                nextPageButton.layer = 2;
            }
            UnityEngine.Object.Destroy(nextPageButton.GetComponent<Rigidbody>());
            nextPageButton.GetComponent<BoxCollider>().isTrigger = true;
            nextPageButton.transform.parent = menu.transform;
            nextPageButton.transform.rotation = Quaternion.identity;
            nextPageButton.transform.localScale = rightPageButtonSize;
            nextPageButton.transform.localPosition = rightPageButton;
            nextPageButton.GetComponent<Renderer>().material.color = buttonColors[0].colors[0].color;
            nextPageButton.AddComponent<Classes.Button>().relatedText = "NextPage";

            colorChanger = nextPageButton.AddComponent<ColorChanger>();
            colorChanger.colorInfo = buttonColors[0];
            colorChanger.Start();

            Text nextText = new GameObject
            {
                transform =
        {
            parent = canvasObject.transform
        }
            }.AddComponent<Text>();
            nextText.font = currentFont;
            nextText.text = ">>>>>>>>>";
            nextText.fontSize = 1;
            nextText.color = textColors[0];
            nextText.alignment = TextAnchor.MiddleCenter;
            nextText.resizeTextForBestFit = true;
            nextText.resizeTextMinSize = 0;
            RectTransform nextComponent = nextText.GetComponent<RectTransform>();
            nextComponent.localPosition = rightPageButtonText;
            nextComponent.sizeDelta = new Vector2(0.2f, 0.03f);
            nextComponent.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));

            // Mod Buttons
            ButtonInfo[] activeButtons = buttons[buttonsType].Skip(pageNumber * buttonsPerPage).Take(buttonsPerPage).ToArray();
            for (int i = 0; i < activeButtons.Length; i++)
            {
                CreateButton(i * 0.1f, activeButtons[i]);
            }
        }

        public static Vector3 buttonCreator = new Vector3(0.56f, 0f, 0.1237412f);
        public static void CreateButton(float offset, ButtonInfo method)
        {
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            if (!UnityInput.Current.GetKey(KeyCode.Q))
            {
                gameObject.layer = 2;
            }
            UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            gameObject.transform.parent = menu.transform;
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.transform.localScale = new Vector3(0.04f, 0.9f, 0.08f);
            gameObject.transform.localPosition = new Vector3(buttonCreator.x, buttonCreator.y, buttonCreator.z - offset);
            gameObject.AddComponent<Classes.Button>().relatedText = method.buttonText;

            ColorChanger colorChanger = gameObject.AddComponent<ColorChanger>();
            if (method.enabled)
            {
                colorChanger.colorInfo = new ExtGradient { colors = GetSolidGradient(new Color(25/255f, 25/255f, 25/255f)) };
            }
            else
            {
                colorChanger.colorInfo = new ExtGradient { colors = GetSolidGradient(new Color(41/255f, 41/255f, 41/255f)) };
            }
            colorChanger.Start();

            Text text = new GameObject
            {
                transform =
                {
                    parent = canvasObject.transform
                }
            }.AddComponent<Text>();
            text.font = currentFont;
            text.text = method.buttonText;
            if (method.overlapText != null)
            {
                text.text = method.overlapText;
            }
            text.supportRichText = true;
            text.fontSize = 1;
            if (method.enabled)
            {
                text.color = textColors[1];
            }
            else
            {
                text.color = textColors[0];
            }
            text.alignment = TextAnchor.MiddleCenter;
            text.fontStyle = FontStyle.Italic;
            text.resizeTextForBestFit = true;
            text.resizeTextMinSize = 0;
            RectTransform component = text.GetComponent<RectTransform>();
            component.localPosition = Vector3.zero;
            component.sizeDelta = new Vector2(.2f, .03f);
            component.localPosition = new Vector3(textForButtonCreator.x, textForButtonCreator.y, textForButtonCreator.z - offset / 2.6f);
            component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
        }
        public static Vector3 textForButtonCreator = new Vector3(0.064f, 0f, 0.04819896f);
        public static void RecreateMenu()
        {
            if (menu != null)
            {
                UnityEngine.Object.Destroy(menu);
                menu = null;

                CreateMenu();
                RecenterMenu(rightHanded, UnityInput.Current.GetKey(keyboardButton));
            }
        }

        public static void RecenterMenu(bool isRightHanded, bool isKeyboardCondition)
        {
            if (!isKeyboardCondition)
            {
                if (!isRightHanded)
                {
                    menu.transform.position = GorillaTagger.Instance.leftHandTransform.position;
                    menu.transform.rotation = GorillaTagger.Instance.leftHandTransform.rotation;
                }
                else
                {
                    menu.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                    Vector3 rotation = GorillaTagger.Instance.rightHandTransform.rotation.eulerAngles;
                    rotation += new Vector3(0f, 0f, 180f);
                    menu.transform.rotation = Quaternion.Euler(rotation);
                }
            }
            else
            {
                try
                {
                    TPC = GameObject.Find("Player Objects/Third Person Camera/Shoulder Camera").GetComponent<Camera>();
                }
                catch { }
                if (TPC != null)
                {
                    TPC.transform.position = new Vector3(-65.3265f, 23.3798f, - 80.4063f);
                    TPC.transform.rotation = Quaternion.identity;
                    menu.transform.parent = TPC.transform;
                    menu.transform.position = (TPC.transform.position + (Vector3.Scale(TPC.transform.forward, new Vector3(0.5f, 0.5f, 0.5f)))) + (Vector3.Scale(TPC.transform.up, new Vector3(-0.02f, -0.02f, -0.02f)));
                    Vector3 rot = TPC.transform.rotation.eulerAngles;
                    rot = new Vector3(rot.x - 90, rot.y + 90, rot.z);
                    menu.transform.rotation = Quaternion.Euler(rot);

                    if (reference != null)
                    {
                        if (Mouse.current.leftButton.isPressed)
                        {
                            Ray ray = TPC.ScreenPointToRay(Mouse.current.position.ReadValue());
                            RaycastHit hit;
                            bool worked = Physics.Raycast(ray, out hit, 100);
                            if (worked)
                            {
                                Classes.Button collide = hit.transform.gameObject.GetComponent<Classes.Button>();
                                if (collide != null)
                                {
                                    collide.OnTriggerEnter(buttonCollider);
                                }
                            }
                        }
                        else
                        {
                            reference.transform.position = new Vector3(999f, -999f, -999f);
                        }
                    }
                }
            }
        }

        public static void CreateReference(bool isRightHanded)
        {
            reference = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            if (isRightHanded)
            {
                reference.transform.parent = GorillaTagger.Instance.leftHandTransform;
            }
            else
            {
                reference.transform.parent = GorillaTagger.Instance.rightHandTransform;
            }
            reference.GetComponent<Renderer>().material.color = backgroundColor.colors[0].color;
            reference.transform.localPosition = new Vector3(0f, -0.1f, 0f);
            reference.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            buttonCollider = reference.GetComponent<SphereCollider>();

            ColorChanger colorChanger = reference.AddComponent<ColorChanger>();
            colorChanger.colorInfo = backgroundColor;
            colorChanger.Start();
        }

        public static void Toggle(string buttonText)
        {
            int lastPage = ((buttons[buttonsType].Length + buttonsPerPage - 1) / buttonsPerPage) - 1;
            if (buttonText == "PreviousPage")
            {
                pageNumber--;
                if (pageNumber < 0)
                {
                    pageNumber = lastPage;
                }
            } else
            {
                if (buttonText == "NextPage")
                {
                    pageNumber++;
                    if (pageNumber > lastPage)
                    {
                        pageNumber = 0;
                    }
                } else
                {
                    ButtonInfo target = GetIndex(buttonText);
                    if (target != null)
                    {
                        if (target.isTogglable)
                        {
                            target.enabled = !target.enabled;
                            if (target.enabled)
                            {
                                if (target.enableMethod != null)
                                {
                                    try { target.enableMethod.Invoke(); } catch { }
                                }
                            }
                            else
                            {
                                if (target.disableMethod != null)
                                {
                                    try { target.disableMethod.Invoke(); } catch { }
                                }
                            }
                        }
                        else
                        {
                            if (target.method != null)
                            {
                                try { target.method.Invoke(); } catch { }
                            }
                        }
                    }
                    else
                    {
                        if (buttonText != "Home")
                        {
                            UnityEngine.Debug.LogError(buttonText + " does not exist");
                        }
                    }
                }
            }
            if (buttonText == "Home")
            {
                Debug.Log("Home Button Pressed!");
                buttonsType = 0;
                pageNumber = 0;
                Global.ReturnHome();
                RecreateMenu();
            }
            RecreateMenu();
        }

        public static GradientColorKey[] GetSolidGradient(Color color)
        {
            return new GradientColorKey[] { new GradientColorKey(color, 0f), new GradientColorKey(color, 1f) };
        }

        public static ButtonInfo GetIndex(string buttonText)
        {
            foreach (ButtonInfo[] buttons in Menu.Buttons.buttons)
            {
                foreach (ButtonInfo button in buttons)
                {
                    if (button.buttonText == buttonText)
                    {
                        return button;
                    }
                }
            }

            return null;
        }

        // Variables
            // Important
                // Objects
                    public static GameObject menu;
                    public static GameObject menuBackground;   
                    public static GameObject reference;
                    public static GameObject canvasObject;

                    public static SphereCollider buttonCollider;
                    public static Camera TPC;
                    public static Text fpsObject;

        // Data
            public static int pageNumber = 0;
            public static int buttonsType = 0;
    }
}
