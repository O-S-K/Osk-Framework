#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OSK
{
    [CustomEditor(typeof(Main))]
    public class MainEditor : Editor
    {
    }
}
#endif