using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class DebugFlag : MonoBehaviour {

    [System.Flags]
    public enum DebugLevel {
        Information = 1,
        Warning = 2,
        Error = 4
    }

    public DebugLevel Level {
        get {
            DebugLevel value = 0;

            value |= Information ? DebugLevel.Information : 0;
            value |= Warning ? DebugLevel.Warning : 0;
            value |= Error ? DebugLevel.Error : 0;

            return value;
        }
    }

    public bool Information;
    public bool Warning;
    public bool Error;

}
