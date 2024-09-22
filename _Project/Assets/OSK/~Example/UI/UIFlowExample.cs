using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

public class UIFlowExample : MonoBehaviour
{
    private void Start()
    {
        World.UI.ShowScreen<HomeUIExample>();
    } 
}
