using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/* 
Zeon
Created by athena (@trix9x)
Zeon falls under the GPL-3.0 license. Do not take credit for the work as it remains copyrighted under the terms of this license.

Any comments here are completely for me, do not take my code without permission
*/
namespace Zeon.Patches
{
    internal class CoroutineManager : MonoBehaviour
    {
        public static CoroutineManager instance = null;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
        }

        public static Coroutine RunCoroutine(IEnumerator enumerator)
        {
            EnsureInstanceExists();
            return instance.StartCoroutine(enumerator);
        }

        public static void EndCoroutine(Coroutine enumerator)
        {
            if (instance != null)
            {
                instance.StopCoroutine(enumerator);
            }
        }

        private static void EnsureInstanceExists()
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("CoroutineManager");
                instance = obj.AddComponent<CoroutineManager>();
            }
        }
    }
}