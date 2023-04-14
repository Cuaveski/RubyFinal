using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor.TestTools.TestRunner.Api;
#if GAIM_SYNC
using WebSocketSharp;
#endif

namespace edu.ucf.gaim
{
    [Serializable]
    public struct LogMessage
    {
        public string msg;
        public string src;
        public string st;
        public string type;
    }
    // ensure class initializer is called whenever scripts recompile
    [InitializeOnLoadAttribute]
    [ExecuteAlways]
    public class GaiMBridge
    {

    }

}