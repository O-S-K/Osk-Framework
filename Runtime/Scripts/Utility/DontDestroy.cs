using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public class DontDestroy : MonoBehaviour
    {
        public void DontDesGOOnLoad()
        {
            DontDestroyOnLoad(gameObject);
        }
        
        public void DontDesMonoOnLoad(MonoBehaviour mono)
        {
            DontDestroyOnLoad(mono);
        } 
    }
}
