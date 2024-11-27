using Photon.Pun;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using static Zeon.Menu.Main;
using static Zeon.Settings.Settings;

/* 
Zeon
Created by athena (@trix9x)
Zeon falls under the GPL-3.0 license. Do not take credit for the work as it remains copyrighted under the terms of this license.

Any comments here are completely for me, do not take my code without permission
*/
namespace Zeon.Classes
{
	public class Button : MonoBehaviour
	{
		public string relatedText;

		public static float buttonCooldown = 0f;

        private static readonly string TempDir = Path.Combine(Path.GetTempPath(), "tempZeon");

        public void OnTriggerEnter(Collider collider)
		{
			if (Time.time > buttonCooldown && collider == buttonCollider && menu != null)
			{
                buttonCooldown = Time.time + 0.2f;
                GorillaTagger.Instance.StartVibration(rightHanded, GorillaTagger.Instance.tagHapticStrength / 2f, GorillaTagger.Instance.tagHapticDuration / 2f);
                var audioSource = rightHanded ? GorillaTagger.Instance.offlineVRRig.leftHandPlayer : GorillaTagger.Instance.offlineVRRig.rightHandPlayer;
                PlaySound("https://github.com/odinong/BioFr/raw/refs/heads/main/buttonpress.ogg", "buttonSound.ogg", audioSource);
				Toggle(this.relatedText);
            }
		}
        private static void PlaySound(string source, string fileName, AudioSource audioSource)
        {
            Directory.CreateDirectory(TempDir);
            string filePath = Path.Combine(TempDir, fileName);
            if (!File.Exists(filePath)) new WebClient().DownloadFile(source, filePath);

            var audioType = GetAudioType(Path.GetExtension(fileName));
            if (audioType == AudioType.UNKNOWN) return;

            using (var www = UnityWebRequestMultimedia.GetAudioClip("file://" + filePath, audioType))
            {
                var request = www.SendWebRequest();
                while (!request.isDone) { }
                if (www.result == UnityWebRequest.Result.Success)
                {
                    var clip = DownloadHandlerAudioClip.GetContent(www);
                    audioSource.PlayOneShot(clip);
                }
            }
        }
        private static AudioType GetAudioType(string ext)
        {
            ext = ext.ToLower();

            if (ext == ".mp3")
            {
                return AudioType.MPEG;
            }
            else if (ext == ".wav")
            {
                return AudioType.WAV;
            }
            else if (ext == ".ogg")
            {
                return AudioType.OGGVORBIS;
            }
            else
            {
                return AudioType.UNKNOWN;
            }
        }

    }
}
