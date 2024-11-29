using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using PlayFab.EventsModels;
using System;
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
namespace Zeon.Patchers
{
    public class AntiLag : MonoBehaviour
    {
        private static bool Prefix(byte eventCode, object eventContent, RaiseEventOptions raiseEventOptions, SendOptions sendOptions) // this isnt antilag but im adding it anyways
        {
            Debug.Log(string.Format("Event code: {0}, Event Context: {1}, RaiseEvent options: {2}, Send Options: {3}", new object[]
            {
                eventCode,
                eventContent,
                raiseEventOptions,
                sendOptions
            }));
            return false;
        }
    }
}
