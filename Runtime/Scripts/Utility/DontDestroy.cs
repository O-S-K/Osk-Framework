using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public class DontDestroy : MonoBehaviour
    {
        public bool _isDontDestroyOnLoad = false;
        private void Awake()
        {
            if (!_isDontDestroyOnLoad) return;
            if(FindObjectsOfType(GetType()).Length > 1)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        
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
