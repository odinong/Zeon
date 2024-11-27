using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/* 
Zeon
Created by athena (@trix9x)
Zeon falls under the GPL-3.0 license. Do not take credit for the work as it remains copyrighted under the terms of this license.

Any comments here are completely for me, do not take my code without permission
*/
namespace Zeon.Classes
{
    public class TimedBehaviour : MonoBehaviour
    {
        public virtual void Start()
        {
            startTime = Time.time;
        }

        public virtual void Update()
        {
            if (!complete)
            {
                progress = Mathf.Clamp((Time.time - startTime) / duration, 0f, 1f);
                if (Time.time - startTime > duration)
                {
                    if (loop)
                    {
                        OnLoop();
                    }
                    else
                    {
                        complete = true;
                    }
                }
            }
        }

        public virtual void OnLoop()
        {
            startTime = Time.time;
        }

        public bool complete = false;

        public bool loop = true;

        public float progress = 0f;

        protected bool paused = false;

        protected float startTime;

        protected float duration = 2f;
    }
}
