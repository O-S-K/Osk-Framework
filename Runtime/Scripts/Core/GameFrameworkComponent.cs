using UnityEngine;

namespace OSK
{
    [DefaultExecutionOrder(-1001)]
    public abstract class GameFrameworkComponent : MonoBehaviour
    {
        protected void Awake()
        {
            Main.Register(this);
        }
        
        public void OnDestroy() 
        {
            Main.UnRegister(this);
        }

        public abstract void OnInit();
    }
}