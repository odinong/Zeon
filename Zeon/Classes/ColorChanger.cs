using UnityEngine;

/* 
Zeon
Created by athena (@trix9x)
Zeon falls under the GPL-3.0 license. Do not take credit for the work as it remains copyrighted under the terms of this license.

Any comments here are completely for me, do not take my code without permission
*/
namespace Zeon.Classes
{
    public class ColorChanger : TimedBehaviour
    {
        public override void Start()
        {
            base.Start();
            renderer = base.GetComponent<Renderer>();
            Update();
        }
        public override void Update()
        {
            base.Update();
            if (colorInfo != null)
            {
                if (!colorInfo.copyRigColors)
                {
                    Color color = new Gradient { colorKeys = colorInfo.colors }.Evaluate((Time.time / 2f) % 1);
                    if (colorInfo.isRainbow)
                    {
                        float h = (Time.frameCount / 180f) % 1f;
                        color = UnityEngine.Color.HSVToRGB(h, 1f, 1f);
                    }
                    renderer.material.color = color;
                }
                else
                {
                    renderer.material = GorillaTagger.Instance.offlineVRRig.mainSkin.material;
                }
            }
        }
        public Renderer renderer;
        public ExtGradient colorInfo;
    }
}
